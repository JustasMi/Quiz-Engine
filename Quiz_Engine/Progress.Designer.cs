namespace Quiz_Engine
{
    partial class Progress
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.easy_label = new System.Windows.Forms.Label();
            this.intermediate_label = new System.Windows.Forms.Label();
            this.hard_label = new System.Windows.Forms.Label();
            this.bookwork_label = new System.Windows.Forms.Label();
            this.application_label = new System.Windows.Forms.Label();
            this.background_label = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "{User}  Progress";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 120);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(225, 23);
            this.progressBar1.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 104);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "label2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(244, 129);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "label3";
            // 
            // easy_label
            // 
            this.easy_label.AutoSize = true;
            this.easy_label.Location = new System.Drawing.Point(9, 155);
            this.easy_label.Name = "easy_label";
            this.easy_label.Size = new System.Drawing.Size(30, 13);
            this.easy_label.TabIndex = 4;
            this.easy_label.Text = "Easy";
            // 
            // intermediate_label
            // 
            this.intermediate_label.AutoSize = true;
            this.intermediate_label.Location = new System.Drawing.Point(9, 178);
            this.intermediate_label.Name = "intermediate_label";
            this.intermediate_label.Size = new System.Drawing.Size(65, 13);
            this.intermediate_label.TabIndex = 5;
            this.intermediate_label.Text = "Intermediate";
            // 
            // hard_label
            // 
            this.hard_label.AutoSize = true;
            this.hard_label.Location = new System.Drawing.Point(9, 202);
            this.hard_label.Name = "hard_label";
            this.hard_label.Size = new System.Drawing.Size(30, 13);
            this.hard_label.TabIndex = 6;
            this.hard_label.Text = "Hard";
            // 
            // bookwork_label
            // 
            this.bookwork_label.AutoSize = true;
            this.bookwork_label.Location = new System.Drawing.Point(153, 202);
            this.bookwork_label.Name = "bookwork_label";
            this.bookwork_label.Size = new System.Drawing.Size(55, 13);
            this.bookwork_label.TabIndex = 9;
            this.bookwork_label.Text = "Bookwork";
            // 
            // application_label
            // 
            this.application_label.AutoSize = true;
            this.application_label.Location = new System.Drawing.Point(153, 178);
            this.application_label.Name = "application_label";
            this.application_label.Size = new System.Drawing.Size(59, 13);
            this.application_label.TabIndex = 8;
            this.application_label.Text = "Application";
            // 
            // background_label
            // 
            this.background_label.AutoSize = true;
            this.background_label.Location = new System.Drawing.Point(153, 155);
            this.background_label.Name = "background_label";
            this.background_label.Size = new System.Drawing.Size(65, 13);
            this.background_label.TabIndex = 7;
            this.background_label.Text = "Background";
            // 
            // Progress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(423, 289);
            this.Controls.Add(this.bookwork_label);
            this.Controls.Add(this.application_label);
            this.Controls.Add(this.background_label);
            this.Controls.Add(this.hard_label);
            this.Controls.Add(this.intermediate_label);
            this.Controls.Add(this.easy_label);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.label1);
            this.Name = "Progress";
            this.Text = "Progress";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label easy_label;
        private System.Windows.Forms.Label intermediate_label;
        private System.Windows.Forms.Label hard_label;
        private System.Windows.Forms.Label bookwork_label;
        private System.Windows.Forms.Label application_label;
        private System.Windows.Forms.Label background_label;
    }
}