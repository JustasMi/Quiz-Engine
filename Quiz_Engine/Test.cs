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
    public partial class Test : Form
    {
        // Connection to database
        private DButility db = new DButility();
        // Quiz parameters
        private int questionLimit = 20;
        private List<Topic> topics;
        private List<Question> questions = new List<Question>();
        private int displayedQuestionIndex = 0;
        private User currentUser;
        
        public Test(List<Topic> topics, User user)
        {
            InitializeComponent();
            this.topics = topics;
            this.currentUser = user;


            prepareQuestions();
            prepareUi();
        }

        private void prepareUi()
        {
            string topicsLabel = "";

            foreach (Topic t in topics)
            {
                topicsLabel += t.Name + ", ";
            }
            toolStripLabel2.Text = topicsLabel.Remove(topicsLabel.Length-2);

            toolStripProgressBar1.Minimum = 0;
            toolStripProgressBar1.Maximum = questions.Count;
            toolStripProgressBar1.Value = 0;
            toolStripProgressBar1.Step = 1;

            toolStripUserLabel.Text = currentUser.Name;
        }

        private void prepareQuestions()
        {
            int questionAmount = (int) Math.Ceiling((double)questionLimit / (double)topics.Count);
            
            foreach (Topic t in topics)
            {
                t.Questions = db.getQuestions(t, questionAmount);
                questions.AddRange(t.Questions);
            }

            if (questions.Count == 0)
            {
                MessageBox.Show("There were no questions found for selected topics. Please close the form and try again with other topics");
            }
            else
            {
                // Display first question
                displayQuestion(displayedQuestionIndex);
            }
        }

        private void displayQuestion(int index)
        {
            // Set question
            textBox1.Text = questions[index].QuestionText;
            // Set topic
            textBox2.Text = questions[index].Topic.Name;
            // Set up answers

            // If answers are not retrieved from DB, get them
            if (questions[index].Answers == null)
            {
                questions[index].Answers = db.getAnswers(questions[index].Id);
            }

            listBox1.DataSource = questions[index].Answers;
            listBox1.DisplayMember = "AnswerText";
            //listBox1.ValueMember = ""; //

            // Set up selected answers
            listBox1.ClearSelected();
            foreach (Answer a in questions[index].Answers)
            {
                if (a.Selected)
                {
                    listBox1.SelectedItem = a;
                }
            }

            textBox3.Text = (displayedQuestionIndex+1)+"/"+questions.Count;
        }

        private void next_Button_Click(object sender, EventArgs e)
        {
            if (displayedQuestionIndex < questions.Count - 1)
            {
                displayedQuestionIndex += 1;
                displayQuestion(displayedQuestionIndex);
            }
        }

        private void previous_Button_Click(object sender, EventArgs e)
        {
            if (displayedQuestionIndex > 0)
            {
                displayedQuestionIndex -= 1;
                displayQuestion(displayedQuestionIndex);
            }
        }

        private void listBox1_Click(object sender, EventArgs e)
        {
            ListBox lb = (ListBox) sender;
            if (lb.SelectedItem != null)
            {
                Answer a = (Answer)lb.SelectedItem;
                a.Selected = true;
                toolStripProgressBar1.PerformStep();
            }
        }

        private void submit_Button_Click(object sender, EventArgs e)
        {
            db.addQuizResult(currentUser, questions, DateTime.Now);
            //System.Diagnostics.Debug.WriteLine(DateTime.Now.Date);
        }

    }
}
