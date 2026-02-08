namespace EngineerCalc.Tui.Readline;

internal sealed class CompleterState
{
    private readonly ICompleter? _completer;
    private readonly List<string> _completions;
    private int _currentIndex;
    private bool _searchPerformed;

    public CompleterState(ICompleter? completer)
    {
        _completer = completer;
        _completions = new List<string>();
    }

    public void Reset()
    {
        _completions.Clear();
        _searchPerformed = false;
        _currentIndex = 0;
    }

    public bool CompletionsAvailable => _completions.Count > 0 && _searchPerformed;

    public bool TryGetNext(string currentText, int currentPosition, out string completion)
    {
        if (_completer == null)
        {
            completion = string.Empty;
            return false;
        }

        if (_completions.Count == 0)
            FillCompletions(currentText, currentPosition);

        if (CompletionsAvailable)
        {
            _currentIndex = (_currentIndex + 1) % _completions.Count;
            completion = _completions[_currentIndex];
            return true;
        }

        completion = string.Empty;
        return false;
    }

    public bool TryGetPrevious(string currentText, int currentPosition, out string completion)
    {
        if (_completer == null)
        {
            completion = string.Empty;
            return false;
        }

        if (_completions.Count == 0)
            FillCompletions(currentText, currentPosition);

        if (CompletionsAvailable)
        {
            _currentIndex = (_currentIndex - 1) % _completions.Count;
            completion = _completions[_currentIndex];
            return true;
        }

        completion = string.Empty;
        return false;
    }

    private void FillCompletions(string currentText, int currentPosition)
    {
        if (_completer == null)
            return;

        _completions.AddRange(_completer.GetCompletion(currentText, currentPosition));
        _currentIndex = 0;
        _searchPerformed = true;
    }
}
