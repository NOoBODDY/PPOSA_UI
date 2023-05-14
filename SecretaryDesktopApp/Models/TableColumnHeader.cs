using SecretaryDesktopApp.ViewModels;

namespace SecretaryDesktopApp.Models;

public class TableColumnHeader:NotifyBase
{
    public TableColumnHeader(string visibleName, string originalName)
    {
        _visibleName = visibleName;
        _originalName = originalName;
    }

    private string _visibleName;

    public string VisibleName
    {
        get => _visibleName;
        set => Update(ref _visibleName, value);
    }

    private string _originalName;

    public string OriginalName
    {
        get => _originalName;
        set => Update(ref _originalName, value);
    }

    public override string ToString()
    {
        return string.IsNullOrEmpty(_visibleName)? _originalName: _visibleName;
    }

    public static bool operator ==(TableColumnHeader? first, TableColumnHeader? second)
    {
        if (first is null && second is null)
            return true;
        if (first is null || second is null)
            return false;
        return first.VisibleName == second.VisibleName;
    }

    public static bool operator !=(TableColumnHeader? first, TableColumnHeader? second)
    {
        if (first is null && second is null)
            return false;
        if (first is null || second is null)
            return true;
        return (first.VisibleName != second.VisibleName);
    }
}