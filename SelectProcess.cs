using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace SaveMyGame
{
    public partial class SelectProcess : Form
    {
        public SelectProcess()
        {
            InitializeComponent();
        }

        public string? SelectedProcessExecutable;
        public int? SelectedProcessId;
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
        } 
        private void SelectProcess_Load(object sender, EventArgs e)
        {

        }

        private void btnSelProcess_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Executable|*.exe";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (System.IO.File.Exists(openFileDialog.FileName))
                {
                    tbExePath.Text = System.IO.Path.GetFileNameWithoutExtension(openFileDialog1.FileName);
                    
                }
            }
            
        }

        private void btnTrace_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(tbExePath.Text))
            {
                MessageBox.Show("Choose at least one process to trace!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if(System.IO.File.Exists(tbExePath.Text))
                {
                    SelectedProcessExecutable = tbExePath.Text;
                }
                else
                {
                    MessageBox.Show("File not exists!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
