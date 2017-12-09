using ipmt.Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ipmt
{
    class Program
    {
        static void Main(string[] args)
        {
            CommandDescription c = CommandDescription.ParseFrom(args);
            var cmd = Command.BuildFrom(c);
            cmd.Execute();
        }

        private static void PrintOptsAndFiles(CommandDescription c)
        {
            foreach (var opt in c.Options)
            {
                Console.WriteLine(opt);
            }
            foreach (var str in c.TextFiles)
            {
                Console.WriteLine(str);
            }
        }
    }
}
