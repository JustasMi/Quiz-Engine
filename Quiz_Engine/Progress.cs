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
    public partial class Progress : Form
    {
        private User currentUser;
        private List<Topic> selectedTopics;
        private DButility db = new DButility();

        public Progress(User currentUser, List<Topic> list)
        {
            InitializeComponent();
            this.currentUser = currentUser;
            this.selectedTopics = list;
            initializeProgress();
            label1.Text = currentUser.Name + " progress";
        }

        private void initializeProgress()
        {
            TopicProgress progress = db.getProgress(selectedTopics[0], currentUser);
            label2.Text = "Topic: " + selectedTopics[0].Name;
            if (progress.Total != 0)
            {
                label3.Text = progress.Correct + "/" + progress.Total + " (" + (progress.Correct * 100) / progress.Total + "%) correct";
            }
            else
            {
                label3.Text = progress.Correct + "/" + progress.Total + " (0%) correct";
            }

            progressBar1.Maximum = progress.Total;
            progressBar1.Step = progress.Correct;
            progressBar1.Value = progress.Correct;

            easy_label.Text = formatPropertyString("Easy", progress);
            intermediate_label.Text = formatPropertyString("Intermediate", progress);
            hard_label.Text = formatPropertyString("Hard", progress);

            application_label.Text = formatPropertyString("Application", progress);
            background_label.Text = formatPropertyString("Background", progress);
            bookwork_label.Text = formatPropertyString("Bookwork", progress);

        }

        private string formatPropertyString(string propertyName, TopicProgress progress)
        {
            string returnStrng = propertyName+": N/A";
            if (progress.Properties.ContainsKey(propertyName))
            {
                if (progress.Properties[propertyName][0] != 0)
                    returnStrng = propertyName + ": " + progress.Properties[propertyName][1] + "/" + progress.Properties[propertyName][0] + " ("+(progress.Properties[propertyName][1] * 100) / progress.Properties[propertyName][0]+" %)";
            }

            return returnStrng;
        }

    }
}
