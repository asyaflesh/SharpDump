using System.Diagnostics;
using System.IO;
using Moq;
using SharpDump;
using SharpDump.Interfeces;
using Xunit;

namespace SharpDumpTest
{
    public class WorkingWithFileTest
    {
        private IWorkingWithFile _workingWithFile = new WorkingWithFile();

        [Fact]
        public void FileExists_WhenFileExists_ResultTrue()
        {
            var file = System.IO.File.Create("./../../../files/file 1.txt").Name;
            Assert.True(_workingWithFile.FileExists(file));
        }

        [Fact]
        public void FileExists_WhenFileNotExists_ResultFalse()
        {
            Assert.False(_workingWithFile.FileExists("./../../../files/notFile.txt"));
        }

       [Theory]
       [InlineData("./../../../files/file 2.txt")]
        public void FileDelete_ResultFileExistsFalse(string path)
        {
            File.Create(path).Close();
            _workingWithFile.FileDelete(path);
            Assert.False(_workingWithFile.FileExists(path));
        }
    }
}
