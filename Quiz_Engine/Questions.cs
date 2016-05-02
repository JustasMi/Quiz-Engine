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
    public partial class Questions : Form
    {
        private DButility db = new DButility();

        public enum Question_Type { MultipleChoice, MultipleAnswers, TrueFalse, FillIntheAnswer };

        public Questions()
        {
            InitializeComponent();
        }

        private void Questions_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'mydbDataSet11.subjects' table. You can move, or remove it, as needed.
            this.subjectsTableAdapter.Fill(this.mydbDataSet11.subjects);
            // TODO: This line of code loads data into the 'mydbDataSet1.question_types' table. You can move, or remove it, as needed.
            this.question_typesTableAdapter.Fill(this.mydbDataSet1.question_types);
            // TODO: This line of code loads data into the 'mydbDataSet1.topics' table. You can move, or remove it, as needed.
            this.topicsTableAdapter.Fill(this.mydbDataSet1.topics);

            comboBox3.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
        }

        // Add questions/answers to database
        private void addButton_Click(object sender, EventArgs e)
        {
            // question text, topic ID, type, difficulty, nature, feedback
            int questionID = db.addQuestion(textBox1.Text, (int) comboBox1.SelectedValue, comboBox2.Text, comboBox4.Text, comboBox5.Text, textBox4.Text);

            List<Answer> answers = new List<Answer>();
            foreach (var s in checkedListBox1.Items)
            {
                CheckState st = checkedListBox1.GetItemCheckState(checkedListBox1.Items.IndexOf(s));
                if (st == CheckState.Checked)
                {
                    answers.Add(new Answer(s.ToString(), true));
                }
                else
                {
                    answers.Add(new Answer(s.ToString(), false));
                }
            }

            // Add answers to DB differently, based on the type
            if (comboBox2.Text == Quiz_Engine.Properties.Resources.multipleChoice)
            {
                db.addAnswers(questionID, answers);
            }
            else if (comboBox2.Text == Quiz_Engine.Properties.Resources.multipleAnswer)
            {
                db.addAnswers(questionID, answers);
            }
            else if (comboBox2.Text == Quiz_Engine.Properties.Resources.trueFalse)
            {
                db.addTrueFalseAnswer(questionID, Convert.ToBoolean(comboBox6.Text));
            }
            else if (comboBox2.Text == Quiz_Engine.Properties.Resources.fillInTheAnswer)
            {
                db.addFillInAnswer(questionID, textBox3.Text);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            checkedListBox1.Items.Add(textBox2.Text);
            textBox2.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            checkedListBox1.Items.Remove(checkedListBox1.SelectedItem);
        }

        private void comboBox3_TextChanged(object sender, EventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("Click detected!");
            updateTopics();
            comboBox1.SelectedIndex = -1;
        }

        private void updateTopics()
        {
            List<Topic> topics = new List<Topic>();

            if (comboBox3.SelectedValue != null)
                topics = db.getTopics(Int32.Parse(comboBox3.SelectedValue.ToString()));

            this.comboBox1.DataSource = topics;
            this.comboBox1.DisplayMember = "name";
            this.comboBox1.ValueMember = "id";
        }

        private void questionType_TextChanged(object sender, EventArgs e)
        {
            hideAllPanels();
            if (comboBox2.Text == "Multiple Choice" || comboBox2.Text == "Multiple Answers")
            {
                panel1.Visible = true;
                panel1.Location = new Point(12, 426);
            }
            else if (comboBox2.Text == "True/False")
            {
                panel2.Visible = true;
                //panel2.Location = new Point(335, 53);
                panel2.Location = new Point(12, 426);
            }
            else if (comboBox2.Text == "Fill In The Answer")
            {
                panel3.Visible = true;
                //panel3.Location = new Point(335, 53);
                panel3.Location = new Point(12, 426);
            }
        }

        private void hideAllPanels()
        {
            panel1.Visible = false;
            panel2.Visible = false;
            panel3.Visible = false;
        }
    }
}
