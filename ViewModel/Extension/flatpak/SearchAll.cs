using System;
using System.Diagnostics;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Interactivity;

namespace EncurtadorDownload;

public partial class SearchAll : Window
{
    public SearchAll()
    {
        InitializeComponent();
    }

    public void SearchDeb(object? sender, RoutedEventArgs e)
    {
        var psi = new ProcessStartInfo
        {
            FileName = "dpkg-query",
            Arguments = "-W -f=\"${Package}\\n\"",
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
            new NotificationWindow(output, "Pacotes", "White").Show();
        }
    }

    public void SearchFlatpak(object? sender, RoutedEventArgs e)
    {
        var psi = new ProcessStartInfo
        {
            FileName = "flatpak",
            Arguments = "list --app --columns=name,application",
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
            new NotificationWindow(output, "Pacotes", "White").Show();
        }
    }
}