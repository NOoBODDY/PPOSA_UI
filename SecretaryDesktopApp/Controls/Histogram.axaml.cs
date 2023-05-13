using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SecretaryDesktopApp.Controls;

public partial class Histogram : UserControl
{
    public Histogram()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}