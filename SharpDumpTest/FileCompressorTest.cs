using System;
using Xunit;
using System.IO;
using SharpDump;
using SharpDump.Interfeces;

namespace SharpDumpTest
{
    public class FileCompressorTest
    {
        readonly IFileCompressor _fileCompressor = new FileCompressor();

        [Fact]
        public void FileCompressor_OutFileExists_ReturnsException()
        {
            Assert.Throws<IOException>(() =>
            {
                File.Create("./../../../files/test 2.txt");
                _fileCompressor.Compress("./../../../files/test 1.txt", "./../../../files/test 2.txt");
            });
        }

        [Fact]
        public void FileCompressor_Exception_ReturnsException()
        {

            Assert.Throws<Exception>(() =>
            {
                _fileCompressor.Compress("./../../../files/test 3.txt", "./../../../files/test_3.txt");
            });
        }

    }
}
