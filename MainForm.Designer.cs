namespace SaveMyGame
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
            this.components = new System.ComponentModel.Container();
            this.btnSave = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.btn_FromDirSel = new System.Windows.Forms.Button();
            this.lvDetails = new System.Windows.Forms.ListView();
            this.label2 = new System.Windows.Forms.Label();
            this.nudTime = new System.Windows.Forms.NumericUpDown();
            this.btnRestore = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.btn_ToDirSel = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.cbAutoDelete = new System.Windows.Forms.CheckBox();
            this.cbAutoClear = new System.Windows.Forms.CheckBox();
            this.btnStartByProcess = new System.Windows.Forms.Button();
            this.btnManSave = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudTime)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.Green;
            this.btnSave.Location = new System.Drawing.Point(509, 105);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(192, 75);
            this.btnSave.TabIndex = 15;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 31);
            this.label1.TabIndex = 14;
            this.label1.Text = "From";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(91, 8);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(518, 38);
            this.textBox1.TabIndex = 13;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 834);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(710, 41);
            this.statusStrip1.TabIndex = 20;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(85, 31);
            this.toolStripStatusLabel1.Text = "Ready";
            // 
            // btn_FromDirSel
            // 
            this.btn_FromDirSel.Location = new System.Drawing.Point(615, 1);
            this.btn_FromDirSel.Name = "btn_FromDirSel";
            this.btn_FromDirSel.Size = new System.Drawing.Size(86, 48);
            this.btn_FromDirSel.TabIndex = 19;
            this.btn_FromDirSel.Text = "...";
            this.btn_FromDirSel.UseVisualStyleBackColor = true;
            this.btn_FromDirSel.Click += new System.EventHandler(this.btn_FromDirSel_Click);
            // 
            // lvDetails
            // 
            this.lvDetails.FullRowSelect = true;
            this.lvDetails.GridLines = true;
            this.lvDetails.Location = new System.Drawing.Point(12, 186);
            this.lvDetails.MultiSelect = false;
            this.lvDetails.Name = "lvDetails";
            this.lvDetails.Size = new System.Drawing.Size(689, 567);
            this.lvDetails.TabIndex = 18;
            this.lvDetails.UseCompatibleStateImageBehavior = false;
            this.lvDetails.View = System.Windows.Forms.View.Details;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 125);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 31);
            this.label2.TabIndex = 17;
            this.label2.Text = "Time(s)";
            // 
            // nudTime
            // 
            this.nudTime.Location = new System.Drawing.Point(119, 122);
            this.nudTime.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
            this.nudTime.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudTime.MinimumSize = new System.Drawing.Size(117, 0);
            this.nudTime.Name = "nudTime";
            this.nudTime.Size = new System.Drawing.Size(117, 38);
            this.nudTime.TabIndex = 16;
            this.nudTime.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // btnRestore
            // 
            this.btnRestore.Location = new System.Drawing.Point(509, 759);
            this.btnRestore.Name = "btnRestore";
            this.btnRestore.Size = new System.Drawing.Size(192, 69);
            this.btnRestore.TabIndex = 21;
            this.btnRestore.Text = "Restore";
            this.btnRestore.UseVisualStyleBackColor = true;
            this.btnRestore.Click += new System.EventHandler(this.btnRestore_Click);
            // 
            // btn_ToDirSel
            // 
            this.btn_ToDirSel.Location = new System.Drawing.Point(615, 56);
            this.btn_ToDirSel.Name = "btn_ToDirSel";
            this.btn_ToDirSel.Size = new System.Drawing.Size(86, 48);
            this.btn_ToDirSel.TabIndex = 25;
            this.btn_ToDirSel.Text = "...";
            this.btn_ToDirSel.UseVisualStyleBackColor = true;
            this.btn_ToDirSel.Click += new System.EventHandler(this.btn_ToDirSel_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(145, 31);
            this.label3.TabIndex = 24;
            this.label3.Text = "Destination";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(163, 61);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(446, 38);
            this.textBox2.TabIndex = 23;
            // 
            // cbAutoDelete
            // 
            this.cbAutoDelete.AutoSize = true;
            this.cbAutoDelete.Location = new System.Drawing.Point(12, 759);
            this.cbAutoDelete.Name = "cbAutoDelete";
            this.cbAutoDelete.Size = new System.Drawing.Size(243, 35);
            this.cbAutoDelete.TabIndex = 22;
            this.cbAutoDelete.Text = "Auto delete(>20)";
            this.cbAutoDelete.UseVisualStyleBackColor = true;
            // 
            // cbAutoClear
            // 
            this.cbAutoClear.AutoSize = true;
            this.cbAutoClear.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.cbAutoClear.Location = new System.Drawing.Point(12, 796);
            this.cbAutoClear.Name = "cbAutoClear";
            this.cbAutoClear.Size = new System.Drawing.Size(389, 35);
            this.cbAutoClear.TabIndex = 26;
            this.cbAutoClear.Text = "Clear dirctory before restore";
            this.cbAutoClear.UseVisualStyleBackColor = true;
            // 
            // btnStartByProcess
            // 
            this.btnStartByProcess.Enabled = false;
            this.btnStartByProcess.Location = new System.Drawing.Point(401, 759);
            this.btnStartByProcess.Name = "btnStartByProcess";
            this.btnStartByProcess.Size = new System.Drawing.Size(102, 69);
            this.btnStartByProcess.TabIndex = 27;
            this.btnStartByProcess.Text = "Attach to process";
            this.btnStartByProcess.UseVisualStyleBackColor = true;
            this.btnStartByProcess.Click += new System.EventHandler(this.btnStartByProcess_Click);
            // 
            // btnManSave
            // 
            this.btnManSave.Location = new System.Drawing.Point(311, 102);
            this.btnManSave.Name = "btnManSave";
            this.btnManSave.Size = new System.Drawing.Size(192, 75);
            this.btnManSave.TabIndex = 28;
            this.btnManSave.Text = "Manully Save";
            this.btnManSave.UseVisualStyleBackColor = true;
            this.btnManSave.Click += new System.EventHandler(this.btnManSave_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(710, 875);
            this.Controls.Add(this.btnManSave);
            this.Controls.Add(this.btnStartByProcess);
            this.Controls.Add(this.cbAutoClear);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btn_FromDirSel);
            this.Controls.Add(this.lvDetails);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.nudTime);
            this.Controls.Add(this.btnRestore);
            this.Controls.Add(this.btn_ToDirSel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.cbAutoDelete);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(736, 946);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(736, 946);
            this.Name = "MainForm";
            this.Text = "Directory Auto Saver";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudTime)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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
        private Button btnStartByProcess;
        private Button btnManSave;
    }
}