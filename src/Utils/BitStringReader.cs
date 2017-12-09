using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ipmt.Utils
{
    class BitStringReader
    {
        const int charSize = 16;

        String str;
        int charIndex;
        int bitIndex;

        char currChar;
        public BitStringReader(string str)
        {
            this.str = str;
            bitIndex = 0;
            charIndex = 0;
            currChar = str[charIndex];
        }
        public bool ReadBit()
        {
            currChar = str[charIndex];
            char myBit = (char)((0x01 << bitIndex) & currChar);
            bitIndex++;

            if(bitIndex == charSize)
            {
                bitIndex = 0;
                charIndex++;
            }

            return myBit == 0x00;
        }
    }
}
