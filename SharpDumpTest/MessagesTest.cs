using SharpDump;
using SharpDump.Interfeces;
using Xunit;

namespace SharpDumpTest
{
    public class MessagesTest
    {
        IMessages _messages = new Messages();

        [Theory]
        [InlineData("SystemRoot\\Temp\\")]
        public void DumpDirecroryDoesNotExistTest(string dumpDir)
        {
            var expected = "[X] Dump directory \"SystemRoot\\Temp\\\" doesn't exist!";
            Assert.Equal(_messages.DumpDirecroryDoesNotExist(dumpDir), expected);
        }

        [Fact]
        public void PleaseUseSharpdumpExePidFormatTest()
        {
            var expected = "Please use \"SharpDump.exe [pid]\" format";
            Assert.Equal(_messages.PleaseUseSharpdumpExePidFormat(), expected);
        }

        [Theory]
        [InlineData(123)]
        public void DumpingCompletedTest(uint targetProcessId)
        {
            var expected = "[+] Dumping completed. Rename file to \"debug123.gz\" to decompress";
            Assert.Equal(_messages.DumpingCompleted(targetProcessId), expected);
        }

        [Theory]
        [InlineData("file.txt")]
        public void DeletingTest(string dumpFile)
        {
            var expected = "[*] Deleting file.txt";
            Assert.Equal(_messages.Deleting(dumpFile), expected);
        }

        [Theory]
        [InlineData("Operation System", "Architecture")]
        public void OperatingSystemAndArchitectureTest(string OS, string arch)
        {
            var expected = @"[*] Operating System : Operation System
                   [*] Architecture     : Architecture" + "\n" +
                           "[*] Use \"sekurlsa::minidump debug.out\" \"sekurlsa::logonPasswords full\" on the same OS/arch";

            Assert.Equal(_messages.OperatingSystemAndArchitecture(OS, arch), expected);
        }

        [Theory]
        [InlineData("systemRoot\\Temp\\debug123.out", "systemRoot\\Temp\\debug123.bin")]
        public void DumpSuccessfulAndCompressingTest(string dumpFile, string zipFile)
        {
            var expected = @"[+] Dump successful!
                   [*] Compressing systemRoot\Temp\debug123.out to systemRoot\Temp\debug123.bin gzip file";

            Assert.Equal(_messages.DumpSuccessfulAndCompressing(dumpFile, zipFile), expected);
        }
    }
}
