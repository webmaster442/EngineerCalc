//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace EngineerCalc.Models;

internal sealed class DirectoryPath : IEquatable<DirectoryPath?>
{
    private readonly string _value;

    public DirectoryPath(string value)
    {
        _value = value;
    }

    public override bool Equals(object? obj) 
        => Equals(obj as FilePath);

    public bool Equals(DirectoryPath? other)
    {
        return other is not null &&
               _value == other._value;
    }

    public override string ToString()
        => _value;

    public override int GetHashCode()
        => HashCode.Combine(_value);

    public static bool operator ==(DirectoryPath? left, DirectoryPath? right) 
        => EqualityComparer<DirectoryPath>.Default.Equals(left, right);

    public static bool operator !=(DirectoryPath? left, DirectoryPath? right)
        => !(left == right);

    public static implicit operator string(DirectoryPath path)
        => path._value;

    public static implicit operator DirectoryPath(string path)
        => new(path);
}
