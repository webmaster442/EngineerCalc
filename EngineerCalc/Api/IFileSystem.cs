//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using EngineerCalc.Models;

namespace EngineerCalc.Api;

internal interface IFileSystem
{
    bool FileExists(string filePath);
    IEnumerable<FolderItem> GetDirectoryNames(string fullPath);
    IEnumerable<FileItem> GetFileNames(string fullPath);
    Stream OpenRead(string fullPath);
    Stream Create(string fullPath);
    void SetCurrentDirectory(string fullPath);
}
