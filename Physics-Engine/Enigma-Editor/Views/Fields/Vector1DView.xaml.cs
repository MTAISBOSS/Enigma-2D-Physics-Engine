using System.Windows;
using UserControl = System.Windows.Controls.UserControl;

namespace Enigma_Editor.Views.Fields;

public partial class Vector1DView : UserControl
{
    public static readonly DependencyProperty LabelProperty =
        DependencyProperty.Register(nameof(Label), typeof(string), typeof(Vector1DView),
            new PropertyMetadata("Vector1"));

    public static readonly DependencyProperty ValueProperty =
        DependencyProperty.Register(nameof(Value), typeof(double), typeof(Vector1DView), new PropertyMetadata(0.0));

    public Vector1DView()
    {
        InitializeComponent();
    }

    public string Label
    {
        get => (string)GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }


    public double Value
    {
        get => (double)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }
}