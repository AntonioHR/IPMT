using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ipmt.Engine.Sellers
{
    public class SellersStringMatchCommand : ApproximateStringMatchCommand
    {
        public SellersStringMatchCommand(List<string> textFileNames, List<string> patterns, ResultHandler.Params handlerParams, int distance) : base(textFileNames, patterns, handlerParams, distance)
        {
        }



        protected override void Match(string text)
        {
            foreach (var pat in Patterns)
            {
                Match(text, pat);
            }
        }

        private void Match(string text, string pat)
        {
            ResultHandler handler = new ResultHandler(HandlerParams);

            int[] curr = ArrayWithZeroInBeginning(pat.Length + 1);

            int[] prev = ArrayWithCount(pat.Length + 1);

            for (int i = 0; i < text.Length; i++)
            {
                for (int j = 1; j < pat.Length + 1; j++)
                {
                    bool wordMatches = text[i] == pat[j - 1];
                    int a = prev[j - 1] + (wordMatches ? 0 : 1);
                    int b = curr[j - 1] + 1;
                    int c = prev[j] + 1;

                    curr[j] = Math.Min(Math.Min(a, b), c);
                }

                if (curr[pat.Length] <= MaxDistance)
                    handler.OnNewMatch(text);
                var temp = curr;
                curr = prev;
                prev = temp;
                if (text[i] == '\n')
                    handler.OnNewLine(i);
            }
            handler.OnAlgorithmEnd(pat);
        }

        static int[] ArrayWithZeroInBeginning(int size)
        {
            int[] result= new int[size];
            result[0] = 0;
            return result;
        }

        static int[] ArrayWithCount(int size)
        {
            int[] result = new int[size];
            for (int i = 0; i < size; i++)
            {
                result[i] = i;
            }
            return result;
        }

        protected override void Train()
        {
        }
    }
}
