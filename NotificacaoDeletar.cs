using System;
using Avalonia.Controls;
using Avalonia.Media;

public class NotificacaoDeletar : Window{

    public NotificacaoDeletar(string mensage, string title)
    {
        Title = title;
        Width = 550;
        Height = 350;

        var stackPanel = new StackPanel
        {
            Margin = new Avalonia.Thickness(10)
        };
        var textBlock = new TextBlock
        {
            Text = mensage,
            Foreground = Brushes.White,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
        };

        var buttonOpt1 = new Button
        {
            Content = "Deletar apenas pacote?",
            Margin = new Avalonia.Thickness(0, 300, 0, 0)
        };

        stackPanel.Children.Add(textBlock);
        stackPanel.Children.Add(buttonOpt1);

        this.Content = stackPanel;
    }
}