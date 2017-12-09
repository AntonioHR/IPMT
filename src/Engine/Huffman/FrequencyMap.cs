using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ipmt.Engine.Huffman
{
    public class FrequencyMap : Dictionary<char, int>
    {
        public FrequencyMap(string text) : base(text.Length)
        {
            Build(text);
        }

        public FrequencyMap(Dictionary<char, int> dict) : base(dict)
        {

        }

        private void Build(string text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                TryGetValue(text[i], out int c);
                this[text[i]] = c + 1;
            }
        }
    }
}
