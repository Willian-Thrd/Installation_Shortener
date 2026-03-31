using Avalonia.Controls;
using Avalonia.Interactivity;

namespace EncurtadorDownload;

public partial class NotificacaoDeletar : Window
{
    public NotificacaoDeletar()
    {
        InitializeComponent();
    }

    private void DelPack(object? sender, RoutedEventArgs e)
    {
        string? textEntry = this.FindControl<TextBox>("PackageEntry")?.Text;

        string packageName = textEntry;

        if (packageName != null){
            new Aviso("AVISO", "Deseja deletar o programa?", packageName, "pacote").Show();
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
            new Aviso("AVISO", "Deseja deletar o programa, incluindo todos os arquivos do sistema do programa e \nconfigurações globais do sistema?", packageName, "all").Show();
        }
        else
        {
            new NotificationWindow("Preencha todos os campos.", "ERROR", "Red").Show();
        }
    }

    
}