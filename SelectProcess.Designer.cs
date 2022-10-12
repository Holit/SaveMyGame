namespace SaveMyGame
{
    partial class SelectProcess
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
            this.lvProcDetail = new System.Windows.Forms.ListView();
            this.btnTrace = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnSelProcess = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // lvProcDetail
            // 
            this.lvProcDetail.FullRowSelect = true;
            this.lvProcDetail.GridLines = true;
            this.lvProcDetail.Location = new System.Drawing.Point(12, 72);
            this.lvProcDetail.MultiSelect = false;
            this.lvProcDetail.Name = "lvProcDetail";
            this.lvProcDetail.Size = new System.Drawing.Size(949, 1007);
            this.lvProcDetail.TabIndex = 0;
            this.lvProcDetail.UseCompatibleStateImageBehavior = false;
            this.lvProcDetail.View = System.Windows.Forms.View.Details;
            // 
            // btnTrace
            // 
            this.btnTrace.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnTrace.Location = new System.Drawing.Point(781, 1097);
            this.btnTrace.Name = "btnTrace";
            this.btnTrace.Size = new System.Drawing.Size(180, 72);
            this.btnTrace.TabIndex = 1;
            this.btnTrace.Text = "Trace!";
            this.btnTrace.UseVisualStyleBackColor = true;
            this.btnTrace.Click += new System.EventHandler(this.btnTrace_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(562, 62);
            this.label1.TabIndex = 2;
            this.label1.Text = "When process starts, auto saver will start as well\r\n..process exits, auto saver w" +
    "ill stop saving.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 1085);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(441, 31);
            this.label2.TabIndex = 3;
            this.label2.Text = "Or choose program waiting to launch";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 1119);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(628, 38);
            this.textBox1.TabIndex = 4;
            // 
            // btnSelProcess
            // 
            this.btnSelProcess.Location = new System.Drawing.Point(646, 1114);
            this.btnSelProcess.Name = "btnSelProcess";
            this.btnSelProcess.Size = new System.Drawing.Size(62, 46);
            this.btnSelProcess.TabIndex = 5;
            this.btnSelProcess.Text = "...";
            this.btnSelProcess.UseVisualStyleBackColor = true;
            this.btnSelProcess.Click += new System.EventHandler(this.btnSelProcess_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // SelectProcess
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(973, 1181);
            this.Controls.Add(this.btnSelProcess);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnTrace);
            this.Controls.Add(this.lvProcDetail);
            this.Name = "SelectProcess";
            this.Text = "Select Process to Trace";
            this.Load += new System.EventHandler(this.SelectProcess_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ListView lvProcDetail;
        private Button btnTrace;
        private Label label1;
        private Label label2;
        private TextBox textBox1;
        private Button btnSelProcess;
        private OpenFileDialog openFileDialog1;
    }
}