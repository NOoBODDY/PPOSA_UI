using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using ExcelLoader;

namespace SecretaryDesktopApp.ViewModels;

public class ExcelLoaderViewModel:PageBaseViewModel
{

    private string _fileName;

    public string FileName
    {
        get => _fileName;
        set => Update(ref _fileName, value);
    }

    private ObservableCollection<string> _sheetsNames;

    public ObservableCollection<string> SheetsNames
    {
        get => _sheetsNames;
        set => Update(ref _sheetsNames, value);
    }

    private string _selectedSheet;

    public string SelectedSheet
    {
        get => _selectedSheet;
        set
            {
                if (Update( ref _selectedSheet, value))
                {
                    SelectedSheetChanged(SheetsNames.IndexOf(value));
                }
            }
    }

    private ObservableCollection<ColumnProperty> _columnPropertyComparison;

    public ObservableCollection<ColumnProperty> ColumnPropertyComparison
    {
        get => _columnPropertyComparison;
        set => Update(ref _columnPropertyComparison, value);
    }

    private ObservableCollection<string> _columnsNames;

    public ObservableCollection<string> ColumnsNames
    {
        get => _columnsNames;
        set => Update(ref _columnsNames, value);
    }

    private ObservableCollection<string> _propertiesNames;

    public ObservableCollection<string> PropertiesName
    {
        get => _propertiesNames;
        set => Update(ref _propertiesNames, value);
    }

    private ObservableCollection<Test> _objectsCollection;

    public ObservableCollection<Test> ObjectsCollection
    {
        get => _objectsCollection;
        set => Update(ref _objectsCollection, value);
    }

    public ExcelLoaderViewModel()
    {
        Title = "Импорт из Экселя";
        IconData = "M14 2H6C4.89 2 4 2.9 4 4V20C4 21.11 4.89 22 6 22H18C19.11 22 20 21.11 20 20V8L14 2M18 20H6V4H13V9H18V20M15 11.93V19H7.93L10.05 16.88L7.22 14.05L10.05 11.22L12.88 14.05L15 11.93Z";
        SheetsNames = new ObservableCollection<string>();
        ColumnPropertyComparison = new ObservableCollection<ColumnProperty>();
    }

    private ExcelReader<Test>? _excelReader;
    public void OpenFile(object str)
    {
        if (str is not string stringPath) return;
        FileName = Path.GetFileName(stringPath);
        Trace.WriteLine(stringPath);
        _excelReader = new ExcelReader<Test>(stringPath, () => new Test());
        SheetsNames.Clear();
        foreach (var sheetsName in _excelReader.GetSheetsNames())
        {
            SheetsNames.Add(sheetsName);
        }

        SelectedSheet = SheetsNames[0];
    }

    private void SelectedSheetChanged(int newSheetNumber)
    {
        if (_excelReader == null) return;
        ColumnsNames = new ObservableCollection<string>(_excelReader.GetColumnsNamesFromDocument(newSheetNumber));
        PropertiesName = new ObservableCollection<string>(_excelReader.GetTProperties());
        ColumnPropertyComparison.Clear();
        for (var index = 0; index < _propertiesNames.Count; index++)
        {
           ColumnPropertyComparison.Add(new ColumnProperty() {PropertyName = PropertiesName[index], ColumnName = ColumnsNames[index]});
        }
    }

    public void ReadFile()
    {
        Dictionary<string, string> columnPropertyComparisonDict = new();
        foreach (var columnProperty in ColumnPropertyComparison)
        {
            columnPropertyComparisonDict.Add(columnProperty.ColumnName, columnProperty.PropertyName);
        }

        ObjectsCollection = new ObservableCollection<Test>(_excelReader.GetObjects(columnPropertyComparisonDict, SheetsNames.IndexOf(SelectedSheet)));
    }
    
    public class Test
    {
        public string Name { get; set; }
        public string Family { get; set; }
        public string Otchestvo { get; set; }
        public Test()
        {
        }
    }

    public class ColumnProperty:NotifyBase
    {
        private string _columnName;

        public string ColumnName
        {
            get => _columnName;
            set => Update(ref _columnName, value);
        }

        private string _propertyName;

        public string PropertyName
        {
            get => _propertyName;
            set => Update(ref _propertyName, value);
        }

        public KeyValuePair<string, string> GetValuePair()
        {
            return new KeyValuePair<string, string>(PropertyName, ColumnName);
        }
    }
}