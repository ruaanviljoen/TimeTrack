using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTrack
{
    interface ITimedTask
    {
        string Description { get; }
        void Start();
        void Stop();
        /// <summary>
        /// Total duration of this task
        /// </summary>
        /// <returns></returns>
        TimeSpan TotalDuration();
    }
}
