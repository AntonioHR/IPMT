using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ipmt.Engine.Commands
{
    public abstract class StringMatchCommand : MultiTextCommand
    {
        protected List<string> Patterns { get; private set; }
        protected ResultHandler.Params HandlerParams { get; private set; }

        public StringMatchCommand(List<string> textFileNames, List<string> patterns, ResultHandler.Params handlerParams)
            :base(textFileNames)
        {
            this.Patterns = patterns;
            this.HandlerParams = handlerParams;
        }

        public override void AfterExecute()
        {
            throw new NotImplementedException();
        }
        public override void BeforeExecute()
        {
            Train();
        }
        public override void ExecuteForText(string text, string fileName)
        {
            Match(text);
        }


        protected abstract void Train();
        protected abstract void Match(string text);

        public static new StringMatchCommand BuildFrom(CommandDescription description)
        {
            var txtFiles = new List<string>(description.TextFiles);

            List<string> patts = GetAllPatterns(description);

            bool shouldPrint = !description.Contains(CommandDescription.OptionType.Count);
            var p = new ResultHandler.Params(shouldPrint);


            throw new NotImplementedException();
        }

    }

}
