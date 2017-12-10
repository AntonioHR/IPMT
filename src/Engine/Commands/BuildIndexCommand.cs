using ipmt.Engine.Huffman;
using ipmt.Engine.SuffArray;
using ipmt.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ipmt.Engine.Commands
{
    public class BuildIndexCommand : MultiTextCommand
    {
        public BuildIndexCommand(List<String> files) :base(files)
        {

        }
        

        public override void ExecuteForText(string text, string fileName)
        {
            SuffixArray suffArray = SuffixArray.BuildFromText(text);

            string fullText = suffArray.SerializedToString() + text;


            FrequencyMap map = new FrequencyMap(fullText);
            HuffmanTree tree = new HuffmanTree(map);
            HuffmanEncoding encoding = new HuffmanEncoding(tree);

            string encoded = encoding.Encode(fullText);

            StringBuilder sb = new StringBuilder();

            sb.Append(tree.SerializeToString());
            sb.Append(encoded);

            int fileExtensionStart = fileName.LastIndexOf('.');
            string indexFileName = ((fileExtensionStart == -1) ? fileName : fileName.Substring(0, fileExtensionStart)) + ".idx";
            
            FileUtils.StringToFile(sb.ToString(), indexFileName, Encoding.UTF7);

            //string read = TestUtils.ReadFromFile(Encoding.UTF7, indexFileName);

            //HuffmanTree deserializedTree = HuffmanTree.DeserializeFromString(read, out int currIndex);

            //string data = read.Substring(currIndex);

            //string decodedData = deserializedTree.Decode(data);

            ////Console.WriteLine(deserializedTree.SerializeToString());

            //Console.WriteLine(decodedData);

            ////Utils.TestUtils.WriteSeparator("Decoded");
            ////Console.WriteLine(tree.Decode(encoded));
        }

        public static new BuildIndexCommand BuildFrom(CommandDescription description)
        {
            var txtFiles = new List<string>(description.TextFiles);

            return new BuildIndexCommand(txtFiles);
        }
    }
}
