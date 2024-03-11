using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace SaveMyGame
{
    public class DatabaseHelper
    {
        private string _connectionString;
        private const string DatabaseFileName = "data.db";

        public DatabaseHelper()
        {
            _connectionString = $"Data Source={DatabaseFileName};Version=3;";
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            if (!System.IO.File.Exists(DatabaseFileName))
            {
                SQLiteConnection.CreateFile(DatabaseFileName);
                CreateConfigTable();
            }
            CreateFileRecordsTable();
        }

        private void CreateConfigTable()
        {
            string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS applicationConfig (
                    frompath TEXT,
                    topath TEXT,
                    _7zpath TEXT,
                    interval INTEGER,
                    isFastMode INTEGER,
                    isUsing7Z INTEGER,
                    isRemindedSize INTEGER,
                    isDeleteOldFiles INTEGER,
                    isClearBeforeRestore INTEGER
                )";

            ExecuteNonQuery(createTableQuery);
        }
        private void CreateFileRecordsTable()
        {
            string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS FileRecords (
                    filePath TEXT,
                    restorePath TEXT,
                    date TEXT,
                    size INTEGER
                )";

            ExecuteNonQuery(createTableQuery);
        }

        public void SaveConfig(applicationConfig config)
        {
            // 删除已有的配置数据
            string deleteQuery = "DELETE FROM applicationConfig";
            ExecuteNonQuery(deleteQuery);

            string insertQuery = @"
                INSERT INTO applicationConfig (frompath, topath, _7zpath, interval, isFastMode, isUsing7Z, isRemindedSize, isDeleteOldFiles, isClearBeforeRestore)
                VALUES (@frompath, @topath, @_7zpath, @interval, @isFastMode, @isUsing7Z, @isRemindedSize, @isDeleteOldFiles, @isClearBeforeRestore)";

            using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@frompath", config.frompath);
                    command.Parameters.AddWithValue("@topath", config.topath);
                    command.Parameters.AddWithValue("@_7zpath", config._7zpath);
                    command.Parameters.AddWithValue("@interval", config.interval);
                    command.Parameters.AddWithValue("@isFastMode", config.isFastMode ? 1 : 0);
                    command.Parameters.AddWithValue("@isUsing7Z", config.isUsing7Z ? 1 : 0);
                    command.Parameters.AddWithValue("@isRemindedSize", config.isRemindedSize ? 1 : 0);
                    command.Parameters.AddWithValue("@isDeleteOldFiles", config.isDeleteOldFiles ? 1 : 0);
                    command.Parameters.AddWithValue("@isClearBeforeRestore", config.isClearBeforeRestore ? 1 : 0);


                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        public applicationConfig LoadConfig()
        {
            applicationConfig config = new applicationConfig();

            string selectQuery = "SELECT * FROM applicationConfig";

            using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(selectQuery, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            config.frompath = reader["frompath"].ToString();
                            config.topath = reader["topath"].ToString();
                            config._7zpath = reader["_7zpath"].ToString();
                            config.interval = Convert.ToInt32(reader["interval"]);
                            config.isFastMode = Convert.ToBoolean(reader["isFastMode"]);
                            config.isUsing7Z = Convert.ToBoolean(reader["isUsing7Z"]);
                            config.isRemindedSize = Convert.ToBoolean(reader["isRemindedSize"]);
                            config.isDeleteOldFiles = Convert.ToBoolean(reader["isDeleteOldFiles"]);
                            config.isClearBeforeRestore = Convert.ToBoolean(reader["isClearBeforeRestore"]);
                        }
                    }
                }

                connection.Close();
            }

            return config;
        }

        public void InsertFileRecord(archiveRecord record)
        {
            InsertFileRecord(record.filePath, 
                record.date, 
                record.restorePath, 
                record.size);
        }
        public void InsertFileRecord(string? filePath, DateTime date, string? restorePath, long size)
        {
            string insertQuery = @"
                INSERT INTO FileRecords (FilePath, Date, RestorePath, Size)
                VALUES (@FilePath, @Date, @RestorePath, @Size)";

            using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@FilePath", filePath);
                    command.Parameters.AddWithValue("@Date", date);
                    command.Parameters.AddWithValue("@RestorePath", restorePath);
                    command.Parameters.AddWithValue("@Size", size);

                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }
        public int DeleteFileRecord(string filePath, long size)
        {
            int deletedRecords = 0;
            string deleteQuery = "DELETE FROM FileRecords WHERE FilePath = @FilePath AND Size = @Size";

            using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(deleteQuery, connection))
                {
                    command.Parameters.AddWithValue("@FilePath", filePath);
                    command.Parameters.AddWithValue("@Size", size);

                    deletedRecords = command.ExecuteNonQuery();
                }

                connection.Close();
            }

            return deletedRecords;
        }
        public bool ExistFileRecord(string filePath)
        {
            bool recordExists = false;
            string selectQuery = "SELECT COUNT(*) FROM FileRecords WHERE FilePath = @FilePath";

            using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@FilePath", filePath);

                    // 执行查询并获取结果
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    recordExists = count > 0;
                }

                connection.Close();
            }

            return recordExists;
        }

        public List<archiveRecord> ReadAllFileRecord()
        {
            List<archiveRecord> records = new List<archiveRecord>();

            string selectQuery = "SELECT * FROM FileRecords";

            using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(selectQuery, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string? filePath = reader["FilePath"].ToString();
                            DateTime date = Convert.ToDateTime(reader["Date"]);
                            string? restorePath = reader["RestorePath"].ToString();
                            long size = Convert.ToInt64(reader["Size"]);

                            archiveRecord record = new archiveRecord();
                            record.filePath = filePath;
                            record.date = date; 
                            record.size = size;
                            record.restorePath = restorePath;
                            
                            records.Add(record);
                        }
                    }
                }
                connection.Close();
                return records;
            }
        }
        public archiveRecord ReadFileRecordByName(string name)
        {
            archiveRecord record = new archiveRecord();

            string selectQuery = "SELECT * FROM FileRecords WHERE FilePath LIKE @Name";

            using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@Name", $"%{name}%");

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string filePath = reader["FilePath"].ToString();
                            DateTime date = Convert.ToDateTime(reader["Date"]);
                            string restorePath = reader["RestorePath"].ToString();
                            long size = Convert.ToInt64(reader["Size"]);

                            record = new archiveRecord
                            {
                                filePath = filePath,
                                date = date,
                                size = size,
                                restorePath = restorePath
                            };
                        }
                    }
                }

                connection.Close();
            }

            return record;
        }


        private void ExecuteNonQuery(string query)
        {
            using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }
    }

}
