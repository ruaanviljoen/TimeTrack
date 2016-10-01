using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTrack
{
    public class ConsoleTimeTracker : TimeTracker
    {
        public void StartConsole()
        {
            string userInput;
            Console.WriteLine(new Commands().GetUsage());
            while ((userInput = Console.ReadLine()) != "quit")
            {
                string invokedVerb = null;
                object invokedVerbInstance = null;
                string[] inputArgs = userInput.Split(' ');

                //TODO extract this passed action to a shared scope so we don't create it each time and move this out of the loop. But this will be tricky
                //since we are creating a new instance of Commands each time so we clear any existing options. We don't want the suboptions for commands to
                //exist on the next user input.
                if (!CommandLine.Parser.Default.ParseArguments(inputArgs, new Commands(),
                       (verb, subOptions) =>
                       {
                           // if parsing succeeds the verb name and correct instance
                           // will be passed to onVerbCommand delegate (string,object)
                           invokedVerb = verb;
                           invokedVerbInstance = subOptions;
                       }))
                {
                    Environment.Exit(CommandLine.Parser.DefaultExitCodeFail);
                }

                if (invokedVerb == "start")
                {
                    //TODO warn if you are starting one without stopping previous, OR automatically stop previous
                    var startSubOptions = (StartSubOptions)invokedVerbInstance;
                    OnStartVerb(startSubOptions);
                }
                else if (invokedVerb == "stop")
                {
                    //TODO don't allow stopping if already stopped, warn and handle
                    var stopSubOptions = invokedVerbInstance as StopSubOptions;
                    OnStopVerb(stopSubOptions);
                }
                else if (invokedVerb == "report")
                {
                    var reportSubOptions = invokedVerbInstance as ReportSubOptions;
                    OnReportVerb(reportSubOptions);
                }
            }
        }

        private void OnStopVerb(StopSubOptions stopSubOptions)
        {
            TimeLog log;
            if (logs.ContainsKey(stopSubOptions.Code))
            {
                log = logs[stopSubOptions.Code];
                log.Stop();
                Console.WriteLine("Stopping time tracking for " + log.TimeCode + ". Time so far: " + log.GetTotal());
            }
            else
            {
                //TODO throw code does not exist, can't stop one you haven't start. Suggest existing ones or last used code
            }
        }

        private void OnReportVerb(ReportSubOptions reportSubOptions)
        {
            if (!reportSubOptions.ShowFullReport)
            {
                if (lastLog != null)
                    Console.WriteLine("Currently tracking " + lastLog.TimeCode + ". Total time tracked " + lastLog.GetTotal());
                else
                    Console.WriteLine("Currently not tracking anything");
            }
            else
            {
                Console.WriteLine("Reporting current set of logs...");
                TimeSpan totalTime = new TimeSpan();
                foreach (var log in logs)
                {
                    Console.WriteLine(String.Format("{0,-10} : {1} - {2} : {3}", log.Value.TimeCode, log.Value.StartTime, log.Value.EndTime, log.Value.GetTotal()));
                    totalTime += log.Value.GetTotal();
                }
                Console.WriteLine("Total time tracked : " + totalTime);
            }
        }

        private void OnStartVerb(StartSubOptions startSubOptions)
        {
            TimeLog log;
            if (logs.ContainsKey(startSubOptions.Code))
            {
                //TODO this is kinda a dud option. We don't really want to do this. Either create an new one or expand the business object to track multiple logs for a time code. Latter seems too complicated
                log = logs[startSubOptions.Code];
            }
            else
            {
                log = new TimeLog(startSubOptions.Code, DateTime.Now);
                logs[log.TimeCode] = log;
            }
            lastLog = log;
            Console.WriteLine("Starting time tracking for " + log.TimeCode);
        }
    }
}
