using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ipmt.Utils;
using System.IO;

namespace ipmt.Engine.Huffman
{
    public class HuffmanEncoding
        : Dictionary<char, BitArray>
    {
        public HuffmanEncoding (HuffmanTree tree)
        {
            AddRecursively(tree.Top, new List<bool>());
        }

        private void AddRecursively(HuffmanTree.Node node, List<bool> path)
        {
            if(!node.isLeaf)
            {
                path.Add(false);
                AddRecursively(node.left, path);

                path[path.Count-1] = true;
                AddRecursively(node.right, path);

                path.RemoveAt(path.Count-1);
            } else
            {
                Add(node.character, new BitArray(path.ToArray()));
            }
        }
        

        public new string ToString => string.Join("\n", this.Select(x => x.Key + " " + x.Value.ToBitString()).ToArray());

        internal string EncodeAsDebugString(string txt)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < txt.Length; i++)
            {
                BitArray ba = this[txt[i]];
                builder.Append(ba.ToBitString());
            }

            
            return builder.ToString();
        }

        internal string Encode(string txt)
        {
            BitStringBuilder builder = new BitStringBuilder(txt.Length / 3);

            for (int i = 0; i < txt.Length; i++)
            {
                BitArray ba = this[txt[i]];
                for (int j = 0; j < ba.Length; j++)
                {
                    builder.WriteBit(ba[j]);
                }
            }

            return builder.End();
        }
    }
}
