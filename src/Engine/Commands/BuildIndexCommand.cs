using ipmt.Engine.Huffman;
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
            FrequencyMap map = new FrequencyMap(text);

            //HuffmanTree
        }

        public static new BuildIndexCommand BuildFrom(CommandDescription description)
        {
            var txtFiles = new List<string>(description.TextFiles);

            return new BuildIndexCommand(txtFiles);
        }
    }
}
