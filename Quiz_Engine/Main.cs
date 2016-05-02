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
            //listView1.Groups.Add(new ListViewGroup("New group?", HorizontalAlignment.Left));
            //listView1.Items[listView1.Items.Count - 1].Group = listView1.Groups[0];
            //listView1.Items[listView1.Items.Count - 2].Group = listView1.Groups[0];
            //listView1.Items.Add("lol").SubItems.AddRange(new string[] { "lol", "lol", "lol" });
            //listView1.Items[2].add
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*
            MySqlConnection conn;

            String sql1 = "INSERT INTO test VALUES(7,\"Wohoo c# 3\");";
            String sql2 = "SELECT * FROM test";
            String sql3 = "INSERT INTO topics (name) VALUES('Hyo');";
            try
            {
                conn = new MySqlConnection();
                conn.ConnectionString = myConnectionString;
                conn.Open();

                MySqlCommand command = new MySqlCommand(sql3, conn);
                
                MySqlDataReader rdr = command.ExecuteReader();

                while (rdr.Read())
                {
                    System.Diagnostics.Debug.WriteLine(rdr[0] + " -- " + rdr[1]);
                }
                rdr.Close();

                //command.ExecuteNonQuery();
                conn.Close();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
             * */


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
            updateTopics();
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            updateTopics();
        }

        private void updateTopics()
        {
            List<Topic> topics = new List<Topic>();

            if (comboBox1.SelectedValue != null)
                topics = db.getTopics(Int32.Parse(comboBox1.SelectedValue.ToString()));

            this.checkedListBox1.DataSource = topics;
            this.checkedListBox1.DisplayMember = "name";
            this.checkedListBox1.ValueMember = "id";
        }

        // Take test
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

        private void testRetake_Button_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                Quiz quiz = quiz_history.Find(q => q.Id == (int) listView1.SelectedItems[0].Tag);
                //System.Diagnostics.Debug.WriteLine("Selected item TAG: " + listView1.SelectedItems[0].Tag);

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
