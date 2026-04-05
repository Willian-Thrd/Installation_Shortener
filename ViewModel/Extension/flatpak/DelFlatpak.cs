using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;

public class DelFlatpak : Window
{
    public bool Resultado {get; private set;} = false;
    string package;
    string type;


    public DelFlatpak(string title, string mensage, string package, string type)
    {
        this.package = package;
        this.type = type;
        Title = title;
        Width = 550;
        Height = 200;
        Background = Brushes.Black;

        var canva = new Canvas
        {
            Width = 550,
            Height = 200
        };

        var textBox = new TextBox
        {
            Text = mensage,
            IsReadOnly = true,
            Background = Brushes.Black,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
            BorderBrush = Brushes.Black,
            Foreground = Brushes.Red,
            TextAlignment = TextAlignment.Center,
            Width = 500
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
        Canvas.SetLeft(textBox, 25);
        Canvas.SetRight(textBox, 25);
        Canvas.SetBottom(confirmButton, 25);
        Canvas.SetLeft(confirmButton, 20);
        Canvas.SetBottom(dimissButton, 25);
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
                FileName = "flatpak",
                ArgumentList = {"uninstall", "-y", package},
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
                FileName = "flatpak",
                ArgumentList = {"uninstall", "-y", "--delete-data", package},
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
