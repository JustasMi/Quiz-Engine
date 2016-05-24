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

        QuizPreference preferences;
        
        // Constructor for taking new tests
        public Test(List<Topic> topics, User user)
        {
            InitializeComponent();
            this.topics = topics;
            this.currentUser = user;


            prepareQuestions(false);
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

            prepareQuestions(false);
            prepareUi();
        }
        
        //List<Topic> selectedTopics
        public Test(List<Question> questions, QuizPreference preferences, User currentUser)
        {
            InitializeComponent();
            this.questions = questions;
            this.topics = preferences.Topics;
            this.currentUser = currentUser;
            this.preferences = preferences;
            prepareQuestions(true);
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

            if (preferences != null)
            {
                if (!preferences.Help)
                {
                    menuStrip1.Visible = false;
                }
            }
            /*
            toolStripProgressBar1.Minimum = 0;
            toolStripProgressBar1.Maximum = questions.Count;
            toolStripProgressBar1.Value = 0;
            toolStripProgressBar1.Step = 1;
            */
            toolStripUserLabel.Text = currentUser.Name;
        }

        private void prepareQuestions(bool questionsInitialized)
        {
            if (!questionsInitialized)
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
                questions.AddRange(questionList);
            }

            // preload answers to questions
            foreach (Question q in questions)
            {
                q.Answers = db.getAnswers(q.Id);
            }


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
                textBox7.Text = questions[displayedQuestionIndex].Answers[0].TypedAnswer;
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
            DialogResult dialogResult = MessageBox.Show("Would you like to submit the results?(If retaking test, results won't be added)", "Confirmation", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                if (!retakingQuiz)
                {
                    db.addQuizResult(currentUser, questions, toolStripLabel2.Text);
                }                
                //this.Close();
            }
            else if (dialogResult == DialogResult.No)
            {
                //this.Close();
            }

            // SHOW FEEDBACK SCREEN

            this.Hide();

            Form form; 
            if (retakingQuiz)
            {
                form = new Feedback(questions, retakingQuiz, quiz);
            }
            else
            {
                form = new Feedback(questions, retakingQuiz);
            }
            //Form form = new Feedback(questions, retakingQuiz);
            form.Closed += (s, args) => this.Close();
            form.Show();

        }

        private void hideAnswerPanels()
        {
            panel1.Visible = false;
            panel2.Visible = false;
            panel3.Visible = false;
        }

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

        // Save answer whenever the text is chagned
        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            if ( ((TextBox) sender).Text != String.Empty)
                questions[displayedQuestionIndex].Answers[0].TypedAnswer = ((TextBox)sender).Text;

            if (questions[displayedQuestionIndex].Answers[0].TypedAnswer != String.Empty)
            {
                questions[displayedQuestionIndex].Answers[0].Selected = true;
            }
            else
            {
                questions[displayedQuestionIndex].Answers[0].Selected = false;
            }
        }

        /*
        // Give help if user asks
        private void feedback_Click(object sender, EventArgs e)
        {
            String selectedAnswer = "";
            String correctAnswer = "";
            bool answeredCorrectly = false;
            if (questions[displayedQuestionIndex].QuestionType == Quiz_Engine.Properties.Resources.trueFalse || questions[displayedQuestionIndex].QuestionType == Quiz_Engine.Properties.Resources.multipleChoice)
            {
                foreach (Answer a in questions[displayedQuestionIndex].Answers)
                {
                    if (a.Selected)
                    {
                        selectedAnswer = a.AnswerText;
                    }
                    if (a.Correct)
                    {
                        correctAnswer = a.AnswerText;
                    }
                    if (a.Selected && a.Correct)
                    {
                        answeredCorrectly = true;
                    }
                }
            }
            else if (questions[displayedQuestionIndex].QuestionType == Quiz_Engine.Properties.Resources.fillInTheAnswer)
            {
                if (questions[displayedQuestionIndex].Answers[0].TypedAnswer.Equals(questions[displayedQuestionIndex].Answers[0].AnswerText, StringComparison.InvariantCultureIgnoreCase))
                {
                    answeredCorrectly = true;
                }
                selectedAnswer = questions[displayedQuestionIndex].Answers[0].TypedAnswer;
                correctAnswer = questions[displayedQuestionIndex].Answers[0].AnswerText;
            }
            else if (questions[displayedQuestionIndex].QuestionType == Quiz_Engine.Properties.Resources.multipleAnswer)
            {
                int correctAnswerCount = 0;
                foreach (Answer a in questions[displayedQuestionIndex].Answers)
                {
                    if ((a.Correct && a.Selected) || (!a.Correct && !a.Selected))
                        correctAnswerCount += 1;
                }

                if (correctAnswerCount == questions[displayedQuestionIndex].Answers.Count)
                {
                    answeredCorrectly = true;
                }
                selectedAnswer = questions[displayedQuestionIndex].getSelectedAnswer();
                correctAnswer = questions[displayedQuestionIndex].getCorrectAnswer();
            }
            MessageBox.Show("Quesstion feedback: " + questions[displayedQuestionIndex].Feedback + "\nQuestion correctness: " + answeredCorrectly + "\nSelected answer is: " + selectedAnswer + "\nCorrect answer is: " + correctAnswer);
        }
        */

        // Show Feedback
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(questions[displayedQuestionIndex].Feedback))
            {
                MessageBox.Show("There is no feedback attached to the question");
            }
            else
            {
                MessageBox.Show(questions[displayedQuestionIndex].Feedback);
            }            
        }

        // Show the correct answer
        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Correct answer: " +questions[displayedQuestionIndex].getCorrectAnswer());
        }

        // Check if answer is correct
        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            bool answeredCorrectly = false;
            if (questions[displayedQuestionIndex].QuestionType == Quiz_Engine.Properties.Resources.trueFalse || questions[displayedQuestionIndex].QuestionType == Quiz_Engine.Properties.Resources.multipleChoice)
            {
                foreach (Answer a in questions[displayedQuestionIndex].Answers)
                {
                    if (a.Selected && a.Correct)
                    {
                        answeredCorrectly = true;
                    }
                }
            }
            else if (questions[displayedQuestionIndex].QuestionType == Quiz_Engine.Properties.Resources.fillInTheAnswer)
            {
                if (questions[displayedQuestionIndex].Answers[0].TypedAnswer.Equals(questions[displayedQuestionIndex].Answers[0].AnswerText, StringComparison.InvariantCultureIgnoreCase))
                {
                    answeredCorrectly = true;
                }
            }
            else if (questions[displayedQuestionIndex].QuestionType == Quiz_Engine.Properties.Resources.multipleAnswer)
            {
                int correctAnswerCount = 0;
                foreach (Answer a in questions[displayedQuestionIndex].Answers)
                {
                    if ((a.Correct && a.Selected) || (!a.Correct && !a.Selected))
                        correctAnswerCount += 1;
                }

                if (correctAnswerCount == questions[displayedQuestionIndex].Answers.Count)
                {
                    answeredCorrectly = true;
                }
            }
            MessageBox.Show("Correct answer provided: " + answeredCorrectly);
        }
    }
}
