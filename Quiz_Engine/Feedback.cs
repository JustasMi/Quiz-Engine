﻿using Quiz_Engine.Classes;
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
                flowLayoutPanel1.Controls.Add(panel);

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

                // Generate feedback for nature
                // Nature types 'Application', 'Bookwork', 'Background'
                //Dictionary<string, List<String>> recommendation = new Dictionary<string, List<String>>();
                List<String> recommendation = new List<String>();
                List<String> priorityRecommendation = new List<String>();
                bool flag = false;
                if (natureDic.ContainsKey("Application") && natureDic.ContainsKey("Bookwork"))
                {
                    int bookWorkPercentage = (100 * natureDic["Bookwork"].Where(q => q.isAnsweredCorrectly()).Count()) / natureDic["Bookwork"].Count;
                    int applicationPercentage = (100 * natureDic["Application"].Where(q => q.isAnsweredCorrectly()).Count()) / natureDic["Application"].Count;
                    double coefficient = 1.3;
                    if (bookWorkPercentage > applicationPercentage * coefficient)
                    {
                        // if Bookwork higher (by a third)than application
                        priorityRecommendation.Add("Bookwork");
                        flag = true;
                    }
                    else if (bookWorkPercentage * coefficient < applicationPercentage)
                    {
                        // if Application (by a third) higher than bookwork
                        priorityRecommendation.Add("Application");
                        flag = true;
                    }
                    else
                    {
                        // Check which ones to study if ANY below 80%
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
                    }
                }
                else
                {
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
                }
                
                // Display nature feedback
                panel = getPanel();
                if (flag)
                {
                    if (priorityRecommendation[0] == "Application")
                        panel.Controls.Add(getQuestionText("Recommendations: Priority. Score suggests that you know the theory, but when required to apply it, the score is considerably worse."));
                    else
                        panel.Controls.Add(getQuestionText("Recommendations: Priority. Scores suggests that you are performing better in applying your knowledge, but you lack theoretical knowledge."));
                }
                else
                {
                    string rec = "Recommendations: ";
                    if (priorityRecommendation.Count > 0)
                    {
                        rec += "Priority. Suggested revision: ";
                        foreach (String s in priorityRecommendation)
                        {
                            rec += s+", ";
                        }
                        rec = rec.Remove(rec.Length - 2);
                    }

                    if (recommendation.Count > 0)
                    {
                        if ( priorityRecommendation.Count > 0)
                        {
                            rec += "\nRevision should include: ";
                        }

                        foreach (String s in recommendation)
                        {
                            rec += s + ", ";
                        }
                        rec = rec.Remove(rec.Length - 2);
                        rec += " types of questions.";
                    }
                    panel.Controls.Add(getQuestionText(rec));
                }
                flowLayoutPanel1.Controls.Add(panel);

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
                    panel.Controls.Add(getQuestionText(difficultyPair.Key + " : " + difficultyPair.Value.Where(q => q.isAnsweredCorrectly()).Count() + "/" + difficultyPair.Value.Count + " are correct"));
                }

                flowLayoutPanel1.Controls.Add(panel);
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
