using System.Diagnostics;
using System.IO;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Platform.Storage;

namespace EncurtadorDownload;
public partial class RepairDebWindow : Window
{
    private string? package;
    string appId;
    public RepairDebWindow()
    {
        InitializeComponent();

        var btnRepair = this.FindControl<Button>("RepairButton");
        var btnFinalize = this.FindControl<Button>("FinalizeButton");
        var btnSearch = this.FindControl<Button>("SearchButton");
        var btnUpdate = this.FindControl<Button>("UpdateButton");
        var btnUpdateList = this.FindControl<Button>("btnUpdateList");
        var btnUpdateAll = this.FindControl<Button>("btnUpdateAll");
        var btnSelectArchive = this.FindControl<Button>("btnSelectArchive");
        var btnReinstallPackage = this.FindControl<Button>("btnReinstallPackage");
        
        if (btnFinalize != null && btnRepair != null && btnSearch != null 
        && btnUpdate != null && btnUpdateList != null && btnUpdateAll != null
        && btnSelectArchive != null && btnReinstallPackage != null)
        {
            btnRepair.Background = Brushes.White;
            btnRepair.BorderBrush = Brushes.Black;
            btnRepair.Foreground = Brushes.Black;

            btnFinalize.Background = Brushes.White;
            btnFinalize.BorderBrush = Brushes.Black;
            btnFinalize.Foreground = Brushes.Black;

            btnSearch.Background = Brushes.White;
            btnSearch.BorderBrush = Brushes.Black;
            btnSearch.Foreground = Brushes.Black;

            btnUpdate.Background = Brushes.White;
            btnUpdate.BorderBrush = Brushes.Black;
            btnUpdate.Foreground = Brushes.Black;

            btnUpdateList.Background = Brushes.White;
            btnUpdateList.BorderBrush = Brushes.Black;
            btnUpdateList.Foreground = Brushes.Black;

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
            new Efeito(btnSearch);
            new Efeito(btnUpdate);
            new Efeito(btnUpdateList);
            new Efeito(btnUpdateAll);
            new Efeito(btnSelectArchive);
            new Efeito(btnReinstallPackage);
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
            new NotificationWindow("Ocorreu um erro ao corrigir dependências:\n" + error, "ERROR", "Red").Show();
        }
         else
        {
            new NotificationWindow("Dependências corrigidas com sucesso:\n" + output, "SUCESSO", "Lime").Show();
        }
    }

    private void FinishDownload(object? sender, RoutedEventArgs e)
    {
        var (output, error) = ExecuteCommand("pkexec", "dpkg --configure -a");
        var (output2, error2) = ExecuteCommand("pkexec", "apt-get --fix-broken install -y");

        if (!string.IsNullOrWhiteSpace(error))
        {
            new NotificationWindow("Ocorreu um erro ao corrigir dependências:\n" + error, "ERROR", "Red").Show();
        } 
        else if (!string.IsNullOrWhiteSpace(error2))
        {
            new NotificationWindow("Erro ao corrigir dependências:\n" + error2, "ERROR", "Red").Show();    
        }
        else
        {
            new NotificationWindow("Dependências corrigidas com sucesso", "Sucesso", "Lime").Show();
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

    private void SearchUpdates(object? sender, RoutedEventArgs e)
    {
        var (upgrateOutput, upgradeError) = ExecuteCommand("apt-get", "-s upgrade");
        var (autoOutput, autoError) = ExecuteCommand("apt-get", "-s autoremove");

        bool hasUpdates = !upgrateOutput.Contains("0 upgraded");
        bool hasTrash = !autoOutput.Contains("0 to remove");

        if (!string.IsNullOrWhiteSpace(upgradeError))
        {
            new NotificationWindow(upgradeError, "ERROR", "Red").Show();
            return;
        }

        string menssage = "";

        menssage += hasUpdates ? "Atualizações disponíveis:\n" : "Nenhuma atualização disponível.\n";
        
        menssage += hasTrash ? "Pacotes para remoção automática:\n" : "Nenhum pacote para remoção automática.";

        new NotificationWindow(menssage, "Satatus do Sistema", "Lime").Show();
    }

    private void UpdateList(object? sender, RoutedEventArgs e)
    {
        var psi = new ProcessStartInfo
        {
            FileName = "pkexec",
            ArgumentList = {"apt-get", "update"},
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
            new NotificationWindow("Erro ao atualizar lista:\n" + error, "Error", "Red").Show();
        } 
        else
        {
            new NotificationWindow(output, "Sucesso", "White").Show();
        }

    }

    private void UpdateAll(object? sender, RoutedEventArgs e)
    {
        var (output, error) = ExecuteCommand("pkexec", "apt-get update");
        var (output2, error2) = ExecuteCommand("pkexec", "apt-get upgrade -y");

        if (!string.IsNullOrWhiteSpace(error))
        {
            new NotificationWindow("Erro ao atualizar toda a lista:\n" + error, "Error", "Red").Show();
        } 
        else if (!string.IsNullOrEmpty(error2))
        {
            new NotificationWindow("Erro ao atualizar toda a lista:\n" + error2, "Error", "Red").Show();
        }
        else
        {
            new NotificationWindow(output2, "Sucesso", "White").Show();
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
                new FilePickerFileType("Arquivos .deb")
                {
                    Patterns = new[] {"*.deb"}
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
                FileName = "pkexec",
                Arguments = $"env DEBIAN_FRONTEND=noninteractive apt-get install --reinstall -y {package}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            };

            var process = Process.Start(psi);

            string error = process.StandardError.ReadToEnd();
            string output = process.StandardOutput.ReadToEnd();

            if (!string.IsNullOrWhiteSpace(error))
            {
                new NotificationWindow("Erro ao reinstalar pacote:\n" + error, "Error", "Red").Show();
            } 
            else
            {
                new NotificationWindow(output, "Sucesso", "White").Show();
            }
        }
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

    private string getId(string? file)
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
}
