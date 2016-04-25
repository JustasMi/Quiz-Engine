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

        // Continue button
        private void button1_Click(object sender, EventArgs e)
        {
            goToMainForm(listBox1.GetItemText(listBox1.SelectedItem));
        }

        private void Login_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'mydbDataSet1.users' table. You can move, or remove it, as needed.
            this.usersTableAdapter.Fill(this.mydbDataSet1.users);

        }

        // Add new User
        private void button2_Click(object sender, EventArgs e)
        {
            db.addUser(textBox1.Text);
            goToMainForm(textBox1.Text);
        }

        private void goToMainForm(String userName)
        {
            //this.Close();
            //Form main = new Form1(userName);
            //main.Show();

            this.Hide();
            Form form = new Form1(userName);
            form.Closed += (s, args) => this.Close();
            form.Show();
        }
    }
}
