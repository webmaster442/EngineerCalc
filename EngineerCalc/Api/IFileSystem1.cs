using EngineerCalc.Models;

namespace EngineerCalc.Api;

internal interface IFileSystem
{
    IEnumerable<FolderItem> GetDirectoryNames(string fullPath);
    IEnumerable<FileItem> GetFileNames(string fullPath);
}
