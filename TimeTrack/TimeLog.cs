using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTrack
{
    public class TimeLog : SimpleTimedTask
    {
        long _id = -1;
        string _timeCode;
        DateTime _starttime;
        DateTime? _endtime = null;
        TimeTracker _timeTrackerInstance;

        public TimeLog(TimeTracker t,string timeCode,DateTime startTime,DateTime? endTime)
        {
            _id = -1;
            _timeCode = timeCode;
            _starttime = startTime;
            _endtime = endTime;
            _timeTrackerInstance = t;
        }

        public TimeLog(TimeTracker t,string timeCode, DateTime startTime)
        {
            _id = -1;
            _timeCode = timeCode;
            _starttime = startTime;
            _timeTrackerInstance = t;
        }

        public long Id
        {
            get { return _id; }
        }

        public string TimeCode
        {
            get { return _timeCode; }
        }        

        public void UpdateId(long id)
        {
            _id = id;
        }

        public void Save()
        {
            _timeTrackerInstance.Save(this);
        }        

        public override bool Equals(object obj)
        {
            TimeLog other = obj as TimeLog;
            return this.Id == other.Id
                && this.TimeCode == other.TimeCode
                //TODO this is bad, instead fix the database handling of the datetime so it actually returns the same datetime
                && this.StartTime.ToString(Database._sqLiteDateTimeFormatString) == other.StartTime.ToString(Database._sqLiteDateTimeFormatString)
                && this.EndTime == other.EndTime;
        }
    }
}
