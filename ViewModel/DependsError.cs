using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Interactivity;
using Avalonia.Media;
using static InstallDeb;

public class DependsError : Window
{
    List<string> dependency;
    private AptErrorType errorType;
    
    public DependsError(string title, string mensage, List<string> dependency, AptErrorType type)
    {
        this.dependency = dependency;
        this.errorType = type;
        Title = title;
        Width = 600;
        Height = 400;
        Background = Brushes.Black;

        var canvas = new Canvas
        {
            Width = 600,
            Height = 400
        };

        var textBox = new TextBox
        {
            Text = mensage,
            Width = 550,
            IsReadOnly = true,
            Foreground = Brushes.Red,
            Background = Brushes.Black,
            BorderBrush = Brushes.Black
        };

        var aviso = new TextBox
        {
            Text = "Parece que houve um erro de instalação, \ngostaria de reparar as dependências/pacotes?",
            Width = 500,
            IsReadOnly = true,
            Background = Brushes.Transparent,
            TextAlignment = TextAlignment.Center,
            Foreground = Brushes.White,
            BorderBrush = Brushes.Black
        };

        var buttonOpt1 = new Button
        {
            Content = "Aceitar",
            Foreground = Brushes.White,
            BorderBrush = Brushes.White,
            BorderThickness = new Thickness(2)
        };
        buttonOpt1.Click += Confirmate;

        var buttonOpt2 = new Button
        {
            Content = "Negar",
            Foreground = Brushes.Red,
            BorderBrush = Brushes.White,
            BorderThickness = new Thickness(2)
        };
        buttonOpt2.Click += Dismiss;

        Canvas.SetTop(textBox, 20);
        Canvas.SetLeft(textBox, 25);
        Canvas.SetRight(textBox, 15);
        Canvas.SetTop(aviso, 200);
        Canvas.SetLeft(aviso, 50);
        Canvas.SetRight(aviso, 50);
        Canvas.SetTop(buttonOpt1, 350);
        Canvas.SetRight(buttonOpt1, 50);
        Canvas.SetTop(buttonOpt2, 350);
        Canvas.SetLeft(buttonOpt2, 50);

        canvas.Children.Add(textBox);
        canvas.Children.Add(aviso);
        canvas.Children.Add(buttonOpt1);
        canvas.Children.Add(buttonOpt2);

        this.Content = canvas;
    }

    public async void Confirmate(object? sender, RoutedEventArgs e)
    {
        (string output, string error, int exitCode) result = ("", "", -1);

        switch (errorType)
        {
            case AptErrorType.DependencyError:
                string deps = string .Join(" ", dependency);
                result = await RunCommand("pkexec", $"apt install -y {deps}");
            break;

            case AptErrorType.PackageBroken:
                result = await RunCommand("pkexec", $"apt-get -f install -y");
            break;

            case AptErrorType.HeldPackage:
                foreach (var d in dependency)
                {
                    await RunCommand("pkexec", $"apt-mark unhold {d}");
                }

                result = ("", "", 0);
            break;

            default:
                return;
        }

        if (result.exitCode != 0)
        {
            new NotificationWindow(result.error, "ERROR", "Red").Show();
        } else
        {
            new NotificationWindow("Processo concluído.", "Sucesso", "White").Show();
        }

        this.Close();
    }

    public void Dismiss(object? sender, RoutedEventArgs e)
    {
        
    }

    public static async Task<(string output, string error, int exitCode)> RunCommand(string arg1, string arg2)
    {
        var psi = new ProcessStartInfo
        {
            FileName = arg1,
            Arguments = arg2,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        var process = Process.Start(psi);

        if (process == null)
        {
            return ("", "Erro ao iniciar processo", -1);
        }

        string output = await process.StandardOutput.ReadToEndAsync();
        string error = await process.StandardError.ReadToEndAsync();

        await process.WaitForExitAsync();

        return (output, error, process.ExitCode);
    }
}