using ipmt.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ipmt.Engine.BruteForce
{
    class BruteFroceMatchCommand : StringMatchCommand
    {
        public BruteFroceMatchCommand(List<string> textFileNames, List<string> patterns, ResultHandler.Params handlerParams) :
            base(textFileNames, patterns, handlerParams)
        {
        }

        protected override void Train()
        {
            //Doesn't Train Cause he's a bruuuute
        }
        protected override void Match(string text)
        {
            foreach (var patt in Patterns)
            {
                ResultHandler handler = new ResultHandler(HandlerParams);

                for (int i = 0; i < text.Length; i++)
                {
                    if (text[i] == '\n')
                        handler.OnNewLine(i + 1);
                    bool matched = true;
                    for (int j = 0; j < patt.Length && i + j < text.Length; j++)
                    {
                        if (patt[j] != text[i + j])
                        {
                            matched = false;
                            break;
                        }
                    }
                    if (matched)
                        handler.OnNewMatch(text);
                }
                handler.OnAlgorithmEnd(patt);
            }

        }

    }
}