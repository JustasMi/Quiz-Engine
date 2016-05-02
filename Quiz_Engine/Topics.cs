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
    public partial class Topics : Form
    {
        DButility db = new DButility();
        public Topics()
        {
            InitializeComponent();
        }

        private void Topics_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'mydbDataSet1.subjects' table. You can move, or remove it, as needed.
            this.subjectsTableAdapter.Fill(this.mydbDataSet1.subjects);
            // TODO: This line of code loads data into the 'mydbDataSet1.topics' table. You can move, or remove it, as needed.
            this.topicsTableAdapter.Fill(this.mydbDataSet1.topics);

        }

        // Add topic
        private void button1_Click(object sender, EventArgs e)
        {
            db.addTopic(newTopicBox.Text, Int32.Parse(comboBox1.SelectedValue.ToString()));
            newTopicBox.Clear();
            this.topicsTableAdapter.Fill(this.mydbDataSet1.topics);
        }

        // Add subject
        private void button2_Click(object sender, EventArgs e)
        {
            db.addSubject(newSubjectTextBox.Text);
            newSubjectTextBox.Clear();
            this.subjectsTableAdapter.Fill(this.mydbDataSet1.subjects);
        }
    }
}
