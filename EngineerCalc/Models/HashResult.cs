namespace EngineerCalc.Models;

internal ref struct HashResult
{
    private readonly Span<byte> _hash;

    public HashResult(Span<byte> hash)
    {
        _hash = hash;
    }

    public override string ToString()
    {
        return $"""
            hex:    {Convert.ToHexString(_hash)}
            base64: {Convert.ToBase64String(_hash)}
            """;
    }
}
