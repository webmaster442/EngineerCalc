//-----------------------------------------------------------------------------
// (c) 2024-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace EngineerCalc.Models;

internal class FilePath : IEquatable<FilePath?>
{
    private readonly string _value;

    public FilePath(string value)
    {
        _value = value;
    }

    public override bool Equals(object? obj) 
        => Equals(obj as FilePath);

    public bool Equals(FilePath? other)
    {
        return other is not null &&
               _value == other._value;
    }

    public override string ToString()
        => _value;

    public override int GetHashCode()
        => HashCode.Combine(_value);

    public static bool operator ==(FilePath? left, FilePath? right) 
        => EqualityComparer<FilePath>.Default.Equals(left, right);

    public static bool operator !=(FilePath? left, FilePath? right)
        => !(left == right);

    public static implicit operator string(FilePath path) 
        => path._value;

    public static implicit operator FilePath(string path)
        => new(path);
}
