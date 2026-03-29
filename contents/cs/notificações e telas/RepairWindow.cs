using Avalonia.Controls;
using Avalonia.Media;

public class RepairWindow : Window
{
    public RepairWindow()
    {
        Title = "Janela de reparo";
        Width = 800;
        Height = 600;
        Background = Brushes.White;
    
        var canvas = new Canvas
        {
            Width = 800,
            Height = 600,
            Background = Brushes.Transparent,
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
        };

        var title = new Label
        {
            Content = "Reparador de pacotes",
            FontSize = 28,
            Foreground = Brushes.Black,
            Width = 283
        };

        Canvas.SetTop(title, 20);
        Canvas.SetLeft(title, 258.5);
        Canvas.SetRight(title, 258.5);

        canvas.Children.Add(title);

        Content = canvas;
    }
}