using ipmt.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ipmt.Engine.KMP
{
    public class KMPMatchCommand : StringMatchCommand
    {
        public KMPMatchCommand(List<string> textFileNames, List<string> patterns, ResultHandler.Params handlerParams) : base(textFileNames, patterns, handlerParams)
        {
            failTables = new FailTable[patterns.Count];
        }

        FailTable[] failTables;

        protected override void Train()
        {
            for (int i = 0; i < Patterns.Count; i++)
            {
                failTables[i] = new FailTable(Patterns[i]);
            }
        }

        protected override void Match(string text)
        {
            for (int i = 0; i < Patterns.Count; i++)
            {

                MatchPatternText(Patterns[i], text, failTables[i]);
            }
        }

        private void MatchPatternText(string patt, string text, FailTable failTable)
        {
            ResultHandler handler = new ResultHandler(HandlerParams);
            int i = 0;
            int j = 0;
            
            while(i+j < text.Length)
            {

                while (patt[j] == text[i + j])
                {
                    if (j+1 == patt.Length)
                    {
                        handler.OnNewMatch(text);
                        break;
                    }
                    if (text[j + i] == '\n')
                        handler.OnNewLine(j+1);
                    j++;
                }
                if (j == 0)
                {
                    if (text[j + i] == '\n')
                        handler.OnNewLine(j + i);
                    i++;
                }
                else
                {
                    int jump = failTable.failValues[j - 1];
                    i += j - jump;
                    j = jump;
                }
            }
            handler.OnAlgorithmEnd(patt);
        }

    }

    class FailTable
    {
        public int[] failValues;
        public FailTable(string pattern)
        {
            failValues = new int[pattern.Length];
            failValues[0] = 0;
            int i = 1, j = 0;

            while(i < failValues.Length)
            {

                if(pattern[i] == pattern[j])
                {
                    failValues[i] = j + 1;
                    i++;
                    j++;
                } else if(j == 0)
                {
                    failValues[i] = 0;
                    i++;
                } else
                {
                    j = failValues[j - 1];
                }
            }
 
        }
    }
}