using SharpDump;
using System;
using Xunit;

namespace SharpDumpTest
{
    public class FileDumperTest
    {
        [Fact]
        public void FileDumper_WithoutParameters_ReturnsException()
        {
            Assert.Throws<Exception>(() =>
            {
                FileDumper.MiniDump();
            });
        }

        [Fact]
        public void FileDumper_1000AsPerameter_ReturnsException()
        {
            Assert.Throws<Exception>(() =>
            {
                FileDumper.MiniDump(1000);
            });
        }
    }
}
