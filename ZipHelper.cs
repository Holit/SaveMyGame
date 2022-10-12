using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaveMyGame
{
    public class ZipHelper
    {
        // Fields
        private string _7zInstallPath = @"C:\Program Files\7-Zip\7za.exe";

        // Methods
        public ZipHelper(string str7zInstallPath)
        {
            this._7zInstallPath = str7zInstallPath;
        }

        /// <summary>
        /// 压缩文件夹目录
        /// </summary>
        /// <param name="strInDirectoryPath">指定需要压缩的目录，如C:\test\，将压缩test目录下的所有文件</param>
        /// <param name="strOutFilePath">压缩后压缩文件的存放目录</param>
        public void CompressDirectory(string strInDirectoryPath, string strOutFilePath)
        {
            Process process = new Process();
            process.StartInfo.FileName = this._7zInstallPath;
            process.StartInfo.Arguments = " a -t7z -mx=9 -ms=64m -m0=LZMA2 " + strOutFilePath + " " + strInDirectoryPath + " -r";
            //隐藏DOS窗口
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.Start();
            process.WaitForExit();
            process.Close();
        }

        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="strInFilePath">指定需要压缩的文件，如C:\test\demo.xlsx，将压缩demo.xlsx文件</param>
        /// <param name="strOutFilePath">压缩后压缩文件的存放目录</param>
        public void CompressFile(string strInFilePath, string strOutFilePath)
        {
            Process process = new Process();
            process.StartInfo.FileName = this._7zInstallPath;
            process.StartInfo.Arguments = " a -t7z -mx -ms=on -m0=LZMA2 " + strOutFilePath + " " + strInFilePath + "";
            //隐藏DOS窗口
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.Start();
            process.WaitForExit();
            process.Close();
        }

        /// <summary>
        /// 解压缩
        /// </summary>
        /// <param name="strInFilePath">压缩文件的路径</param>
        /// <param name="strOutDirectoryPath">解压缩后文件的路径</param>
        public void DecompressFileToDestDirectory(string strInFilePath, string strOutDirectoryPath)
        {
            Process process = new Process();
            process.StartInfo.FileName = this._7zInstallPath;
            process.StartInfo.Arguments = " x " + strInFilePath + " -o" + strOutDirectoryPath + " -r -aoa";
            //隐藏DOS窗口
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.Start();
            process.WaitForExit();
            process.Close();
        }

    }
}
