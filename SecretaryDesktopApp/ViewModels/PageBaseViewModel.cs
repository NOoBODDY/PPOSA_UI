namespace SecretaryDesktopApp.ViewModels;

public class PageBaseViewModel:NotifyBase
{

    private string _title;

    public string Title
    {
        get => _title;
        set => Update(ref _title, value);
    }

    private string _iconData;

    public string IconData
    {
        get => _iconData;
        set => Update(ref _iconData, value);
    }

}