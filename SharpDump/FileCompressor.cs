using System;
using System.IO;
using System.IO.Compression;

namespace SharpDump
{
    public class FileCompressor
    {
        public static void Compress(string inFile, string outFile)
        {
            if (File.Exists(outFile))
            {
                File.Delete(outFile);
                throw new IOException("file exists");
            }
            try
            {
                var bytes = File.ReadAllBytes(inFile);
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
