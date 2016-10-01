using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTrack
{
    public class TimeTracker
    {
        protected Database db;
        protected Dictionary<string, TimeLog> logs;
        protected TimeLog lastLog;
        public Dictionary<string,TimeLog> Logs
        {
            get { return logs; }
        }
        public TimeTracker()
        {
            logs = new Dictionary<string, TimeLog>();
            db = new Database();
        }       
    }
}
