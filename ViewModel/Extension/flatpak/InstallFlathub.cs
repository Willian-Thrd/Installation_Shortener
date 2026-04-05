using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Media;

namespace EncurtadorDownload;

public class InstallFlathub : Window
{
    public InstallFlathub(string title, string mensage)
    {
        Title = title;
        Width = 500;
        Height = 200;
        Background = Brushes.Black;

        var canvas = new Canvas
        {
            Width = 500,
            Height = 200,
            Background = Brushes.Black
        };

        var textBox = new TextBox
        {
            Text = mensage,
            TextAlignment = TextAlignment.Center,
            Width = 450,
            Height = 22,
            Foreground = Brushes.Red,
            BorderBrush = Brushes.Transparent,
            FontSize = 18,
            IsReadOnly = true
        };

        var textBox2 = new TextBox
        {
            Text = "Você necessita baixar o flathub para continuar. \nDeseja instalá-lo?",
            TextAlignment = TextAlignment.Center,
            FontSize = 16,
            Foreground = Brushes.White,
            BorderBrush = Brushes.Transparent,
            Width = 450,
            IsReadOnly = true
        };

        var btnAccept = new Button
        {
            Content = "Aceitar",
            HorizontalContentAlignment = Avalonia.Layout.HorizontalAlignment.Center,
            BorderBrush = Brushes.White
        };
        btnAccept.Click += (s, e) =>
        {
            var psi = new ProcessStartInfo
            {
                FileName = "flatpak",
                Arguments = "remote-add --if-not-exists flathub https://flathub.org/repo/flathub.flatpakrepo",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            var process = Process.Start(psi);

            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            if (!string.IsNullOrWhiteSpace(error))
            {
                new NotificationWindow(error, "ERROR", "Red").Show();
            } 
            else
            {
                new NotificationWindow("Flathub instalado com sucesso!", "Sucesso", "Red").Show();
            }
        };

        var btnDismiss = new Button
        {
            Content = "Negar",
            HorizontalContentAlignment = Avalonia.Layout.HorizontalAlignment.Center,
            BorderBrush = Brushes.White
        };
        btnDismiss.Click += (s, e) =>
        {
            this.Close();
        };


        Canvas.SetTop(textBox, 20);
        Canvas.SetRight(textBox, 20);
        Canvas.SetLeft(textBox, 20);
        Canvas.SetTop(textBox2, 62);
        Canvas.SetRight(textBox2, 20);
        Canvas.SetLeft(textBox2, 20);
        Canvas.SetBottom(btnAccept, 20);
        Canvas.SetRight(btnAccept, 20);
        Canvas.SetBottom(btnDismiss, 20);
        Canvas.SetLeft(btnDismiss, 20);

        canvas.Children.Add(textBox);
        canvas.Children.Add(textBox2);
        canvas.Children.Add(btnAccept);
        canvas.Children.Add(btnDismiss);

        this.Content = canvas;
    }
}