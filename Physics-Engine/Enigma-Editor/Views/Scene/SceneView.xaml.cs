using System.Windows;
using Enigma_Editor.Input;
using Enigma_Framework;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Wpf;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using MouseButtonEventArgs = System.Windows.Input.MouseButtonEventArgs;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;
using UserControl = System.Windows.Controls.UserControl;

namespace Enigma_Editor.Views.Scene;

public partial class SceneView : UserControl
{
    private EnigmaCore? engineCore;

    public SceneView()
    {
        InitializeComponent();
        var settings = new GLWpfControlSettings
        {
            MajorVersion = 3,
            MinorVersion = 3,
            RenderContinuously = true,
            Profile = ContextProfile.Compatability,
            ContextFlags = ContextFlags.Default
        };

        OpenTkControl.Start(settings);
    }


    private void OpenTkControl_OnReady()
    {
        engineCore = new EnigmaCore();

        var width = (int)Math.Max(OpenTkControl.ActualWidth, 1);
        var height = (int)Math.Max(OpenTkControl.ActualHeight, 1);

        engineCore.Load(width, height);
    }


    private void OpenTkControl_OnRender(TimeSpan deltaTime)
    {
        engineCore?.Update(deltaTime.TotalSeconds);
        engineCore?.Render((int)OpenTkControl.ActualWidth, (int)OpenTkControl.ActualHeight);
    }


    private void OpenTkControl_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        engineCore?.Resize((int)e.NewSize.Width, (int)e.NewSize.Height);
    }

    private void OpenTkControl_PreviewKeyDown(object sender, KeyEventArgs keyEventArgs)
    {
        var openTkKey = WpfInputAdapter.ConvertWpfKeyToOpenTkKey(keyEventArgs.Key);
        engineCore?.SetKeyDown(openTkKey);
    }

    private void OpenTkControl_PreviewKeyUp(object sender, KeyEventArgs keyEventArgs)
    {
        var openTkKey = WpfInputAdapter.ConvertWpfKeyToOpenTkKey(keyEventArgs.Key);
        engineCore?.SetKeyUp(openTkKey);
    }

    private void OpenTkControl_PreviewMouseMove(object sender, MouseEventArgs mouseEventArgs)
    {
        var position = mouseEventArgs.GetPosition(OpenTkControl);
        var glY = OpenTkControl.ActualHeight - position.Y;

        engineCore?.SetMousePosition(position.X, glY);
    }

    private void OpenTkControl_PreviewMouseDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
    {
        OpenTkControl.Focus();
        engineCore?.SetMouseDown((MouseButton)mouseButtonEventArgs.ChangedButton);
    }

    private void OpenTkControl_PreviewMouseUp(object sender, MouseButtonEventArgs e)
    {
        engineCore?.SetMouseUp((MouseButton)e.ChangedButton);
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        OpenTkControl.Focus();
    }
}