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

        private Dictionary<string,TimeLog> Logs
        {
            get { return logs; }
        }

        public TimeTracker()
        {
            logs = new Dictionary<string, TimeLog>();
            db = new Database(this);
        } 
        
        //TODO some serious design issues here. We can add to Logs but then when we hit here we think we have persisted...We should always save when we add a log so wrap the dictionary
        //in our own collection that handles all the database persistence invisibly
        public void Save(TimeLog timeLog)
        {
            if(!Logs.ContainsValue(timeLog))
            {
                //TODO this actually won't work if we store more than one timelog for the same timecode
                Logs[timeLog.TimeCode] = timeLog;
                //TODO think about this design a little. What if we have multiple calls to save for the same log...
                db.InsertTimeLog(timeLog);
            }else
            {
                //TODO just update the timelog
            }
        }

        public TimeLog CreateLog(string timeCode)
        {
            TimeLog t = new TimeLog(this, timeCode, DateTime.Now);
            Logs[timeCode] = t;
            db.InsertTimeLog(t);
            return t;
            //TODO to keep track of dirty data that needs to be persisted, subscribe to a change event on each log created.
        }
        
        public TimeLog FindLog(long id)
        {
            return db.ReadTimeLogs(new List<long> { id }).FirstOrDefault<TimeLog>();
        } 
        
        public List<TimeLog> FindLogs(DateTime date)
        {
            return db.ReadTimeLogs(date);
        }
        
        public void Save()
        {
            foreach(var log in Logs.Values)
            {
                db.UpdateTimeLog(log);
            }
        }    
    }
}
