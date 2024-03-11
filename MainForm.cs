using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Diagnostics;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing;
using System.IO.Compression;
using System.Linq;
using System.Resources;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SaveMyGame
{
    public partial class MainForm : Form
    {
        ParameterizedThreadStart pthsChild;
        Thread thChild;
        static config runtimeConfig;
        DatabaseHelper dbHelper;

        void ReadConfig()
        {
            runtimeConfig = dbHelper.LoadConfig();

            //获取配置
            tbFrom.Text = runtimeConfig.frompath;
            tbSaveto.Text = runtimeConfig.topath;
            if (runtimeConfig.interval <= 0) runtimeConfig.interval = 60;
            nudTime.Value = runtimeConfig.interval;
            cbFast.Checked = runtimeConfig.isFastMode;
            cbUsing7z.Checked = runtimeConfig.bUsing7Z;
        }

        void SaveConfig(config _config)
        {
            dbHelper.SaveConfig(_config);
        }
        void SaveConfig()
        {
            //更新配置
            runtimeConfig.frompath = tbFrom.Text;
            runtimeConfig.topath = tbSaveto.Text;
            runtimeConfig.interval = (int)nudTime.Value;
            runtimeConfig.isFastMode = cbFast.Checked;
            runtimeConfig.bUsing7Z = cbUsing7z.Checked;

            SaveConfig(runtimeConfig);
            dbHelper.SaveConfig(runtimeConfig);
        }
        /// <summary>
        ///  //跨盘移动
        /// </summary>
        /// <param name="sourcePath">源文件路径</param>
        /// <param name="destPath">新文件路径</param>
        /// <returns></returns>
        void MoveFiles(string sourcePath, string destPath)//文件移动函数
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
                    MoveFiles(directoryInfos[i].FullName, destPath);
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
                Console.WriteLine("获取文件夹大小失败:" + ex.Message);
            }

        }
        public MainForm()
        {
            InitializeComponent();
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            dbHelper = new DatabaseHelper();
            ReadConfig();
            //初始化表头
            lvDetails.Columns.Add("名称", (int)(0.3 * lvDetails.Size.Width));

            lvDetails.Columns.Add("日期", (int)(0.5 * lvDetails.Size.Width));
            lvDetails.Columns.Add("未压缩大小", (int)(0.2 * lvDetails.Size.Width));

            tbSaveto.Text = Application.StartupPath;
            if (string.IsNullOrEmpty(runtimeConfig._7zpath))
            {
                if (File.Exists(Application.StartupPath + "7z.exe"))
                {
                    runtimeConfig._7zpath = Application.StartupPath + "7z.exe";
                }
                else if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles) + "\\7-Zip\\7z.exe"))
                {
                    runtimeConfig._7zpath = Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles) + "\\7-Zip\\7z.exe";
                }
                else if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFilesX86) + "\\7-Zip\\7z.exe"))
                {
                    runtimeConfig._7zpath = Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFilesX86) + "\\7-Zip\\7z.exe";
                }
                else
                {
                    runtimeConfig._7zpath = "";
                }
            }
            if(runtimeConfig._7zpath =="")
            {
                cbUsing7z.Enabled = false;
                LogToListbox("7z文件未找到：请将7z.exe放在应用程序同层的文件夹下；已禁用7z操作。");
            }

        }
        private void btn_FromDirSel_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            //打开的文件夹浏览对话框上的描述  

            dialog.Description = "选择要存档的文件夹";
            //是否显示对话框左下角 新建文件夹 按钮，默认为 true  
            dialog.ShowNewFolderButton = false;

            string path = tbFrom.Text;
            dialog.SelectedPath = path;
            //按下确定选择的按钮  
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                //记录选中的目录  
                path = dialog.SelectedPath;
                if (!string.IsNullOrEmpty(path) && System.IO.Directory.Exists(path))
                {
                    tbFrom.Text = path;
                    Debug.WriteLine("Game archive: " + path);
                }
            }
        }

        private void btn_ToDirSel_Click(object sender, EventArgs e)
        {

            FolderBrowserDialog dialog = new FolderBrowserDialog();
            //打开的文件夹浏览对话框上的描述  
            dialog.Description = "存储到...";
            //是否显示对话框左下角 新建文件夹 按钮，默认为 true  
            dialog.ShowNewFolderButton = false;

            string path = tbSaveto.Text;
            dialog.SelectedPath = path;
            //按下确定选择的按钮  
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                //记录选中的目录  
                path = dialog.SelectedPath;
                if (!string.IsNullOrEmpty(path) && System.IO.Directory.Exists(path))
                {
                    tbSaveto.Text = path;
                }
            }
            long dirSize = 0;
            if (!runtimeConfig.isRemindedSize)
            {
                GetDirSizeByPath(path, ref dirSize);
                //如果目标文件夹大小大于800M
                if ((dirSize / 1024) / 1024 > 800)
                {
                    MessageBox.Show("目标文件夹内容较多\n清除目标文件夹以节省计算机空间。",
                        "提醒",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                runtimeConfig.isRemindedSize = true;
            }


        }

        public void ArchiveCallee(object para)
        {
            //线程参数不能访问主程序的config，此结构通过para传递。
            config runtimeConfigTh = (config)para;

            //确定7z文件路径，实例化助手类
            SevenZipCmdHelper _7zHelper = new SevenZipCmdHelper(runtimeConfigTh._7zpath);

            //通过定时器，定时触发。
            System.Timers.Timer timer = new System.Timers.Timer()
            {
                Interval = runtimeConfigTh.interval * 1000,
                Enabled = false
            };

            try
            {
                if (runtimeConfigTh.bUsing7Z)
                {
                    timer.Elapsed += (state, e) =>
                    {
                        if (!started)
                        {
                            timer.Stop();
                            timer.Dispose();

                            return;
                        }
                        //确定文件夹存在且合法
                        if (!System.IO.Directory.Exists(runtimeConfigTh.frompath))
                        {
                            LogToListbox($"{runtimeConfigTh.frompath}不存在", true);
                            return;
                        }
                        //确定文件名
                        DateTime dt = DateTime.Now;
                        string filename = dt.ToString("yyMMddhhmmssffffff");

                        filename += ".7z";

                        string savepath = runtimeConfigTh.topath + "\\" + filename;

                        //本应该检测文件是否存在，但是由于使用时间戳的6位毫秒形式保存文件，则不太可能存在一样的文件。

                        Debug.WriteLine("7z attempt to compress path " + runtimeConfigTh.frompath + "/*.*");

                        Task task = new Task(() => _7zHelper.CompressDirectory(runtimeConfigTh.frompath + "/*.*", savepath, runtimeConfigTh.isFastMode));
                        try
                        {
                            //将进度条设置为Marquee
                            DelegateSetMarqueeStyle delegateSetMarqueeStyle = new DelegateSetMarqueeStyle(SetMarqueeStyle);
                            this.BeginInvoke((SetMarqueeStyle));

                            task.Start();
                            task.Wait();

                        }
                        catch (Exception ex)
                        {
                            NotifyByStatusBar($"压缩时出现错误: {ex.Message}");
                            progOperation.BackColor = Color.Red;
                        }
                        finally
                        {
                            //通过状态栏通知用户
                            DelegateNotifyByStatusBar delegateNotifyByStatusBar = new DelegateNotifyByStatusBar(NotifyByStatusBar);
                            this.BeginInvoke(delegateNotifyByStatusBar, new object[] { "存储到:" + savepath });

                            //确定文件夹大小
                            string[] files = Directory.GetFiles(runtimeConfigTh.frompath, "*", SearchOption.AllDirectories);
                            long totalSize = CalculateDirectorySize(files);

                            //向ListView中添加详细记录
                            DelegateModifylvDetail delegateModifylvDetail = new DelegateModifylvDetail(ModifylvDetail);
                            this.BeginInvoke(delegateModifylvDetail, new object[] { savepath, dt, totalSize });

                            //将进度条设置为Regular
                            DelegateSetRegularStyle delegateSetRegularStyle = new DelegateSetRegularStyle(SetRegularStyle);
                            this.BeginInvoke((SetRegularStyle));
                        }
                    };
                }
                else
                {
                    timer.Elapsed += (state, e) =>
                    {
                        //确定文件名
                        DateTime dt = DateTime.Now;
                        string filename = dt.ToString("yyMMddhhmmssffffff");

                        if (runtimeConfigTh.bUsing7Z) { filename += ".7z"; }
                        else { filename += ".zip"; }

                        string savepath = runtimeConfigTh.topath + "\\" + filename;

                        string sourceDirectory = runtimeConfigTh.frompath;
                        string zipFilePath = savepath;

                        progOperation.Minimum = 0;
                        progOperation.Maximum = 100;
                        progOperation.Value = 0;

                        progOperation.BackColor = Color.Green;

                        Task task = new Task(() => CompressDirectory(sourceDirectory, zipFilePath, true));
                        try
                        {
                            task.Start();
                        }
                        catch (Exception ex)
                        {
                            NotifyByStatusBar($"压缩时出现错误: {ex.Message}");
                            progOperation.BackColor = Color.Red;
                            //progOperation.Value = 100;
                        }
                        finally
                        {
                            //通过状态栏通知用户
                            DelegateNotifyByStatusBar delegateNotifyByStatusBar = new DelegateNotifyByStatusBar(NotifyByStatusBar);
                            this.BeginInvoke(delegateNotifyByStatusBar, new object[] { savepath + " saved." });

                            //确定文件夹大小
                            string[] files = Directory.GetFiles(runtimeConfigTh.frompath, "*", SearchOption.AllDirectories);
                            long totalSize = CalculateDirectorySize(files);

                            //向ListView中添加详细记录
                            DelegateModifylvDetail delegateModifylvDetail = new DelegateModifylvDetail(ModifylvDetail);
                            this.BeginInvoke(delegateModifylvDetail, new object[] { savepath, dt, totalSize });
                        }
                    };
                }

                timer.Start();
            }
            catch (Exception ex)
            {
                LogToListbox($"线程出现故障:{ex.Message}", true);
                if (timer.Enabled)
                {
                    timer.Stop();
                    timer.Dispose();
                }
                return;
            }
        }
        /// <summary>
        /// 状态栏计时器，用于确定什么时候清除状态栏
        /// </summary>
        System.Timers.Timer tmStatusBar;
        delegate void DelegateNotifyByStatusBar(string info);
        /// <summary>
        /// 向状态栏中写入一条临时消息
        /// </summary>
        /// <param name="info"></param>
        public void NotifyByStatusBar(string info)
        {
            if (tmStatusBar != null)
            {
                tmStatusBar.Stop();
            }
            LogToListbox(info);
            toolStripStatusLabel1.Text = info;
            tmStatusBar = new System.Timers.Timer()
            {
                //设置5s之后清空状态栏
                Interval = 5000,
                Enabled = false
            };
            tmStatusBar.Elapsed += (state, e) =>
            {
                toolStripStatusLabel1.Text = "就绪";
                tmStatusBar.Stop();
                tmStatusBar.Dispose();
            };
            tmStatusBar.Start();

        }
        delegate void DelegateSetMarqueeStyle();
        delegate void DelegateSetRegularStyle();

        /// <summary>
        /// 在线程中设置ProgressBar的样式为Marquee
        /// </summary>
        public void SetMarqueeStyle()
        {
            // 使用委托设置ProgressBar的样式为Marquee
            DelegateSetMarqueeStyle setMarqueeStyle = () =>
            {
                progOperation.Style = ProgressBarStyle.Marquee;
            };

            // 使用控件的Invoke方法确保在主线程中执行设置样式的操作
            if (progOperation.InvokeRequired)
            {
                progOperation.BeginInvoke(setMarqueeStyle);
            }
            else
            {
                setMarqueeStyle();
            }
        }

        /// <summary>
        /// 在线程中设置ProgressBar的样式为Regular
        /// </summary>
        public void SetRegularStyle()
        {
            // 使用委托设置ProgressBar的样式为Regular
            DelegateSetRegularStyle setRegularStyle = () =>
            {
                progOperation.Style = ProgressBarStyle.Blocks;
            };

            // 使用控件的Invoke方法确保在主线程中执行设置样式的操作
            if (progOperation.InvokeRequired)
            {
                progOperation.BeginInvoke(setRegularStyle);
            }
            else
            {
                setRegularStyle();
            }
        }

        /// <summary>
        /// 向日志栏中添加一条日志
        /// </summary>
        /// <param name="info"></param>
        /// <param name="bError">如果设置为True，则标记该行。</param>
        public void LogToListbox(string info, bool bError = false)
        {
            // 使用当前时间创建时间戳
            string timestamp = DateTime.Now.ToString("[HH:mm:ss]");

            // 格式化日志字符串
            string formattedLog = "+";
            if (bError) formattedLog = "!";
            formattedLog += $"{timestamp} {info}";

            // 向 ListBox 中添加日志
            lbLogs.Items.Add(formattedLog);
        }
        /// <summary>
        /// 允许日志栏执行复制。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbLogs_KeyDown(object sender, KeyEventArgs e)
        {
            // 检查是否按下了 Ctrl 键和 C 键
            if (e.Control && e.KeyCode == Keys.C)
            {
                if (lbLogs.SelectedItems.Count <= 0) return;
                // 创建一个 StringBuilder 来存储选定项的文本
                var selectedItemsText = new List<string>();
                foreach (var selectedItem in lbLogs.SelectedItems)
                {
                    selectedItemsText.Add(lbLogs.GetItemText(selectedItem));
                }

                // 将选定项的文本连接成一个字符串，每个项之间用换行符分隔
                string textToCopy = string.Join(Environment.NewLine, selectedItemsText);

                // 将文本复制到剪贴板
                Clipboard.SetText(textToCopy);
            }
        }

        delegate void DelegateModifylvDetail(string name, DateTime date, long size);

        /// <summary>
        /// 向详细信息ListView中增加记录
        /// </summary>
        /// <param name="name"></param>
        /// <param name="date"></param>
        /// <param name="size"></param>
        public void ModifylvDetail(string name, DateTime date, long size)
        {
            int delete_max = 2;
            if (cbAutoDelete.Checked)
            {
                if (lvDetails.Items.Count >= delete_max)
                {
                    if (System.IO.File.Exists(runtimeConfig.topath + "\\" + lvDetails.Items[0].SubItems[0].Text))
                    {
                        System.IO.File.Delete(runtimeConfig.topath + "\\" + lvDetails.Items[0].SubItems[0].Text);
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

            string strSize = "";
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
            else if ((int)((size / 1024) / 1024 / 1024) <= 1024)
            {
                strSize = (size / 1024.00 / 1024.00 / 1024.00).ToString("f2") + "GB";
            }
            else
            {
                strSize = "LARGE:" + size.ToString() + "B";
            }

            lvitem.SubItems.Add(strSize);

            lvDetails.Items.Add(lvitem);
            lvDetails.Items[lvDetails.Items.Count - 1].Selected = true;
            lvDetails.Items[lvDetails.Items.Count - 1].EnsureVisible();
        }

        private bool started = false;
        private void btnSave_Click(object sender, EventArgs e)
        {
            //更新并保存配置
            SaveConfig();

            pthsChild = new ParameterizedThreadStart(ArchiveCallee);
            thChild = new Thread(pthsChild);
            thChild.IsBackground = true;

            if (!started)
            {
                if (tbFrom.Text != "" && tbSaveto.Text != "")
                {
                    thChild.Start(runtimeConfig);
                    LogToListbox($"线程 {thChild.ManagedThreadId} 启动");

                    started = true;
                    btnSave.Text = "终止";
                    btnSave.BackColor = Color.Red;
                }
            }
            else
            {
                //unsafe exit method:
                //using bool to notify a loop thread.
                btnSave.Text = "存档";
                btnSave.BackColor = Color.Green;
                started = false;
            }
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            if(runtimeConfig._7zpath == "")
            {
                MessageBox.Show("由于7z文件缺失，暂时无法完成解压缩操作。","错误",MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DelegateNotifyByStatusBar delegateNotifyByStatusBar;
            if (thChild != null && started)
            {
                btnSave.Text = "存档";
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
                SevenZipCmdHelper zipHelper = new SevenZipCmdHelper(runtimeConfig._7zpath);
                if (cbAutoClear.Checked)
                {
                    if (System.IO.Directory.Exists(runtimeConfig.topath + "\\" + ".bak"))
                    {
                        DirectoryInfo dirInfo = new DirectoryInfo(runtimeConfig.topath + "\\" + ".bak");
                        long dirSize = 0;
                        GetDirSizeByPath(runtimeConfig.topath + "\\" + ".bak", ref dirSize);
                        DialogResult dr = MessageBox.Show(
                            "上一个存档(或者是同名称的)文件夹存在\n删除吗?\n\n"
                            + dirInfo.FullName + "\n"
                            + "最后修改: " + dirInfo.LastWriteTime.ToString("yyyy/MM/dd HH:mm:ss.ffff") + "\n"
                            + "大小(KB): " + (int)(dirSize / 1024)
                            , "文件已存在", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dr == DialogResult.Yes)
                        {
                            try
                            {
                                System.IO.Directory.Delete(runtimeConfig.topath + "\\" + ".bak", true);
                            }
                            catch (IOException ioex)
                            {
                                LogToListbox($"删除时遇到IO问题:{ioex.Message}");
                                //Notify user by status bar
                                //repeatable structure.
                                delegateNotifyByStatusBar = new DelegateNotifyByStatusBar(NotifyByStatusBar);
                                this.BeginInvoke(delegateNotifyByStatusBar,
                                        new object[] { $"删除时遇到IO问题:{ioex.Message}" });
                            }
                            catch (Exception ex)
                            {

                                LogToListbox($"删除时遇到问题:{ex.Message}");
                                //Notify user by status bar
                                //repeatable structure.
                                delegateNotifyByStatusBar = new DelegateNotifyByStatusBar(NotifyByStatusBar);
                                this.BeginInvoke(delegateNotifyByStatusBar,
                                        new object[] { $"删除时遇到问题:{ex.Message}" });
                            }
                        }
                    }
                    System.IO.Directory.CreateDirectory(runtimeConfig.topath + "\\" + ".bak");
                    MoveFiles(runtimeConfig.frompath + "\\", runtimeConfig.topath + "\\" + ".bak");
                }
                Task task = new Task(() => zipHelper.DecompressFileToDestDirectory(runtimeConfig.topath + "\\" + name, runtimeConfig.frompath));
                try
                {
                    //将进度条设置为Marquee
                    DelegateSetMarqueeStyle delegateSetMarqueeStyle = new DelegateSetMarqueeStyle(SetMarqueeStyle);
                    this.BeginInvoke((SetMarqueeStyle));

                    task.Start();
                    task.Wait();

                }
                catch (Exception ex)
                {
                    NotifyByStatusBar($"解压缩时出现错误: {ex.Message}");
                    progOperation.BackColor = Color.Red;
                }
                finally
                {
                    //通过状态栏通知用户
                    delegateNotifyByStatusBar = new DelegateNotifyByStatusBar(NotifyByStatusBar);
                    this.BeginInvoke(delegateNotifyByStatusBar, new object[] { "解压缩到:" + runtimeConfig.frompath });

                    //将进度条设置为Regular
                    DelegateSetRegularStyle delegateSetRegularStyle = new DelegateSetRegularStyle(SetRegularStyle);
                    this.BeginInvoke((SetRegularStyle));
                }
            }
            else
            {
                //通过状态栏通知用户
                delegateNotifyByStatusBar = new DelegateNotifyByStatusBar(NotifyByStatusBar);
                this.BeginInvoke(delegateNotifyByStatusBar, new object[] { "没有存档:" + runtimeConfig.frompath });
            }
        }
        /// <summary>
        /// 获取文件夹大小
        /// </summary>
        /// <param name="path">文件夹路径</param>
        /// <returns></returns>
        private long CalculateDirectorySize(string path)
        {
            long totalSize = 0;
            string[] files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                totalSize += new FileInfo(file).Length;
            }
            return totalSize;
        }
        /// <summary>
        /// 获取文件集大小
        /// </summary>
        /// <param name="files">文件路径集</param>
        /// <returns></returns>
        private long CalculateDirectorySize(string[] files)
        {
            long totalSize = 0;
            foreach (string file in files)
            {
                totalSize += new FileInfo(file).Length;
            }
            return totalSize;
        }
        /// <summary>
        /// 用原生ZipFile创建存档
        /// </summary>
        /// <param name="sourceDirectory"></param>
        /// <param name="zipFilePath"></param>
        private void CompressDirectory(string sourceDirectory, string zipFilePath, bool replace = false)
        {
            if (!System.IO.Directory.Exists(sourceDirectory)) throw new FileNotFoundException($"Source directory {sourceDirectory} is not found.");
            if (System.IO.File.Exists(zipFilePath) && !replace) throw new IOException($"文件 {zipFilePath} 已存在");
            if (replace) { if (System.IO.File.Exists(zipFilePath)) File.Delete(zipFilePath); }

            string[] files = Directory.GetFiles(sourceDirectory, "*", SearchOption.AllDirectories);

            // Get the total size of the directory
            long totalSize = CalculateDirectorySize(files);

            string basePath = sourceDirectory + "\\";

            long processedFileSize = 0;

            using (ZipArchive zip = ZipFile.Open(zipFilePath, ZipArchiveMode.Create))
            {
                foreach (string file in files)
                {
                    try
                    {
                        string relativePath = file.Replace(basePath, "");
                        zip.CreateEntryFromFile(file, relativePath);

                        processedFileSize += new FileInfo(file).Length;
                        int progressPercentage = (int)((double)processedFileSize / totalSize * 100);
                        UpdateProgress(progressPercentage);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"压缩 {file} 时遇到问题： {ex.Message}");
                        continue;
                    }
                }
            }
        }
        /// <summary>
        /// 更新进度条进度
        /// </summary>
        /// <param name="value"></param>
        private void UpdateProgress(int value)
        {
            if (progOperation.InvokeRequired)
            {
                progOperation.BeginInvoke(new Action<int>(UpdateProgress), value);
            }
            else
            {
                progOperation.Value = value;
            }
        }


        public bool ManuallySaved = false;
        private void btnManSave_Click(object sender, EventArgs e)
        {

            //更新并保存配置
            SaveConfig();

            //确定文件夹存在且合法
            if (!System.IO.Directory.Exists(runtimeConfig.frompath))
            {
                LogToListbox($"{runtimeConfig.frompath}不存在", true);
                return;
            }
            //确定文件名
            DateTime dt = DateTime.Now;
            string filename = dt.ToString("yyMMddhhmmssffffff");

            if (runtimeConfig.bUsing7Z) { filename += ".7z"; }
            else { filename += ".zip"; }

            string savepath = runtimeConfig.topath + "\\" + filename;

            if (runtimeConfig.bUsing7Z)
            {
                SevenZipCmdHelper _7zHelper = new SevenZipCmdHelper(runtimeConfig._7zpath);
                try
                {
                    Task task = new Task(() =>
                    {
                        //本应该检测文件是否存在，但是由于使用时间戳的6位毫秒形式保存文件，则不太可能存在一样的文件。

                        Debug.WriteLine("7z attempt to compress path " + runtimeConfig.frompath + "/*.*");

                        Task task = new Task(() => _7zHelper.CompressDirectory(runtimeConfig.frompath + "/*.*", savepath, runtimeConfig.isFastMode));
                        try
                        {
                            //将进度条设置为Marquee
                            DelegateSetMarqueeStyle delegateSetMarqueeStyle = new DelegateSetMarqueeStyle(SetMarqueeStyle);
                            this.BeginInvoke((SetMarqueeStyle));

                            task.Start();
                            task.Wait();

                        }
                        catch (Exception ex)
                        {
                            NotifyByStatusBar($"压缩时出现错误: {ex.Message}");
                            progOperation.BackColor = Color.Red;
                        }
                        finally
                        {
                            //通过状态栏通知用户
                            DelegateNotifyByStatusBar delegateNotifyByStatusBar = new DelegateNotifyByStatusBar(NotifyByStatusBar);
                            this.BeginInvoke(delegateNotifyByStatusBar, new object[] { "存储到:" + savepath });

                            //确定文件夹大小
                            string[] files = Directory.GetFiles(runtimeConfig.frompath, "*", SearchOption.AllDirectories);
                            long totalSize = CalculateDirectorySize(files);

                            //向ListView中添加详细记录
                            DelegateModifylvDetail delegateModifylvDetail = new DelegateModifylvDetail(ModifylvDetail);
                            this.BeginInvoke(delegateModifylvDetail, new object[] { savepath, dt, totalSize });

                            //将进度条设置为Regular
                            DelegateSetRegularStyle delegateSetRegularStyle = new DelegateSetRegularStyle(SetRegularStyle);
                            this.BeginInvoke((SetRegularStyle));
                        }
                    });
                    task.Start();

                }
                catch (Exception ex)
                {
                    LogToListbox($"压缩时出现错误: {ex.Message}");
                    Debug.WriteLine("Exception@thread: " + ex.Message);
                    return;
                }
            }
            else
            {
                string sourceDirectory = runtimeConfig.frompath;
                string zipFilePath = savepath;

                // Disable compress button during compression
                btnManSave.Enabled = false;

                // Initialize progress bar
                progOperation.Minimum = 0;
                progOperation.Maximum = 100;
                progOperation.Value = 0;

                progOperation.BackColor = Color.Green;

                Task task = new Task(() =>
                    {
                        CompressDirectory(sourceDirectory, zipFilePath, true);
                    }
                );
                try
                {
                    task.Start();
                }
                catch (Exception ex)
                {
                    NotifyByStatusBar($"压缩时出现错误: {ex.Message}");
                    progOperation.BackColor = Color.Red;
                    progOperation.Value = 100;
                }
                finally
                {
                    btnManSave.Enabled = true;
                    //通过状态栏通知用户
                    DelegateNotifyByStatusBar delegateNotifyByStatusBar = new DelegateNotifyByStatusBar(NotifyByStatusBar);
                    this.BeginInvoke(delegateNotifyByStatusBar, new object[] { "存储到:" + savepath });

                    //确定文件夹大小
                    string[] files = Directory.GetFiles(runtimeConfig.frompath, "*", SearchOption.AllDirectories);
                    long totalSize = CalculateDirectorySize(files);

                    //向ListView中添加详细记录
                    DelegateModifylvDetail delegateModifylvDetail = new DelegateModifylvDetail(ModifylvDetail);
                    this.BeginInvoke(delegateModifylvDetail, new object[] { savepath, dt, totalSize });
                }
                return;
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveConfig();
        }
    }
}
