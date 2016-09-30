using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTrack
{
    public class Database
    {
        static readonly string _defaultDatabaseFileName = "timetrack.sqlite";
        public const string _sqLiteDateTimeFormatString = _sqLiteDateFormatString + " hh:mm:ss.fff";
        const string _sqLiteDateFormatString = "yyyy-MM-dd";

        public static string DefaultDatabaseFileName
        {
            get
            {
                return _defaultDatabaseFileName;
            }
        }

        public Database()
        {
            ConfigureDatabase();
        }

        private string GetConnectionString()
        {
            return String.Format("Data Source={0};Version=3;", _defaultDatabaseFileName);
        }

        private void ConfigureDatabase()
        {
            if (!DatabaseExists())
            {
                SQLiteConnection.CreateFile(_defaultDatabaseFileName);
            }
            ConfigureDatabaseTables();
        }

        private bool DatabaseExists()
        {
            return File.Exists(_defaultDatabaseFileName);
        }

        private void ConfigureDatabaseTables()
        {
            using (SQLiteConnection con = new SQLiteConnection(GetConnectionString()))
            {
                string sql = "create table if not exists time_log (id INTEGER PRIMARY KEY NOT NULL,time_code NVARCHAR(25),starttime DATETIME NOT NULL,endtime DATETIME)";
                SQLiteCommand command = new SQLiteCommand(sql, con);
                con.Open();
                command.ExecuteNonQuery();

                sql = "create table if not exists time_code_alias(time_code NVARCHAR(25),alias NVARCHAR(25))";
                command = new SQLiteCommand(sql, con);
                command.ExecuteNonQuery();
            }
        }

        private string GetSQLiteDateString(DateTime? datetime)
        {
            if (datetime == null)
                return "null";
            else
                return ((DateTime)datetime).ToString(_sqLiteDateTimeFormatString, System.Globalization.CultureInfo.InvariantCulture);
        }

        private DateTime GetDateTimeFromSQLiteDateString(string datestring)
        {
            return DateTime.ParseExact(datestring, _sqLiteDateTimeFormatString, System.Globalization.CultureInfo.InvariantCulture);
        }

        public void InsertTimeLog(TimeLog timeLog)
        {
            using (SQLiteConnection con = new SQLiteConnection(GetConnectionString()))
            {
                string sql = string.Format("insert into time_log (time_code,starttime,endtime) values ('{0}','{1}','{2}')", timeLog.TimeCode, this.GetSQLiteDateString(timeLog.StartTime), this.GetSQLiteDateString(timeLog.EndTime));
                SQLiteCommand command = new SQLiteCommand(sql, con);
                con.Open();
                using (SQLiteTransaction transaction = con.BeginTransaction())
                {
                    command.ExecuteNonQuery();
                    timeLog.UpdateId(con.LastInsertRowId);
                    transaction.Commit();
                }
            }
        }

        public void UpdateTimeLog(TimeLog timeLog)
        {
            using (SQLiteConnection con = new SQLiteConnection(GetConnectionString()))
            {
                string sql = string.Format("update time_log set time_code='{0}',starttime='{1}',endtime ='{2}' where id={3}", timeLog.TimeCode, this.GetSQLiteDateString(timeLog.StartTime), this.GetSQLiteDateString(timeLog.EndTime), timeLog.Id);
                SQLiteCommand command = new SQLiteCommand(sql, con);
                con.Open();
                using (SQLiteTransaction transaction = con.BeginTransaction())
                {
                    int rowsAffected = command.ExecuteNonQuery();
                    transaction.Commit();
                    if (rowsAffected != 1)
                    {
                        //TODO warn, something went wrong here, handle a little better
                        throw new Exception("Expected to affect 1 rows, but affected " + rowsAffected);
                    }
                }
            }
        }

        public List<TimeLog> ReadTimeLogs(DateTime date)
        {
            List<TimeLog> logs = new List<TimeLog>();
            using (SQLiteConnection con = new SQLiteConnection(GetConnectionString()))
            {
                string sql = string.Format("select id,time_code,starttime,endtime from time_log where date(starttime) = '{0}' ", date.Date.ToString(_sqLiteDateFormatString));
                SQLiteCommand command = new SQLiteCommand(sql, con);
                con.Open();
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    DateTime starttime = reader.GetDateTime(2);
                    string endtimeString = reader.GetString(3);
                    DateTime? endtime = endtimeString == "null" ? null : (DateTime?)GetDateTimeFromSQLiteDateString(endtimeString);
                    TimeLog log = new TimeLog(reader["time_code"].ToString(), starttime, endtime);
                    log.UpdateId(reader.GetInt32(0));
                    logs.Add(log);
                }
            }
            return logs;
        }

        //TODO not unittested yet
        public List<TimeLog> ReadTimeLogs(IEnumerable<long> ids)
        {
            List<TimeLog> logs = new List<TimeLog>();
            using (SQLiteConnection con = new SQLiteConnection(GetConnectionString()))
            {
                string sql = string.Format("select id,time_code,starttime,endtime from time_log where id in ({0}) ", string.Join<long>(",",ids));
                SQLiteCommand command = new SQLiteCommand(sql, con);
                con.Open();
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    DateTime starttime = reader.GetDateTime(2);
                    string endtimeString = reader.GetString(3);
                    DateTime? endtime = endtimeString == "null" ? null : (DateTime?)GetDateTimeFromSQLiteDateString(endtimeString);
                    TimeLog log = new TimeLog(reader["time_code"].ToString(), starttime, endtime);
                    log.UpdateId(reader.GetInt32(0));
                    logs.Add(log);
                }
            }
            return logs;
        }
    }
}
