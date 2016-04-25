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
    public partial class Questions : Form
    {
        DButility db = new DButility();

        public Questions()
        {
            InitializeComponent();
        }

        private void Questions_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'mydbDataSet1.topics' table. You can move, or remove it, as needed.
            this.topicsTableAdapter.Fill(this.mydbDataSet1.topics);

        }

        // Add questions/answers to database
        private void addButton_Click(object sender, EventArgs e)
        {
            int questionID = db.addQuestion(textBox1.Text, (int) comboBox1.SelectedValue);
            db.addAnswers(questionID, listBox1.Items.Cast<String>().ToList());
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Add(textBox2.Text);
            textBox2.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.Items.Remove(listBox1.SelectedItem);
            
            //System.Diagnostics.Debug.WriteLine(myOtherList.ToString());
        }
    }
}
