using ipmt.Engine.SuffArray;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ipmt.Engine.Commands
{
    public class SuffArrayMatch : IndexedStringMatchCommand
    {
        SuffixArray suff;
        private List<string> txtFiles;
        private List<string> patts;

        public SuffArrayMatch(List<string> textFileNames, List<string> patterns, bool justCount) : base(textFileNames, patterns, justCount)
        {
        }
        
        protected override void Match(string patt)
        {
            if(!justCount)
                Utils.TestUtils.WriteSeparator("Patterns for " + patt);
            string result = suff.MatchAndPrint(patt, justCount);
            Console.WriteLine(result);
            //throw new NotImplementedException();
        }

        protected override void ReadIndex(string serialized)
        {
            suff = SuffixArray.DeserializeFromString(serialized);

            //Console.WriteLine(suff.SerializedToString());
        }

        protected override void Train()
        {
            //throw new NotImplementedException();
        }
    }
}
