using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using SecretaryDesktopApp.ViewModels;
using SecretaryDesktopApp.Views;

namespace SecretaryDesktopApp;

public class ViewLocator:IDataTemplate
{
    public IControl Build(object param)
    {
        if (param is ExcelLoaderViewModel)
            return new Views.ExcelLoader();
        return new TextBlock(){Text = param.ToString()};
    }

    public bool Match(object data)
    {
        return (data is ExcelLoaderViewModel);
    }
}