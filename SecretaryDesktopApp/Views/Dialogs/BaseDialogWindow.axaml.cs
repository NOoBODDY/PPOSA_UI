using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SecretaryDesktopApp.Views.Dialogs;

public partial class BaseDialogWindow : Window
{
    public BaseDialogWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}