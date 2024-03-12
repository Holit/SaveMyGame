using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO.Compression;
using System.Windows.Forms;
using SaveMyGame.src;
using SaveMyGame.src.Models;
using SharpCompress.Common;

namespace SaveMyGame
{
    public partial class MainForm : Form
    {
        static ApplicationConfig runtimeConfig = new();
        static readonly ProgramDbContext db = new();

        /// <summary>
        /// 从数据库恢复配置
        /// </summary>
        void ReadConfig()
        {
            runtimeConfig = db.ApplicationConfigs.FirstOrDefault() ?? new();
            if (runtimeConfig == default(ApplicationConfig)) db.ApplicationConfigs.Add(runtimeConfig);
            db.SaveChanges();
            tbFrom.Text = runtimeConfig.FromPath;
            tbSaveto.Text = runtimeConfig.ToPath;
            if (runtimeConfig.Interval <= 0) runtimeConfig.Interval = 60;
            nudTime.Value = runtimeConfig.Interval;
            cbFast.Checked = runtimeConfig.IsFastMode;
            cbUsing7z.Checked = runtimeConfig.IsUsingLZMA;
            cbDeleteOldFiles.Checked = runtimeConfig.IsDeleteOldFiles;
            cbClearBeforeRestore.Checked = runtimeConfig.IsClearBeforeRestore;
        }

        /// <summary>
        /// 将运行时配置写入数据库
        /// </summary>
        void SaveConfig()
        {
            //更新配置
            runtimeConfig.FromPath = tbFrom.Text;
            runtimeConfig.ToPath = tbSaveto.Text;
            runtimeConfig.Interval = (int)nudTime.Value;
            runtimeConfig.IsFastMode = cbFast.Checked;
            runtimeConfig.IsUsingLZMA = cbUsing7z.Checked;
            runtimeConfig.IsDeleteOldFiles = cbDeleteOldFiles.Checked;
            runtimeConfig.IsClearBeforeRestore = cbClearBeforeRestore.Checked;
            db.SaveChanges();
        }
        /// <summary>
        ///  //跨盘移动
        /// </summary>
        /// <param name="sourcePath">源文件路径</param>
        /// <param name="destPath">新文件路径</param>
        /// <returns></returns>
        void MoveFiles(string sourcePath, string destPath) //文件移动函数
        {
            DirectoryInfo dir = new DirectoryInfo(sourcePath);
            if (Directory.Exists(sourcePath))
            {
                destPath = Path.Combine(destPath, dir.Name);
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
                    string destFilePath = Path.Combine(destPath, fileInfos[i].Name);
                    fileInfos[i].MoveTo(destFilePath);
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
            ReadConfig();
            //初始化表头
            lvDetails.Columns.Add("名称", (int)(0.3 * lvDetails.Size.Width));
            lvDetails.Columns.Add("日期", (int)(0.5 * lvDetails.Size.Width));
            lvDetails.Columns.Add("未压缩大小", (int)(0.2 * lvDetails.Size.Width));

            if (string.IsNullOrEmpty(runtimeConfig.ToPath))
            {
                tbSaveto.Text = Application.StartupPath;
            }
            else if (!Directory.Exists(runtimeConfig.ToPath))
            {
                tbSaveto.Text = Application.StartupPath;
            }
            //根据数据库更新ListView
            UpdatelvDetail();

        }
        private void btn_FromDirSel_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new()
            {
                //打开的文件夹浏览对话框上的描述  

                Description = "选择要存档的文件夹",
                //是否显示对话框左下角 新建文件夹 按钮，默认为 true  
                ShowNewFolderButton = false
            };
            string path = tbFrom.Text;
            dialog.SelectedPath = path;
            //按下确定选择的按钮  
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                //记录选中的目录  
                path = dialog.SelectedPath;
                if (!string.IsNullOrEmpty(path) && Directory.Exists(path))
                {
                    tbFrom.Text = path;
                    Debug.WriteLine("Game archive: " + path);
                }
            }
        }

