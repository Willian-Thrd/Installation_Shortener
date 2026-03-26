using System;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Templates;
using Avalonia.Interactivity;
using Avalonia.Media;


namespace EncurtadorDownload;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        var btnSelected = this.FindControl<Button>("ConfirmButton");
        if (btnSelected != null)
        {
            btnSelected.Background = Brushes.White;
            btnSelected.BorderBrush = Brushes.Black;
            btnSelected.Foreground = Brushes.Black;

            Efeito(btnSelected);   
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

    private void SelectArchive_Click(object? sender, RoutedEventArgs e)
    {
        Console.WriteLine("Clicou");
    }
}