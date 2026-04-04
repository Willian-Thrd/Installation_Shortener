
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection.Metadata;
using Avalonia.Controls;

namespace EncurtadorDownload;

public partial class InstallFlatpakref
{ 
    public InstallFlatpakref()
    {
        
    }

    public void Run(string archive)
    {
        Checker(archive);
    }

    private void installer(string archive)
    {
        var psi = new ProcessStartInfo
        {
            FileName = "flatpak",
            Arguments = $"install {archive}",
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
            var notific = new NotificationWindow("Download feito com sucesso", "Sucesso", "Lime");
            notific.Timer();
        }
    }

    private void Checker(string archive)
    {
        try {
            var psi = new ProcessStartInfo
            {
                FileName = "flatpak",
                Arguments = "--version",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            var process = Process.Start(psi);
            if (process == null)
            {
                new FlatpakInstaller().Show();
                return;
            }

            string error = process.StandardError.ReadToEnd();

            process.WaitForExit();

            if (process.ExitCode == 0 || !string.IsNullOrWhiteSpace(error))
            {
                new FlatpakInstaller().Show();
            } 
            else
            {
                installer(archive);
            }
        }
        catch
        {
            new FlatpakInstaller().Show();
        }
    }
}