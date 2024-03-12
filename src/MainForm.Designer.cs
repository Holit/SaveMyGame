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
            components = new System.ComponentModel.Container();
            btnSave = new Button();
            label1 = new Label();
            tbFrom = new TextBox();
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
            tbSaveto = new TextBox();
            cbDeleteOldFiles = new CheckBox();
            cbClearBeforeRestore = new CheckBox();
            btnManSave = new Button();
            cbFast = new CheckBox();
            progOperation = new ProgressBar();
            label4 = new Label();
            lbLogs = new ListBox();
            cbUsing7z = new CheckBox();
            toolTip1 = new ToolTip(components);
            statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudTime).BeginInit();
            SuspendLayout();
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.Green;
            btnSave.Location = new Point(254, 58);
            btnSave.Margin = new Padding(2);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(96, 41);
            btnSave.TabIndex = 15;
            btnSave.Text = "存档";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(6, 7);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(20, 17);
            label1.TabIndex = 14;
            label1.Text = "自";
            // 
            // tbFrom
            // 
            tbFrom.Location = new Point(29, 4);
            tbFrom.Margin = new Padding(2);
            tbFrom.Name = "tbFrom";
            tbFrom.ReadOnly = true;
            tbFrom.Size = new Size(278, 23);
            tbFrom.TabIndex = 13;
            // 
            // timer1
            // 
            timer1.Tick += timer1_Tick;
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(32, 32);
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1 });
            statusStrip1.Location = new Point(0, 478);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Padding = new Padding(0, 0, 7, 0);
            statusStrip1.Size = new Size(357, 22);
            statusStrip1.TabIndex = 20;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(32, 17);
            toolStripStatusLabel1.Text = "就绪";
            // 
            // btn_FromDirSel
            // 
            btn_FromDirSel.Location = new Point(308, 1);
            btn_FromDirSel.Margin = new Padding(2);
            btn_FromDirSel.Name = "btn_FromDirSel";
            btn_FromDirSel.Size = new Size(43, 26);
            btn_FromDirSel.TabIndex = 19;
            btn_FromDirSel.Text = "...";
            btn_FromDirSel.UseVisualStyleBackColor = true;
            btn_FromDirSel.Click += btn_FromDirSel_Click;
            // 
            // lvDetails
            // 
            lvDetails.FullRowSelect = true;
            lvDetails.GridLines = true;
            lvDetails.Location = new Point(6, 132);
            lvDetails.Margin = new Padding(2);
            lvDetails.MultiSelect = false;
            lvDetails.Name = "lvDetails";
            lvDetails.Size = new Size(346, 208);
            lvDetails.TabIndex = 18;
            lvDetails.UseCompatibleStateImageBehavior = false;
            lvDetails.View = View.Details;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(6, 58);
            label2.Margin = new Padding(2, 0, 2, 0);
            label2.Name = "label2";
            label2.Size = new Size(58, 17);
            label2.TabIndex = 17;
            label2.Text = "计时器(s)";
            // 
            // nudTime
            // 
            nudTime.Location = new Point(66, 56);
            nudTime.Margin = new Padding(2);
            nudTime.Maximum = new decimal(new int[] { 3600, 0, 0, 0 });
            nudTime.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudTime.MinimumSize = new Size(58, 0);
            nudTime.Name = "nudTime";
            nudTime.Size = new Size(58, 23);
            nudTime.TabIndex = 16;
            nudTime.Value = new decimal(new int[] { 60, 0, 0, 0 });
            // 
            // btnRestore
            // 
            btnRestore.Location = new Point(252, 416);
            btnRestore.Margin = new Padding(2);
            btnRestore.Name = "btnRestore";
            btnRestore.Size = new Size(96, 38);
            btnRestore.TabIndex = 21;
            btnRestore.Text = "恢复";
            toolTip1.SetToolTip(btnRestore, "如果未选中，则默认恢复最近的存档");
            btnRestore.UseVisualStyleBackColor = true;
            btnRestore.Click += btnRestore_Click;
            // 
            // btn_ToDirSel
            // 
            btn_ToDirSel.Location = new Point(308, 31);
            btn_ToDirSel.Margin = new Padding(2);
            btn_ToDirSel.Name = "btn_ToDirSel";
            btn_ToDirSel.Size = new Size(43, 26);
            btn_ToDirSel.TabIndex = 25;
            btn_ToDirSel.Text = "...";
            btn_ToDirSel.UseVisualStyleBackColor = true;
            btn_ToDirSel.Click += btn_ToDirSel_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(6, 32);
            label3.Margin = new Padding(2, 0, 2, 0);
            label3.Name = "label3";
            label3.Size = new Size(20, 17);
            label3.TabIndex = 24;
            label3.Text = "到";
            // 
            // tbSaveto
            // 
            tbSaveto.Location = new Point(28, 30);
            tbSaveto.Margin = new Padding(2);
            tbSaveto.Name = "tbSaveto";
            tbSaveto.ReadOnly = true;
            tbSaveto.Size = new Size(278, 23);
            tbSaveto.TabIndex = 23;
            // 
            // cbDeleteOldFiles
            // 
            cbDeleteOldFiles.AutoSize = true;
            cbDeleteOldFiles.Location = new Point(6, 416);
            cbDeleteOldFiles.Margin = new Padding(2);
            cbDeleteOldFiles.Name = "cbDeleteOldFiles";
            cbDeleteOldFiles.Size = new Size(123, 21);
            cbDeleteOldFiles.TabIndex = 22;
            cbDeleteOldFiles.Text = "保存时删除旧项目";
            toolTip1.SetToolTip(cbDeleteOldFiles, "保存新存档时自动删除旧存档，目标文件夹内按照所有文件计算，不超过10个文件。");
            cbDeleteOldFiles.UseVisualStyleBackColor = true;
            cbDeleteOldFiles.CheckedChanged += cbDeleteOldFiles_CheckedChanged;
            // 
            // cbClearBeforeRestore
            // 
            cbClearBeforeRestore.AutoSize = true;
            cbClearBeforeRestore.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            cbClearBeforeRestore.Location = new Point(6, 437);
            cbClearBeforeRestore.Margin = new Padding(2);
            cbClearBeforeRestore.Name = "cbClearBeforeRestore";
            cbClearBeforeRestore.Size = new Size(147, 21);
            cbClearBeforeRestore.TabIndex = 26;
            cbClearBeforeRestore.Text = "恢复时清空目标文件夹";
            cbClearBeforeRestore.UseVisualStyleBackColor = true;
            cbClearBeforeRestore.CheckedChanged += cbClearBeforeRestore_CheckedChanged;
            // 
            // btnManSave
            // 
            btnManSave.Location = new Point(164, 58);
            btnManSave.Margin = new Padding(2);
            btnManSave.Name = "btnManSave";
            btnManSave.Size = new Size(88, 41);
            btnManSave.TabIndex = 28;
            btnManSave.Text = "手动存档";
            btnManSave.UseVisualStyleBackColor = true;
            btnManSave.Click += btnManSave_Click;
            // 
            // cbFast
            // 
            cbFast.AutoSize = true;
            cbFast.Location = new Point(6, 80);
            cbFast.Margin = new Padding(2);
            cbFast.Name = "cbFast";
            cbFast.Size = new Size(63, 21);
            cbFast.TabIndex = 29;
            cbFast.Text = "仅存储";
            toolTip1.SetToolTip(cbFast, "使用仅存储策略压缩，减少压缩、解压缩时间");
            cbFast.UseVisualStyleBackColor = true;
            cbFast.CheckedChanged += cbFast_CheckedChanged;
            // 
            // progOperation
            // 
            progOperation.Location = new Point(40, 103);
            progOperation.Margin = new Padding(2);
            progOperation.Name = "progOperation";
            progOperation.Size = new Size(308, 25);
            progOperation.TabIndex = 30;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(6, 106);
            label4.Margin = new Padding(2, 0, 2, 0);
            label4.Name = "label4";
            label4.Size = new Size(32, 17);
            label4.TabIndex = 31;
            label4.Text = "进度";
            // 
            // lbLogs
            // 
            lbLogs.FormattingEnabled = true;
            lbLogs.HorizontalScrollbar = true;
            lbLogs.ItemHeight = 17;
            lbLogs.Location = new Point(6, 342);
            lbLogs.Margin = new Padding(2);
            lbLogs.Name = "lbLogs";
            lbLogs.SelectionMode = SelectionMode.MultiSimple;
            lbLogs.Size = new Size(346, 72);
            lbLogs.TabIndex = 32;
            lbLogs.KeyDown += lbLogs_KeyDown;
            // 
            // cbUsing7z
            // 
            cbUsing7z.AutoSize = true;
            cbUsing7z.Location = new Point(66, 81);
            cbUsing7z.Margin = new Padding(2);
            cbUsing7z.Name = "cbUsing7z";
            cbUsing7z.Size = new Size(60, 21);
            cbUsing7z.TabIndex = 33;
            cbUsing7z.Text = "LZMA";
            toolTip1.SetToolTip(cbUsing7z, "利用LZMA算法完成压缩操作");
            cbUsing7z.UseVisualStyleBackColor = true;
            cbUsing7z.CheckedChanged += cbUsing7z_CheckedChanged;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(357, 500);
            Controls.Add(cbUsing7z);
            Controls.Add(lbLogs);
            Controls.Add(label4);
            Controls.Add(progOperation);
            Controls.Add(cbFast);
            Controls.Add(btnManSave);
            Controls.Add(cbClearBeforeRestore);
            Controls.Add(btnSave);
            Controls.Add(label1);
            Controls.Add(tbFrom);
            Controls.Add(statusStrip1);
            Controls.Add(btn_FromDirSel);
            Controls.Add(lvDetails);
            Controls.Add(label2);
            Controls.Add(nudTime);
            Controls.Add(btnRestore);
            Controls.Add(btn_ToDirSel);
            Controls.Add(label3);
            Controls.Add(tbSaveto);
            Controls.Add(cbDeleteOldFiles);
            KeyPreview = true;
            Margin = new Padding(2);
            MaximizeBox = false;
            MaximumSize = new Size(373, 539);
            MinimizeBox = false;
            MinimumSize = new Size(373, 539);
            Name = "MainForm";
            Text = "文件夹存档工具";
            FormClosing += MainForm_FormClosing;
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
        private TextBox tbFrom;
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
        private TextBox tbSaveto;
        private CheckBox cbDeleteOldFiles;
        private CheckBox cbClearBeforeRestore;
        private Button btnManSave;
        private CheckBox cbFast;
        private ProgressBar progOperation;
        private Label label4;
        private ListBox lbLogs;
        private CheckBox cbUsing7z;
        private ToolTip toolTip1;
    }
}