using ipmt.Utils;
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

        public HuffmanTree (Node Top)
        {
            this.Top = Top;
        }
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
            
            int nodeCount = 0;
            while (stack.Count > 0)
            {
                nodeCount++;
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
            builder.Insert(0, string.Format("{0}\n", nodeCount));
            return builder.ToString();
        }

        public static HuffmanTree DeserializeFromString(string str, out int resultIndex)
        {
            int eol =  str.IndexOf('\n');
            int nodeCount = int.Parse(str.Substring(0, eol));

            int charsPerNode = 4;
            int index = eol + 1;
            resultIndex = index + nodeCount * charsPerNode;

            
            Node top = null;

            char prevNodeChar;
            Stack<Node> parents = new Stack<Node>();
            while (index < resultIndex)
            {
                Debug.Assert(str[index] == '1' || str[index] == '0');
                var debug = str.Substring(Math.Max(0, index - charsPerNode), charsPerNode * 3).ToCharArray();
                bool isLeaf = str[index] == '0';
                char nodeChar = str[index + 2];


                //if (nodeChar == '\n')
                //{
                //    if (str[index + 3] == '\r')
                //    {
                //        //nodeChar = '\n';
                //        //resultIndex += 1;
                //        //index += 1;
                //    }
                //    else
                //    {
                //        nodeChar = '\r';
                //        resultIndex -= 2;
                //        index -= 2;
                //    }
                //}
                    //if (nodeChar == '\r')
                    //{
                    //    resultIndex -= 1;
                    //    index -= 1;
                    //}
                Node curr = new Node(isLeaf, nodeChar);


                if (top == null)
                {
                    top = curr;
                }
                else
                {
                    Node parent = parents.Peek();

                    if (parent.left == null)
                        parent.left = curr;
                    else
                    {
                        Debug.Assert(parent.right == null);
                        parent.right = curr;
                        parents.Pop();
                    }
                }

                if(!isLeaf)
                {
                    parents.Push(curr);
                }

                prevNodeChar = nodeChar;
                index += charsPerNode;
            }
            return new HuffmanTree(top);
        }

        internal string Decode(string encoded)
        {
            char[] split = { '\n' };
            var countAndText = encoded.Split(split, 2);

            int size = int.Parse(countAndText[0]);
            return Decode(countAndText[1], size);

        }
        public string Decode(string encoded, int size)
        {
            BitStringReader reader = new BitStringReader(encoded);
            StringBuilder sb = new StringBuilder(encoded.Length * 5);
            Node curr = Top;

            for (int i = 0; i < size; i++)
            {
                if (curr.isLeaf)
                {
                    sb.Append(curr.character);
                    curr = Top;
                }
                curr = reader.ReadBit() ? curr.left : curr.right;
            }
            sb.Append(curr.character);

            return sb.ToString();
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

            public Node(bool isLeaf, char character)
            {
                this.isLeaf = isLeaf;
                this.character = character;
                totalFreq = -1;
            }

            public String SerializeToString()
            {
                return String.Format("{0} {1}", isLeaf ? '0' : '1', character);
            }
        }
    }
}
