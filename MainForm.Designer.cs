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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
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
            resources.ApplyResources(this.btnSave, "btnSave");
            this.btnSave.BackColor = System.Drawing.Color.Green;
            this.btnSave.Name = "btnSave";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // textBox1
            // 
            resources.ApplyResources(this.textBox1, "textBox1");
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            // 
            // statusStrip1
            // 
            resources.ApplyResources(this.statusStrip1, "statusStrip1");
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Name = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            resources.ApplyResources(this.toolStripStatusLabel1, "toolStripStatusLabel1");
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            // 
            // btn_FromDirSel
            // 
            resources.ApplyResources(this.btn_FromDirSel, "btn_FromDirSel");
            this.btn_FromDirSel.Name = "btn_FromDirSel";
            this.btn_FromDirSel.UseVisualStyleBackColor = true;
            this.btn_FromDirSel.Click += new System.EventHandler(this.btn_FromDirSel_Click);
            // 
            // lvDetails
            // 
            resources.ApplyResources(this.lvDetails, "lvDetails");
            this.lvDetails.FullRowSelect = true;
            this.lvDetails.GridLines = true;
            this.lvDetails.MultiSelect = false;
            this.lvDetails.Name = "lvDetails";
            this.lvDetails.UseCompatibleStateImageBehavior = false;
            this.lvDetails.View = System.Windows.Forms.View.Details;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // nudTime
            // 
            resources.ApplyResources(this.nudTime, "nudTime");
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
            this.nudTime.Name = "nudTime";
            this.nudTime.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // btnRestore
            // 
            resources.ApplyResources(this.btnRestore, "btnRestore");
            this.btnRestore.Name = "btnRestore";
            this.btnRestore.UseVisualStyleBackColor = true;
            this.btnRestore.Click += new System.EventHandler(this.btnRestore_Click);
            // 
            // folderBrowserDialog1
            // 
            resources.ApplyResources(this.folderBrowserDialog1, "folderBrowserDialog1");
            // 
            // btn_ToDirSel
            // 
            resources.ApplyResources(this.btn_ToDirSel, "btn_ToDirSel");
            this.btn_ToDirSel.Name = "btn_ToDirSel";
            this.btn_ToDirSel.UseVisualStyleBackColor = true;
            this.btn_ToDirSel.Click += new System.EventHandler(this.btn_ToDirSel_Click);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // textBox2
            // 
            resources.ApplyResources(this.textBox2, "textBox2");
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            // 
            // cbAutoDelete
            // 
            resources.ApplyResources(this.cbAutoDelete, "cbAutoDelete");
            this.cbAutoDelete.Name = "cbAutoDelete";
            this.cbAutoDelete.UseVisualStyleBackColor = true;
            // 
            // cbAutoClear
            // 
            resources.ApplyResources(this.cbAutoClear, "cbAutoClear");
            this.cbAutoClear.Name = "cbAutoClear";
            this.cbAutoClear.UseVisualStyleBackColor = true;
            // 
            // btnStartByProcess
            // 
            resources.ApplyResources(this.btnStartByProcess, "btnStartByProcess");
            this.btnStartByProcess.Name = "btnStartByProcess";
            this.btnStartByProcess.UseVisualStyleBackColor = true;
            this.btnStartByProcess.Click += new System.EventHandler(this.btnStartByProcess_Click);
            // 
            // btnManSave
            // 
            resources.ApplyResources(this.btnManSave, "btnManSave");
            this.btnManSave.Name = "btnManSave";
            this.btnManSave.UseVisualStyleBackColor = true;
            this.btnManSave.Click += new System.EventHandler(this.btnManSave_Click);
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
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
            this.MinimizeBox = false;
            this.Name = "MainForm";
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