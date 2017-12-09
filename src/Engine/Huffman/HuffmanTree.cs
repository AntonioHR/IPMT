using ipmt.Utils.MinMaxHeap;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ipmt.Engine.Huffman
{
    public class HuffmanTree
    {
        private static NodeComparer Comparer {get{return new NodeComparer();} }

        public Node Top;

        public HuffmanTree(FrequencyMap map)
        {

            List<Node> nodes = new List<Node>(map.Count);
            foreach (var keyValuePair in map)
            {
                nodes.Add(new Node(keyValuePair.Key, keyValuePair.Value));
            }

            MinHeap<Node> heap = new MinHeap<Node>(nodes, Comparer);
            

            while(heap.Count != 1)
            {
                Node left = heap.ExtractMin();
                Node right = heap.ExtractMin();
                Top = new Node(left, right);

                heap.Add(Top);
            }
        }
        
        public string SerializeToString()
        {
            StringBuilder builder = new StringBuilder();

            Stack<Node> stack = new Stack<Node>();
            stack.Push(Top);

            while(stack.Count > 0)
            {
                var node = stack.Pop();

                builder.Append(node.SerializeToString());
                builder.Append('\n');

                if (!node.isLeaf)
                { 
                    Debug.Assert(node.left != null && node.right != null);
                    stack.Push(node.right);
                    stack.Push(node.left);
                }
            }

            return builder.ToString();
        }

        internal string DecodeFromDebugString(string encoded)
        {
            StringBuilder sb = new StringBuilder(encoded.Length/5);

            Node curr = Top;
            for (int i = 0; i < encoded.Length; i++)
            {
                if(curr.isLeaf)
                {
                    sb.Append(curr.character);
                    curr = Top;
                }

                Debug.Assert(encoded[i] == '0' || encoded[i] == '1');
                curr = encoded[i] == '0' ? curr.left : curr.right;
            }
            sb.Append(curr.character);

            return sb.ToString();
        }
        

        class NodeComparer : IComparer<Node>
        {
            public int Compare(Node x, Node y)
            {
                return x.totalFreq - y.totalFreq;
            }
        }

        public class Node
        {
            public bool isLeaf;
            public char character;
            public int totalFreq;
            public Node left, right;

            public Node(char c, int totalFreq)
            {
                this.isLeaf = true;
                this.character = c;
                this.totalFreq = totalFreq;
            }

            public Node(Node left, Node right)
            {
                this.character = '$';
                this.left = left;
                this.right = right;
                this.isLeaf = false;
                this.totalFreq = left.totalFreq + right.totalFreq;
            }

            public String SerializeToString()
            {
                return String.Format("{0} {1}", isLeaf ? '0' : '1', character);
            }
        }
    }
}
