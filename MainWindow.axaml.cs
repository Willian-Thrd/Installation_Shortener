using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Templates;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Platform.Storage;


namespace EncurtadorDownload;

public partial class MainWindow : Window
{

    string? pathWay;
    string? fileName;
    string? packageName;

    public MainWindow()
    {
        InitializeComponent();

        var btnSelected = this.FindControl<Button>("ConfirmButton");
        var btnDownload = this.FindControl<Button>("DownloadButton");
        var btnSearch = this.FindControl<Button>("SearchButton");
        var btnDel = this.FindControl<Button>("DeleteButton");

        if (btnSelected != null && btnDownload != null && btnSearch != null && btnDel != null)
        {
            btnSelected.Background = Brushes.White;
            btnSelected.BorderBrush = Brushes.Black;
            btnSelected.Foreground = Brushes.Black;

            btnDownload.Background = Brushes.White;
            btnDownload.BorderBrush = Brushes.Black;
            btnDownload.Foreground = Brushes.Black;

            btnSearch.Background = Brushes.White;
            btnSearch.BorderBrush = Brushes.Black;
            btnSearch.Foreground = Brushes.Black;

            btnDel.Background = Brushes.White;
            btnDel.BorderBrush = Brushes.Black;
            btnDel.Foreground = Brushes.Black;

            Efeito(btnSelected);   
            Efeito(btnDownload);
            Efeito(btnSearch);
            Efeito(btnDel);
        }
    

    }
    private void Efeito(Button button)
    {
        button.Template = new FuncControlTemplate<Button>((parent, scope) =>
        {
            var border = new Border
            {
              Background = parent.Background,
              BorderBrush = parent.BorderBrush,
              BorderThickness = parent.BorderThickness,
              Child = new ContentPresenter
              {
                Content = parent.Content,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center
              }  
            };
            return border;
        });
    }

    private async void SelectArchive(object? sender, RoutedEventArgs e)
    {
        string? way = PathExibicao.Content as string;

        await OpenExplorer();
    }

    private void Instalar(object? sender, RoutedEventArgs e)
    {

        if(string.IsNullOrWhiteSpace(pathWay))
        {
            var notific = new NotificationWindow("Preencha todos os campos para prosseguir com o download.", "ERRO", "Red");
            notific.Timer();
        }
        else
        {
            var psi = new ProcessStartInfo
            {
                FileName = "dpkg-deb",
                Arguments = $"-f \"{pathWay}\" Package",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
        }
        
    }

    private void Procurar(object? sender, RoutedEventArgs e)
    {
        if (pathWay == null) {
            new NotificationWindow("Por favor, preencha todos os campos", "ERROR", "Red").Show();
        } 
        else 
        {
            var psi = new ProcessStartInfo
            {
                FileName = "dpkg-deb",
                ArgumentList = {$"-c {pathWay}"},
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            var process = Process.Start(psi);
            
            packageName = process.StandardOutput.ReadToEnd().Trim();
            process.WaitForExit();

            var psi2 = new ProcessStartInfo
            {
                FileName = "dpkg",
                ArgumentList = {$"-l {packageName}"},
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            
            var process2 = Process.Start(psi2);
            if (process2 == null) return;

            var result = process2.StandardOutput.ReadToEnd();
            var error = process2.StandardError.ReadToEnd();

            process2.WaitForExit();

            if (!string.IsNullOrWhiteSpace(error))
            {
                new NotificationWindow(error, "Console", "Red").Show();
            }
            else
            {
                new NotificationWindow(result, "Console", "Lime").Show();
            }
        }
    }

    private void Deletar(object? sender, RoutedEventArgs e)
    {
        new NotificacaoDeletar("Você está prestes a deletar um pacote de seu sistema. Se pretende prosseguir, \nescolha uma das duas opções de exclusão de pacotes.", "ATENÇÃO").Show();
    }

    private async Task OpenExplorer() {
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
            pathWay = archive.Path.LocalPath;
            PathExibicao.Content = archive.Name;
            fileName = archive.Name;

            var notific = new NotificationWindow("O item foi selecionado", "Notificação", "White");
            notific.Timer();
        }
    }   
}