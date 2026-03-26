using System;
using System.Threading.Tasks;
using Avalonia.Controls;

public class NotificationWindow : Window
{
    public NotificationWindow(string mensage, String title)
    {
        Title = title;
        Width = 300;
        Height = 150;
        Content = new TextBlock
        {
            Text = mensage,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
            TextWrapping = Avalonia.Media.TextWrapping.Wrap
        };
    }

    public async void Timer()
    {
        this.Show();

        await Task.Delay(3000);

        this.Close();
    }
}