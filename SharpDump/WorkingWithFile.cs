using System.IO;
using SharpDump.Interfeces;

namespace SharpDump
{
    public class WorkingWithFile : IWorkingWithFile
    {

        public byte[] FileReadAllBytes(string inFile)
        {
            return File.ReadAllBytes(inFile);
        }

        public bool FileExists(string file)
        {
            return File.Exists(file);
        }

        public void FileDelete(string file)
        {
            File.Delete(file);
        }
    }
}
