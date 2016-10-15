using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTrack
{
    public class TimePeriod
    {
        //TODO don't allow timeperiods with an end before a beginning
        public DateTime Beginning { get; set; }
        public DateTime End { get; set; }

        public TimePeriod()
        {
            Beginning = DateTime.Now;
        }

        public TimeSpan Duration()
        {
            return End - Beginning;
        }
    }

    public class ReusableTimedTask : ITimedTask
    {
        TimePeriod _lastTimePeriod;
        List<TimePeriod> _recordedTimePeriods;
        public string Description
        {
            get; private set;
        }

        public ReusableTimedTask(string description)
        {
            Description = description;
        }

        public TimeSpan TotalDuration()
        {
            return new TimeSpan(_recordedTimePeriods.Sum<TimePeriod>(x => x.Duration().Ticks));
        }

        public void Start()
        {
            if(_lastTimePeriod?.End != default(DateTime))
            {
                TimePeriod newPeriod = new TimePeriod();
                _lastTimePeriod = new TimePeriod();
                _recordedTimePeriods.Add(newPeriod);
            }
            else
            {
                //just keep tracking the existing one so we don't have to do anything! \o/
            }
        }

        public void Stop()
        {
            if(_lastTimePeriod?.End == default(DateTime))
            {
                DateTime now = DateTime.Now;
                if(now < _lastTimePeriod.Beginning)
                {
                    throw new Exception("Check system time. Cannot call Stop before Start on ReusableTimedTask");
                }
                _lastTimePeriod.End = DateTime.Now;
            }
            else
            {
                throw new InvalidOperationException("ReusableTimedTask cannot be stopped while in a stopped state.");
            }
        }
    }
}
