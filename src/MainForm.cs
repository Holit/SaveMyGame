using System.Data;
using System.Diagnostics;
using System.IO.Compression;
using SaveMyGame.src;
using SaveMyGame.src.Models;

namespace SaveMyGame
{
    public partial class MainForm : Form
    {
        ApplicationConfig runtimeConfig = new();

        /// <summary>
        /// 从数据库恢复配置
        /// </summary>
        void ReadConfig()
        {
            using var db = ProgramDbContext.Create();
            runtimeConfig = db.ApplicationConfigs.FirstOrDefault();
            if (runtimeConfig == default(ApplicationConfig))
            {
                runtimeConfig = new();
                db.ApplicationConfigs.Add(runtimeConfig);
                db.SaveChanges();
            }
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
            using var db = ProgramDbContext.Create();
            db.ApplicationConfigs.Update(runtimeConfig);
            db.SaveChanges();
        }
        /// <summary>
        /// Validates that a resolved path stays within an allowed base directory (prevents path traversal).
        /// </summary>
        private static bool IsPathWithinDirectory(string path, string baseDirectory)
        {
            string fullPath = Path.GetFullPath(path);
            string fullBase = Path.GetFullPath(baseDirectory);
            return fullPath.StartsWith(fullBase + Path.DirectorySeparatorChar)
                || fullPath == fullBase;
        }

        /// <summary>
        /// 获取某一文件夹的大小
        /// </summary>
        /// <param name="dir">文件夹目录</param>
        /// <param name="dirSize">文件夹大小</param>
        private void GetDirSizeByPath(string dir, ref long dirSize)
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
                LogToListbox("获取文件夹大小失败:" + ex.Message, true);
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
                tmStatusBar.Dispose();
            }
            LogToListbox(info);
            toolStripStatusLabel1.Text = info;
            var timer = new System.Timers.Timer()
            {
                //设置5s之后清空状态栏
                Interval = 5000,
                Enabled = false
            };
            timer.Elapsed += (state, e) =>
            {
                BeginInvoke(() => toolStripStatusLabel1.Text = "就绪");
                timer.Stop();
                timer.Dispose();
            };
            tmStatusBar = timer;
            timer.Start();

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
                using var db = ProgramDbContext.Create();
                List<FileRecord> fileRecords = db.FileRecords.ToList();

                // 筛选出文件夹中存在记录的文件
                List<string> existingFilesInFolder = filesInFolder.Where(filePath => fileRecords.Any(record => record.FilePath == filePath)).ToList();

