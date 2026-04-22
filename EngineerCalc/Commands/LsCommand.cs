using System.Text.RegularExpressions;

using EngineerCalc.Api;
using EngineerCalc.Commands.Abstraction;
using EngineerCalc.Models;

namespace EngineerCalc.Commands;

internal sealed class LsCommand : TableCommand<FolderItem>
{
    private readonly IFileSystem _fileSystem;
    private readonly State _state;

    public LsCommand(IFileSystem fileSystem, State state)
    {
        _fileSystem = fileSystem;
        _state = state;
    }

    protected override IEnumerable<FolderItem> GetDataSet(Regex filter)
    {
        return _fileSystem.GetDirectoryNames(_state.CurrentDirectory)
            .Concat(_fileSystem.GetFileNames(_state.CurrentDirectory))
            .Where(item => filter.IsMatch(item.Name));
    }

    protected override string[] GetTableHeaders()
        => [nameof(FileItem.Name), nameof(FileItem.Extension), nameof(FileItem.LastModified), nameof(FileItem.FileSize)];

    protected override string[] ToTableRow(FolderItem data)
    {
        if (data is FileItem file)
        {
            return [file.Name, file.Extension, file.LastModified.ToString("g"), file.FileSize.ToString()];
        }
        else
        {
            return [data.Name, string.Empty, data.LastModified.ToString("g"), string.Empty];
        }
    }
}
