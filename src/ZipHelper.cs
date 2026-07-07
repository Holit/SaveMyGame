using SharpCompress.Common;
using SharpCompress.Writers;
using SharpCompress.Writers.Zip;
using SharpCompress.Archives;
using SharpCompress.Archives.Zip;
using SharpCompress.Compressors.Deflate;

namespace SaveMyGame.src
{
    public static class ZipHelper
    {
        public static void CompressDirectory(string strInDirectoryPath, string strOutFilePath, bool bFastZip = false)
        {
            CompressionLevel compressionLevel = bFastZip ? CompressionLevel.BestSpeed : CompressionLevel.BestCompression;
            using Stream stream = File.OpenWrite(strOutFilePath);
            using var writer = new ZipWriter(stream, new ZipWriterOptions(CompressionType.LZMA) { DeflateCompressionLevel = compressionLevel, LeaveStreamOpen = false });
            writer.WriteAll(strInDirectoryPath, "*", SearchOption.AllDirectories);
        }
        public static void CompressFile(string? strInFilePath, string? strOutFilePath)
        {
            if (string.IsNullOrWhiteSpace(strInFilePath) || !File.Exists(strInFilePath)) throw new FileNotFoundException($"File {strInFilePath} not found");
            if (string.IsNullOrWhiteSpace(strOutFilePath)) throw new ArgumentNullException(nameof(strOutFilePath));
            using Stream stream = File.OpenWrite(strOutFilePath);
            using var writer = new ZipWriter(stream, new ZipWriterOptions(CompressionType.LZMA) { LeaveStreamOpen = false });
            writer.Write(Path.GetFileName(strInFilePath), strInFilePath);
        }
        public static void DecompressFileToDestDirectory(string? strInFilePath, string? strOutDirectoryPath)
        {
            if (string.IsNullOrWhiteSpace(strInFilePath) || !File.Exists(strInFilePath)) throw new FileNotFoundException($"File {strInFilePath} not found");
            if (string.IsNullOrWhiteSpace(strOutDirectoryPath)) throw new ArgumentNullException(nameof(strOutDirectoryPath));

            string fullDestPath = Path.GetFullPath(strOutDirectoryPath);

            using var archive = ZipArchive.Open(strInFilePath);
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
