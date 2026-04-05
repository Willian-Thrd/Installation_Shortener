using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using Tmds.DBus.Protocol;

namespace EncurtadorDownload;
public partial class RepairFlatpakWindow : Window
{
    private string? package;

    public RepairFlatpakWindow()
    {
        InitializeComponent();

        var btnRepair = this.FindControl<Button>("RepairButton");
        var btnFinalize = this.FindControl<Button>("FinalizeButton");
        var btnUpdate = this.FindControl<Button>("UpdateButton");
        var btnUpdateAll = this.FindControl<Button>("btnUpdateAll");
        var btnSelectArchive = this.FindControl<Button>("btnSelectArchive");
        var btnReinstallPackage = this.FindControl<Button>("btnReinstallPackage");
        
        if (btnFinalize != null && btnRepair != null && btnUpdate != null  
        && btnUpdateAll != null && btnSelectArchive != null && btnReinstallPackage != null)
        {
            btnRepair.Background = Brushes.White;
            btnRepair.BorderBrush = Brushes.Black;
            btnRepair.Foreground = Brushes.Black;

            btnFinalize.Background = Brushes.White;
            btnFinalize.BorderBrush = Brushes.Black;
            btnFinalize.Foreground = Brushes.Black;

            btnUpdate.Background = Brushes.White;
            btnUpdate.BorderBrush = Brushes.Black;
            btnUpdate.Foreground = Brushes.Black;

            btnUpdateAll.Background = Brushes.White;
            btnUpdateAll.BorderBrush = Brushes.Black;
            btnUpdateAll.Foreground = Brushes.Black;

            btnSelectArchive.Background = Brushes.White;
            btnSelectArchive.BorderBrush = Brushes.Black;
            btnSelectArchive.Foreground = Brushes.Black;

            btnReinstallPackage.Background = Brushes.White;
            btnReinstallPackage.BorderBrush = Brushes.Black;
            btnReinstallPackage.Foreground = Brushes.Black;

            new Efeito(btnRepair);
            new Efeito(btnFinalize);
            new Efeito(btnUpdate);
            new Efeito(btnUpdateAll);
            new Efeito(btnSelectArchive);
            new Efeito(btnReinstallPackage);
        }
    }

    private void RepairFlatpak(object? sender, RoutedEventArgs e)
    {
        var psi = new ProcessStartInfo
        {
            FileName = "flatpak",
            Arguments = "repair",
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
            new NotificationWindow("Ocorreu um erro ao corrigir dependências", "ERROR", "Red").Show();
        }
         else
        {
            new NotificationWindow("Dependências corrigidas com sucesso", "SUCESSO", "Lime").Show();
        }
    }

    private void FinishDownload(object? sender, RoutedEventArgs e)
    {
        var archiveSpace = this.FindControl<Label>("ArchiveSpace");
        string archive = archiveSpace?.Content?.ToString() ?? "";

        var psi = new ProcessStartInfo
        {
            FileName = "flatpak",
            ArgumentList = {"install --reinstall \"{archive}\""},
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
            new NotificationWindow("Ocorreu um erro ao corrigir dependências", "ERROR", "Red").Show();
        }
         else
        {
            new NotificationWindow("Dependências corrigidas com sucesso", "Sucesso", "Lime").Show();
        }
    }

    private void SearchUpdates(object? sender, RoutedEventArgs e)
    {
        var (upgrateOutput, upgradeError) = ExecuteCommand("flatpak", "remote-ls --updates");
        

        if (!string.IsNullOrWhiteSpace(upgradeError))
        {
            new NotificationWindow(upgradeError, "ERROR", "Red").Show();
            return;
        }

        bool hasUpdates = upgrateOutput.Contains("Nothing to do");

        string menssage = hasUpdates ? "Atualizações disponíveis:\n" : "Nenhuma atualização disponível.\n";

        new NotificationWindow(menssage, "Satatus do Sistema", "Lime").Show();
    }

    private void UpdateAll(object? sender, RoutedEventArgs e)
    {
        var (output, error) = ExecuteCommand("flatpak", "update -y");

        if (!string.IsNullOrWhiteSpace(error))
        {
            new NotificationWindow(error, "Error", "Red").Show();
        } 
        else if (string.IsNullOrWhiteSpace(output) || output.Contains("Nothing to do"))
        {
            new NotificationWindow("Nada a se atualizar.", "Notificação", "White").Show();
        }
        else
        {
            new NotificationWindow(output, "Sucesso", "White").Show();
        }
    }

    private async void SelectArchive(object? sender, RoutedEventArgs e) {
        var archiveSpace = this.FindControl<Label>("ArchiveSpace");
        var topLevel = TopLevel.GetTopLevel(this);
    
        var archives = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Explorador de arquivos",
            AllowMultiple = false,

            FileTypeFilter = new[]
            {
                new FilePickerFileType("Arquivos .flatpakref")
                {
                    Patterns = new[] {"*.flatpakref"}
                },
                new FilePickerFileType("Todos os arquivos")
                {
                    Patterns = new[] {"*.*"}
                }
            }
        });

        if (archives.Count > 0)
        {
            var archive = archives[0];
            package = archive.Path.LocalPath;
            archiveSpace.Content = archive.Name;


            var notific = new NotificationWindow("O item foi selecionado", "Notificação", "White");
            notific.Timer();
        }
    }   

    private void ReinstallPackage(object? sender, RoutedEventArgs e)
    {
        if (package == null)
        {
            var notific = new NotificationWindow("Por favor, preencha todos os campos", "ERROR", "Red");
            notific.Timer();
        } 
        else 
        {
            var psi = new ProcessStartInfo
            {
                FileName = "flatpak",
                ArgumentList = {"uninstall", "-y", package},
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            };

            var process = Process.Start(psi);

            process.WaitForExit();

            string error = process.StandardError.ReadToEnd();
            string output = process.StandardOutput.ReadToEnd();

            if (!string.IsNullOrWhiteSpace(error))
            {
                new NotificationWindow(error, "Error", "Red").Show();
            } 
            else
            {
                new NotificationWindow(output, "Sucesso", "White").Show();
            }
        }
    }

    private string getId(string file)
    {
        foreach (var line in File.ReadAllLines(file))
        {
            if (line.StartsWith("Name="))
            {
                return line.Substring(5).Trim();
            }
        }

        return null;
    }

    private (string output, string error) ExecuteCommand(string arg1, string arg2)
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

        string output = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();

        process.WaitForExit();

        return (output, error);
    }
}
