using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace SaveMyGame
{
    public partial class MainForm : Form
    {
        struct _config
        {
            public string frompath;
            public string topath;
            public string _7zpath;
            public int interval;
        }
        ParameterizedThreadStart pthsChild;
        Thread thChild;
        static _config config;

        public MainForm()
        {
            InitializeComponent();
        }

        private void btn_FromDirSel_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            //打开的文件夹浏览对话框上的描述  

            dialog.Description = "Select your game archive directory";
            //是否显示对话框左下角 新建文件夹 按钮，默认为 true  
            dialog.ShowNewFolderButton = false;

            string path = textBox1.Text;
            dialog.SelectedPath = path;
            //按下确定选择的按钮  
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                //记录选中的目录  
                path = dialog.SelectedPath;
                if (!string.IsNullOrEmpty(path) && System.IO.Directory.Exists(path))
                {
                    textBox1.Text = path;
                }
            }
        }

        private void btn_ToDirSel_Click(object sender, EventArgs e)
        {

            FolderBrowserDialog dialog = new FolderBrowserDialog();
            //打开的文件夹浏览对话框上的描述  
            dialog.Description = "Select your saving directory";
            //是否显示对话框左下角 新建文件夹 按钮，默认为 true  
            dialog.ShowNewFolderButton = false;

            string path = textBox2.Text;
            dialog.SelectedPath = path;
            //按下确定选择的按钮  
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                //记录选中的目录  
                path = dialog.SelectedPath;
                if (!string.IsNullOrEmpty(path) && System.IO.Directory.Exists(path))
                {
                    textBox2.Text = path;
                }
            }
            long dirSize = 0;
            GetDirSizeByPath(path, ref dirSize);
            if ((dirSize / 1024) / 1024 > 800)
            {
                MessageBox.Show("Large redundancy directory.\nClean destination folder to save your disk.",
                    "Reminder", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


        }
        /// <summary>
        /// 获取某一文件夹的大小
        /// </summary>
        /// <param name="dir">文件夹目录</param>
        /// <param name="dirSize">文件夹大小</param>
        private static void GetDirSizeByPath(string dir, ref long dirSize)
        {
            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(dir);

                DirectoryInfo[] dirs = dirInfo.GetDirectories();
                FileInfo[] files = dirInfo.GetFiles();

                foreach (var item in dirs)
                {
                    GetDirSizeByPath(item.FullName, ref dirSize);
                }

                foreach (var item in files)
                {
                    dirSize += item.Length;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot get directory size:" + ex.Message);
            }

        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            lvDetails.Columns.Add("Name", (int)(0.3 * lvDetails.Size.Width));

            lvDetails.Columns.Add("Date", (int)(0.5 * lvDetails.Size.Width));
            lvDetails.Columns.Add("Size", (int)(0.2 * lvDetails.Size.Width));
            textBox2.Text = Application.StartupPath;
            config._7zpath = Application.StartupPath + "7z.exe";
        }
        public void ArchiveCallTo(object para)
        {
            _config config = (_config)para;

            ZipHelper zipHelper = new ZipHelper(config._7zpath);
            System.Timers.Timer timer = new System.Timers.Timer()
            {
                Interval = config.interval * 1000,
                Enabled = false
            };
            /*
             * System.Timers.Timer will create another thread.
             * This is a monitor thread for timer manage and stop.
             */
            try
            {
                timer.Elapsed += (state, e) =>
                {
                    if (!started)
                    {
                        timer.Stop();
                        timer.Dispose();

                        return;
                    }
                    DateTime dt = DateTime.Now;
                    string name = dt.ToString("yy-MM-dd-hh-mm-ss-ffff") + ".7z";
                    string savepath = config.topath + "\\" + name;
                    zipHelper.CompressDirectory(config.frompath + "/*.*", savepath);
                    X509Certificate cert =  X509Certificate.CreateFromSignedFile(savepath);

                    //Notify user by status bar
                    //repeatable structure.
                    DelegateNotifyByStatusBar delegateNotifyByStatusBar = new DelegateNotifyByStatusBar(NotifyByStatusBar);
                    this.BeginInvoke(delegateNotifyByStatusBar,
                            new object[] {
                                savepath  + " saved."
                            });
                    DelegateModifylvDetail de = new DelegateModifylvDetail(ModifylvDetail);
                    string strSize = "";
                    long size = new System.IO.FileInfo(savepath).Length;
                    if (size <= 1024)
                    {
                        strSize = size.ToString() + "B";
                    }
                    else if ((int)(size / 1024) <= 1024)
                    {
                        strSize = (size / 1024.00).ToString("f2") + "KB";
                    }
                    else if ((int)((size / 1024) / 1024) <= 1024)
                    {
                        strSize = (size / 1024.00 / 1024.00).ToString("f2") + "MB";
                    }
                    else
                    {
                        strSize = "LARGE:" + size.ToString() + "B";
                    }
                    this.BeginInvoke(de,
                        new object[] {
                        name,
                        dt,
                        strSize
                        }
                    );
                    Debug.WriteLine("this.BeginInvoke");
                };
                timer.Start();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception@thread: " + ex.Message);
                if (timer.Enabled)
                {
                    timer.Stop();
                    timer.Dispose();

                }
                return;
            }
        }
        System.Timers.Timer tmStatusBar;
        delegate void DelegateNotifyByStatusBar(string info);
        public void NotifyByStatusBar(string info)
        {
            if (tmStatusBar != null)
            {
                tmStatusBar.Stop();
            }
            toolStripStatusLabel1.Text = info;
            tmStatusBar = new System.Timers.Timer()
            {
                Interval = 1000,
                Enabled = false
            };
            tmStatusBar.Elapsed += (state, e) =>
            {
                toolStripStatusLabel1.Text = "Ready";
                tmStatusBar.Stop();
                tmStatusBar.Dispose();
            };
            tmStatusBar.Start();

        }
        delegate void DelegateModifylvDetail(string name, DateTime date, string size);

        public void ModifylvDetail(string name, DateTime date, string size)
        {
            int delete_max = 2;
            if (cbAutoDelete.Checked)
            {
                if (lvDetails.Items.Count >= delete_max)
                {
                    if (System.IO.File.Exists(config.topath + "\\" + lvDetails.Items[0].SubItems[0].Text))
                    {
                        System.IO.File.Delete(config.topath + "\\" + lvDetails.Items[0].SubItems[0].Text);
                    }
                    else
                    {
                        //Notify user by status bar
                        //repeatable structure.
                        DelegateNotifyByStatusBar delegateNotifyByStatusBar = new DelegateNotifyByStatusBar(NotifyByStatusBar);
                        this.BeginInvoke(delegateNotifyByStatusBar,
                                new object[] {
                                lvDetails.Items[0].SubItems[0].Text + "not found!"
                                });
                    }
                    lvDetails.Items.Remove(lvDetails.Items[0]);
                    lvDetails.Refresh();
                }
            }
            ListViewItem lvitem = new ListViewItem();
            lvitem.Text = name;
            lvitem.SubItems.Add(date.ToString("yyyy/MM/dd HH:mm:ss.ffff"));
            lvitem.SubItems.Add(size);
            lvDetails.Items.Add(lvitem);
            lvDetails.Items[lvDetails.Items.Count - 1].Selected = true;
            lvDetails.Items[lvDetails.Items.Count - 1].EnsureVisible();
        }
        private bool started = false;
        private void btnSave_Click(object sender, EventArgs e)
        {

            config.frompath = textBox1.Text;
            config.topath = textBox2.Text;
            config._7zpath = Application.StartupPath + "7z.exe";
            config.interval = (int)nudTime.Value;

            pthsChild = new ParameterizedThreadStart(ArchiveCallTo);
            thChild = new Thread(pthsChild);
            thChild.IsBackground = true;
            if (!started)
            {
                if (textBox1.Text != "" && textBox2.Text != "")
                {
                    Debug.WriteLine("thChild.Start(config);");
                    thChild.Start(config);
                    started = true;
                    btnSave.Text = "Stop";
                    btnSave.BackColor = Color.Red;
                }
            }
            else
            {
                //unsafe exit method:
                //using bool to notify a loop thread.
                btnSave.Text = "Save";
                btnSave.BackColor = Color.Green;
                started = false;
            }
        }
        /// <summary>
        ///  //跨盘移动
        /// </summary>
        /// <param name="sourcePath">源文件路径</param>
        /// <param name="destPath">新文件路径</param>
        /// <returns></returns>
        void Move(string sourcePath, string destPath)//文件移动函数
        {
            DirectoryInfo dir = new DirectoryInfo(sourcePath);
            if (Directory.Exists(sourcePath))
            {
                destPath = destPath + "\\" + dir.Name;
                Directory.CreateDirectory(destPath);
            }
            DirectoryInfo[] directoryInfos = dir.GetDirectories();
            if (directoryInfos != null)
            {
                for (int i = 0; i < directoryInfos.Length; i++)
                {
                    Move(directoryInfos[i].FullName, destPath);
                }
            }
            FileInfo[] fileInfos = dir.GetFiles();
            if (fileInfos != null)
            {
                for (int i = 0; i < fileInfos.Length; i++)
                {
                    fileInfos[i].MoveTo(destPath + "\\" + fileInfos[i].Name);
                }
            }
            Directory.Delete(sourcePath);
        }
        private void btnRestore_Click(object sender, EventArgs e)
        {
            DelegateNotifyByStatusBar delegateNotifyByStatusBar;
            if (thChild != null && started)
            {
                btnSave.Text = "Save";
                btnSave.BackColor = Color.Green;
                started = false;
            }
            if (lvDetails.Items.Count > 0)
            {
                if (lvDetails.SelectedItems.Count == 0)
                {
                    lvDetails.Items[lvDetails.Items.Count - 1].Selected = true;
                }
                string name = lvDetails.SelectedItems[0].SubItems[0].Text;
                ZipHelper zipHelper = new ZipHelper(config._7zpath);
                if (cbAutoClear.Checked)
                {

                    if (System.IO.Directory.Exists(config.topath + "\\" + ".bak"))
                    {
                        DirectoryInfo dirInfo = new DirectoryInfo(config.topath + "\\" + ".bak");
                        long dirSize = 0;
                        GetDirSizeByPath(config.topath + "\\" + ".bak", ref dirSize);
                        DialogResult dr = MessageBox.Show("Last save backup exists.\nDelete them?\n\n"
                            + dirInfo.FullName + "\n"
                            + "Last Write: " + dirInfo.LastWriteTime.ToString("yyyy/MM/dd HH:mm:ss.ffff") + "\n"
                            + "Size(KB): " + (int)(dirSize / 1024)
                            ,"Existing files.",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                        if (dr == DialogResult.Yes)
                        {
                            try
                            {

                                System.IO.Directory.Delete(config.topath + "\\" + ".bak", true);
                            }
                            catch (IOException)
                            {
                                //Notify user by status bar
                                //repeatable structure.
                                delegateNotifyByStatusBar = new DelegateNotifyByStatusBar(NotifyByStatusBar);
                                this.BeginInvoke(delegateNotifyByStatusBar,
                                        new object[] {
                                name  + " is being used by others."
                                        });
                            }
                        }
                    }
                    System.IO.Directory.CreateDirectory(config.topath + "\\" + ".bak");
                    Move(config.frompath + "\\", config.topath + "\\" + ".bak");
                }
                zipHelper.DecompressFileToDestDirectory(config.topath + "\\" + name, config.frompath);

                //Notify user by status bar
                //repeatable structure.
                delegateNotifyByStatusBar = new DelegateNotifyByStatusBar(NotifyByStatusBar);
                this.BeginInvoke(delegateNotifyByStatusBar,
                        new object[] {
                                name  + " restored."
                        });
            }
            else
            {
                //Notify user by status bar
                //repeatable structure.
                delegateNotifyByStatusBar = new DelegateNotifyByStatusBar(NotifyByStatusBar);
                this.BeginInvoke(delegateNotifyByStatusBar,
                        new object[] {
                        "No save!"
                        });

            }
        }

        struct _proc_monitor_config
        {
            public string ExecutablePath;
            public long Pid;
            public bool isRunning;
        }
        _proc_monitor_config proc_Monitor_Config;
        public void MonitorProcessStartCallTo(object para)
        {
            _proc_monitor_config monitor_config = (_proc_monitor_config)(para);
            do
            {
                if (System.Diagnostics.Process.GetProcessesByName(monitor_config.ExecutablePath).Length > 0)
                {
                    proc_Monitor_Config.Pid = System.Diagnostics.
                        Process.GetProcessesByName(monitor_config.ExecutablePath)[0].Id;
                    proc_Monitor_Config.isRunning = true;
                }

            } while (isStartMonitorProcess);
            return;
        }
        private bool isStartMonitorProcess = false;
        private void btnStartByProcess_Click(object sender, EventArgs e)
        {
            SelectProcess frmSelectProcess = new SelectProcess();
            if (frmSelectProcess.ShowDialog() == DialogResult.OK)
            {
                string? process = frmSelectProcess.SelectedProcessExecutable;
                if (process != null)
                {
                    ParameterizedThreadStart pthsMonitorProcess = new ParameterizedThreadStart(MonitorProcessStartCallTo);
                    Thread thsMonitorProcess = new Thread(pthsChild);
                    thsMonitorProcess.IsBackground = true;
                    isStartMonitorProcess = true;
                    proc_Monitor_Config.ExecutablePath = process;
                    proc_Monitor_Config.Pid = 0;
                    thsMonitorProcess.Start();
                }
            }

        }
        public bool ManuallySaved = false;
        private void btnManSave_Click(object sender, EventArgs e)
        {
            config.frompath = textBox1.Text;
            config.topath = textBox2.Text;
            config._7zpath = Application.StartupPath + "7z.exe";
            config.interval = (int)nudTime.Value;

            if (!System.IO.Directory.Exists(config.frompath)
                || !System.IO.Directory.Exists(config.topath))
            {
                return;
            }
            ZipHelper zipHelper = new ZipHelper(config._7zpath);

            /*
             * System.Timers.Timer will create another thread.
             * This is a monitor thread for timer manage and stop.
             */
            try
            {
                Task task = new Task(() =>
                {
                    if (!ManuallySaved)
                    {
                        DateTime dt = DateTime.Now;
                        string name = dt.ToString("yy-MM-dd-hh-mm-ss-ffff") + ".7z";
                        string savepath = config.topath + "\\" + name;
                        zipHelper.CompressDirectory(config.frompath + "/*.*", savepath);

                        //Notify user by status bar
                        //repeatable structure.
                        DelegateNotifyByStatusBar delegateNotifyByStatusBar = new DelegateNotifyByStatusBar(NotifyByStatusBar);
                        this.BeginInvoke(delegateNotifyByStatusBar,
                                new object[] {
                                savepath  + " saved."
                                });
                        DelegateModifylvDetail de = new DelegateModifylvDetail(ModifylvDetail);
                        string strSize = "";
                        long size = new System.IO.FileInfo(savepath).Length;
                        if (size <= 1024)
                        {
                            strSize = size.ToString() + "B";
                        }
                        else if ((int)(size / 1024) <= 1024)
                        {
                            strSize = (size / 1024.00).ToString("f2") + "KB";
                        }
                        else if ((int)((size / 1024) / 1024) <= 1024)
                        {
                            strSize = (size / 1024.00 / 1024.00).ToString("f2") + "MB";
                        }
                        else
                        {
                            strSize = "LARGE:" + size.ToString() + "B";
                        }
                        this.BeginInvoke(de,
                            new object[] {
                        name,
                        dt,
                        strSize
                            }
                        );
                        ManuallySaved = true;
                    };
                });
                task.Start();

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception@thread: " + ex.Message);
                ManuallySaved = false;
                return;
            }
            ManuallySaved = false;
        }
    }
}
