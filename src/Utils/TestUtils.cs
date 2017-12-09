using ipmt.Engine.Huffman;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    }
}
