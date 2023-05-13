using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.TextFormatting;

namespace SecretaryDesktopApp.Controls;

public class HistogramControl:Control
{

    static HistogramControl()
    {
        AffectsArrange<HistogramControl>(MainValuesProperty, SecondaryValuesProperty, MainBlockColorProperty, SecondaryBlockColorProperty);
    }
    
    public static StyledProperty<int[]> MainValuesProperty =
        AvaloniaProperty.Register<HistogramControl, int[]>(nameof(MainValues));

    public int[] MainValues
    {
        get => GetValue(MainValuesProperty);
        set => SetValue(MainValuesProperty, value);
    }
    
    public static StyledProperty<int[]> SecondaryValuesProperty =
        AvaloniaProperty.Register<HistogramControl, int[]>(nameof(SecondaryValues));

    public int[] SecondaryValues
    {
        get => GetValue(SecondaryValuesProperty);
        set => SetValue(SecondaryValuesProperty, value);
    }
    
    public static StyledProperty<string[]> NamesProperty =
        AvaloniaProperty.Register<HistogramControl, string[]>(nameof(Names));

    public string[] Names
    {
        get => GetValue(NamesProperty);
        set => SetValue(NamesProperty, value);
    }

    public static StyledProperty<IBrush> MainBlockColorProperty =
        AvaloniaProperty.Register<HistogramControl, IBrush>(nameof(MainBlockColor), new SolidColorBrush(Colors.Blue));
    
    public IBrush MainBlockColor
    {
        get => GetValue(MainBlockColorProperty);
        set => SetValue(MainBlockColorProperty, value);
    }
    
    public static StyledProperty<IBrush> SecondaryBlockColorProperty =
        AvaloniaProperty.Register<HistogramControl, IBrush>(nameof(SecondaryBlockColor), new SolidColorBrush(Colors.Red));
    
    public IBrush SecondaryBlockColor
    {
        get => GetValue(SecondaryBlockColorProperty);
        set => SetValue(SecondaryBlockColorProperty, value);
    }
    
    public static StyledProperty<IBrush> StrokeColorProperty =
        AvaloniaProperty.Register<HistogramControl, IBrush>(nameof(StrokeColor), new SolidColorBrush(Colors.Black));
    
    public IBrush StrokeColor
    {
        get => GetValue(StrokeColorProperty);
        set => SetValue(StrokeColorProperty, value);
    }
    
    public static StyledProperty<FontFamily> FontFamilyProperty =
        AvaloniaProperty.Register<HistogramControl, FontFamily>(nameof(FontFamily), FontFamily.Default);
    
    public FontFamily FontFamily
    {
        get => GetValue(FontFamilyProperty);
        set => SetValue(FontFamilyProperty, value);
    }
    
    /*public static StyledProperty<string> HeaderProperty =
        AvaloniaProperty.Register<HistogramControl, string>(nameof(Header));
    
    public string Header
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }*/

    public HistogramControl()
    {
        _scaleTransform = new ScaleTransform(1, -1);
        //RenderTransform = _scaleTransform;
    }

    private ScaleTransform _scaleTransform;

    protected override Size ArrangeOverride(Size finalSize)
    {
        return base.ArrangeOverride(finalSize);
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        return availableSize;
    }

    public override void Render(DrawingContext context)
    {
        base.Render(context);
        var pen = new Pen(StrokeColor, 1);
        
        context.DrawLine(pen, new Point(15,Bounds.Height - 20), new Point(15, 0));
        context.DrawLine(pen, new Point(15,Bounds.Height -40), new Point(Bounds.Width - 15, Bounds.Height -40));

        

        if (MainValues.Any() || SecondaryValues.Any())
        {
            var withSecondary = MainValues.Length == SecondaryValues.Length;
            var max = Math.Max(MainValues.Max(), SecondaryValues.Max()) + 10;

            var heightToValue = Bounds.Height / max;

            var columnWidth = Bounds.Width /
                              ((withSecondary ? MainValues.Length*3 : MainValues.Length*2));
            
            for (int i = 0; i < MainValues.Count(); i++)
            {
                var text = new FormattedText(Names[i], new Typeface(FontFamily), 14, TextAlignment.Center,
                    TextWrapping.NoWrap, new Size());

                var textPont = new Point(20 +(withSecondary? columnWidth + i*columnWidth*3: columnWidth/2 + i*columnWidth*2) - text.Bounds.Width/2, Bounds.Height - 10 - text.Bounds.Height);
                context.DrawText(StrokeColor, textPont, text);
                
                var height = MainValues[i] * heightToValue;
                var x = 20 + i * (withSecondary?columnWidth*3: columnWidth*2);
                var y = Bounds.Height - 40 - height;

                var rect = new Rect(x, y, columnWidth, height);
                context.DrawRectangle(MainBlockColor, null, rect);

                text = new FormattedText(MainValues[i].ToString(), new Typeface(FontFamily), 14, TextAlignment.Center,
                    TextWrapping.NoWrap, new Size());
                
                textPont = new Point(x + columnWidth/2 - text.Bounds.Width/2,  y - text.Bounds.Height -10 );
                
                context.DrawText(StrokeColor, textPont, text);
                
                
                if (!withSecondary) continue;
                var secondaryHeight = SecondaryValues[i] * heightToValue;
                var secondaryX = 20 + columnWidth + i * (columnWidth*3);
                var secondaryY = Bounds.Height - 40 - secondaryHeight;
                var secondaryRect = new Rect(secondaryX, secondaryY, columnWidth, secondaryHeight);
                context.DrawRectangle(SecondaryBlockColor, null, secondaryRect);
                
                
                text = new FormattedText(SecondaryValues[i].ToString(), new Typeface(FontFamily), 14, TextAlignment.Center,
                    TextWrapping.NoWrap, new Size());
                
                textPont = new Point(secondaryX + columnWidth/2 - text.Bounds.Width/2,  secondaryY - text.Bounds.Height -10 );
                
                context.DrawText(StrokeColor, textPont, text);
            }

        }
        
        
    }
}