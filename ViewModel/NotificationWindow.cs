using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

public class NotificationWindow : Window
{
    public NotificationWindow(string mensage, string title, string color)
    {
        Title = title;
        Width = 250;
        Height = 150;
        Background = Brushes.Black;

        var brush = new SolidColorBrush(Color.Parse(color));

        var textBox = new TextBox
        {
            Text = mensage,
            IsReadOnly = true,
            Width = 200,
            BorderThickness = new Avalonia.Thickness(0),
            Background = Brushes.Black,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
            TextWrapping = TextWrapping.Wrap,
            TextAlignment = TextAlignment.Center,
            Foreground = brush
        };

        var canvas = new Canvas
        {
            Width = 250,
            Height = 100,
            Background = Brushes.Black  
        };
        
        var copyButton = new Button
        {
            Content = "Copiar",
            BorderBrush = Brushes.White,
            BorderThickness = new Thickness(2),
            Foreground = Brushes.White,
            HorizontalContentAlignment = Avalonia.Layout.HorizontalAlignment.Center,
            FontSize = 12,
            Width = 90,
            Height = 28
        };

        copyButton.Click += async (sender, e) =>
        {
            var topLevel = TopLevel.GetTopLevel(this);

            if (topLevel != null)
            {
                var clipboard = TopLevel.GetTopLevel(this);

                if (topLevel != null)
                {
                    var cb = topLevel.Clipboard;
                    await cb.SetTextAsync(mensage);
                }
            }
        };

        Canvas.SetTop(textBox, 10);
        Canvas.SetLeft(textBox, 25);
        Canvas.SetRight(textBox, 25);
        Canvas.SetTop(copyButton, 65);
        Canvas.SetLeft(copyButton, 80);
        Canvas.SetRight(copyButton, 80);

        canvas.Children.Add(textBox);
        canvas.Children.Add(copyButton);

        Content = canvas;
    }

    public async void Timer()
    {
        this.Show();

        await Task.Delay(3000);

        this.Close();
    }

    internal void ShowDialog()
    {
        throw new NotImplementedException();
    }
}