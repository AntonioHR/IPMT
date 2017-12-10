using ipmt.Engine.Huffman;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            HuffmanTree deserializedTree = HuffmanTree.DeserializeFromString(text, out int currIndex);

            string data = text.Substring(currIndex);

            string decodedData = deserializedTree.Decode(data);

            ReadIndex(decodedData);

            foreach (var patt in Patterns)
            {
                Match(patt);
            }

            //Console.WriteLine(deserializedTree.SerializeToString());

            //Console.WriteLine(decodedData);

            //Utils.TestUtils.WriteSeparator("Decoded");
            //Console.WriteLine(tree.Decode(encoded));

            //Match(text);
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