        private void btn_ToDirSel_Click(object sender, EventArgs e)
        {

            FolderBrowserDialog dialog = new()
            {
                //打开的文件夹浏览对话框上的描述  
                Description = "存储到...",
                //是否显示对话框左下角 新建文件夹 按钮，默认为 true  
                ShowNewFolderButton = false
            };

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
            if (!runtimeConfig.IsRemindedSize)
            {
                GetDirSizeByPath(path, ref dirSize);
                //如果目标文件夹大小大于800M
                if ((dirSize / 1024) / 1024 > 800)
                {
                    MessageBox.Show("目标文件夹内容较多\n清除目标文件夹以节省计算机空间。",
                        "提醒",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                runtimeConfig.IsRemindedSize = true;
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

        /// <summary>
        /// 自动清理旧文件
        /// </summary>
        public void DeleteOldFiles()
        {
            int delete_max = 10;
            if (runtimeConfig.IsDeleteOldFiles)
            {
                IEnumerable<string> filesInFolder = Directory.EnumerateFiles(runtimeConfig.ToPath);
                List<FileRecord> fileRecords = db.FileRecords.ToList();

                // 筛选出文件夹中存在记录的文件
                List<string> existingFilesInFolder = filesInFolder.Where(filePath => fileRecords.Any(record => record.FilePath == filePath)).ToList();

                if (lvDetails.Items.Count >= delete_max)
                {
                    List<string> filesToDelete = existingFilesInFolder.OrderBy(filePath => File.GetCreationTime(filePath)).ToList();
                    int filesToDeleteCount = existingFilesInFolder.Count - delete_max;
                    for (int i = 0; i < filesToDeleteCount; i++)
                    {
                        string filePathToDelete = filesToDelete[i];
                        DeleteRecord(filePathToDelete, new FileInfo(filePathToDelete).Length);
                    }
                }
            }
        }

        /// <summary>
        /// 删除记录，同时删除记录对应的文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="size"></param>
        public void DeleteRecord(string filePath, long size)
        {
            //删除文件
            if (File.Exists(filePath))
            {
                if (new FileInfo(filePath).Length == size)
                {
                    // TODO:
                    // 增加回收而不是删除功能
                    new FileInfo(filePath).Delete();
                }
            }
            //删除数据库
            db.FileRecords.Remove(db.FileRecords.First(b => b.FilePath == filePath && b.Size == size));
            db.SaveChanges();
        }
        delegate void DelegateInsertRecord(string filePath, DateTime date, long size);
        public void InsertRecord(string filePath, DateTime date, long size)
        {
            //自动限制长度增长，根据配置文件
            DeleteOldFiles();
            //同步数据库和ListView
            UpdatelvDetail();
            db.FileRecords.Add(new FileRecord(filePath, runtimeConfig.FromPath!, date, size));
            db.SaveChanges();
            //向ListView中添加详细记录
            DelegateModifylvDetail delegateModifylvDetail = new DelegateModifylvDetail(ModifylvDetail);
            BeginInvoke(delegateModifylvDetail, new object[] { filePath, date, size });
        }
        delegate void DelegateModifylvDetail(string name, DateTime date, long size);
        /// <summary>
        /// 向详细信息ListView中增加记录
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="date"></param>
        /// <param name="size"></param>
        public void ModifylvDetail(string filePath, DateTime date, long size)
        {
            ListViewItem lvitem = new ListViewItem();
            lvitem.Text = Path.GetFileName(filePath);
            lvitem.SubItems.Add(date.ToString("yyyy/MM/dd HH:mm:ss.ffff"));

            string strSize = FormatSize(size);

            lvitem.SubItems.Add(strSize);

            lvDetails.Items.Add(lvitem);
            lvDetails.Items[lvDetails.Items.Count - 1].Selected = true;
            lvDetails.Items[lvDetails.Items.Count - 1].EnsureVisible();
        }

        public static string FormatSize(long size)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = size;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len /= 1024;
            }

            return string.Format("{0:0.##} {1}", len, sizes[order]);
        }

        public void UpdatelvDetail()
        {
            VerifydbPath();
            lvDetails.Items.Clear();
            List<FileRecord> fileRecords = db.FileRecords.ToList();
            foreach (var fileRecord in fileRecords)
            {
                ModifylvDetail(fileRecord.FilePath, fileRecord.Date, fileRecord.Size);
            }
        }
        public void VerifydbPath()
        {
            List<FileRecord> fileRecords = db.FileRecords.ToList();
            foreach (var fileRecord in fileRecords)
            {
                if (!File.Exists(fileRecord.FilePath))
                {
                    db.FileRecords.RemoveRange(db.FileRecords.Where(b => b.FilePath == fileRecord.FilePath && b.Size == fileRecord.Size));
                }
            }
            db.SaveChanges();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            //更新并保存配置
            SaveConfig();

            if (!timer1.Enabled)
            {
                if (tbFrom.Text != "" && tbSaveto.Text != "")
                {
                    timer1.Interval = runtimeConfig.Interval * 1000;
                    timer1.Start();
                    LogToListbox($"线程启动");
                    btnSave.Text = "终止";
                    btnSave.BackColor = Color.Red;
                }
            }
            else
            {
                //unsafe exit method:
                //using bool to notify a loop thread.
                timer1.Stop();
                btnSave.Text = "存档";
                btnSave.BackColor = Color.Green;
            }
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            //无效操作
            //按下恢复后，终止计时器
            if (timer1.Enabled)
            {
                timer1.Stop();
                btnSave.Text = "存档";
                btnSave.BackColor = Color.Green;
            }

            DelegateNotifyByStatusBar delegateNotifyByStatusBar;
            //确定文件列表中存在文件
            if (lvDetails.Items.Count > 0)
            {
                //如果未选中，默认恢复最近的一个
                if (lvDetails.SelectedItems.Count == 0)
                {
                    lvDetails.Items[lvDetails.Items.Count - 1].Selected = true;
                }

                string name = lvDetails.SelectedItems[0].SubItems[0].Text;
                FileRecord zipToRestore = db.FileRecords.First(b => b.FilePath.Contains(name));


                //如果指定了恢复前清空目标文件夹
                Task taskRecyle = new Task(() => { });
                if (runtimeConfig.IsClearBeforeRestore)
                {
                    taskRecyle = new Task(() =>
                    {
                        if (Directory.Exists(runtimeConfig.FromPath))
                        {
                            Microsoft.VisualBasic.FileIO.FileSystem.DeleteDirectory(
                                                        runtimeConfig.FromPath + "\\",
                                                        Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs,
                                                        Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin);

                            //通过状态栏通知用户
                            delegateNotifyByStatusBar = new DelegateNotifyByStatusBar(NotifyByStatusBar);
                            this.BeginInvoke(delegateNotifyByStatusBar, new object[] { $"文件夹 {runtimeConfig.FromPath} 已移动到回收站" });
                        }

                        //将进度条设置为Regular
                        DelegateSetRegularStyle delegateSetRegularStyle = new DelegateSetRegularStyle(SetRegularStyle);
                        this.BeginInvoke((SetRegularStyle));
                    });

                    try
                    {
                        //将进度条设置为Marquee
                        DelegateSetMarqueeStyle delegateSetMarqueeStyle = new DelegateSetMarqueeStyle(SetMarqueeStyle);
                        this.BeginInvoke((SetMarqueeStyle));

                        taskRecyle.Start();
                    }
                    catch (Exception ex)
                    {
                        NotifyByStatusBar($"回收文件夹时出现错误: {ex.Message}");
                        progOperation.BackColor = Color.Red;
                    }

                }
                Task task = new Task(() =>
                {
                    if (runtimeConfig.IsClearBeforeRestore) taskRecyle.Wait();
                    string? restorePath = zipToRestore.RestorePath;
                    if (!Directory.Exists(restorePath))
                    {
                        if (!string.IsNullOrEmpty(restorePath))
                        {
                            Directory.CreateDirectory(restorePath);
                        }
                    }
                    ZipHelper.DecompressFileToDestDirectory(zipToRestore.FilePath, restorePath);

                    //通过状态栏通知用户
                    delegateNotifyByStatusBar = new DelegateNotifyByStatusBar(NotifyByStatusBar);
                    this.BeginInvoke(delegateNotifyByStatusBar, new object[] { "解压缩到:" + restorePath });

                    //将进度条设置为Regular
                    DelegateSetRegularStyle delegateSetRegularStyle = new DelegateSetRegularStyle(SetRegularStyle);
                    this.BeginInvoke((SetRegularStyle));
                });
                try
                {
                    //将进度条设置为Marquee
                    DelegateSetMarqueeStyle delegateSetMarqueeStyle = new DelegateSetMarqueeStyle(SetMarqueeStyle);
                    this.BeginInvoke((SetMarqueeStyle));

                    task.Start();
                }
                catch (Exception ex)
                {
                    NotifyByStatusBar($"解压缩时出现错误: {ex.Message}");
                    progOperation.BackColor = Color.Red;
                }
            }
            else
            {
                //通过状态栏通知用户
                delegateNotifyByStatusBar = new DelegateNotifyByStatusBar(NotifyByStatusBar);
                this.BeginInvoke(delegateNotifyByStatusBar, new object[] { "没有存档:" + runtimeConfig.FromPath });
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
        private void CompressDirectory(string? sourceDirectory, string zipFilePath, bool replace = false)
        {
            if (!Directory.Exists(sourceDirectory)) throw new FileNotFoundException($"Source directory {sourceDirectory} is not found.");
            if (File.Exists(zipFilePath) && !replace) throw new IOException($"文件 {zipFilePath} 已存在");
            if (replace) { if (File.Exists(zipFilePath)) File.Delete(zipFilePath); }

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
            if (!Directory.Exists(runtimeConfig.FromPath))
            {
                LogToListbox($"{runtimeConfig.FromPath}不存在", true);
                return;
            }
            //确定文件名
            DateTime dt = DateTime.Now;
            string filename = dt.ToString("yyMMddhhmmssffffff") + ".zip";

            string savepath = runtimeConfig.ToPath + "\\" + filename;

            // 禁用手动保存按钮
            btnManSave.Enabled = false;

            if (runtimeConfig.IsUsingLZMA)
            {
                try
                {
                    Task task = new Task(() =>
                    {
                        //本应该检测文件是否存在，但是由于使用时间戳的6位毫秒形式保存文件，则不太可能存在一样的文件。

                        Debug.WriteLine("zip attempt to compress path " + runtimeConfig.FromPath + "/*.*");

                        Task task = new Task(() =>
                        {
                            ZipHelper.CompressDirectory(runtimeConfig.FromPath , savepath, runtimeConfig.IsFastMode);

                            //通过状态栏通知用户
                            DelegateNotifyByStatusBar delegateNotifyByStatusBar = new DelegateNotifyByStatusBar(NotifyByStatusBar);
                            BeginInvoke(delegateNotifyByStatusBar, new object[] { "存储到:" + savepath });

                            //确定文件夹大小
                            string[] files = Directory.GetFiles(runtimeConfig.FromPath, "*", SearchOption.AllDirectories);
                            long totalSize = CalculateDirectorySize(files);


                            //更新记录
                            DelegateInsertRecord delegateInsertRecord = new DelegateInsertRecord(InsertRecord);
                            this.BeginInvoke(delegateInsertRecord, new object[] { savepath, dt, totalSize });

                            //将进度条设置为Regular
                            DelegateSetRegularStyle delegateSetRegularStyle = new DelegateSetRegularStyle(SetRegularStyle);
                            this.BeginInvoke((SetRegularStyle));
                        });
                        try
                        {
                            //将进度条设置为Marquee
                            DelegateSetMarqueeStyle delegateSetMarqueeStyle = new DelegateSetMarqueeStyle(SetMarqueeStyle);
                            this.BeginInvoke((SetMarqueeStyle));

                            task.Start();

                        }
                        catch (Exception ex)
                        {
                            NotifyByStatusBar($"压缩时出现错误: {ex.Message}");
                            progOperation.BackColor = Color.Red;
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
                string sourceDirectory = runtimeConfig.FromPath;
                string zipFilePath = savepath;


                // Initialize progress bar
                progOperation.Minimum = 0;
                progOperation.Maximum = 100;
                progOperation.Value = 0;

                progOperation.BackColor = Color.Green;

                Task task = new Task(() =>
                    {
                        CompressDirectory(sourceDirectory, zipFilePath, true);

                        //通过状态栏通知用户
                        DelegateNotifyByStatusBar delegateNotifyByStatusBar = new DelegateNotifyByStatusBar(NotifyByStatusBar);
                        this.BeginInvoke(delegateNotifyByStatusBar, new object[] { "存储到:" + savepath });

                        //确定文件夹大小
                        string[] files = Directory.GetFiles(runtimeConfig.FromPath, "*", SearchOption.AllDirectories);
                        long totalSize = CalculateDirectorySize(files);

                        //更新记录
                        DelegateInsertRecord delegateInsertRecord = new DelegateInsertRecord(InsertRecord);
                        this.BeginInvoke(delegateInsertRecord, new object[] { savepath, dt, totalSize });
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
            }

            btnManSave.Enabled = true;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveConfig();
        }

        private void cbFast_CheckedChanged(object sender, EventArgs e)
        {
            runtimeConfig.IsFastMode = cbFast.Checked;
        }

        private void cbUsing7z_CheckedChanged(object sender, EventArgs e)
        {
            runtimeConfig.IsUsingLZMA = cbUsing7z.Checked;
        }

        private void cbDeleteOldFiles_CheckedChanged(object sender, EventArgs e)
        {
            runtimeConfig.IsDeleteOldFiles = cbDeleteOldFiles.Checked;
        }

        private void cbClearBeforeRestore_CheckedChanged(object sender, EventArgs e)
        {
            runtimeConfig.IsClearBeforeRestore = cbClearBeforeRestore.Checked;
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            if (runtimeConfig.IsUsingLZMA)
            {
                if (!Directory.Exists(runtimeConfig.FromPath))
                {
                    LogToListbox($"{runtimeConfig.FromPath}不存在", true);
                    return;
                }
                //确定文件名
                DateTime dt = DateTime.Now;
                string filename = dt.ToString("yyMMddhhmmssffffff") + ".zip";

                string savepath = runtimeConfig.ToPath + "\\" + filename;

                //本应该检测文件是否存在，但是由于使用时间戳的6位毫秒形式保存文件，则不太可能存在一样的文件。

                Debug.WriteLine("zip attempt to compress path " + runtimeConfig.FromPath + "/*.*");

                Task task = new Task(() =>
                {
                    ZipHelper.CompressDirectory(runtimeConfig.FromPath, savepath, runtimeConfig.IsFastMode);
                    //通过状态栏通知用户
                    DelegateNotifyByStatusBar delegateNotifyByStatusBar = new DelegateNotifyByStatusBar(NotifyByStatusBar);
                    BeginInvoke(delegateNotifyByStatusBar, new object[] { "存储到:" + savepath });

                    //确定文件夹大小
                    string[] files = Directory.GetFiles(runtimeConfig.FromPath, "*", SearchOption.AllDirectories);
                    long totalSize = CalculateDirectorySize(files);

                    //更新记录
                    DelegateInsertRecord delegateInsertRecord = new DelegateInsertRecord(InsertRecord);
                    BeginInvoke(delegateInsertRecord, new object[] { savepath, dt, totalSize });

                    //将进度条设置为Regular
                    DelegateSetRegularStyle delegateSetRegularStyle = new DelegateSetRegularStyle(SetRegularStyle);
                    BeginInvoke((SetRegularStyle));
                });
                try
                {
                    //将进度条设置为Marquee
                    DelegateSetMarqueeStyle delegateSetMarqueeStyle = new DelegateSetMarqueeStyle(SetMarqueeStyle);
                    BeginInvoke((SetMarqueeStyle));

                    task.Start();
                    task.Wait();

                }
                catch (Exception ex)
                {
                    NotifyByStatusBar($"压缩时出现错误: {ex.Message}");
                    progOperation.BackColor = Color.Red;
                }
            }
            else
            {
                //确定文件名
                DateTime dt = DateTime.Now;
                string filename = dt.ToString("yyMMddhhmmssffffff") + ".zip";

                string savepath = runtimeConfig.ToPath + "\\" + filename;

                string? sourceDirectory = runtimeConfig.FromPath;
                string zipFilePath = savepath;

                progOperation.Minimum = 0;
                progOperation.Maximum = 100;
                progOperation.Value = 0;

                progOperation.BackColor = Color.Green;

                Task task = new Task(() =>
                {
                    CompressDirectory(sourceDirectory, zipFilePath, true);
                    //通过状态栏通知用户
                    DelegateNotifyByStatusBar delegateNotifyByStatusBar = new DelegateNotifyByStatusBar(NotifyByStatusBar);
                    this.BeginInvoke(delegateNotifyByStatusBar, new object[] { savepath + " saved." });

                    //确定文件夹大小
                    string[] files = Directory.GetFiles(runtimeConfig.FromPath, "*", SearchOption.AllDirectories);
                    long totalSize = CalculateDirectorySize(files);

                    //更新记录
                    DelegateInsertRecord delegateInsertRecord = new DelegateInsertRecord(InsertRecord);
                    this.BeginInvoke(delegateInsertRecord, new object[] { savepath, dt, totalSize });
                });
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
            }
        }
    }
}
