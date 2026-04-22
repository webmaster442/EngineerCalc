using EngineerCalc.Models;

namespace EngineerCalc.Api;

internal interface IFileSystem
{
    bool FileExists(string filePath);
    IEnumerable<FolderItem> GetDirectoryNames(string fullPath);
    IEnumerable<FileItem> GetFileNames(string fullPath);
    Stream OpenRead(string fullPath);
    void SetCurrentDirectory(string fullPath);
}
