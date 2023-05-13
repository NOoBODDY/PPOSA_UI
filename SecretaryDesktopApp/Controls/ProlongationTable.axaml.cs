using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SecretaryDesktopApp.Controls;

public partial class ProlongationTable : UserControl
{
    public ProlongationTable()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}