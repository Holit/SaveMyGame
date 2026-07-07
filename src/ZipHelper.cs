using SharpCompress.Archives;
using SharpCompress.Archives.Zip;
using SharpCompress.Common;
using SharpCompress.Writers.Zip;

namespace SaveMyGame.src
{
    public static class ZipHelper
    {
        public static void CompressDirectory(string strInDirectoryPath, string strOutFilePath, bool bFastZip = false)
        {
            if (string.IsNullOrWhiteSpace(strInDirectoryPath) || !Directory.Exists(strInDirectoryPath))
                throw new DirectoryNotFoundException($"Directory {strInDirectoryPath} not found");
            if (string.IsNullOrWhiteSpace(strOutFilePath))
                throw new ArgumentNullException(nameof(strOutFilePath));
            int compressionLevel = bFastZip ? 1 : 9;
            using var archive = ZipArchive.CreateArchive();
            archive.AddAllFromDirectory(strInDirectoryPath);
            archive.SaveTo(strOutFilePath, new ZipWriterOptions(CompressionType.LZMA) { CompressionLevel = compressionLevel });
        }
        public static void CompressFile(string? strInFilePath, string? strOutFilePath)
        {
            if (string.IsNullOrWhiteSpace(strInFilePath) || !File.Exists(strInFilePath)) throw new FileNotFoundException($"File {strInFilePath} not found");
            if (string.IsNullOrWhiteSpace(strOutFilePath)) throw new ArgumentNullException(nameof(strOutFilePath));
            using var archive = ZipArchive.CreateArchive();
            archive.AddEntry(Path.GetFileName(strInFilePath), strInFilePath);
            archive.SaveTo(strOutFilePath, new ZipWriterOptions(CompressionType.LZMA));
        }
        public static void DecompressFileToDestDirectory(string? strInFilePath, string? strOutDirectoryPath)
        {
            if (string.IsNullOrWhiteSpace(strInFilePath) || !File.Exists(strInFilePath)) throw new FileNotFoundException($"File {strInFilePath} not found");
            if (string.IsNullOrWhiteSpace(strOutDirectoryPath)) throw new ArgumentNullException(nameof(strOutDirectoryPath));

            string fullDestPath = Path.GetFullPath(strOutDirectoryPath);

            using var archive = ZipArchive.OpenArchive(strInFilePath);
            foreach (var entry in archive.Entries)
            {
                if (entry.IsDirectory) continue;

                string entryKey = entry.Key;
                if (string.IsNullOrWhiteSpace(entryKey)) continue;

                string fullTargetPath = Path.GetFullPath(Path.Combine(fullDestPath, entryKey));
                if (!fullTargetPath.StartsWith(fullDestPath + Path.DirectorySeparatorChar))
                    throw new InvalidOperationException($"Zip entry '{entryKey}' attempts to extract outside of target directory.");

                string? targetDir = Path.GetDirectoryName(fullTargetPath);
                if (targetDir != null) Directory.CreateDirectory(targetDir);

                using var entryStream = entry.OpenEntryStream();
                using var fileStream = File.Create(fullTargetPath);
                entryStream.CopyTo(fileStream);
            }
        }
    }
}
