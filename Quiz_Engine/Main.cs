using MySql.Data.MySqlClient;
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
    public partial class Main : Form
    {
        // TUTORIAL https://dev.mysql.com/doc/connector-net/en/connector-net-tutorials-sql-command.html
        //String myConnectionString = "server=127.0.0.1;uid=root;pwd=justas;database=mydb;";
        private DButility db = new DButility();
        private User currentUser;
        List<Quiz> quiz_history;

        public Main(User user)
        {
            InitializeComponent();
            currentUser = user;
            titleLabel.Text = "Hello, "+user.Name+".\nWelcome to this AMAZING Quiz Engine!";

            setUpPreviousQuizes();
        }

        private void setUpPreviousQuizes()
        {
            quiz_history = db.getQuizHistory(currentUser);

            foreach (Quiz quiz in quiz_history)
            {
                listView1.Items.Add(quiz.Topics).SubItems.AddRange(new string[] {quiz.Date.ToString(), quiz.TotalQuestions.ToString(), quiz.CorrectAnswers.ToString() });
                listView1.Items[listView1.Items.Count - 1].Tag = quiz.Id;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form form2 = new Topics();
            form2.Show();
            //childForm = CreateTheChildForm();
            //childForm.MoreClick += More_Click;
            //childForm.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form questions = new Questions();
            questions.Show();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'mydbDataSet1.subjects' table. You can move, or remove it, as needed.
            this.subjectsTableAdapter.Fill(this.mydbDataSet1.subjects);
            //updateTopics();
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;

            for (int i=0; i < checkedListBox3.Items.Count; i++)
            {
                checkedListBox3.SetItemChecked(i, true);
            }

            for (int i = 0; i < checkedListBox4.Items.Count; i++)
            {
                checkedListBox4.SetItemChecked(i, true);
            }
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            updateTopics(cb);
        }

        private void updateTopics(ComboBox cb)
        {
            List<Topic> topics = new List<Topic>();

            if (cb.SelectedValue != null)
                topics = db.getTopics(Int32.Parse(cb.SelectedValue.ToString()));

            if (cb.Tag.ToString().Equals("Fast"))
            {
                this.checkedListBox1.DataSource = topics;
                this.checkedListBox1.DisplayMember = "name";
                this.checkedListBox1.ValueMember = "id";
            }
            else
            {
                this.checkedListBox2.DataSource = topics;
                this.checkedListBox2.DisplayMember = "name";
                this.checkedListBox2.ValueMember = "id";
            }

        }

        // Take fast Quiz
        private void button4_Click(object sender, EventArgs e)
        {
            List<Topic> selectedTopics = new List<Topic>();

            foreach (object o in checkedListBox1.CheckedItems)
            {
                selectedTopics.Add((Topic) o);
            }

            this.Hide();
            Form form = new Test(selectedTopics, currentUser);
            form.Closed += (s, args) => this.Show();
            form.Show();
        }

        // Create custom Quiz
        private void button6_Click(object sender, EventArgs e)
        {
            QuizPreference preferences = new QuizPreference();
            preferences.Easy = checkedListBox3.GetItemChecked(0);
            preferences.Intermediate = checkedListBox3.GetItemChecked(1);
            preferences.Hard = checkedListBox3.GetItemChecked(2);

            preferences.Bookwork = checkedListBox4.GetItemChecked(0);
            preferences.Background = checkedListBox4.GetItemChecked(1);
            preferences.Application = checkedListBox4.GetItemChecked(2);

            preferences.QuizSize = Int32.Parse(textBox1.Text);

            List<Topic> selectedTopics = new List<Topic>();

            foreach (object o in checkedListBox2.CheckedItems)
            {
                selectedTopics.Add((Topic)o);
            }

            preferences.Topics = selectedTopics;
            preferences.Feedback = checkBox1.Checked;
            preferences.Summary = checkBox2.Checked;

            List<Question> questions = db.getQuestionsFromPreferences(preferences);
            
            this.Hide();
            Form form = new Test(questions, preferences, currentUser);
            //Form form = new Test(selectedTopics, currentUser);
            form.Closed += (s, args) => this.Show();
            form.Show();
        }

        // Take past quiz
        private void testRetake_Button_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                Quiz quiz = quiz_history.Find(q => q.Id == (int) listView1.SelectedItems[0].Tag);

                this.Hide();
                Form form = new Test(quiz, currentUser);
                form.Closed += (s, args) => this.Show();
                form.Show();
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Nothing is selected");
            }
        }
    }
}
