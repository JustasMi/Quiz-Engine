using MySql.Data.MySqlClient;
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
    public partial class Form1 : Form
    {
        // TUTORIAL https://dev.mysql.com/doc/connector-net/en/connector-net-tutorials-sql-command.html
        String myConnectionString = "server=127.0.0.1;uid=root;pwd=justas;database=mydb;";
        public Form1(String userName)
        {
            InitializeComponent();
            titleLabel.Text = "Hello, "+userName+".\nWelcome to this AMAZING Quiz Engine!";
        }

        private void button1_Click(object sender, EventArgs e)
        {
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
    }
}
