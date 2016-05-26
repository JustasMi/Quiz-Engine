using Quiz_Engine.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quiz_Engine
{
    public partial class Feedback : Form
    {
        List<Question> questions;
        bool retakingQuiz;
        Quiz quiz;
        DButility db = new DButility();
        public Feedback(List<Question> questions, bool retakingQuiz)
        {
            InitializeComponent();
            this.questions = questions;
            this.retakingQuiz = retakingQuiz;
            setupFeedback();
        }

        public Feedback(List<Question> questions, bool retakingQuiz, Quiz quiz)
        {
            InitializeComponent();
            this.questions = questions;
            this.retakingQuiz = retakingQuiz;
            this.quiz = quiz;
            setupFeedback();
        }

        private void setupFeedback()
        {
            FlowLayoutPanel panel = getPanel();
            panel.Controls.Add(getQuestionHeading("Quiz Summary"));
            flowLayoutPanel1.Controls.Add(panel);

            int pastQuizSum = 0;
            int currentQuizSum = 0;
            // Feedback for every question
            for (int i = 0; i < questions.Count; i++ )
            {
                panel = getPanel();

                panel.Controls.Add(getQuestionHeading("Question "+(i+1)));
                panel.Controls.Add(getQuestionText(questions[i].QuestionText));
                panel.Controls.Add(getQuestionText("Answered correctly: "+questions[i].isAnsweredCorrectly().ToString()));
                if (questions[i].isAnsweredCorrectly())
                    currentQuizSum += 1;
                panel.Controls.Add(getSelectedAnswer(questions[i].getSelectedAnswer()));
                panel.Controls.Add(getCorrectAnswer(questions[i].getCorrectAnswer()));
                panel.Controls.Add(addFeedbackText(questions[i].Feedback));
                // IF retaking quiz
                if (retakingQuiz)
                {
                    // CONTINUE HERE
                    bool previousAnswerCorectness = db.checkPreviousAnswer(questions[i].Id, quiz.Id);
                    if (previousAnswerCorectness)
                        pastQuizSum += 1;
                    string answer = previousAnswerCorectness ? "Correctly" : "Incorrectly";
                    panel.Controls.Add(getQuestionText("Previously answered: "+answer));
                }

                flowLayoutPanel1.Controls.Add(panel);
            }
            // Overall summarry compared to previous test
            if (retakingQuiz)
            {
                string fdb;
                if (pastQuizSum > currentQuizSum)
                {
                    fdb = "You have performed worse than in the past attempt.";
                }
                else if (currentQuizSum > pastQuizSum)
                {
                    fdb = "Excellent! Your scores have improved on this test.";
                }
                else
                {
                    fdb = "Overall score is the same as last time";
                }
                panel = getPanel();

                panel.Controls.Add(getQuestionText("Performance comparison: "+fdb));
                flowLayoutPanel1.Controls.Add(panel);
            }

            // Overall feedback
            Dictionary<string,List<Question>> dic = new Dictionary<string,List<Question>>();
            
            foreach (Question q in questions)
            {
                if (dic.ContainsKey(q.Topic.Name))
                {
                    List<Question> list = dic[q.Topic.Name];
                    list.Add(q);
                    dic.Remove(q.Topic.Name);
                    dic.Add(q.Topic.Name, list);
                }
                else
                {
                    List<Question> list = new List<Question>();
                    list.Add(q);
                    dic.Add(q.Topic.Name, list);
                }
            }

            List<string> keys = dic.Keys.ToList();

            foreach (string key in keys)
            {
                // Give summaries
                panel = getPanel();
                panel.Controls.Add(getQuestionHeading("Topic '" + key + "' summary"));
                flowLayoutPanel2.Controls.Add(panel);

                Dictionary<string, List<Question>> natureDic = new Dictionary<string, List<Question>>();
                // Summarise by nature
                foreach (Question q in dic[key])
                {
                    if (natureDic.ContainsKey(q.Nature))
                    {
                        List<Question> list = natureDic[q.Nature];
                        list.Add(q);
                        natureDic.Remove(q.Nature);
                        natureDic.Add(q.Nature, list);
                    }
                    else
                    {
                        List<Question> list = new List<Question>();
                        list.Add(q);
                        natureDic.Add(q.Nature, list);
                    }
                }

                panel.Controls.Add(getQuestionText("Nature summary: ")); 
                foreach (KeyValuePair<string, List<Question>> naturePair in natureDic)
                {
                    int totalQuestions = naturePair.Value.Count;
                    int correctAnswers = naturePair.Value.Where(q => q.isAnsweredCorrectly()).Count();
                    int percentage = (100 * correctAnswers) / totalQuestions;

                    panel.Controls.Add(getQuestionText(naturePair.Key + " : "+correctAnswers+"/" + totalQuestions + " ("+percentage+"%) are correct"));
                }
                flowLayoutPanel2.Controls.Add(panel);
                panel = getPanel();

                // Generate feedback for nature
                // Nature types 'Application', 'Bookwork', 'Background'
                //Dictionary<string, List<String>> recommendation = new Dictionary<string, List<String>>();
                List<String> recommendation = new List<String>();
                List<String> priorityRecommendation = new List<String>();
                bool flag = false;
                string analysis = String.Empty;
                if (natureDic.ContainsKey("Application") && natureDic.ContainsKey("Bookwork"))
                {
                    int bookWorkPercentage = (100 * natureDic["Bookwork"].Where(q => q.isAnsweredCorrectly()).Count()) / natureDic["Bookwork"].Count;
                    int applicationPercentage = (100 * natureDic["Application"].Where(q => q.isAnsweredCorrectly()).Count()) / natureDic["Application"].Count;
                    double coefficient = 1.3;
                    if (bookWorkPercentage > applicationPercentage * coefficient)
                    {
                        // if Bookwork higher (by a third)than application
                        //priorityRecommendation.Add("Bookwork");
                        analysis = "Priority! Score suggests that you did well in 'Bookwork' questions, but there was a significantly worse score in 'Application' type. You should work on applying theoretical knowledge better.";
                        flag = true;
                    }
                    else if (bookWorkPercentage * coefficient < applicationPercentage)
                    {
                        // if Application (by a third) higher than bookwork
                        //priorityRecommendation.Add("Application");
                        analysis = "Priority! Scores suggests that did well in 'Application' questions, but there was a significantly worse score in 'Bookwork' type. You should learn more theory.";
                        flag = true;
                    }

                    /*
                    // Check which ones to study if ANY below 80%
                    foreach (KeyValuePair<string, List<Question>> naturePair in natureDic)
                    {
                        if (!(flag && (naturePair.Key == "Bookwork" || naturePair.Key == "Application")))
                        {
                            int totalQuestions = naturePair.Value.Count;
                            int correctAnswers = naturePair.Value.Where(q => q.isAnsweredCorrectly()).Count();
                            int percentage = (100 * correctAnswers) / totalQuestions;
                            if (percentage <= 60)
                            {
                                priorityRecommendation.Add(naturePair.Key);
                            }
                            else if (percentage <= 80)
                            {
                                recommendation.Add(naturePair.Key);
                            }
                        }
                    }
                     */
                }

                // check individually
                foreach (KeyValuePair<string, List<Question>> naturePair in natureDic)
                {
                    int totalQuestions = naturePair.Value.Count;
                    int correctAnswers = naturePair.Value.Where(q => q.isAnsweredCorrectly()).Count();
                    int percentage = (100 * correctAnswers) / totalQuestions;
                    if (percentage <= 60)
                    {
                        priorityRecommendation.Add(naturePair.Key);
                    }
                    else if (percentage <= 80)
                    {
                        recommendation.Add(naturePair.Key);
                    }
                }
                
                // Display nature feedback
                panel = getPanel();
                panel.Controls.Add(getQuestionHeading("Topic '"+key+"' Recommendations"));
                if (flag)
                {
                    panel.Controls.Add(getQuestionText(analysis));
                    /*
                    if (priorityRecommendation[0] != "Application")
                        panel.Controls.Add(getQuestionText("Priority! Score suggests that you did well in 'Bookwork' questions, but there was a significantly worse score in 'Application' type. You should work on applying theoretical knowledge better."));
                    else
                        panel.Controls.Add(getQuestionText("Priority! Scores suggests that did well in 'Application' questions, but there was a significantly worse score in 'Bookwork' type. You should learn more theory."));
                     */
                }

                string rec = "";
                if (priorityRecommendation.Count > 0)
                {
                    rec += "Priority! Suggested revision: ";
                    foreach (String s in priorityRecommendation)
                    {
                        rec += "'"+s+"', ";
                    }
                    rec = rec.Remove(rec.Length - 2);
                    rec += " questions.";
                    panel.Controls.Add(getQuestionText(rec));
                }

                if (recommendation.Count > 0)
                {
                    rec += "Suggested revision: ";

                    foreach (String s in recommendation)
                    {
                        rec += "'"+s + "', ";
                    }
                    rec = rec.Remove(rec.Length - 2);
                    rec += " questions.";
                    panel.Controls.Add(getQuestionText(rec));
                }
                
                flowLayoutPanel3.Controls.Add(panel);

                panel = getPanel();
                // Summarise by difficulty
                Dictionary<string, List<Question>> diffDic = new Dictionary<string, List<Question>>();
                foreach (Question q in dic[key])
                {
                    if (diffDic.ContainsKey(q.Difficulty))
                    {
                        List<Question> list = diffDic[q.Difficulty];
                        list.Add(q);
                        diffDic.Remove(q.Difficulty);
                        diffDic.Add(q.Difficulty, list);
                    }
                    else
                    {
                        List<Question> list = new List<Question>();
                        list.Add(q);
                        diffDic.Add(q.Difficulty, list);
                    }
                }

                panel.Controls.Add(getQuestionText("Difficulty summary: "));
                foreach (KeyValuePair<string, List<Question>> difficultyPair in diffDic)
                {
                    // Count correct answers
                    //naturePair.Value.Where(q => q.isAnsweredCorrectly()).Count();
                    int correctAnswers = difficultyPair.Value.Where(q => q.isAnsweredCorrectly()).Count();
                    int totalQuestions = difficultyPair.Value.Count;
                    int percentage = (100 * correctAnswers) / totalQuestions;
                    panel.Controls.Add(getQuestionText(difficultyPair.Key + " : " + correctAnswers + "/" + totalQuestions + " ("+percentage+"%) are correct"));
                }

                flowLayoutPanel2.Controls.Add(panel);
                /*
                int easyPerc;
                int mediumPerc;
                int hardPerc;
                bool easyFlag = false;
                bool mediumFlag = false;
                bool hardFlag = false;
                 */
                List<string> goodPerf = new List<string>();
                List<string> badPerf = new List<string>();

                foreach (KeyValuePair<string, List<Question>> difficultyPair in diffDic)
                {
                    int correctAnswers = difficultyPair.Value.Where(q => q.isAnsweredCorrectly()).Count();
                    int totalQuestions = difficultyPair.Value.Count;
                    int percentage = (100 * correctAnswers) / totalQuestions;
                    if (percentage >= 80)
                    {
                        goodPerf.Add(difficultyPair.Key);
                    }
                    else if (percentage <= 60)
                    {
                        badPerf.Add(difficultyPair.Key);
                    }
                }
                /*
                if (diffDic.ContainsKey("Easy"))
                {
                    int correctAnswers = diffDic["Easy"].Where(q => q.isAnsweredCorrectly()).Count();
                    int totalQuestions = diffDic["Easy"].Count;
                    easyPerc = (correctAnswers * 100 ) / totalQuestions;
                    easyFlag = true;
                }

                if (diffDic.ContainsKey("Intermediate"))
                {
                    int correctAnswers = diffDic["Intermediate"].Where(q => q.isAnsweredCorrectly()).Count();
                    int totalQuestions = diffDic["Intermediate"].Count;
                    mediumPerc = (correctAnswers * 100) / totalQuestions;
                    mediumFlag = true;
                }

                if (diffDic.ContainsKey("Hard"))
                {
                    int correctAnswers = diffDic["Hard"].Where(q => q.isAnsweredCorrectly()).Count();
                    int totalQuestions = diffDic["Hard"].Count;
                    hardPerc = (correctAnswers * 100) / totalQuestions;
                    hardFlag = true;
                }
                 */
                panel = getPanel();
                panel.Controls.Add(getQuestionText("Performance by difficulty: "));
                string goodMsg = "You performed only in : ";
                foreach (string m in goodPerf)
                {
                    goodMsg += "'" + m + "', ";
                }
                if (goodPerf.Count > 0)
                {
                    goodMsg = goodMsg.Remove(goodMsg.Length-2)+" difficulty questions";
                }

                string badMsg = "your performance was lacking in: ";
                foreach (string m in badPerf)
                {
                   badMsg += "'" + m + "', ";
                }
                if (badPerf.Count > 0)
                {
                    badMsg = badMsg.Remove(badMsg.Length - 2) + " difficulty questions";
                }

                if (goodPerf.Count == 3)
                {
                    panel.Controls.Add(getQuestionText("You did really well in all difficulty levels"));
                }
                else if (goodPerf.Count == 2)
                {
                    if (badPerf.Count > 0)
                    {
                        panel.Controls.Add(getQuestionText("You did quite well, however " + badMsg));
                    }
                    else
                    {
                        panel.Controls.Add(getQuestionText("You did quite well in all difficulty levels"));
                    }
                }
                else if (goodPerf.Count == 1)
                {
                    if (badPerf.Count > 0)
                    {
                        panel.Controls.Add(getQuestionText(goodMsg + ". However, " + badMsg));
                    }
                    else
                    {
                        panel.Controls.Add(getQuestionText("You did quite well in all difficulty levels"));
                    }
                }
                else if (goodPerf.Count == 0 && badPerf.Count > 0)
                {
                    panel.Controls.Add(getQuestionText("Your scores are not very good, especially "+badMsg));
                }

                if (goodPerf.Count > 0 || badPerf.Count > 0)
                {
                    flowLayoutPanel3.Controls.Add(panel);
                }

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            setupFeedback();
        }


        private Label getQuestionHeading(String text)
        {
            Label l = new Label();
            l.Font = new Font(label1.Font.FontFamily, 16);
            l.Text = text;
            l.AutoSize = true;
            //flowLayoutPanel1.Controls.Add(l);
            return l;
        }

        private Label getQuestionText(String text)
        {
            Label l = new Label();
            l.Font = new Font(label1.Font.FontFamily, 10);
            l.Text = text;
            l.AutoSize = true;
            //flowLayoutPanel1.Controls.Add(l);
            return l;
        }

        private Label getSelectedAnswer(String text)
        {
            Label l = new Label();
            l.Font = new Font(label1.Font.FontFamily, 10);
            l.Text = "Selected answer: "+text;
            l.AutoSize = true;
            //flowLayoutPanel1.Controls.Add(l);
            return l;
        }

        private Label getCorrectAnswer(String text)
        {
            Label l = new Label();
            l.Font = new Font(label1.Font.FontFamily, 10);
            l.Text = "Correct answer: " + text;
            l.AutoSize = true;
            //flowLayoutPanel1.Controls.Add(l);
            return l;
        }

        private Label addFeedbackText(String text)
        {
            Label l = new Label();
            l.Font = new Font(label1.Font.FontFamily, 10);
            l.Text = "Additional feedback: "+text;
            l.AutoSize = true;
            return l;
        }

        private FlowLayoutPanel getPanel()
        {
            FlowLayoutPanel panel = new FlowLayoutPanel();
            panel.Size = new Size(600, 100);
            panel.FlowDirection = FlowDirection.TopDown;
            panel.AutoSize = true;
            panel.WrapContents = false;

            return panel;
        }
    }
}
