namespace EngineerCalc.Api;

internal sealed class RingBuffer<T>
{
    private readonly T[] _buffer;
    private readonly Lock _lock;
    private int _start;
    private int _end;

    public RingBuffer(int capacity)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(capacity, 1, nameof(capacity));
        _buffer = new T[capacity];
        _lock = new Lock();
        _start = 0;
        _end = 0;
    }

    public void Add(T item)
    {
        lock (_lock)
        {
            _buffer[_end] = item;
            _end = (_end + 1) % _buffer.Length;
            if (_end == _start)
            {
                _start = (_start + 1) % _buffer.Length; // Overwrite the oldest item
            }
        }
    }

    public void Clear()
    {
        lock (_lock)
        {
            _start = 0;
            _end = 0;
            Array.Clear(_buffer);
        }
    }

    public List<T> GetValues()
    {
        lock (_lock)
        {
            return Enumerate().ToList();
        }
         
    }

    private IEnumerable<T> Enumerate()
    {
        int count = (_end - _start + _buffer.Length) % _buffer.Length;
        for (int i = 0; i < count; i++)
        {
            yield return _buffer[(_start + i) % _buffer.Length];
        }
    }
}
