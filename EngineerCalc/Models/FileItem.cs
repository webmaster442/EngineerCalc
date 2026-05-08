//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace EngineerCalc.Models;

internal sealed record class FileItem : FolderItem
{
    public required string Extension { get; init; }
    public required FileSize FileSize { get; init; }
}
