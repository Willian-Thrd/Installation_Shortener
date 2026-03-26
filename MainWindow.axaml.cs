using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Avalonia.Controls;
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
                FileName = "pkexec",
                ArgumentList = {"apt", "install", pathWay},
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            };

            var process = new Process();
            process.StartInfo = psi;

            process.OutputDataReceived += (s, e) =>
            {
                if (e.Data != null)
                Console.WriteLine(e.Data);
            };

            process.Start();
            process.BeginOutputReadLine();
        }
    }

    private void Procurar(object? sender, RoutedEventArgs e)
    {
        if(string.IsNullOrWhiteSpace(pathWay))
        {
            var notific = new NotificationWindow("Preencha todos os campos para procurar o pacote.", "ERRO", "Red");
            notific.Timer();
        }
        else
        {
            var psi = new ProcessStartInfo
            {
                FileName = "dpkg-deb",
                Arguments = $"-f \"{pathWay}\" Package",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = false
            };

            var process = Process.Start(psi);

            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            process.WaitForExit();

            if (!string.IsNullOrWhiteSpace(output))
            {
                new NotificationWindow(output, "NOME DO PACOTE", "Lime").Show();
            } else
            {
                new NotificationWindow(error, "ERROR", "Red").Show();
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