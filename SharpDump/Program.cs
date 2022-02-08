using System;
using System.IO;
using SharpDump.Interfeces;

namespace SharpDump
{
    class Program
    {
        static void Main(string[] args)
        {
            FileDumper fileDumper = new FileDumper();
            IMessages message = new Messages();

            string systemRoot = Environment.GetEnvironmentVariable("SystemRoot");
            string dumpDir = $"{systemRoot}\\Temp\\";
            if (!Directory.Exists(dumpDir))
            {
                Console.WriteLine(message.DumpDirecroryDoesNotExist(dumpDir));
                return;
            }

            if (args.Length == 0)
            {
                // dump LSASS by default
                fileDumper.MiniDump();
            }
            else if (args.Length == 1)
            {
                int retNum;
                if (int.TryParse(Convert.ToString(args[0]), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum))
                {
                    // arg is a number, so we're specifying a PID
                    fileDumper.MiniDump(retNum);
                }
                else
                {
                    Console.WriteLine(message.PleaseUseSharpdumpExePidFormat());
                }
            }
            else if (args.Length == 2)
            {
                Console.WriteLine(message.PleaseUseSharpdumpExePidFormat());
            }
        }
    }
}
