using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaveMyGame
{
    public struct config
    {
        public string?   frompath;
        public string?   topath;
        public string?   _7zpath;
        public int      interval;
        public bool     isFastMode;
        public bool     bUsing7Z;
        public bool     isRemindedSize;
    }
    public struct archiveRecord
    {
        public string? name;
        public string? restorePath;
        public DateTime date;
        public long size;
    }
}
