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
    public partial class Login : Form
    {
        DButility db = new DButility();

        public Login()
        {
            InitializeComponent();
        }

        /*
        private void Login_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'mydbDataSet1.users' table. You can move, or remove it, as needed.
            this.usersTableAdapter.Fill(this.mydbDataSet1.users);
            listBox1.ClearSelected();

        }
        */
        // Add new User
        private void button2_Click(object sender, EventArgs e)
        {
            int userID = db.addUser(textBox1.Text, textBox2.Text.GetHashCode());
            goToMainForm(new User(textBox1.Text, userID));
        }

        // Login button
        private void button1_Click(object sender, EventArgs e)
        {
            User user = db.verifyUser(username_textBox.Text, textBox3.Text.GetHashCode());
            if (user != null)
            //if (db.verifyUser(listBox1.SelectedValue.ToString(), textBox3.Text.GetHashCode()))
            {
                goToMainForm(user);
            }
            else
            {
                MessageBox.Show("Invalid login information was given");
            }
        }

        private void goToMainForm(User user)
        {
            this.Hide();
            Form form = new Main(user);
            form.Closed += (s, args) => this.Close();
            form.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            panel2.Hide();
            panel1.Show();
        }
    }
}
