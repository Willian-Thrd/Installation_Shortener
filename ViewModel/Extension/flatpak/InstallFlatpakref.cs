
using System;
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
            if (error.Contains("flathub", StringComparison.OrdinalIgnoreCase) &&
            error.Contains("not found", StringComparison.OrdinalIgnoreCase))
            {
                new InstallFlathub("ERROR", "Erro: Flathub não está instalado no sistema.").Show();
            }
            else
            {
            new NotificationWindow(error, "ERROR", "Red").Show();                
            }
        }
        else
        {
            var notific = new NotificationWindow("Download feito com sucesso", "Sucesso", "Lime");
            notific.Timer();
        }
    }

    private void Checker(string archive)
    {
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
            ShowInstallerAndRetry(archive);
            return;
        }

        string error = process.StandardError.ReadToEnd();

        process.WaitForExit();

        bool flatpakExists = process.ExitCode == 0;

        if (!flatpakExists)
        {
            ShowInstallerAndRetry(archive);
            return;
        }
        
        installer(archive);

    }

    private void ShowInstallerAndRetry(string archive)
    {
        var installed = new FlatpakInstaller();
        installed.Show();

        installed.Closed += (_, __) =>
        {
            Checker(archive);
        };
    }
}