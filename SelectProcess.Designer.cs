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
            this.btnTrace = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbExePath = new System.Windows.Forms.TextBox();
            this.btnSelProcess = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnTrace
            // 
            this.btnTrace.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnTrace.Location = new System.Drawing.Point(542, 164);
            this.btnTrace.Name = "btnTrace";
            this.btnTrace.Size = new System.Drawing.Size(180, 72);
            this.btnTrace.TabIndex = 2;
            this.btnTrace.Text = "Trace!";
            this.btnTrace.UseVisualStyleBackColor = true;
            this.btnTrace.Click += new System.EventHandler(this.btnTrace_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(730, 62);
            this.label1.TabIndex = 2;
            this.label1.Text = "When process starts, auto saver will start as well\r\n..process exits, auto saver w" +
    "ill save last one and stop auto-save";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(409, 31);
            this.label2.TabIndex = 3;
            this.label2.Text = "Choose program waiting to launch";
            // 
            // tbExePath
            // 
            this.tbExePath.Location = new System.Drawing.Point(12, 117);
            this.tbExePath.Name = "tbExePath";
            this.tbExePath.Size = new System.Drawing.Size(828, 38);
            this.tbExePath.TabIndex = 4;
            // 
            // btnSelProcess
            // 
            this.btnSelProcess.Location = new System.Drawing.Point(846, 112);
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
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnCancel.Location = new System.Drawing.Point(728, 164);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(180, 72);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // SelectProcess
            // 
            this.AcceptButton = this.btnTrace;
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(920, 248);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSelProcess);
            this.Controls.Add(this.tbExePath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnTrace);
            this.Name = "SelectProcess";
            this.Text = "Select Process to Trace";
            this.Load += new System.EventHandler(this.SelectProcess_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Button btnTrace;
        private Label label1;
        private Label label2;
        private TextBox tbExePath;
        private Button btnSelProcess;
        private OpenFileDialog openFileDialog1;
        private Button btnCancel;
    }
}