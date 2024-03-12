using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaveMyGame.src
{
    /// <summary>
    /// 应用程序配置
    /// </summary>
    public struct applicationConfig
    {
        public string? frompath;
        public string? topath;
        public string? _7zpath;
        public int interval;

        public bool isFastMode;
        public bool isUsing7Z;
        public bool isRemindedSize;
        public bool isDeleteOldFiles;
        public bool isClearBeforeRestore;
    }
    /// <summary>
    /// 文件结构
    /// </summary>
    public struct archiveRecord
    {
        public string? filePath;
        public string? restorePath;
        public DateTime date;
        public long size;
    }
}
