using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ipmt.Utils
{
    public class FileUtils
    {
        public static void StringToFile(string data, string path, Encoding encoding)
        {
            using (StreamWriter sw = new StreamWriter(new FileStream(path, FileMode.OpenOrCreate), encoding))
            {
                sw.Write(data);
            }
        }
    }
}
