using Avalonia.Controls;
using Avalonia.Interactivity;

namespace EncurtadorDownload;

public partial class ChooseTypeDelete : Window
{
    public ChooseTypeDelete()
    {
        InitializeComponent();
    }

    private void Deb(object? sender, RoutedEventArgs e)
    {
        new NotificacaoDeletarDeb().Show();
    }

    private void Flatpak(object? sender, RoutedEventArgs e)
    {
        new NotificacaoDeletarFlatpak().Show();
    }
}