namespace SharpDump.Interfeces
{
    public interface IFileCompressor
    {
        void Compress(string inFile, string outFile);
    }
}