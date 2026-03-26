using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Platform.Storage;


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

    private async void SelectArchive(object? sender, RoutedEventArgs e)
    {
        var path = this.FindControl<TextBox>("PathExibicao");

        if (path != null && !string.IsNullOrWhiteSpace(PathExibicao.Text))
        {
            var notific = new NotificationWindow("O item foi selecionado", "Notificação");
            notific.Timer();

            await OpenExplorer();

        } else
        {
            var notific = new NotificationWindow("Preencha todos os campos para prosseguir com o download", "ERRO");
            notific.Timer();
        }
    }

    private async void SearchFile(object? sender, PointerPressedEventArgs e)
    {
        Console.WriteLine("Clicou");
        if (e.ClickCount == 2)
        {
            Console.WriteLine("Clicou 2");
            await OpenExplorer();
        }
    }

    private async Task OpenExplorer() {
        var archives = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
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
            PathExibicao.Text = archive.Name;
        }
    }
        
}