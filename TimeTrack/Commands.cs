using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTrack
{
    public class Commands
    {
        [VerbOption("start", HelpText = "Start tracking time")]
        public StartSubOptions StartVerb { get; set; }

        [VerbOption("stop", HelpText = "Stop tracking time")]
        public StopSubOptions StopVerb { get; set; }

        [VerbOption("report", HelpText = "Show report for tracked time")]
        public ReportSubOptions ReportVerb { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return "Hello.\n Use \"start -c MyTimeCode\" to start tracking time. .\n Stop has the same syntax.";
        }
    }
    public class StartSubOptions
    {
        [Option('c', "code", HelpText = "Time code")]
        public string Code
        {
            get;set;
        }
    }

    public class StopSubOptions
    {
        [Option('c', "code", HelpText = "Time code")]
        public string Code
        {
            get; set;
        }
    }

    public class ReportSubOptions
    {
        [Option('f', "full", HelpText = "Show full report")]
        public bool ShowFullReport
        {
            get; set;
        }
    }
}
