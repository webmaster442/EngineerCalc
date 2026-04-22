using System.Numerics;

namespace EngineerCalc.Models;

internal readonly record struct FileSize :
    IComparable,
    IComparable<FileSize>,
    IComparisonOperators<FileSize, FileSize, bool>
{
    private readonly long _size;

    private FileSize(long size)
    {
        _size = size;
    }

    public static implicit operator FileSize(long size)
        => new FileSize(size);

    public static implicit operator long(FileSize fileSize)
        => new FileSize(fileSize._size);

    public static bool operator >(FileSize left, FileSize right)
        => left._size > right._size;

    public static bool operator >=(FileSize left, FileSize right)
        => left._size >= right._size;

    public static bool operator <(FileSize left, FileSize right)
        => (left._size < right._size);

    public static bool operator <=(FileSize left, FileSize right)
        => left._size <= right._size;

    public override string ToString()
    {
        const long KiB = 1024;
        const long MiB = KiB * 1024;
        const long GiB = MiB * 1024;
        const long TiB = GiB * 1024;
        const long PiB = TiB * 1024;
        const long EiB = PiB * 1024;

        return _size switch
        {
            >= EiB => $"{_size / (double)EiB:F2} EiB",
            >= PiB => $"{_size / (double)PiB:F2} PiB",
            >= TiB => $"{_size / (double)TiB:F2} TiB",
            >= GiB => $"{_size / (double)GiB:F2} GiB",
            >= MiB => $"{_size / (double)MiB:F2} MiB",
            >= KiB => $"{_size / (double)KiB:F2} KiB",
            _ => $"{_size} B"
        };
    }

    public int CompareTo(object? obj)
    {
        if (obj is FileSize other)
        {
            return CompareTo(other);
        }
        return -1;
    }

    public int CompareTo(FileSize other)
        => _size.CompareTo(other._size);
}
