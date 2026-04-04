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
            Width = 225,
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
            Height = 150
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

        var stack = new StackPanel();

        stack.Children.Add(textBox);
        
        var scroll = new ScrollViewer
        {
            Height = 100,
            Content = stack
        };

        Canvas.SetTop(scroll, 5);
        Canvas.SetLeft(scroll, 0);
        Canvas.SetRight(scroll, 0);
        Canvas.SetBottom(scroll, 20);
        Canvas.SetBottom(copyButton, 10);
        Canvas.SetLeft(copyButton, 80);
        Canvas.SetRight(copyButton, 80);

        canvas.Children.Add(scroll);
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