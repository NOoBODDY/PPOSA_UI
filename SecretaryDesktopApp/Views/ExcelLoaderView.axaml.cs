using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SecretaryDesktopApp.Views;

public partial class ExcelLoaderView : UserControl
{
    public ExcelLoaderView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public async Task<object> OpenFileDialog()
    {
        var dialog = new OpenFileDialog
        {
            AllowMultiple = false,
            Title = "Открыть файл",
            Filters = new List<FileDialogFilter>()
            {
                new FileDialogFilter() { Extensions = new List<string>() { "xlsx" }, Name = "excel" }
            }
        };
        return (await dialog.ShowAsync(this.VisualRoot as Window)).FirstOrDefault();
    }
}