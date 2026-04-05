using Avalonia.Controls;
using Avalonia.Interactivity;

namespace EncurtadorDownload;

public partial class NotificacaoDeletarFlatpak : Window
{
    public NotificacaoDeletarFlatpak()
    {
        InitializeComponent();
    }

    private void DelPack(object? sender, RoutedEventArgs e)
    {
        string? textEntry = this.FindControl<TextBox>("PackageEntry")?.Text;

        string packageName = textEntry;

        if (packageName != null)
        {
            new DelFlatpak("AVISO", "Deseja deletar o programa?", packageName, "pacote").Show();
        }
        else
        {
            new NotificationWindow("Preencha todos os campos.", "ERROR", "Red").Show();
        }
        
    }

    private void DelAll(object? sender, RoutedEventArgs e)
    {
        string? textEntry = this.FindControl<TextBox>("PackageEntry")?.Text;

        string packageName = textEntry;

        if (packageName != null){
            new DelFlatpak("AVISO", "Deseja deletar o programa, incluindo todos os arquivos do programa e \nconfigurações globais do sistema?", packageName, "all").Show();
        }
        else
        {
            new NotificationWindow("Preencha todos os campos.", "ERROR", "Red").Show();
        }
    }

    
}