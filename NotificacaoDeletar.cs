using System;
using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Metadata;

public class NotificacaoDeletar : Window{
        private TextBox entryBox;

    public NotificacaoDeletar(string mensage, string title)
    {
        Title = title;
        Width = 600;
        Height = 400;

        var canva = new Canvas
        {
            Width = 600,
            Height = 400
        };

        var textBlock = new TextBlock
        {
            Text = mensage,
            Width = 523,
            Foreground = Brushes.White,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
        };

        entryBox = new TextBox
        {
            Width = 200,
            BorderBrush = Brushes.White,
            BorderThickness = new Avalonia.Thickness(2)
        };

        var buttonOpt1 = new Button
        {
            Content = "Deletar apenas pacote.",
        };
        buttonOpt1.Click += PartiallyDelete;

        var buttonOpt2 = new Button
        {
            Content = "Deletar todos os \narquivos e seus resíduos",
        };
        buttonOpt2.Click += DeleteAll;

        Canvas.SetLeft(buttonOpt1, 40);
        Canvas.SetTop(buttonOpt1, 350);
        Canvas.SetRight(buttonOpt2, 40);
        Canvas.SetBottom(buttonOpt2, 25);
        Canvas.SetTop(textBlock, 20);
        Canvas.SetLeft(textBlock, 38.5);
        Canvas.SetRight(textBlock, 38.5);
        Canvas.SetTop(entryBox, 100);
        Canvas.SetLeft(entryBox, 200);
        Canvas.SetRight(entryBox, 200);

        canva.Children.Add(textBlock);
        canva.Children.Add(buttonOpt1);
        canva.Children.Add(buttonOpt2);
        canva.Children.Add(entryBox);

        this.Content = canva;
    }

    private void PartiallyDelete(object? sender, RoutedEventArgs e)
    {
        string packageName = entryBox.Text;

        if (packageName != null){
            
        }
    }

    private void DeleteAll(object? sender, RoutedEventArgs e)
    {
        string packageName = entryBox.Text;

        if (packageName != null){
            new Aviso("AVISO", "Deseja deletar o programa, incluindo todos os arquivos do sistema do programa e \nconfigurações globais do sistema?", packageName).Show();
        }
    }

    
}