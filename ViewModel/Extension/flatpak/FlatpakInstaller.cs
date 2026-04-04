using System.Diagnostics;
using System.IO;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace EncurtadorDownload;

public partial class FlatpakInstaller : Window
{
    public FlatpakInstaller()
    {
        InitializeComponent();
    }

    public void Confirm(object? sender, RoutedEventArgs e)
    {
        var psi = new ProcessStartInfo
        {
            FileName = "pkexec",
            Arguments = "DEBIAN_FRONTEND=noninteractive apt-get install -y flatpak",
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
            var notific = new NotificationWindow("Flatpak instalado com sucesso!", "Sucesso", "Lime");
            notific.Timer();
            this.Close();
        }
    }

    public void Dismiss(object? sender, RoutedEventArgs e)
    {
        this.Close();
    }
}