                if (existingFilesInFolder.Count > delete_max)
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
            if (string.IsNullOrEmpty(runtimeConfig.ToPath) || !IsPathWithinDirectory(filePath, runtimeConfig.ToPath))
            {
                LogToListbox($"拒绝删除: 文件路径超出存档目录范围", true);
                return;
            }
            //先删除数据库记录，再删除文件，避免数据不一致
            using var db = ProgramDbContext.Create();
            var record = db.FileRecords.FirstOrDefault(b => b.FilePath == filePath && b.Size == size);
            if (record != null)
            {
                db.FileRecords.Remove(record);
                db.SaveChanges();
            }
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
        }
        delegate void DelegateInsertRecord(string filePath, DateTime date, long size);
        public void InsertRecord(string filePath, DateTime date, long size)
        {
            //自动限制长度增长，根据配置文件
            DeleteOldFiles();
            //同步数据库和ListView
            UpdatelvDetail();
            using var db = ProgramDbContext.Create();
            string restorePath = runtimeConfig.FromPath ?? tbFrom.Text;
            db.FileRecords.Add(new FileRecord(filePath, restorePath, date, size));
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
            using var db = ProgramDbContext.Create();
            List<FileRecord> fileRecords = db.FileRecords.ToList();
            foreach (var fileRecord in fileRecords)
            {
                ModifylvDetail(fileRecord.FilePath, fileRecord.Date, fileRecord.Size);
            }
        }
        public void VerifydbPath()
        {
            using var db = ProgramDbContext.Create();
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
                    StartFileWatcher();
                    LogToListbox($"自动存档已启动 (防抖:{DebounceIntervalMs / 1000}s, 冷却:{runtimeConfig.Interval}s)");
                    btnSave.Text = "终止";
                    btnSave.BackColor = Color.Red;
                }
            }
            else
            {
                timer1.Stop();
                StopFileWatcher();
                btnSave.Text = "存档";
                btnSave.BackColor = Color.Green;
            }
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            // 按下恢复后，终止计时器和文件监控
            if (timer1.Enabled)
            {
                timer1.Stop();
                StopFileWatcher();
                btnSave.Text = "存档";
                btnSave.BackColor = Color.Green;
            }

            if (lvDetails.Items.Count == 0)
            {
                NotifyByStatusBar("没有存档:" + runtimeConfig.FromPath);
                return;
            }

            // 如果未选中，默认恢复最近的一个
            if (lvDetails.SelectedItems.Count == 0)
            {
                lvDetails.Items[lvDetails.Items.Count - 1].Selected = true;
            }

            string name = lvDetails.SelectedItems[0].SubItems[0].Text;
            using var db = ProgramDbContext.Create();
            FileRecord? zipToRestore = db.FileRecords.FirstOrDefault(b => b.FilePath.EndsWith(name));
            if (zipToRestore == null)
            {
                NotifyByStatusBar($"未找到匹配的存档记录: {name}");
                return;
            }

            DoRestore(zipToRestore);
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

            long processedFileSize = 0;

            using (ZipArchive zip = ZipFile.Open(zipFilePath, ZipArchiveMode.Create))
            {
                foreach (string file in files)
                {
                    try
                    {
                        string relativePath = Path.GetRelativePath(sourceDirectory, file);
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

        /// <summary>
        /// 防止压缩任务堆积的标志位
        /// </summary>
        private volatile bool _isCompressing = false;

        /// <summary>
        /// 文件系统监视器，用于检测源文件夹的变化
        /// </summary>
        private FileSystemWatcher? _fileWatcher;

        /// <summary>
        /// 防抖计时器：源文件夹发生变化后重置，超时后触发备份
        /// </summary>
        private System.Timers.Timer? _debounceTimer;

        /// <summary>
        /// 标记自上次备份后源文件夹是否有变化
        /// </summary>
        private volatile bool _hasPendingChanges = false;

        /// <summary>
        /// 上次备份完成的时间，用于控制冷却期
        /// </summary>
        private DateTime _lastBackupTime = DateTime.MinValue;

        /// <summary>
        /// 防抖等待时间（毫秒）：文件夹最后变化后等待此时间再触发备份
        /// </summary>
        private const int DebounceIntervalMs = 10000;

        /// <summary>
        /// 启动 FileSystemWatcher 监控源文件夹变化
        /// </summary>
        private void StartFileWatcher()
        {
            StopFileWatcher();

            if (string.IsNullOrEmpty(runtimeConfig.FromPath) || !Directory.Exists(runtimeConfig.FromPath))
                return;

            _fileWatcher = new FileSystemWatcher(runtimeConfig.FromPath)
            {
                IncludeSubdirectories = true,
                NotifyFilter = NotifyFilters.FileName
                             | NotifyFilters.DirectoryName
                             | NotifyFilters.LastWrite
                             | NotifyFilters.Size,
                EnableRaisingEvents = true
            };

            _fileWatcher.Changed += OnFileSystemChanged;
            _fileWatcher.Created += OnFileSystemChanged;
            _fileWatcher.Deleted += OnFileSystemChanged;
            _fileWatcher.Renamed += OnFileSystemChanged;

            _debounceTimer = new System.Timers.Timer(DebounceIntervalMs)
            {
                AutoReset = false
            };
            _debounceTimer.Elapsed += OnDebounceTimerElapsed;

            LogToListbox($"文件监控已启动: {runtimeConfig.FromPath}");
        }

        /// <summary>
        /// 停止 FileSystemWatcher 和防抖计时器
        /// </summary>
        private void StopFileWatcher()
        {
            if (_debounceTimer != null)
            {
                _debounceTimer.Stop();
                _debounceTimer.Dispose();
                _debounceTimer = null;
            }

            if (_fileWatcher != null)
            {
                _fileWatcher.EnableRaisingEvents = false;
                _fileWatcher.Dispose();
                _fileWatcher = null;
                LogToListbox("文件监控已停止");
            }

            _hasPendingChanges = false;
        }

        /// <summary>
        /// 文件系统变化事件处理：记录变化并重置防抖计时器
        /// </summary>
        private void OnFileSystemChanged(object sender, FileSystemEventArgs e)
        {
            _hasPendingChanges = true;

            // 重置防抖计时器
            _debounceTimer?.Stop();
            _debounceTimer?.Start();
        }

        /// <summary>
        /// 防抖计时器到期：文件夹已稳定，触发备份
        /// </summary>
        private void OnDebounceTimerElapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            if (!_hasPendingChanges || _isCompressing)
                return;

            // 冷却检查：距上次备份需超过配置间隔
            double cooldownMs = runtimeConfig.Interval * 1000;
            if ((DateTime.Now - _lastBackupTime).TotalMilliseconds < cooldownMs)
                return;

            // 在主线程上触发备份
            BeginInvoke(new Action(() => DoBackup()));
        }

        /// <summary>
        /// 执行备份操作（供 debounce、timer、手动存档共用）
        /// </summary>
        /// <param name="onCompleted">备份完成后在主线程执行的回调（如恢复按钮状态）</param>
        private void DoBackup(Action? onCompleted = null)
        {
            if (_isCompressing)
                return;

            if (string.IsNullOrEmpty(runtimeConfig.FromPath) || !Directory.Exists(runtimeConfig.FromPath))
            {
                LogToListbox($"{runtimeConfig.FromPath}不存在", true);
                return;
            }
            if (string.IsNullOrEmpty(runtimeConfig.ToPath) || !Directory.Exists(runtimeConfig.ToPath))
            {
                LogToListbox($"目标文件夹无效: {runtimeConfig.ToPath}", true);
                return;
            }

            DateTime dt = DateTime.Now;
            string filename = dt.ToString("yyMMddHHmmssffffff") + ".zip";
            string savepath = Path.Combine(runtimeConfig.ToPath, filename);

            _hasPendingChanges = false;

            if (runtimeConfig.IsUsingLZMA)
            {
                try
                {
                    SetMarqueeStyle();
                    Task task = new Task(() =>
                    {
                        try
                        {
                            _isCompressing = true;
                            ZipHelper.CompressDirectory(runtimeConfig.FromPath, savepath, runtimeConfig.IsFastMode);
                            OnBackupCompleted(savepath, dt, onCompleted);
                        }
                        finally
                        {
                            _isCompressing = false;
                        }
                    });
                    task.Start();
                }
                catch (Exception ex)
                {
                    NotifyByStatusBar($"压缩时出现错误: {ex.Message}");
                    progOperation.BackColor = Color.Red;
                    onCompleted?.Invoke();
                }
            }
            else
            {
                string? sourceDirectory = runtimeConfig.FromPath;
                string zipFilePath = savepath;

                progOperation.Minimum = 0;
                progOperation.Maximum = 100;
                progOperation.Value = 0;
                progOperation.BackColor = Color.Green;

                try
                {
                    Task task = new Task(() =>
                    {
                        try
                        {
                            _isCompressing = true;
                            CompressDirectory(sourceDirectory, zipFilePath, true);
                            OnBackupCompleted(savepath, dt, onCompleted);
                        }
                        finally
                        {
                            _isCompressing = false;
                        }
                    });
                    task.Start();
                }
                catch (Exception ex)
                {
                    NotifyByStatusBar($"压缩时出现错误: {ex.Message}");
                    progOperation.BackColor = Color.Red;
                    onCompleted?.Invoke();
                }
            }
        }

        /// <summary>
        /// 备份完成后的统一处理：记录、更新 UI、更新冷却时间
        /// </summary>
        private void OnBackupCompleted(string savepath, DateTime dt, Action? onCompleted = null)
        {
            _lastBackupTime = DateTime.Now;

            DelegateNotifyByStatusBar delegateNotifyByStatusBar = new DelegateNotifyByStatusBar(NotifyByStatusBar);
            BeginInvoke(delegateNotifyByStatusBar, new object[] { "存储到:" + savepath });

            // 确定文件夹大小
            string[] files = Directory.GetFiles(runtimeConfig.FromPath, "*", SearchOption.AllDirectories);
            long totalSize = CalculateDirectorySize(files);

            // 更新记录
            DelegateInsertRecord delegateInsertRecord = new DelegateInsertRecord(InsertRecord);
            BeginInvoke(delegateInsertRecord, new object[] { savepath, dt, totalSize });

            // 将进度条设置为Regular
            BeginInvoke((SetRegularStyle));

            // 触发完成回调（如恢复按钮状态）
            if (onCompleted != null)
                BeginInvoke(onCompleted);
        }

        /// <summary>
        /// 执行恢复操作
        /// </summary>
        private void DoRestore(FileRecord zipToRestore)
        {
            DelegateNotifyByStatusBar delegateNotifyByStatusBar;
            Task taskRecyle = new Task(() => { });

            if (runtimeConfig.IsClearBeforeRestore)
            {
                taskRecyle = new Task(() =>
                {
                    if (Directory.Exists(runtimeConfig.FromPath))
                    {
                        Microsoft.VisualBasic.FileIO.FileSystem.DeleteDirectory(
                                                    runtimeConfig.FromPath,
                                                    Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs,
                                                    Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin);

                        delegateNotifyByStatusBar = new DelegateNotifyByStatusBar(NotifyByStatusBar);
                        this.BeginInvoke(delegateNotifyByStatusBar, new object[] { $"文件夹 {runtimeConfig.FromPath} 已移动到回收站" });
                    }

                    BeginInvoke((SetRegularStyle));
                });

                try
                {
                    SetMarqueeStyle();
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
                try
                {
                    if (runtimeConfig.IsClearBeforeRestore) taskRecyle.Wait();
                    string? restorePath = zipToRestore.RestorePath;
                    if (!Directory.Exists(restorePath))
                    {
                        if (!string.IsNullOrEmpty(restorePath))
                            Directory.CreateDirectory(restorePath);
                    }
                    ZipHelper.DecompressFileToDestDirectory(zipToRestore.FilePath, restorePath);

                    delegateNotifyByStatusBar = new DelegateNotifyByStatusBar(NotifyByStatusBar);
                    this.BeginInvoke(delegateNotifyByStatusBar, new object[] { "解压缩到:" + restorePath });
                }
                catch (Exception ex)
                {
                    this.BeginInvoke(new Action(() => NotifyByStatusBar($"解压缩时出现错误: {ex.Message}")));
                }
                finally
                {
                    BeginInvoke((SetRegularStyle));
                }
            });
            try
            {
                SetMarqueeStyle();
                task.Start();
            }
            catch (Exception ex)
            {
                NotifyByStatusBar($"解压缩时出现错误: {ex.Message}");
                progOperation.BackColor = Color.Red;
            }
        }

        private void btnManSave_Click(object sender, EventArgs e)
        {
            SaveConfig();
            btnManSave.Enabled = false;
            DoBackup(() => btnManSave.Enabled = true);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveConfig();
            StopFileWatcher();
        }

        private void cbFast_CheckedChanged(object sender, EventArgs e)
        {
            runtimeConfig.IsFastMode = cbFast.Checked;
            SaveConfig();
        }

        private void cbUsing7z_CheckedChanged(object sender, EventArgs e)
        {
            runtimeConfig.IsUsingLZMA = cbUsing7z.Checked;
            SaveConfig();
        }

        private void cbDeleteOldFiles_CheckedChanged(object sender, EventArgs e)
        {
            runtimeConfig.IsDeleteOldFiles = cbDeleteOldFiles.Checked;
            SaveConfig();
        }

        private void cbClearBeforeRestore_CheckedChanged(object sender, EventArgs e)
        {
            runtimeConfig.IsClearBeforeRestore = cbClearBeforeRestore.Checked;
            SaveConfig();
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            if (_isCompressing) return;

            // 如果没有待处理的变化，跳过本次备份
            if (!_hasPendingChanges) return;

            // 冷却检查
            double cooldownMs = runtimeConfig.Interval * 1000;
            if ((DateTime.Now - _lastBackupTime).TotalMilliseconds < cooldownMs) return;

            DoBackup();
        }
    }
}
