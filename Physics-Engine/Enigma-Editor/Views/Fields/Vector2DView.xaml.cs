using System.Windows;
using UserControl = System.Windows.Controls.UserControl;

namespace Enigma_Editor.Views.Fields;

public partial class Vector2DView : UserControl
{
    public static readonly DependencyProperty LabelProperty =
        DependencyProperty.Register(nameof(Label), typeof(string), typeof(Vector2DView),
            new PropertyMetadata("Vector2"));

    public static readonly DependencyProperty XProperty =
        DependencyProperty.Register(nameof(X), typeof(double), typeof(Vector2DView), new PropertyMetadata(0.0));

    public static readonly DependencyProperty YProperty =
        DependencyProperty.Register(nameof(Y), typeof(double), typeof(Vector2DView), new PropertyMetadata(0.0));

    public Vector2DView()
    {
        InitializeComponent();
    }

    public string Label
    {
        get => (string)GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }


    public double X
    {
        get => (double)GetValue(XProperty);
        set => SetValue(XProperty, value);
    }


    public double Y
    {
        get => (double)GetValue(YProperty);
        set => SetValue(YProperty, value);
    }
}