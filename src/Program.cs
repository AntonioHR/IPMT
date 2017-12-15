using ipmt.Engine;

namespace ipmt
{
    class Program
    {
        static void Main(string[] args)
        {
            //TestUtils.HuffmanTest();
            //TestUtils.HuffmanCompleteTest();
            //TestUtils.SuffArrayTest();


            CommandDescription c = CommandDescription.ParseFrom(args);
            var cmd = Command.BuildFrom(c);
            cmd.Run();
            
        }


        
    }
}
