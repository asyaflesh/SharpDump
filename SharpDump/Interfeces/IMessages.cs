using System.Diagnostics;

namespace SharpDump.Interfeces
{
    public interface IMessages
    {
        string DumpDirecroryDoesNotExist(string dumpDir);
        string PleaseUseSharpdumpExePidFormat();
        string DumpingCompleted(uint targetProcessId);
        string Deleting(string dumpFile);
        string OperatingSystemAndArchitecture(string OS, string arch);
        string DumpSuccessfulAndCompressing(string dumpFile, string zipFile);
        string DumpingTargetProcessToDumpFile(Process targetProcess, string dumpFile);
    }
}