using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;

namespace EncurtadorDownload;

public partial class SelectTypeRepair : Window
{
    public SelectTypeRepair()
    {
        InitializeComponent();

        var btnRepairDeb = this.FindControl<Button>("btnDebRepair");
        var btnRepairFlatpak = this.FindControl<Button>("btnFlatpakRepair");

        if (btnRepairDeb != null && btnRepairFlatpak != null)
        {

            btnRepairDeb.Background = Brushes.White;
            btnRepairDeb.BorderBrush = Brushes.Black;
            btnRepairDeb.Foreground = Brushes.Black;

            btnRepairFlatpak.Background = Brushes.White;
            btnRepairFlatpak.BorderBrush = Brushes.Black;
            btnRepairFlatpak.Foreground = Brushes.Black;
        
            new Efeito(btnRepairDeb);   
            new Efeito(btnRepairFlatpak);
        }
    }

    private void DebRepair(object? sender, RoutedEventArgs e)
    {
        new RepairDebWindow().Show();
    }

    private void FlatpakRepair(object? sender, RoutedEventArgs e)
    {
        new RepairFlatpakWindow().Show();
    }
}