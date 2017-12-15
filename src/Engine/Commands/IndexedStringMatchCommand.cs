using ipmt.Engine.Huffman;
using System.Collections.Generic;
using System.Diagnostics;

namespace ipmt.Engine.Commands
{
    public abstract class IndexedStringMatchCommand : MultiTextCommand
    {
        protected List<string> Patterns { get; private set; }
        protected ResultHandler.Params HandlerParams { get; private set; }

        protected bool justCount;

        public IndexedStringMatchCommand(List<string> textFileNames, List<string> patterns, bool isCount)
            :base(textFileNames)
        {
            this.Patterns = patterns;
            this.justCount = isCount;
        }
        
        public override void BeforeExecute()
        {
            Train();
        }
        public override void ExecuteForText(string text, string fileName)
        {
            Stopwatch watch = Stopwatch.StartNew();
            PrepareIndex(text);
            watch.Stop();
            times.Add("Reading_Index", watch.Elapsed.TotalSeconds);

            
            double total = 0;
            foreach (var patt in Patterns)
            {
                watch.Restart();
                Match(patt);
                watch.Stop();
                total += watch.Elapsed.TotalSeconds;
            }
            times.Add("Average_Search_Time", total);

            //Console.WriteLine(deserializedTree.SerializeToString());

            //Console.WriteLine(decodedData);

            //Utils.TestUtils.WriteSeparator("Decoded");
            //Console.WriteLine(tree.Decode(encoded));

            //Match(text);
        }

        private void PrepareIndex(string text)
        {
            int currIndex;
            HuffmanTree deserializedTree = HuffmanTree.DeserializeFromString(text, out currIndex);

            string data = text.Substring(currIndex);

            string decodedData = deserializedTree.Decode(data);

            ReadIndex(decodedData);
        }

        protected abstract void ReadIndex(string serialized);
        protected abstract void Train();
        protected abstract void Match(string text);

        public static new IndexedStringMatchCommand BuildFrom(CommandDescription description)
        {
            var txtFiles = new List<string>(description.TextFiles);

            List<string> patts = GetAllPatterns(description);

            bool justCount = description.Contains(CommandDescription.OptionType.Count);
            //var p = new ResultHandler.Params(shouldPrint);


            return new SuffArrayMatch(txtFiles, patts, justCount);
        }

    }

}
