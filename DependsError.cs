using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;

public class DependsError : Window
{
    string dependency;
    public DependsError(string title, string mensage, string dependency)
    {
        this.dependency = dependency;
        Title = title;
        Width = 700;
        Height = 700;

        var canvas = new Canvas
        {
            Width = 700,
            Height = 700
        };

        var textBox = new TextBox
        {
            Text = mensage,
            Foreground = Brushes.Red,
            Background = Brushes.Black
        };

        var buttonOpt1 = new Button
        {
            Content = "Aceitar",
            Foreground = Brushes.White,
            BorderBrush = Brushes.White,
            BorderThickness = new Avalonia.Thickness(2)
        };
        buttonOpt1.Click += Confirmate;

        var buttonOpt2 = new Button
        {
            Content = "Negar",
            Foreground = Brushes.Red,
            BorderBrush = Brushes.White,
            BorderThickness = new Avalonia.Thickness(2)
        };
        buttonOpt2.Click += Dismiss;
    }

    public async void Confirmate(object? sender, RoutedEventArgs e)
    {
        var psi = new ProcessStartInfo
        {
            FileName = "pkexec",
            Arguments = $"apt install -y {dependency}",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false
        };

        var process = Process.Start(psi);

        string output = await process.StandardOutput.ReadToEndAsync();
        string error = await process.StandardError.ReadToEndAsync();

        await process.WaitForExitAsync();

        if (!string.IsNullOrWhiteSpace(error))
        {
            new NotificationWindow(error, "ERROR", "Red").Show();
        } 
        else
        {
            new NotificationWindow("Pacote instalado com sucesso", "Sucesso.", "White").Show();
        }
    }

    public void Dismiss(object? sender, RoutedEventArgs e)
    {
        
    }
}