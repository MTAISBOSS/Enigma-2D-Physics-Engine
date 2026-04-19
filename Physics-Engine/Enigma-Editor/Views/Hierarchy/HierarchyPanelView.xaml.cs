using System.Windows;
using Enigma_Editor.ViewModels;
using UserControl = System.Windows.Controls.UserControl;

namespace Enigma_Editor.Views.Hierarchy;

public partial class HierarchyPanelView : UserControl
{
    public static readonly DependencyProperty SelectedItemProperty =
        DependencyProperty.Register("SelectedItem", typeof(object), typeof(HierarchyPanelView),
            new PropertyMetadata(null));

    private readonly HierarchyMenuViewModel viewModel = new();

    public HierarchyPanelView()
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    public object SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        if (AddButton.ContextMenu != null) AddButton.ContextMenu.IsOpen = true;
    }

    private void CreateEntity_Click(object sender, RoutedEventArgs e)
    {
        var entity = viewModel.CreateEntity();
        entity.Components.Add(new TransformViewModel());
    }

    private void CreateCamera_Click(object sender, RoutedEventArgs e)
    {
        var entity = viewModel.CreateCamera();
        entity.Components.Add(new TransformViewModel());
        entity.Components.Add(new CameraComponentViewModel());
    }

    private void CreateBox_Click(object sender, RoutedEventArgs e)
    {
        var entity = viewModel.CreateBox();
        entity.Components.Add(new TransformViewModel());
    }

    private void CreateCircle_Click(object sender, RoutedEventArgs e)
    {
        var entity = viewModel.CreateCircle();
        entity.Components.Add(new TransformViewModel());
    }

    private void Hierarchy_Selected(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
        SelectedItem = e.NewValue;
    }
}