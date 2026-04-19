using Enigma_Editor.ViewModels;
using UserControl = System.Windows.Controls.UserControl;

namespace Enigma_Editor.Views.Components;

public partial class TransformView : UserControl
{
    public TransformView()
    {
        InitializeComponent();
    }

    public TransformView(TransformViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }
}