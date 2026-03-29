using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;

public class NotificacaoDeletar : Window{
        private TextBox entryBox;

    public NotificacaoDeletar(string mensage, string title)
    {
        Title = title;
        Width = 600;
        Height = 300;
        Background = Brushes.Black;

        var canva = new Canvas
        {
            Width = 600,
            Height = 300
        };

        var textBlock = new TextBlock
        {
            Text = mensage,
            Background = Brushes.Black,
            Width = 550,
            Foreground = Brushes.Red,
            FontSize = 14,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
        };

        entryBox = new TextBox
        {
            Width = 200,
            BorderBrush = Brushes.White,
            BorderThickness = new Avalonia.Thickness(2),
            Foreground = Brushes.White,
            Background = Brushes.Black
        };

        var buttonOpt1 = new Button
        {
            Content = "Deletar apenas pacote.",
            BorderBrush = Brushes.White,
            BorderThickness = new Avalonia.Thickness(2),
            Background = Brushes.Black,
            Foreground = Brushes.White
        };
        buttonOpt1.Click += PartiallyDelete;

        var buttonOpt2 = new Button
        {
            Content = "Deletar todos os \narquivos e seus resíduos",
            BorderBrush = Brushes.White,
            BorderThickness = new Avalonia.Thickness(2),
            Background = Brushes.Black,
            Foreground = Brushes.White
        };
        buttonOpt2.Click += DeleteAll;

        Canvas.SetLeft(buttonOpt1, 40);
        Canvas.SetBottom(buttonOpt1, 25);
        Canvas.SetRight(buttonOpt2, 40);
        Canvas.SetBottom(buttonOpt2, 25);
        Canvas.SetTop(textBlock, 20);
        Canvas.SetLeft(textBlock, 25);
        Canvas.SetRight(textBlock, 25);
        Canvas.SetTop(entryBox, 100);
        Canvas.SetLeft(entryBox, 25);
        Canvas.SetRight(entryBox, 375);

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
        else
        {
            new NotificationWindow("Preencha todos os campos.", "ERROR", "Red").Show();
        }
    }

    
}