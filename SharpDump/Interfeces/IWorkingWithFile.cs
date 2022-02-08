namespace SharpDump.Interfeces
{
    public interface IWorkingWithFile
    {
        byte[] FileReadAllBytes(string inFile);
        bool FileExists(string file);
        void FileDelete(string file);
    }
}