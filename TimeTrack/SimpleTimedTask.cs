using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTrack
{
    /// <summary>
    /// A simple single use timed task. Implementation only allows task to be started and stopped once.
    /// </summary>
    public class SimpleTimedTask : ITimedTask
    {
        private DateTime _starttime = DateTime.MinValue;
        private DateTime _endtime = DateTime.MinValue;
        private string _taskDescription;

        public string Description
        {
            get { return _taskDescription; }
        }

        public DateTime StartTime
        {
            get { return _starttime; }
        }

        public DateTime EndTime
        {
            get { return _endtime; }
        }

        public void Start()
        {
            if(_starttime!= DateTime.MinValue)
                _starttime = DateTime.Now;
            //TODO consider warning if task is already started?
        }

        public void Stop()
        {
            if(_endtime==DateTime.MinValue)
                _endtime = DateTime.Now;
            //TODO consider warning task is already stopped?
        }

        public TimeSpan TotalDuration()
        {
            if (EndTime == null)
                return DateTime.Now - StartTime;
            else
                return EndTime - StartTime;
        }
    }
}
