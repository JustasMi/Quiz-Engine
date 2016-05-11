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
        public Feedback(List<Question> questions)
        {
            InitializeComponent();
            this.questions = questions;
            setupFeedback();
        }

        private void setupFeedback()
        {
            // Feedback for every question
            for (int i = 0; i < questions.Count; i++ )
            {
                FlowLayoutPanel panel = new FlowLayoutPanel();
                panel.Size = new Size(600, 100);
                panel.FlowDirection = FlowDirection.TopDown;
                panel.AutoSize = true;
                panel.WrapContents = false;

                panel.Controls.Add(getQuestionHeading("Question "+(i+1)));
                panel.Controls.Add(getQuestionText(questions[i].QuestionText));
                panel.Controls.Add(getQuestionText("Answered correctly: "+questions[i].isAnsweredCorrectly().ToString()));
                panel.Controls.Add(getSelectedAnswer(questions[i].getSelectedAnswer()));
                panel.Controls.Add(getCorrectAnswer(questions[i].getCorrectAnswer()));
                panel.Controls.Add(addFeedbackText(questions[i].Feedback));
                flowLayoutPanel1.Controls.Add(panel);
            }
            // Overall feedback
            Dictionary<string,List<Question>> dic = new Dictionary<string,List<Question>>();

            /*
            for (int i = 0; i < questions.Count;i++ )
            {
                if (dic.ContainsKey(questions[i].Topic.Name))
                {
                    //List<Question> list = dic[q.Topic.Name];
                    //list.Add(q);
                    // dic.Remove(q.Topic.Name);
                    //dic.Add(q.Topic.Name, list);
                }
                else
                {
                    List<Question> list = new List<Question>();
                    list.Add(questions[i]);
                    dic.Add(questions[i].Topic.Name, list);
                }
            }
             */

            
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
                FlowLayoutPanel panel = getPanel();
                panel.Controls.Add(getQuestionHeading("Topic '" + key + "' summary"));
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
                    panel.Controls.Add(getQuestionText(naturePair.Key + " : "+naturePair.Value.Where(q => q.isAnsweredCorrectly()).Count()+"/" + naturePair.Value.Count + " are correct"));
                }

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
            //createQuestionHeading("Question 1");
            //createQuestionText("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.");

            //createSelectedAnswer("Answer 2");
            //createCorrectAnswer("Answer 1");
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
