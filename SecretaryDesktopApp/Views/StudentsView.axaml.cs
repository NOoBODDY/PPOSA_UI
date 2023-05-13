using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SecretaryDesktopApp.Views;

public partial class StudentsView : UserControl
{
    public StudentsView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}