using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia.Threading;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SkiaSharp;

namespace PDFViewer;

public class MainWindowViewModel:INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
    
    private ObservableCollection<PreviewPage> _pages = new();
    public ObservableCollection<PreviewPage> Pages
    {
        get => _pages;
        set => SetField(ref _pages, value);
    }
        
    private float _currentScroll;
    public float CurrentScroll
    {
        get => _currentScroll;
        set => SetField(ref _currentScroll, value);
    }

    private float _scrollViewportSize;
    public float ScrollViewportSize
    {
        get => _scrollViewportSize;
        set
        {
            SetField(ref _scrollViewportSize, value);
            VerticalScrollbarVisible = value < 1;
        }
    }

    private bool _verticalScrollbarVisible;
    public bool VerticalScrollbarVisible
    {
        get => _verticalScrollbarVisible;
        private set => Dispatcher.UIThread.Post(() => SetField(ref _verticalScrollbarVisible, value));
    }

    public MainWindowViewModel()
    {
        var document = Document
            .Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(20).FontFamily("Times New Roman"));

                    page.Header()
                        .Text("Димас, иди нахуй")
                        .SemiBold().FontSize(36).FontColor(Colors.Blue.Darken2);

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(x =>
                        {
                            x.Spacing(20);

                            x.Item().Table(t =>
                            {
                                t.ColumnsDefinition(c =>
                                {
                                    c.RelativeColumn();
                                    c.RelativeColumn(3);
                                });

                                t.Cell().Border(1).Background(Colors.Grey.Lighten3).Padding(5).Text("Номер один");
                                t.Cell().Border(1).Padding(5)
                                    .Text("Классная табличка");
                                t.Cell().Border(1).Background(Colors.Grey.Lighten3).Padding(5).Text("Номер два");
                                t.Cell().Border(1).Padding(5).Text("рад что работает");
                            });

                            x.Item().Text("Но конечно можно и заебаться с такой хуйней");
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Page ");
                            x.CurrentPageNumber();
                        });
                });
            });

        foreach (var pictureData in document.GenerateImages())
        {
            var image = SKImage.FromEncodedData(pictureData);
            //var picture = SKPicture.Deserialize(image.Encode());
            Pages.Add(new PreviewPage(image, image.Width, image.Height));
        }
    }
}