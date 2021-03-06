﻿using ipmt.Engine;
using ipmt.Engine.Huffman;
using ipmt.Engine.SuffArray;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace ipmt.Utils
{
    public static class TestUtils
    {

        public static FrequencyMap DummyFrom(char[] chars, int[] freqs)
        {
            Debug.Assert(chars.Length == freqs.Length);
            Dictionary<char, int> result = new Dictionary<char, int>();
            for (int i = 0; i < chars.Length; i++)
            {
                result.Add(chars[i], freqs[i]);
            }
            return new FrequencyMap(result);
        }
        
        internal static void HuffmanDecodeTest()
        {
            int index;
            var huffmanTree = HuffmanTree.DeserializeFromString(ReadFromFile(Encoding.UTF7, "huffTree.txt"), out index);

            Console.WriteLine(huffmanTree.SerializeToString());
        }

        internal static void SuffArrayTest()
        {

            //var suffArray = SuffixArray.BuildFromText("banana");
            var suffArray = SuffixArray.BuildFromText(ReadFromFile(Encoding.UTF7));
            //Console.WriteLine(suffArray.ToStringDebug());

            //suffArray.DebugTestComparisons("when");
            Console.WriteLine(suffArray.MatchAndPrint("when", false));
        }

        public static void WriteToDummyFile(string txt)
        {
            using (StreamWriter sw = new StreamWriter(new FileStream("dummy.txt", FileMode.OpenOrCreate), Encoding.UTF7))
            {
                    sw.Write(txt);
            }
        }

        public static string ReadFromDummyFile()
        {
            using (StreamReader sr = new StreamReader("dummy.txt", Encoding.UTF7))
            {
                return sr.ReadToEnd();
            }
        }

        public static string ReadFromFile(Encoding encoding, string name = "test.txt")
        {
            using (StreamReader sr = new StreamReader(name, encoding))
            {
                return sr.ReadToEnd();
            }
        }

        internal static void WriteSeparator(string v)
        {
            Console.WriteLine(string.Format("-----------------------------------------------------{0}--------------------------------------", v));
        }


        public static void HuffmanTest()
        {
            //string txt = "Ramona Lisa says: 'Dominic, you and me are a perfect disaster'";
            string txt = TestUtils.ReadFromFile(Encoding.Unicode, "test.txt");
            FrequencyMap freqMap = new FrequencyMap(txt);


            HuffmanTree tree = new HuffmanTree(freqMap);
            HuffmanEncoding encoding = new HuffmanEncoding(tree);

            string debugEncoded = encoding.EncodeAsDebugString(txt);
            string realEncoded = encoding.Encode(txt);

            TestUtils.WriteToDummyFile(realEncoded);
            string encodedReadFromFile = TestUtils.ReadFromDummyFile();

            string debugDecoded = tree.DecodeFromDebugString(debugEncoded);
            string realDecoded = tree.Decode(realEncoded);
            string realDecodedFromFile = tree.Decode(encodedReadFromFile);


            Console.WriteLine("Encoding");
            Console.WriteLine(encoding.ToString);
            Console.WriteLine("Tree");
            Console.WriteLine(tree.SerializeToString());

            TestUtils.WriteSeparator("Encoded Text");
            Console.WriteLine(debugEncoded);

            TestUtils.WriteSeparator("Compressed Encoding");
            Console.WriteLine(realEncoded);

            TestUtils.WriteSeparator("Debug Decoded text");
            Console.WriteLine(debugDecoded);

            TestUtils.WriteSeparator("Real Decoded text");
            Console.WriteLine(realDecoded);

            TestUtils.WriteSeparator("Real Decoded text, read from file");
            Console.WriteLine(realDecodedFromFile);
        }

        public static void HuffmanCompleteTest()
        {
            string txt = TestUtils.ReadFromFile(Encoding.Unicode, "test.txt");
            FrequencyMap freqMap = new FrequencyMap(txt);

            HuffmanTree tree = new HuffmanTree(freqMap);
            HuffmanEncoding encoding = new HuffmanEncoding(tree);


            string encodedText = encoding.Encode(txt);

            string serializedTree = tree.SerializeToString();


            StringBuilder builder = new StringBuilder(serializedTree.Length + encodedText.Length + 2);
            builder.Append(serializedTree);
            builder.Append('\n');
            builder.Append(encodedText);

            WriteToDummyFile(builder.ToString());

            string readFromFile = ReadFromDummyFile();

            int currIndex;
            HuffmanTree.DeserializeFromString(readFromFile, out currIndex);

            string encodedReadFromFile = readFromFile.Substring(currIndex+1);

            string realDecodedFromFile = tree.Decode(encodedReadFromFile);


            Console.WriteLine("Encoding");
            Console.WriteLine(encoding.ToString);
            Console.WriteLine("Tree");
            Console.WriteLine(tree.SerializeToString());

            TestUtils.WriteSeparator("Compressed Encoding");
            Console.WriteLine(encodedText);


            TestUtils.WriteSeparator("Real Encoded text, read from file");
            Console.WriteLine(encodedReadFromFile);

            TestUtils.WriteSeparator("Real Decoded text, read from file");
            Console.WriteLine(realDecodedFromFile);
        }

        public static void PrintOptsAndFiles(CommandDescription c)
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
