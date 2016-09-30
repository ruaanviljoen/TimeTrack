using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTrack
{
    class Program
    {

        static void Main(string[] args)
        {
            TimeTracker t = new TimeTracker();
            Commands commands = new Commands();
            Dictionary<string, TimeLog> logs = t.Logs;
            TimeLog lastLog = null;
            string input;
            Console.WriteLine(commands.GetUsage());
            while ((input = Console.ReadLine()) != "quit")
            {
                string invokedVerb = null;
                object invokedVerbInstance = null;
                string[] inputArgs = input.Split(' ');

                //tODO extract this passed action to a shared scope so we don't create it each time and move this out of the loop
                if (!CommandLine.Parser.Default.ParseArguments(inputArgs, commands,
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
                    TimeLog log;
                    if (logs.ContainsKey(startSubOptions.Code))
                    {
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
                else if (invokedVerb == "stop")
                {
                    var stopSubOptions = invokedVerbInstance as StopSubOptions;
                    TimeLog log;
                    if (logs.ContainsKey(stopSubOptions.Code))
                    {
                        log = logs[stopSubOptions.Code];
                        log.EndTime = DateTime.Now;
                        Console.WriteLine("Stopping time tracking for " + log.TimeCode+". Time so far: "+log.GetTotal());
                    }
                    else
                    {
                        //TODO throw code does not exist, can't stop one you haven't start. Suggest existing ones or last used code
                    }
                }else if(invokedVerb =="report")
                {                    
                    var reportSubOptions = invokedVerbInstance as ReportSubOptions;
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
            }
        }
    }
}
