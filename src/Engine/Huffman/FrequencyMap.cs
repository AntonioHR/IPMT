using System.Collections.Generic;

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
                int c;
                TryGetValue(text[i], out c);
                this[text[i]] = c + 1;
            }
        }
    }
}
