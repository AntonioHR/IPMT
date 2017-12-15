using ipmt.Engine.Commands;
using ipmt.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ipmt.Engine
{

    public abstract class Command
    {
        public bool hasTimeReport;

        public Dictionary<string, double> times = new Dictionary<string, double>();

        public void Run()
        {
            Execute();

            if (hasTimeReport)
                ReportTime();
        }

        private void ReportTime()
        {
            TestUtils.WriteSeparator("Execution Times");
            StringBuilder b = new StringBuilder();
            foreach (var time in times)
            {
                b.Append(time.Key);
                b.Append('\t');
                b.Append(time.Value);
                b.Append('\n');
            }
            Console.WriteLine(b.ToString());
        }

        protected abstract void Execute();

        public static Command BuildFrom(CommandDescription description)
        {
            if(description.Operation == CommandDescription.OperationType.Help)
            {
                return new DisplayHelpCommand();
            } else if(description.Operation == CommandDescription.OperationType.Index)
            {
                var result = BuildIndexCommand.BuildFrom(description);
                result.hasTimeReport = description.Contains(CommandDescription.OptionType.TimeReport);
                return result;  

            } else if(description.Operation == CommandDescription.OperationType.Search)
            {
                var result = IndexedStringMatchCommand.BuildFrom(description);
                result.hasTimeReport = description.Contains(CommandDescription.OptionType.TimeReport);
                return result;
            }
            throw new NotImplementedException();
        }

        public static List<string> GetAllPatterns(CommandDescription description)
        {
            List<string> patts = new List<string>();

            if (!description.Contains(CommandDescription.OptionType.PatternFile))
            {
                patts.Add(description.Pattern);
            }
            else
            {
                using (StreamReader sr = new StreamReader(description.Patternfile))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        patts.Add(line);
                    }
                }
            }
            return patts;
        }
    }


    class DisplayHelpCommand : Command
    {
        private static string HelpMessage = " usage:$ ipmt [command] [options] pattern textfile [textfile...]\n\n Commands\n index: creates an index file for later searching\n  search: Searches for patterns in an index file\n\n options \n -p file, --pattern file : search all patterns in file (separated by line break) \n -c, --count : prints only the amount of matches";
        protected override void Execute()
        {
            Console.WriteLine(HelpMessage);
        }
    }

}
