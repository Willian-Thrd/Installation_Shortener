using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Media;

public class NotificationWindow : Window
{
    public NotificationWindow(string mensage, string title, string color)
    {
        Title = title;
        Width = 250;
        Height = 100;

        var brush = new SolidColorBrush(Color.Parse(color));

        var textBox = new TextBox
        {
            Text = mensage,
            IsReadOnly = true,
            BorderThickness = new Avalonia.Thickness(0),
            Background = Brushes.Transparent,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
            TextWrapping = TextWrapping.Wrap,
            Foreground = brush
        };
        
        var copyButton = new Button
        {
            Content = "Copiar",
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
            Margin = new Avalonia.Thickness(0, 10, 0, 0)
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

        var stack = new StackPanel
        {
            Orientation = Avalonia.Layout.Orientation.Vertical,
            Children =
            {
                textBox,
                copyButton
            }
        };

        Content = stack;
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