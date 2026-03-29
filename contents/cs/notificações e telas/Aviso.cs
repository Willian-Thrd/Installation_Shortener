using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;

public class Aviso : Window
{
    public bool Resultado {get; private set;} = false;
    string package;

    public Aviso(string title, string mensage, string package)
    {
        this.package = package;
        Title = title;
        Width = 600;
        Height = 300;
        Background = Brushes.Black;

        var canva = new Canvas
        {
            Width = 600,
            Height = 300
        };

        var textBox = new TextBox
        {
            Text = mensage,
            IsReadOnly = true,
            Background = Brushes.Black,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
            BorderBrush = Brushes.Black,
            Foreground = Brushes.Red
        };

        var confirmButton = new Button
        {
            Content = "Confirmar",
            BorderBrush = Brushes.White,
            BorderThickness = new Thickness(2)
        };
        confirmButton.Click += Confirmate;

        var dimissButton = new Button
        {
            Content = "Negar",
            BorderBrush = Brushes.White,
            BorderThickness = new Thickness(2)
        };
        dimissButton.Click += Dismiss;

        Canvas.SetTop(textBox, 20);
        Canvas.SetLeft(textBox, 20);
        Canvas.SetRight(textBox, 20);
        Canvas.SetTop(confirmButton, 250);
        Canvas.SetLeft(confirmButton, 20);
        Canvas.SetTop(dimissButton, 250);
        Canvas.SetRight(dimissButton, 20);

        canva.Children.Add(textBox);
        canva.Children.Add(confirmButton);
        canva.Children.Add(dimissButton);

        this.Content = canva;
    }

    private void Confirmate(object? snder, RoutedEventArgs e)
    {
        Resultado = true;
        var psi = new ProcessStartInfo
        {
            FileName = "pkexec",
            ArgumentList = {"apt", "purge", "-y", package}
        };
        Process.Start(psi);
        this.Close();
    }

    private void Dismiss(object? snder, RoutedEventArgs e)
    {
        Resultado = false;
        this.Close();
    }
}
