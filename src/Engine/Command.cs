using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ipmt.Engine.Commands;

namespace ipmt.Engine
{

    public abstract class Command
    {
        public abstract void Execute();

        public static Command BuildFrom(CommandDescription description)
        {
            if(description.Operation == CommandDescription.OperationType.Help)
            {
                return new DisplayHelpCommand();
            } else if(description.Operation == CommandDescription.OperationType.Index)
            {
                return BuildIndexCommand.BuildFrom(description);
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
        private static string HelpMessage = " usage:$ pmt [options] pattern textfile [textfile...] \n options \n -e editdistance , --edit editdistance : search with edit distance editdistance\n -p file, --pattern file : search all patterns in file (separated by line break) \n -a algorithm, --algorithm_name algorithm: search with speciffic algorithm(options: kmp, aho(aho corasick), bf(brute force), sel(sellers)\n -c, --count : prints only the amount of matches";
        public override void Execute()
        {
            Console.WriteLine(HelpMessage);
        }
    }

}
