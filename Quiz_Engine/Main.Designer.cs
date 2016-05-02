namespace Quiz_Engine
{
    partial class Main
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
            this.components = new System.ComponentModel.Container();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.titleLabel = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.topicsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.mydbDataSet1 = new Quiz_Engine.mydbDataSet1();
            this.label1 = new System.Windows.Forms.Label();
            this.topicsTableAdapter = new Quiz_Engine.mydbDataSet1TableAdapters.topicsTableAdapter();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.subjectsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.subjectsTableAdapter = new Quiz_Engine.mydbDataSet1TableAdapters.subjectsTableAdapter();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button5 = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            ((System.ComponentModel.ISupportInitialize)(this.topicsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mydbDataSet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.subjectsBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(23, 424);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Test Button";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 62);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(141, 43);
            this.button2.TabIndex = 3;
            this.button2.Text = "Add Subjects/Topics";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(12, 111);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(141, 43);
            this.button3.TabIndex = 4;
            this.button3.Text = "Add Questions";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleLabel.Location = new System.Drawing.Point(178, 13);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(161, 22);
            this.titleLabel.TabIndex = 5;
            this.titleLabel.Text = "Welcome Message";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(181, 379);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(141, 43);
            this.button4.TabIndex = 6;
            this.button4.Text = "Take Test";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // topicsBindingSource
            // 
            this.topicsBindingSource.DataMember = "topics";
            this.topicsBindingSource.DataSource = this.mydbDataSet1;
            // 
            // mydbDataSet1
            // 
            this.mydbDataSet1.DataSetName = "mydbDataSet1";
            this.mydbDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(179, 147);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Topics";
            // 
            // topicsTableAdapter
            // 
            this.topicsTableAdapter.ClearBeforeFill = true;
            // 
            // comboBox1
            // 
            this.comboBox1.DataSource = this.subjectsBindingSource;
            this.comboBox1.DisplayMember = "name";
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(181, 123);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(141, 21);
            this.comboBox1.TabIndex = 9;
            this.comboBox1.ValueMember = "id";
            this.comboBox1.TextChanged += new System.EventHandler(this.comboBox1_TextChanged);
            // 
            // subjectsBindingSource
            // 
            this.subjectsBindingSource.DataMember = "subjects";
            this.subjectsBindingSource.DataSource = this.mydbDataSet1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(178, 107);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Subjects";
            // 
            // subjectsTableAdapter
            // 
            this.subjectsTableAdapter.ClearBeforeFill = true;
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Location = new System.Drawing.Point(181, 163);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(142, 214);
            this.checkedListBox1.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(363, 141);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Your previous tests";
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(366, 379);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(406, 43);
            this.button5.TabIndex = 14;
            this.button5.Text = "Retake Test";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.testRetake_Button_Click);
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.listView1.FullRowSelect = true;
            this.listView1.Location = new System.Drawing.Point(366, 163);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(406, 214);
            this.listView1.TabIndex = 15;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Topic";
            this.columnHeader1.Width = 170;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Date";
            this.columnHeader2.Width = 120;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Questions";
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Correct";
            this.columnHeader4.Width = 48;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.checkedListBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "Main";
            this.Text = "Quiz Engine";
            this.Load += new System.EventHandler(this.Main_Load);
            ((System.ComponentModel.ISupportInitialize)(this.topicsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mydbDataSet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.subjectsBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label1;
        private mydbDataSet1 mydbDataSet1;
        private System.Windows.Forms.BindingSource topicsBindingSource;
        private mydbDataSet1TableAdapters.topicsTableAdapter topicsTableAdapter;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.BindingSource subjectsBindingSource;
        private mydbDataSet1TableAdapters.subjectsTableAdapter subjectsTableAdapter;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
    }
}

