using EngineerCalc.Models;

namespace EngineerCalc.Api;

internal sealed class FileSystem : IFileSystem
{
    public bool FileExists(string filePath)
        => File.Exists(filePath);

    public IEnumerable<FolderItem> GetDirectoryNames(string fullPath)
    {
        var di = new DirectoryInfo(fullPath);
        foreach (var subdir in di.EnumerateDirectories())
        {
            yield return new FolderItem
            {
                LastModified = subdir.LastWriteTime,
                Name = subdir.Name,
            };
        }
    }

    public IEnumerable<FileItem> GetFileNames(string fullPath)
    {
        var di = new DirectoryInfo(fullPath);
        foreach (var file in di.GetFiles())
        {
            yield return new FileItem
            {
                Name = file.Name,
                Extension = file.Extension,
                FileSize = file.Length,
                LastModified = file.LastWriteTime,
            };
        }
    }

    public Stream OpenRead(string fullPath)
        => File.OpenRead(fullPath);

    public void SetCurrentDirectory(string fullPath)
        => Directory.SetCurrentDirectory(fullPath);
}
