using ipmt.Engine.Huffman;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        
        public static void WriteToDummyFile(string txt)
        {
            using (StreamWriter sw = new StreamWriter(new FileStream("dummy.txt", FileMode.OpenOrCreate), Encoding.UTF7))
            {
                    sw.Write(txt);
            }
        }

        public static string ReadFromDummyFile()
        {
            using (StreamReader sr = new StreamReader("dummy.txt", Encoding.UTF7 ))
            {
                return sr.ReadToEnd();
            }
        }

        public static string ReadFromFile(Encoding encoding, string name = "text.txt")
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
    }
}
