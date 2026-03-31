using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;

namespace EncurtadorDownload;
public partial class RepairWindow : Window
{
    public RepairWindow()
    {
        InitializeComponent();

        var btnRepair = this.FindControl<Button>("RepairButton");
        var btnFinalize = this.FindControl<Button>("FinalizeButton");

        if (btnFinalize != null && btnRepair != null)
        {
            btnRepair.Background = Brushes.White;
            btnRepair.BorderBrush = Brushes.Black;
            btnRepair.Foreground = Brushes.Black;

            btnFinalize.Background = Brushes.White;
            btnFinalize.BorderBrush = Brushes.Black;
            btnFinalize.Foreground = Brushes.Black;

             new Efeito(btnRepair);
             new Efeito(btnFinalize);
        }
    }

    private void RepairDep(object? sender, RoutedEventArgs e)
    {
        var psi = new ProcessStartInfo
        {
            FileName = "pkexec",
            ArgumentList = {"apt install -f"},
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        var process = Process.Start(psi);

        string error = process.StandardError.ReadToEnd();
        string output = process.StandardOutput.ReadToEnd();

        if (!string.IsNullOrWhiteSpace(error))
        {
            new NotificationWindow("ERROR", "Ocorreu um erro ao corrigir dependências", "Red").Show();
        }
         else
        {
            new NotificationWindow("SUCESSO", "Dependências corrigidas com sucesso", "Lime").Show();
        }
    }

    private void FinishDownload(object? sender, RoutedEventArgs e)
    {
        var psi = new ProcessStartInfo
        {
            FileName = "pkexec",
            ArgumentList = {"dpkg --configure -a"},
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        var process = Process.Start(psi);

        string error = process.StandardError.ReadToEnd();
        string output = process.StandardOutput.ReadToEnd();

        if (!string.IsNullOrWhiteSpace(error))
        {
            new NotificationWindow("ERROR", "Ocorreu um erro ao corrigir dependências", "Red").Show();
        }
         else
        {
            new NotificationWindow("SUCESSO", "Dependências corrigidas com sucesso", "Lime").Show();
        }
    }

    private async void SearchBroken(object? sender, RoutedEventArgs e)
    {
        var psi = new ProcessStartInfo
        {
            FileName = "pkexec",
            ArgumentList = {"bash", "-c", "dpkg -l | grep '^..r'"},
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        var process = new Process { StartInfo = psi };

        process.Start();

        string output = await process.StandardOutput.ReadToEndAsync();
        string error = await process.StandardError.ReadToEndAsync();

        await process.WaitForExitAsync();

        if (!string.IsNullOrWhiteSpace(error))
        {
            new NotificationWindow(error, "ERROR", "Red").Show();
        }         else if (!string.IsNullOrWhiteSpace(output))
        {
            new NotificationWindow(output, "Pacotes Encontrados", "Orage").Show();
        } 
        else
        {
            new NotificationWindow("Nenhum pacote quebrado encontrado.", "Sucesso", "Lime").Show();
        }
    }
}
