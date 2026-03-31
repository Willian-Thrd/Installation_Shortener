using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;

public class Aviso : Window
{
    public bool Resultado {get; private set;} = false;
    string package;
    string type;

    public Aviso(string title, string mensage, string package, string type)
    {
        this.package = package;
        this.type = type;
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
        if (type == "pacote")
        {
            var psi = new ProcessStartInfo
            {
                FileName = "pkexec",
                ArgumentList = {"apt", "remove", "-y", package},
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            var process = Process.Start(psi);
            this.Close();

            string error = process.StandardError.ReadToEnd();

            if (!string.IsNullOrWhiteSpace(error))
            {
                new NotificationWindow("Ocorreu um erro ao tentar deletar o pacote.", "ERROR", "Red").Show();
            } else
            {
                var notific = new NotificationWindow("Pacote deletado com sucesso.", "Sucesso", "Green");
                notific.Show();
            }

        } else if (type == "all")
        {
            Resultado = true;
            var psi = new ProcessStartInfo
            {
                FileName = "pkexec",
                ArgumentList = {"apt", "purge", "-y", package},
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            var process = Process.Start(psi);
            this.Close();

            string error = process.StandardError.ReadToEnd();

            if (!string.IsNullOrWhiteSpace(error))
            {
                new NotificationWindow("Ocorreu um erro ao tentar deletar o pacote.", "ERROR", "Red").Show();
            } else
            {
                var notific = new NotificationWindow("Pacote deletado com sucesso.", "Sucesso", "Green");
                notific.Show();
            }
        }
    }

    private void Dismiss(object? snder, RoutedEventArgs e)
    {
        Resultado = false;
        this.Close();
    }
}
