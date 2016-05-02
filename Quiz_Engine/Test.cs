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

        // Variables for quiz retaking
        private bool retakingQuiz = false;
        private Quiz quiz;
        
        // Constructor for taking new tests
        public Test(List<Topic> topics, User user)
        {
            InitializeComponent();
            this.topics = topics;
            this.currentUser = user;


            prepareQuestions();
            prepareUi();
        }

        // Constructor for retaking tests
        public Test(Quiz quiz, User user)
        {
            InitializeComponent();
            this.currentUser = user;
            this.topics = db.getQuizTopics(quiz.Id);
            retakingQuiz = true;
            this.quiz = quiz;

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
            List<Question> questionList = new List<Question>();
            if (retakingQuiz)
            {
                // When retaking past quiz
                questionList.AddRange(db.getQuizQuestions(quiz.Id));
            }
            else
            {
                // Generating a new quiz
                int questionAmount = (int)Math.Ceiling((double)questionLimit / (double)topics.Count);

                foreach (Topic t in topics)
                {
                    // get questions for every topic
                    questionList.AddRange(db.getQuestions(t, questionAmount));
                }
            }

            // preload answers to questions
            foreach (Question q in questionList)
            {
                q.Answers = db.getAnswers(q.Id);

                /*
                if (q.QuestionType == Quiz_Engine.Properties.Resources.trueFalse)
                {
                    Answer a = q.Answers[0];
                    List<Answer> ans = new List<Answer>();
                    ans.Add(new Answer("True", a.Correct));
                    ans.Add(new Answer("False", !a.Correct));
                    q.Answers = ans;
                }
                 */
            }
            questions.AddRange(questionList);

            if (questions.Count == 0)
            {
                MessageBox.Show("There were no questions found for selected topics.");
                Environment.Exit(0);
            }
            else
            {
                // Display first question
                displayQuestion(displayedQuestionIndex);
            }
        }

        private void displayQuestion(int index)
        {
            hideAnswerPanels();
            // Set question
            textBox1.Text = questions[index].QuestionText;
            // Set topic
            textBox2.Text = questions[index].Topic.Name;
            // Set type
            textBox4.Text = questions[index].QuestionType;
            // Set difficulty
            textBox5.Text = questions[index].Difficulty;
            // Set nature
            textBox6.Text = questions[index].Nature;

            // Set up answers
            if (questions[index].QuestionType == Quiz_Engine.Properties.Resources.multipleChoice || questions[index].QuestionType == Quiz_Engine.Properties.Resources.multipleAnswer)
            {
                // Bind answers to the box
                listBox1.DataSource = questions[index].Answers;
                listBox1.DisplayMember = "AnswerText";

                // Clear answer selections
                listBox1.ClearSelected();

                // Set up answers box
                if (questions[index].QuestionType == Quiz_Engine.Properties.Resources.multipleChoice)
                {
                    listBox1.SelectionMode = SelectionMode.One;

                    // Set previous selections
                    foreach (Answer a in questions[index].Answers)
                    {
                        
                        if (a.Selected)
                        {
                            listBox1.SelectedItem = a;
                        }
                         
                    }
                }
                else
                {
                    listBox1.SelectionMode = SelectionMode.MultiSimple;
                    
                    // Set the previous selections
                    for (int i = 0; i < listBox1.Items.Count; i++)
                    {
                        listBox1.SetSelected(i, ((Answer)listBox1.Items[i]).Selected);
                    }
                }
                panel2.Visible = true;
                panel2.Location = new Point(116, 355);

            }
            else if (questions[index].QuestionType == Quiz_Engine.Properties.Resources.trueFalse)
            {
                radioButton1.Checked = false;
                radioButton2.Checked = false;
                foreach (Answer a in questions[index].Answers)
                {
                    if (a.Selected)
                    {
                        if (a.AnswerText == "True")
                        {
                            radioButton1.Checked = true;
                        }
                        else
                        {
                            radioButton2.Checked = true;
                        }
                    }
                }                

                panel1.Visible = true;
                panel1.Location = new Point(116, 355);
            }
            else if (questions[index].QuestionType == Quiz_Engine.Properties.Resources.fillInTheAnswer)
            {
                panel3.Visible = true;
                panel3.Location = new Point(116, 355);
            }

            // Show question number
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


            for (int i = 0; i < lb.Items.Count; i++)
            {
                ((Answer)lb.Items[i]).Selected = lb.GetSelected(i);
                // bool selected = lb.GetSelected(i);
            }
        }

        private void submit_Button_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("You have answered X out of Y questions, do you want to submit the results?(If retaking test, results won't be added)", "Confirmation", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                if (!retakingQuiz)
                {
                    db.addQuizResult(currentUser, questions, toolStripLabel2.Text);
                }                
                this.Close();
            }
            else if (dialogResult == DialogResult.No)
            {
                this.Close();
            }            
        }

        private void hideAnswerPanels()
        {
            panel1.Visible = false;
            panel2.Visible = false;
            panel3.Visible = false;
        }
        /*
        private void trueFalse_Text_Changed(object sender, EventArgs e)
        {
            
            ComboBox cb = (ComboBox)sender;
            
            if (cb.SelectedItem != null)
            {
                foreach (object o in cb.Items)
                {
                    Answer a = (Answer)o;

                    if ((Answer)cb.SelectedItem == a)
                    {
                        System.Diagnostics.Debug.WriteLine("Selected item found");
                        a.Selected = true;
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Selected item not found");
                        a.Selected = false;
                    }

                }
            }
            */

            /*
            if (cb.SelectedItem != null)
            {
                Answer a = (Answer)cb.SelectedItem;
                a.Selected = true;
            }
             
        }
             */


        private void radioButton2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Checked Changed!");

            RadioButton source = (RadioButton)sender;

            foreach (Answer a in questions[displayedQuestionIndex].Answers)
            {
                if (a.AnswerText == "True")
                {
                    a.Selected = radioButton1.Checked;
                }
                else
                {
                    a.Selected = radioButton2.Checked;
                }
            }
        }
    }
}
