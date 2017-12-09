using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ipmt.Engine.Commands
{
    public abstract class MultiTextCommand : Command
    {
        public virtual void BeforeExecute() { }
        public abstract void ExecuteForText(string text, string fileName);
        public virtual void AfterExecute() { }


        protected List<string> TextFileNames { get; private set; }

        public MultiTextCommand(List<string> textFileNames)
        {
            TextFileNames = textFileNames;
        }

        public override void Execute()
        {
            BeforeExecute();
            foreach (var txtFile in TextFileNames)
            {
                using (StreamReader sr = new StreamReader(txtFile))
                {
                    String text = sr.ReadToEnd();
                    ExecuteForText(text, txtFile);
                }
            }
            AfterExecute();
        }
    }
}
