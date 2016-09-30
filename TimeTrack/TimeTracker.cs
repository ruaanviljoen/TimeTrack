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

        public TimeTracker()
        {
            db = new Database();
        }


    }
}
