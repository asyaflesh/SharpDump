using System;
using System.IO;
using System.IO.Compression;
using SharpDump.Interfeces;

namespace SharpDump
{
    public class FileCompressor : IFileCompressor
    {
        private readonly IWorkingWithFile _workingWithFile = new WorkingWithFile();

        public void Compress(string inFile, string outFile)
        {
            if (_workingWithFile.FileExists(outFile))
            {
                _workingWithFile.FileDelete(outFile);
                throw new IOException("file exists");
            }
            try
            {
                var bytes = _workingWithFile.FileReadAllBytes(inFile);
                using (FileStream fs = new FileStream(outFile, FileMode.CreateNew))
                {
                    using (GZipStream zipStream = new GZipStream(fs, CompressionMode.Compress, false))
                    {
                        zipStream.Write(bytes, 0, bytes.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error on compressing file");
            }
        }
    }
}
