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
        }

        private void CreateConfigTable()
        {
            string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS _config (
                    frompath TEXT,
                    topath TEXT,
                    _7zpath TEXT,
                    interval INTEGER,
                    isFastMode INTEGER,
                    bUsing7Z INTEGER,
                    isRemindedSize INTEGER
                )";

            ExecuteNonQuery(createTableQuery);
        }

        public void SaveConfig(config config)
        {
            // 删除已有的配置数据
            string deleteQuery = "DELETE FROM _config";
            ExecuteNonQuery(deleteQuery);

            string insertQuery = @"
                INSERT INTO _config (frompath, topath, _7zpath, interval, isFastMode, bUsing7Z, isRemindedSize)
                VALUES (@frompath, @topath, @_7zpath, @interval, @isFastMode, @bUsing7Z, @isRemindedSize)";

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
                    command.Parameters.AddWithValue("@bUsing7Z", config.bUsing7Z ? 1 : 0);
                    command.Parameters.AddWithValue("@isRemindedSize", config.isRemindedSize ? 1 : 0);

                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        public config LoadConfig()
        {
            config config = new config();

            string selectQuery = "SELECT * FROM _config";

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
                            config.bUsing7Z = Convert.ToBoolean(reader["bUsing7Z"]);
                            config.isRemindedSize = Convert.ToBoolean(reader["isRemindedSize"]);
                        }
                    }
                }

                connection.Close();
            }

            return config;
        }

        public void InsertFileRecord(archiveRecord record)
        {
            InsertFileRecord(record.name, record.date, record.restorePath, record.size);
        }
        public void InsertFileRecord(string name, DateTime date, string restorePath, long size)
        {
            string insertQuery = @"
                INSERT INTO second_table (Name, Date, RestorePath, Size)
                VALUES (@Name, @Date, @File_path, @Size)";

            using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Date", date);
                    command.Parameters.AddWithValue("@RestorePath", restorePath);
                    command.Parameters.AddWithValue("@Size", size);

                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        public archiveRecord[] ReadAllFileRecord()
        {
            archiveRecord[] records = new archiveRecord[0];

            string selectQuery = "SELECT * FROM second_table";

            using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(selectQuery, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            string name = reader["Name"].ToString();
                            DateTime date = Convert.ToDateTime(reader["Date"]);
                            string restorePath = reader["RestorePath"].ToString();
                            long size = Convert.ToInt64(reader["Size"]);

                            archiveRecord record = new archiveRecord();
                            record.name = name;
                            record.date = date; 
                            record.size = size;
                            record.restorePath = restorePath;

                            records.Append(record);
                        }
                    }
                }
                connection.Close();
                return records;
            }
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
