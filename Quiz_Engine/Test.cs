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
        private List<Topic> topics;
        
        public Test(List<Topic> topics)
        {
            InitializeComponent();

            this.topics = topics;

            string topicsLabel = "";

            foreach (Topic t in topics)
            {
                topicsLabel += t.Name+", ";
            }
            toolStripLabel2.Text = topicsLabel;
        }


    }
}
