using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore; 
namespace SaveMyGame.src.Models
{
    internal class ProgramDbContext : DbContext
    {
        public DbSet<ApplicationConfig> ApplicationConfigs { get; set; }
        public DbSet<FileRecord> FileRecords { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(new SqliteConnectionStringBuilder()
            {
                DataSource = Path.Combine(Environment.CurrentDirectory, "data")
            }.ToString());
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
    public class ApplicationConfig
    {
        public long Id { get; set; }
        public string? FromPath { get; set; }
        public string? ToPath { get; set; }
        public int Interval { get; set; }
        public bool IsFastMode { get; set; }
        public bool IsUsingLZMA { get; set; }
        public bool IsRemindedSize { get; set; }
        public bool IsDeleteOldFiles { get; set; }
        public bool IsClearBeforeRestore { get; set; }
    }

    public class FileRecord(string filePath, string restorePath, DateTime date, long size)
    {
        public long Id { get; set; }
        public string FilePath { get; set; } = filePath;
        public string RestorePath { get; set; } = restorePath;
        public DateTime Date { get; set; } = date;
        public long Size { get; set; } = size;
    }
}
