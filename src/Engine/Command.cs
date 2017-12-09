using ipmt.Engine.AhoCorasick;
using ipmt.Engine.BruteForce;
using ipmt.Engine.KMP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ipmt.Engine
{

    public abstract class Command
    {
        public abstract void Execute();

        public static Command BuildFrom(CommandDescription description)
        {
            if (description.Contains(CommandDescription.OptionType.Help))
                return new DisplayHelpCommand();
            else
                return StringMatchCommand.BuildFrom(description);

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

    public abstract class StringMatchCommand : Command
    {
        protected List<string> TextFileNames { get; private set; }
        protected List<string> Patterns { get; private set; }
        protected ResultHandler.Params HandlerParams { get; private set; }

        public StringMatchCommand(List<string> textFileNames, List<string> patterns, ResultHandler.Params handlerParams)
        {
            this.TextFileNames = textFileNames;
            this.Patterns = patterns;
            this.HandlerParams = handlerParams;
        }

        public override void Execute()
        {
            Train();
            foreach (var txtFile in TextFileNames)
            {
                using (StreamReader sr = new StreamReader(txtFile))
                {
                    String text = sr.ReadToEnd();
                    Match(text);
                }
            }
        }

        protected abstract void Train();
        protected abstract void Match(string text);

        public static new StringMatchCommand BuildFrom(CommandDescription description)
        {
            var txtFiles = new List<string>(description.TextFiles);

            List<string> patts = GetAllPatterns(description);

            bool shouldPrint = !description.Contains(CommandDescription.OptionType.Count);
            var p = new ResultHandler.Params(shouldPrint);


            if (description.EditDistance > 0)
            {
                switch (description.Algorithm)
                {
                    case CommandDescription.AlgorithmType.Default:
                        return new Sellers.SellersStringMatchCommand(txtFiles, patts, p, description.EditDistance);
                    case CommandDescription.AlgorithmType.BruteForce:
                        throw new NotImplementedException();
                    case CommandDescription.AlgorithmType.KMP:
                        throw new InvalidOperationException("Edit distance invalid for KMP");
                    case CommandDescription.AlgorithmType.AhoCorasick:
                        throw new InvalidOperationException("Edit distance invalid for Aho Corasick");
                    case CommandDescription.AlgorithmType.Sellers:
                        return new Sellers.SellersStringMatchCommand(txtFiles, patts, p, description.EditDistance);
                    default:
                        throw new NotImplementedException();
                }
            }
            else
            {
                switch (description.Algorithm)
                {
                    case CommandDescription.AlgorithmType.BruteForce:
                        return new BruteFroceMatchCommand(txtFiles, patts, p);
                    case CommandDescription.AlgorithmType.KMP:
                        return new KMPMatchCommand(txtFiles, patts, p);
                    case CommandDescription.AlgorithmType.AhoCorasick:
                        return new AhoCorasickMatchCommand(txtFiles, patts, p);
                    case CommandDescription.AlgorithmType.Default:
                        if (patts.Count > 20)
                            return new AhoCorasickMatchCommand(txtFiles, patts, p);
                        else
                            return new KMPMatchCommand(txtFiles, patts, p);
                    case CommandDescription.AlgorithmType.Sellers:
                        return new Sellers.SellersStringMatchCommand(txtFiles, patts, p, 0);
                    default:
                        throw new NotImplementedException(); ;
                }
            }

            throw new NotImplementedException();
        }

    }
    public abstract class ApproximateStringMatchCommand : StringMatchCommand
    {
        protected int MaxDistance { get; private set; }

        public ApproximateStringMatchCommand(List<string> textFileNames, List<string> patterns, ResultHandler.Params handlerParams, int distance) : base(textFileNames, patterns, handlerParams)
        {
            this.MaxDistance = distance;
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
