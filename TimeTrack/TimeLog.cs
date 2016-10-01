using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTrack
{
    public class TimeLog
    {
        long _id = -1;
        string _timeCode;
        DateTime _starttime;
        DateTime? _endtime = null;

        public TimeLog(string timeCode,DateTime startTime,DateTime? endTime)
        {
            _id = -1;
            _timeCode = timeCode;
            _starttime = startTime;
            _endtime = endTime;
        }

        public TimeLog(string timeCode, DateTime startTime)
        {
            _id = -1;
            _timeCode = timeCode;
            _starttime = startTime;
        }

        public long Id
        {
            get { return _id; }
        }

        public string TimeCode
        {
            get { return _timeCode; }
        }

        public DateTime StartTime
        {
            get { return _starttime; }
        }

        public DateTime? EndTime
        {
            get { return _endtime; }
            set { _endtime = value; }
        }

        public void UpdateId(long id)
        {
            _id = id;
        }

        public TimeSpan GetTotal()
        {
            if (EndTime == null)
                return DateTime.Now - StartTime;
            else
                return EndTime.Value - StartTime;
        }

        public void Stop()
        {
            //TODO don't allow closing if already closed
            EndTime = DateTime.Now;
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
