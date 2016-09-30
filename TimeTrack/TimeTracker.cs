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
        Database db;
        Dictionary<string, TimeLog> logs;

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
