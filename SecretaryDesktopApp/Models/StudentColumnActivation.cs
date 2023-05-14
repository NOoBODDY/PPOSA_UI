using Avalonia.Data;
using SecretaryDesktopApp.ViewModels;

namespace SecretaryDesktopApp.Models;

public class StudentColumnActivation:NotifyBase
{

    private TableColumnHeader _columnName;
    private bool _isActive;

    private IBinding _binding;

    public TableColumnHeader ColumnName
    {
        get => _columnName;
        set => Update(ref _columnName, value);
    }

    public bool IsActive
    {
        get => _isActive;
        set => Update(ref _isActive, value);
    }

    public IBinding Binding
    {
        get => _binding;
        set => Update(ref _binding, value);
    }

    public StudentColumnActivation(TableColumnHeader columnName,  IBinding binding, bool isActive)
    {
        _columnName = columnName;
        _isActive = isActive;
        _binding = binding;
    }
}