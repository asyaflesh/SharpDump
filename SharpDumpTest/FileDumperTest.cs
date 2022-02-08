using SharpDump;
using System;
using Xunit;

namespace SharpDumpTest
{
    public class FileDumperTest
    {
        private readonly FileDumper _fileDumper = new FileDumper();

        [Fact]
        public void FileDumper_WithoutParameters_ReturnsException()
        {
            Assert.Throws<Exception>(() =>
            {
                _fileDumper.MiniDump();
            });
        }

        [Fact]
        public void FileDumper_1000AsPerameter_ReturnsException()
        {
            Assert.Throws<Exception>(() =>
            {
                _fileDumper.MiniDump(1000);
            });
        }
    }
}
