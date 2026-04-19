using System.Diagnostics;
using System.IO;
using System.Windows;
using MessageBox = System.Windows.MessageBox;
using UserControl = System.Windows.Controls.UserControl;

namespace Enigma_Editor.Views.GameStateToolbar;

public partial class GameStateToolbarView : UserControl
{
    public GameStateToolbarView()
    {
        InitializeComponent();
    }

    private void Play_Click(object sender, RoutedEventArgs e)
    {
        var engineExePath = Path.GetFullPath(Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            @"..\..\..\..\Enigma-Framework\bin\Debug\net8.0\Enigma-Framework.exe"
        ));

        if (!File.Exists(engineExePath))
        {
            MessageBox.Show(
                $"Engine executable not found:\n{engineExePath}",
                "Launch Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error
            );
            return;
        }

        Process.Start(new ProcessStartInfo
        {
            FileName = engineExePath,
            UseShellExecute = true
        });
    }
}