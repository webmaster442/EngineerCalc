//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace EngineerCalc.Models;

internal record class FolderItem
{
    public required string Name { get; init; }
    public required DateTime LastModified { get; init; }
}
