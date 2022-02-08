using System.Diagnostics;
using SharpDump.Interfeces;

namespace SharpDump
{
    public class Messages : IMessages
    {
        public string DumpDirecroryDoesNotExist(string dumpDir)
        {
            return $"[X] Dump directory \"{dumpDir}\" doesn't exist!";
        }

        public string PleaseUseSharpdumpExePidFormat()
        {
            return "Please use \"SharpDump.exe [pid]\" format";
        }

        public string DumpingCompleted(uint targetProcessId)
        {
            return $"[+] Dumping completed. Rename file to \"debug{targetProcessId}.gz\" to decompress";
        }

        public string Deleting(string dumpFile)
        {
            return $"[*] Deleting {dumpFile}";
        }

        public string OperatingSystemAndArchitecture(string OS, string arch)
        {
            return $@"[*] Operating System : {OS}
                   [*] Architecture     : {arch}" + "\n" +
                   "[*] Use \"sekurlsa::minidump debug.out\" \"sekurlsa::logonPasswords full\" on the same OS/arch";
        }

        public string DumpSuccessfulAndCompressing(string dumpFile, string zipFile)
        {
            return $@"[+] Dump successful!
                   [*] Compressing {dumpFile} to {zipFile} gzip file";
        }


        public string DumpingTargetProcessToDumpFile(Process targetProcess, string dumpFile)
        {
            return $"[*] Dumping {targetProcess.ProcessName} ({targetProcess.Id}) to {dumpFile}";
        }
    }
}
