using ipmt.Engine.Huffman;
using ipmt.Engine.SuffArray;
using ipmt.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace ipmt.Engine.Commands
{
    public class BuildIndexCommand : MultiTextCommand
    {
        public BuildIndexCommand(List<String> files) :base(files)
        {

        }
        Stopwatch compressionStopwatch;
        Stopwatch indexConstructionStopwatch;

        public override void ExecuteForText(string text, string fileName)
        {
            int serializedIndexLength;
            string encodedText;
            string serializedTree;

            
            {
                Console.WriteLine("Building Index");
                string indexAndText = SerializedIndex(text);
                serializedIndexLength = indexAndText.Length;

                StartCompressionStopwatch();

                Console.WriteLine("Building Compression Tree");

                HuffmanTree tree = BuildTree(indexAndText);
                {
                    Console.WriteLine("Compressing Data");
                    HuffmanEncoding encoding = new HuffmanEncoding(tree);
                    encodedText = encoding.Encode(indexAndText);
                }
                serializedTree = tree.SerializeToString();
            }

            long encodedSize = encodedText.Length;

            StringBuilder sb = new StringBuilder();

            sb.Append(serializedTree);
            sb.Append(encodedText);

            int fileExtensionStart = fileName.LastIndexOf('.');
            string indexFileName = ((fileExtensionStart == -1) ? fileName : fileName.Substring(0, fileExtensionStart)) + ".idx";

            FileUtils.StringToFile(sb.ToString(), indexFileName, Encoding.UTF7);

            double compressionRate = (double)(encodedText.Length + serializedTree.Length) / serializedIndexLength;
            Console.WriteLine(compressionRate.ToString("P", CultureInfo.InvariantCulture));


            compressionStopwatch.Stop();
            times.Add("SerializationAndCompression", compressionStopwatch.Elapsed.TotalSeconds);
        }

        private static HuffmanTree BuildTree(string indexAndText)
        {
            FrequencyMap map = new FrequencyMap(indexAndText);
            HuffmanTree tree = new HuffmanTree(map);
            return tree;
        }

        private string SerializedIndex(string text)
        {
            StartIndexStopwatch();
            SuffixArray suffArray = SuffixArray.BuildFromText(text);
            RegisterIndexStopwatch();


            return suffArray.SerializedToString() + text;
        }

        private void StartCompressionStopwatch()
        {
            compressionStopwatch = Stopwatch.StartNew();
        }

        private void RegisterIndexStopwatch()
        {
            indexConstructionStopwatch.Stop();
            times.Add("Suffix_Array_Construction", indexConstructionStopwatch.Elapsed.TotalSeconds);
        }

        private void StartIndexStopwatch()
        {
            indexConstructionStopwatch = Stopwatch.StartNew();
        }

        public static new BuildIndexCommand BuildFrom(CommandDescription description)
        {
            var txtFiles = new List<string>(description.TextFiles);

            return new BuildIndexCommand(txtFiles);
        }
    }
}
