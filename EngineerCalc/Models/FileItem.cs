namespace EngineerCalc.Models;

internal sealed record class FileItem : FolderItem
{
    public required string Extension { get; init; }
    public required FileSize FileSize { get; init; }
}
