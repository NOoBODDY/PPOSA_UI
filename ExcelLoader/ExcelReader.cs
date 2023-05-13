using System.ComponentModel;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace ExcelLoader;

public class ExcelReader<T> where T:class
{

    private readonly string _path;
    private readonly Func<T> _objActivator;
    public ExcelReader(string path, Func<T> objActivator)
    {
        _path = path;
        _objActivator = objActivator;
    }

    public IEnumerable<string> GetSheetsNames()
    {
        using SpreadsheetDocument spreadsheetDocument =
            SpreadsheetDocument.Open(_path, false);
        WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
        var sheets = workbookPart.Workbook.Sheets.Cast<Sheet>();

        return sheets.Select(x => x.Name.Value);
    }

    public IEnumerable<string> GetColumnsNamesFromDocument(int sheetNumber)
    {
        using SpreadsheetDocument spreadsheetDocument =
            SpreadsheetDocument.Open(_path, false);
        WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
        var sharedStrings = spreadsheetDocument.WorkbookPart.SharedStringTablePart.SharedStringTable;
        var worksheetPart = workbookPart.WorksheetParts.Skip(sheetNumber-1).First();
        return GetColumnsNamesFromSheet(worksheetPart, sharedStrings);
    }

    private IEnumerable<string> GetColumnsNamesFromSheet(WorksheetPart worksheetPart, SharedStringTable sharedStringTable)
    {
        SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();
        string text;
        List<string> strings = new();
        foreach (Row r in sheetData.Elements<Row>())
        {
            foreach (Cell c in r.Elements<Cell>())
            {

                text = GetCellText(c, sharedStringTable);
                strings.Add(text);
            }

            if (strings.Any())
               break;
        }

        return strings;
    }
    
    private string GetCellText(Cell c, SharedStringTable saSST)
    {
        string val = "";

        if ((c.DataType != null) && (c.DataType == CellValues.SharedString))
        {
            int ssid;
            if (int.TryParse(c.CellValue.Text, out ssid))
            {
                if (saSST != null && ssid >= 0 && ssid < saSST.Count)
                {
                    var str = (saSST.ChildElements[ssid] as SharedStringItem);
                    if (str.Count() == 1)
                        val = str.Text.Text;
                    else
                    {
                        val = "";
                        foreach (var run in str.Cast<Run>())
                        {
                            val += run.Text.Text;
                        }
                    }
                }
            }
        }
        else if ((c.DataType != null) && c.DataType == CellValues.InlineString)
        {
            val = c.InnerText;
        }
        else if (c.CellValue != null)
        {
            val = c.CellValue.Text;
        }

        if (val == null)
            val = "";

        return val;

    }

    public IEnumerable<string> GetTProperties()
    {
        return GetTypeProperties(typeof(T));
    }

    private static IEnumerable<string> GetTypeProperties(Type type)
    {
        var props = type.GetProperties();
        return props.Select(info => info.Name);
    }


    private Dictionary<int, Action<T,string>> CreatePropertyColumnComparison(Dictionary<string,string> columnProperty, int sheetNumber)
    {
        var propertyColumnComparison = new Dictionary<int, Action<T,string>>();
        var columnNames = GetColumnsNamesFromDocument(sheetNumber).ToArray();
        var props = typeof(T).GetProperties();
        for (var index = 0; index < columnNames.Length; index++)
        {
            if (columnProperty.TryGetValue(columnNames[index], out var propertyName))
            {
                var property = props.FirstOrDefault(p => p.Name == propertyName);
                if (property != null)
                {
                    propertyColumnComparison.Add(index,
                        (t, s) => property.SetMethod.Invoke(
                            t, new[]
                            {
                                TypeDescriptor.GetConverter(property.PropertyType).ConvertFromString(s)
                            }));
                }
            }
        }

        return propertyColumnComparison;
    }


    public IEnumerable<T> GetObjects(Dictionary<string, string> columnProperty, int sheetNumber)
    {
        var propertyColumnComparison = CreatePropertyColumnComparison(columnProperty, sheetNumber);
        
        using SpreadsheetDocument spreadsheetDocument =
            SpreadsheetDocument.Open(_path, false);
        WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
        var sharedStrings = spreadsheetDocument.WorkbookPart.SharedStringTablePart.SharedStringTable;
        var worksheetPart = workbookPart.WorksheetParts.Skip(sheetNumber-1).First();
        SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();
        string text;
        List<T> objects = new();
        foreach (Row r in sheetData.Elements<Row>())
        {
            T obj = _objActivator.Invoke();
            foreach (var (c, index) in r.Elements<Cell>().WithIndex())
            {
                if (propertyColumnComparison.TryGetValue(index, out var propertySet))
                {
                    text = GetCellText(c, sharedStrings);
                    propertySet(obj, text);
                }
            }
            objects.Add(obj);
            
        }

        return objects;
    }

}