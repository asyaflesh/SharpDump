using System;
using Xunit;
using System.IO;
using SharpDump;

namespace SharpDumpTest
{
    public class FileCompressorTest
    {

        [Fact]
        public void FileCompressor_OutFileExists_ReturnsException()
        {
            Assert.Throws<IOException>(() =>
            {
                File.Create("./../../../files/test 2.txt");
                FileCompressor.Compress("./../../../files/test 1.txt", "./../../../files/test 2.txt");
            });
        }

        [Fact]
        public void FileCompressor_Exception_ReturnsException()
        {
            Assert.Throws<Exception>(() =>
            {
                FileCompressor.Compress("./../../../files/test 3.txt", "./../../../files/test_3.txt");
            });
        }

    }
}
