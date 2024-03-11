﻿namespace SaveMyGame
{
    partial class MainForm
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
            components = new System.ComponentModel.Container();
            btnSave = new Button();
            label1 = new Label();
            textBox1 = new TextBox();
            timer1 = new System.Windows.Forms.Timer(components);
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            btn_FromDirSel = new Button();
            lvDetails = new ListView();
            label2 = new Label();
            nudTime = new NumericUpDown();
            btnRestore = new Button();
            folderBrowserDialog1 = new FolderBrowserDialog();
            btn_ToDirSel = new Button();
            label3 = new Label();
            textBox2 = new TextBox();
            cbAutoDelete = new CheckBox();
            cbAutoClear = new CheckBox();
            btnManSave = new Button();
            cbFast = new CheckBox();
            progressBar1 = new ProgressBar();
            label4 = new Label();
            statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudTime).BeginInit();
            SuspendLayout();
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.Green;
            btnSave.Location = new Point(508, 106);
            btnSave.Margin = new Padding(4);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(192, 75);
            btnSave.TabIndex = 15;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 13);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(73, 31);
            label1.TabIndex = 14;
            label1.Text = "From";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(92, 7);
            textBox1.Margin = new Padding(4);
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.Size = new Size(518, 38);
            textBox1.TabIndex = 13;
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(32, 32);
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1 });
            statusStrip1.Location = new Point(0, 838);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Padding = new Padding(0, 0, 14, 0);
            statusStrip1.Size = new Size(704, 41);
            statusStrip1.TabIndex = 20;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(85, 31);
            toolStripStatusLabel1.Text = "Ready";
            // 
            // btn_FromDirSel
            // 
            btn_FromDirSel.Location = new Point(616, 2);
            btn_FromDirSel.Margin = new Padding(4);
            btn_FromDirSel.Name = "btn_FromDirSel";
            btn_FromDirSel.Size = new Size(86, 47);
            btn_FromDirSel.TabIndex = 19;
            btn_FromDirSel.Text = "...";
            btn_FromDirSel.UseVisualStyleBackColor = true;
            btn_FromDirSel.Click += btn_FromDirSel_Click;
            // 
            // lvDetails
            // 
            lvDetails.FullRowSelect = true;
            lvDetails.GridLines = true;
            lvDetails.Location = new Point(12, 241);
            lvDetails.Margin = new Padding(4);
            lvDetails.MultiSelect = false;
            lvDetails.Name = "lvDetails";
            lvDetails.Size = new Size(688, 512);
            lvDetails.TabIndex = 18;
            lvDetails.UseCompatibleStateImageBehavior = false;
            lvDetails.View = View.Details;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 105);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(97, 31);
            label2.TabIndex = 17;
            label2.Text = "Time(s)";
            // 
            // nudTime
            // 
            nudTime.Location = new Point(119, 103);
            nudTime.Margin = new Padding(4);
            nudTime.Maximum = new decimal(new int[] { 3600, 0, 0, 0 });
            nudTime.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudTime.MinimumSize = new Size(116, 0);
            nudTime.Name = "nudTime";
            nudTime.Size = new Size(126, 38);
            nudTime.TabIndex = 16;
            nudTime.Value = new decimal(new int[] { 60, 0, 0, 0 });
            // 
            // btnRestore
            // 
            btnRestore.Location = new Point(465, 759);
            btnRestore.Margin = new Padding(4);
            btnRestore.Name = "btnRestore";
            btnRestore.Size = new Size(235, 69);
            btnRestore.TabIndex = 21;
            btnRestore.Text = "Restore";
            btnRestore.UseVisualStyleBackColor = true;
            btnRestore.Click += btnRestore_Click;
            // 
            // btn_ToDirSel
            // 
            btn_ToDirSel.Location = new Point(616, 57);
            btn_ToDirSel.Margin = new Padding(4);
            btn_ToDirSel.Name = "btn_ToDirSel";
            btn_ToDirSel.Size = new Size(86, 47);
            btn_ToDirSel.TabIndex = 25;
            btn_ToDirSel.Text = "...";
            btn_ToDirSel.UseVisualStyleBackColor = true;
            btn_ToDirSel.Click += btn_ToDirSel_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 59);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(99, 31);
            label3.TabIndex = 24;
            label3.Text = "Save to";
            // 
            // textBox2
            // 
            textBox2.Location = new Point(119, 55);
            textBox2.Margin = new Padding(4);
            textBox2.Name = "textBox2";
            textBox2.ReadOnly = true;
            textBox2.Size = new Size(491, 38);
            textBox2.TabIndex = 23;
            // 
            // cbAutoDelete
            // 
            cbAutoDelete.AutoSize = true;
            cbAutoDelete.Location = new Point(12, 759);
            cbAutoDelete.Margin = new Padding(4);
            cbAutoDelete.Name = "cbAutoDelete";
            cbAutoDelete.Size = new Size(233, 35);
            cbAutoDelete.TabIndex = 22;
            cbAutoDelete.Text = "Delete old items";
            cbAutoDelete.UseVisualStyleBackColor = true;
            // 
            // cbAutoClear
            // 
            cbAutoClear.AutoSize = true;
            cbAutoClear.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            cbAutoClear.Location = new Point(12, 797);
            cbAutoClear.Margin = new Padding(4);
            cbAutoClear.Name = "cbAutoClear";
            cbAutoClear.Size = new Size(367, 35);
            cbAutoClear.TabIndex = 26;
            cbAutoClear.Text = "Clear folder before restore";
            cbAutoClear.UseVisualStyleBackColor = true;
            // 
            // btnManSave
            // 
            btnManSave.Location = new Point(312, 106);
            btnManSave.Margin = new Padding(4);
            btnManSave.Name = "btnManSave";
            btnManSave.Size = new Size(192, 75);
            btnManSave.TabIndex = 28;
            btnManSave.Text = "Manully save";
            btnManSave.UseVisualStyleBackColor = true;
            btnManSave.Click += btnManSave_Click;
            // 
            // cbFast
            // 
            cbFast.AutoSize = true;
            cbFast.Location = new Point(12, 146);
            cbFast.Name = "cbFast";
            cbFast.Size = new Size(165, 35);
            cbFast.TabIndex = 29;
            cbFast.Text = "Fast mode";
            cbFast.UseVisualStyleBackColor = true;
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(131, 188);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(566, 46);
            progressBar1.TabIndex = 30;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 194);
            label4.Name = "label4";
            label4.Size = new Size(113, 31);
            label4.TabIndex = 31;
            label4.Text = "Progress";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(14F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(704, 879);
            Controls.Add(label4);
            Controls.Add(progressBar1);
            Controls.Add(cbFast);
            Controls.Add(btnManSave);
            Controls.Add(cbAutoClear);
            Controls.Add(btnSave);
            Controls.Add(label1);
            Controls.Add(textBox1);
            Controls.Add(statusStrip1);
            Controls.Add(btn_FromDirSel);
            Controls.Add(lvDetails);
            Controls.Add(label2);
            Controls.Add(nudTime);
            Controls.Add(btnRestore);
            Controls.Add(btn_ToDirSel);
            Controls.Add(label3);
            Controls.Add(textBox2);
            Controls.Add(cbAutoDelete);
            Margin = new Padding(4);
            MaximizeBox = false;
            MaximumSize = new Size(730, 950);
            MinimizeBox = false;
            MinimumSize = new Size(730, 950);
            Name = "MainForm";
            Text = "Directory Auto Saver";
            Load += MainForm_Load;
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudTime).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnSave;
        private Label label1;
        private TextBox textBox1;
        private System.Windows.Forms.Timer timer1;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private Button btn_FromDirSel;
        private ListView lvDetails;
        private Label label2;
        private NumericUpDown nudTime;
        private Button btnRestore;
        private FolderBrowserDialog folderBrowserDialog1;
        private Button btn_ToDirSel;
        private Label label3;
        private TextBox textBox2;
        private CheckBox cbAutoDelete;
        private CheckBox cbAutoClear;
        private Button btnManSave;
        private CheckBox cbFast;
        private ProgressBar progressBar1;
        private Label label4;
    }
}