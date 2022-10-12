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
            lvProcDetail.Items.Clear();
            lvProcDetail.Columns.Add("PID", (int)(lvProcDetail.Width*0.1));
            lvProcDetail.Columns.Add("Name", (int)(lvProcDetail.Width*0.35));
            lvProcDetail.Columns.Add("Running time", (int)(lvProcDetail.Width*0.1));
            lvProcDetail.Columns.Add("Launch time", (int)(lvProcDetail.Width * 0.35));
            
            foreach (var proc in System.Diagnostics.Process.GetProcesses())
            {
                string ProcessName = "";
                long pid = 0;
                string StartTime = "";
                double runningtime = 0;
                try
                {
                    ProcessName = proc.ProcessName;
                    pid = proc.Id;
                    runningtime = (double)((proc.TotalProcessorTime.TotalMilliseconds / 1000.0));
                    StartTime = proc.StartTime.ToString("yyyy/MM/dd hh:mm:ss.ffff");
                }
                catch(Exception ex)
                {
                    //Debug.WriteLine(ex.Message);
                }
                finally
                {

                    ListViewItem lvitem = new ListViewItem();
                    lvitem.Text = pid.ToString();
                    lvitem.SubItems.Add(ProcessName);
                    lvitem.SubItems.Add(runningtime.ToString("f2") + "ms");
                    lvitem.SubItems.Add(StartTime);
                    lvProcDetail.Items.Add(lvitem);
                }
            }

            lvProcDetail.Sort();
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
                    textBox1.Text = System.IO.Path.GetFileNameWithoutExtension(openFileDialog1.FileName);
                    
                }
            }
            
        }

        private void btnTrace_Click(object sender, EventArgs e)
        {
            Process proc = new Process();
            try
            {
                proc = Process.GetProcessById(int.Parse(lvProcDetail.SelectedItems[0].Text));
            }
            catch (Win32Exception ex)
            {
                MessageBox.Show("Permission denined!\nThis Process cannot be monitored.\n-----\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lvProcDetail.SelectedItems.Clear();
                lvProcDetail.Refresh();
            }
            finally
            {
                if(proc != null)
                {
                    textBox1.Text = proc.ProcessName;
                }
            }
            if(!string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Choose at least one process to trace!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                SelectedProcessExecutable = textBox1.Text;
            }
        }
    }
}
