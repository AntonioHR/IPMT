using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ipmt.Utils
{
    public class BitStringBuilder
    {
        StringBuilder builder;
        char current;

        int bitIndex;
        const int charSize = 16;
        const int oneBit = 0x01;

        long size = 0;

        public BitStringBuilder(int startSize)
        {
            current = (char)0x00;
            builder = new StringBuilder(startSize);
        }

        public void WriteBit(bool bit)
        {
            if (bit)
            {
                char shiftedBit = (char)(oneBit << bitIndex);
                current = (char)(current | shiftedBit);
            }
            string debug = ""+current;
            bitIndex++;
            if(bitIndex == charSize)
            {
                builder.Append(current);
                current = (char)0x00;
                bitIndex = 0;
                size += charSize;
            }
        }
        public string End()
        {
            size += bitIndex;
            if(bitIndex != 0)
            {                
                builder.Append(current);
            }
            builder.Insert(0, string.Format("{0}\n", size));

            return builder.ToString();
        }

    }
}
