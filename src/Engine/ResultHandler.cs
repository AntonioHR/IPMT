using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ipmt.Engine
{

    public class ResultHandler
    {
        public struct Params
        {
            public bool shouldPrint;

            public Params(bool shouldPrint)
            {
                this.shouldPrint = shouldPrint;
            }
        }

        Params p;

        int matchCount = 0;

        int lineVal = 0;
        int lastLineStart = 0;
        bool hasMatchOnLine = false;

        public ResultHandler(Params p)
        {
            this.p = p;
        }

        public void OnNewLine(int lineStart)
        {
            lineVal++;
            lastLineStart = lineStart;
            hasMatchOnLine = false;
        }
        public void OnNewMatch(string txt)
        {
            matchCount++;
            if (p.shouldPrint && !hasMatchOnLine)
            {
                int lineEnd = txt.IndexOf('\n', lastLineStart+1);
                string line;
                if (lineEnd == -1)
                {
                    line = txt.Substring(lastLineStart);
                }
                else
                {
                    line = txt.Substring(lastLineStart, lineEnd - lastLineStart);
                }
                Console.WriteLine(line);
            }
            hasMatchOnLine = true;
        }
        public void OnAlgorithmEnd(string patt)
        {
            Console.WriteLine("Matches for \"{1}\": {0}", matchCount, patt);
        }
    }

}