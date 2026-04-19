using System.Collections.ObjectModel;

namespace Enigma_Editor.ViewModels;

public class HierarchyItemViewModel : ViewModelBase
{
    private string name;

    public HierarchyItemViewModel(string name, string icon)
    {
        Name = name;
        this.name = name;
        Icon = icon;
    }

    public string Name
    {
        get => name;
        set => SetProperty(ref name, value);
    }

    public string Icon { get; set; }

    public ObservableCollection<HierarchyItemViewModel> Children { get; set; } = new();

    public ObservableCollection<ComponentViewModel> Components { get; set; } = new();
}