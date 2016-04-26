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
        DButility db = new DButility();

        public Main(String userName)
        {
            InitializeComponent();
            titleLabel.Text = "Hello, "+userName+".\nWelcome to this AMAZING Quiz Engine!";
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
            List<Topic> checkedTopics = new List<Topic>();

            foreach (object o in checkedListBox1.CheckedItems)
            {
                checkedTopics.Add((Topic) o);
            }


            this.Hide();
            Form form = new Test(checkedTopics);
            form.Closed += (s, args) => this.Show();
            form.Show();
        }
    }
}
