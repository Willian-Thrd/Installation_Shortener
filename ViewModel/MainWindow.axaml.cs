using System;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Platform.Storage;


namespace EncurtadorDownload;

public partial class MainWindow : Window
{
    string? pathWay;

    private enum formatType
    {
        Deb,
        Flatpakref,
        Unknown
    }
    
    private formatType GetFormatType (string? path)
    {
        if (Path.GetExtension(path) == ".deb")
        {
            return formatType.Deb;
        } 
        else if (Path.GetExtension(path) == ".flatpakref")
        {
            return formatType.Flatpakref;
        } 
        else
        {
            return formatType.Unknown;
        }
            
    }

    private formatType selectFormatType = formatType.Unknown;

    public MainWindow()
    {
        InitializeComponent();

        var btnSelected = this.FindControl<Button>("ConfirmButton");
        var btnDownload = this.FindControl<Button>("DownloadButton");
        var btnSearch = this.FindControl<Button>("SearchButton");
        var btnDel = this.FindControl<Button>("DeleteButton");
        var btnRepair = this.FindControl<Button>("RepairButton");

        if (btnSelected != null && btnDownload != null && btnSearch != null && btnDel != null && btnRepair != null)
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

            btnRepair.Background = Brushes.White;
            btnRepair.BorderBrush = Brushes.Black;
            btnRepair.Foreground = Brushes.Black;

            new Efeito(btnSelected);
            new Efeito(btnDownload);
            new Efeito(btnSearch);
            new Efeito(btnDel);
            new Efeito(btnRepair);
        }
    

    }

    private async void SelectArchive(object? sender, RoutedEventArgs e)
    {
        var pathExibicao = this.FindControl<Label>("PathExibicao"); 
        string? showcase = pathExibicao.ToString();

        await OpenExplorer();
    }

    private void Instalar(object? sender, RoutedEventArgs e)
    {
        if(pathWay == null)
        {
            var notific = new NotificationWindow("Preencha todos os campos para prosseguir com o download.", "ERRO", "Red");
            notific.Timer();
        }
        else
        {
            selectFormatType = GetFormatType(pathWay);

            switch (selectFormatType)
            {
                case formatType.Deb:
                    new InstallDeb(pathWay);
                break;

                case formatType.Flatpakref:
                    var service = new InstallFlatpakref();
                    service.Run(pathWay);
                break;

                default:
                    new NotificationWindow("Formato não aceito.", "ERROR", "Red");
                break;
            }
        }
        
    }

    private void Procurar(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(pathWay)) 
        {
            new SearchAll().Show();
            return;
        }
        
        if (Path.GetExtension(pathWay) == ".deb") 
        {
            var deb = new DebPackages();
            string pkgName = deb.GetDebFile(pathWay);

            var (output, error) = deb.GetPackage(pkgName);

            if (!string.IsNullOrWhiteSpace(error))
            {
                new NotificationWindow(error, "Console", "Red").Show();
            }
            else
            {
                new NotificationWindow(pkgName, "Console", "White").Show();
            }
            return;
        } 
        else if (Path.GetExtension(pathWay) == ".flatpakref") 
        {
            var flatpak = new FlatpakPackages();
            string pkgName = flatpak.GetFlatpakFile(pathWay);

            if (!string.IsNullOrWhiteSpace(pkgName))
            {
                new NotificationWindow(pkgName, "Console", "White").Show();
                return;
            }
            else
            {
                new NotificationWindow("Falha ao ler flatpakref", "ERROR", "Red").Show();
                return; 
            }
        }
        
        new NotificationWindow("Formato de arquivo não reconhcido", "ERROR", "Red").Show();
            
    }

    private void Deletar(object? sender, RoutedEventArgs e)
    {
        new ChooseTypeDelete().Show();
    }

    private async Task OpenExplorer() {
        var topLevel = TopLevel.GetTopLevel(this);
    
        var archives = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Explorador de arquivos",
            AllowMultiple = false,

            FileTypeFilter = new[]
            {
                new FilePickerFileType(".deb")
                {
                    Patterns = new[] {"*.deb"}
                },
                new FilePickerFileType(".flatpakref")
                {
                    Patterns = new[] {"*.flatpakref"}
                },
                new FilePickerFileType("Todos os formatos")
                {
                    Patterns = new[] {"*.deb", "*.flatpakref"}
                }
            }
        });

        if (archives.Count > 0)
        {
            var archive = archives[0];
            pathWay = archive.Path.LocalPath;
            PathExibicao.Content = archive.Name;

            var notific = new NotificationWindow("O item foi selecionado", "Notificação", "White");
            notific.Timer();
        }
    }   

    private void RepairPackage(object? sender, RoutedEventArgs e)
    {
        new SelectTypeRepair().Show();
    }
}