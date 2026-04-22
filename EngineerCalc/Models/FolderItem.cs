namespace EngineerCalc.Models;

internal record class FolderItem
{
    public required string Name { get; init; }
    public required DateTime LastModified { get; init; }
}
