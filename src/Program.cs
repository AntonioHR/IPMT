using ipmt.Engine;
using ipmt.Engine.Huffman;
using ipmt.Utils;
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
            HuffmanTest();

            //CommandDescription c = CommandDescription.ParseFrom(args);
            //var cmd = Command.BuildFrom(c);
            //cmd.Execute();
        }

        private static void HuffmanTest()
        {
            string txt = "Ramona Lisa says: 'Dominic, you and me are a perfect disaster'";
            FrequencyMap freqMap = new FrequencyMap(txt);
            //char[] chrs = { 'a', 'b', 'c', 'd', 'e', 'f' };
            //int[] freqs = { 5, 9, 12, 13, 16, 45 };

            //FrequencyMap freqMap = TestUtils.DummyFrom(chrs, freqs);


            HuffmanTree tree = new HuffmanTree(freqMap);
            HuffmanEncoding encoding = new HuffmanEncoding(tree);

            string encoded = encoding.EncodeAsDebugString(txt);
            string decoded = tree.DecodeFromDebugString(encoded);


            Console.WriteLine("Encoding");
            Console.WriteLine(encoding.ToString);
            Console.WriteLine("Tree");
            Console.WriteLine(tree.SerializeToString());

            Console.WriteLine("Encoded text");
            Console.WriteLine(encoded);

            Console.WriteLine("Decoded text");
            Console.WriteLine(decoded);
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
