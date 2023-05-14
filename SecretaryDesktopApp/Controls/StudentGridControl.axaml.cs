using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using SecretaryDesktopApp.Models;
using SecretaryDesktopApp.Models.DTO;

namespace SecretaryDesktopApp.Controls;

public partial class StudentGridControl : UserControl
{

    static StudentGridControl()
    {
    }
    private DataGrid _dataGrid;
    
    public StudentGridControl()
    {
        InitializeComponent();
        _dataGrid = this.FindControl<DataGrid>("PART_DataGrid");
        _dataGrid.Sorting+= DataGridOnSorting;
    }

   private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        
    }
   #region StyledProperties

    /// <summary>
    /// Identifies the ItemsSource dependency property.
    /// </summary>
    public static readonly DirectProperty<StudentGridControl, IEnumerable> ItemsProperty =
        AvaloniaProperty.RegisterDirect<StudentGridControl, IEnumerable>(
            nameof(Items),
            o => o.Items,
            (o, v) => o.Items = v);
    
    
    /// <summary>
    /// Gets or sets a collection that is used to generate the content of the control.
    /// </summary>
    public IEnumerable Items
    {
        get { return _dataGrid.Items; }
        set { _dataGrid.Items = value; }
    }

    
    public static readonly DirectProperty<StudentGridControl, ICommand> SortingProperty =
        AvaloniaProperty.RegisterDirect<StudentGridControl, ICommand>(nameof(Sorting),
            button => button.Sorting!, (button, command) => button.Sorting = command, enableDataValidation: true);

    private ICommand _sorting;
    public ICommand? Sorting
    {
        get => _sorting;
        set =>  SetAndRaise(SortingProperty!, ref _sorting!, value);
    }
    
    
    public static readonly DirectProperty<StudentGridControl, ICommand> LoadAdditionalInfoProperty =
        AvaloniaProperty.RegisterDirect<StudentGridControl, ICommand>(nameof(LoadAdditionalInfo),
            button => button.LoadAdditionalInfo!, (button, command) => button.LoadAdditionalInfo = command, enableDataValidation: true);

    private ICommand _loadAdditionalInfo;
    public ICommand? LoadAdditionalInfo
    {
        get => _loadAdditionalInfo;
        set =>  SetAndRaise(LoadAdditionalInfoProperty!, ref _loadAdditionalInfo!, value);
    }
    
    public static readonly StyledProperty<object> SortingParameterProperty =
        AvaloniaProperty.Register<Button, object>(nameof(SortingParameter));
    
    
    public object SortingParameter
    {
        get => GetValue(SortingParameterProperty);
        set => SetValue(SortingParameterProperty, value);
    }

    public static StyledProperty<ObservableCollection<StudentColumnActivation>> StudentColumnActivationsProperty =
        AvaloniaProperty.Register<StudentGridControl, ObservableCollection<StudentColumnActivation>>(nameof(StudentColumnActivations), default!, false, BindingMode.Default, null, null, Notifying);

    private static void Notifying(IAvaloniaObject avaloniaObject, bool arg2)
    {
        if (avaloniaObject is not StudentGridControl studentGridControl) return;
        if (studentGridControl.StudentColumnActivations != null!)
        {
            studentGridControl.StudentColumnActivations.CollectionChanged +=
                StudentColumnActivationsOnCollectionChanged;
            foreach (var studentColumnActivation in studentGridControl.StudentColumnActivations)
            {
                studentColumnActivation.PropertyChanged += studentGridControl.ColumnChanged;
                studentGridControl.AddColumn(studentColumnActivation);

            }
        }
    }

   

    public ObservableCollection<StudentColumnActivation> StudentColumnActivations
    {
        get => GetValue(StudentColumnActivationsProperty);
        set => SetValue(StudentColumnActivationsProperty, value);
    }

    #endregion

    private static void StudentColumnActivationsOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (sender is not StudentGridControl studentGridControl) return;
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                foreach (var item in e.NewItems!)
                {
                    if (item is StudentColumnActivation studentColumnActivation)
                    {
                        studentColumnActivation.PropertyChanged += studentGridControl.ColumnChanged;
                        studentGridControl.AddColumn(studentColumnActivation);
                    }
                }
                break;
            case NotifyCollectionChangedAction.Reset:
            case NotifyCollectionChangedAction.Remove:
                foreach (var item in e.OldItems!)
                {
                    if (item is StudentColumnActivation studentColumnActivation)
                        studentColumnActivation.PropertyChanged-= studentGridControl.ColumnChanged;
                }
                break;
            
        }
    }

    private void AddColumn(StudentColumnActivation studentColumnActivation)
    {
        if (_dataGrid.Columns.FirstOrDefault(column=> column.Header as TableColumnHeader == studentColumnActivation.ColumnName) == default)
            _dataGrid.Columns.Add(new DataGridTextColumn(){ Binding = studentColumnActivation.Binding, IsVisible = studentColumnActivation.IsActive, Header = studentColumnActivation.ColumnName});
    }
    private void ColumnChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (sender is not StudentColumnActivation studentColumnActivation ) return;
        if (e.PropertyName == "IsActive")
        {
            var column =
                _dataGrid.Columns.FirstOrDefault(
                    column => column.Header as TableColumnHeader == studentColumnActivation.ColumnName);
            if (column is { })
            {
                column.IsVisible = studentColumnActivation.IsActive;
            }
        }
    }

    private bool _sortingDesc = false;
    
    private void DataGridOnSorting(object? sender, DataGridColumnEventArgs e)
    {
        if (!e.Handled && Sorting?.CanExecute(e) == true && e.Column.Header is TableColumnHeader header)
        {
            _sortingDesc = !_sortingDesc;
            Sorting.Execute(new SortingEventArg(_sortingDesc, header.OriginalName));
        }
    }

    public record SortingEventArg(bool IsDesc, string ColumnName);


    private void PART_DataGrid_OnLoadingRowDetails(object? sender, DataGridRowDetailsEventArgs e)
    {
        if (LoadAdditionalInfo is not null && e.Row.DataContext is Student student)
        {
            LoadAdditionalInfo.Execute(student);
        }
    }
}