using System.Collections.ObjectModel;

namespace Enigma_Editor.ViewModels;

public class HierarchyMenuViewModel
{
    public ObservableCollection<HierarchyItemViewModel> HierarchyItems { get; set; } = new();

    public HierarchyItemViewModel CreateEntity()
    {
        var entity = new HierarchyItemViewModel(
            "Entity",
            "pack://application:,,,/assets/icons/Asset (99).png"
        );

        HierarchyItems.Add(entity);
        return entity;
    }

    public HierarchyItemViewModel CreateCamera()
    {
        var entity = new HierarchyItemViewModel(
            "Camera",
            "pack://application:,,,/assets/icons/Asset (41).png"
        );

        HierarchyItems.Add(entity);
        return entity;
    }

    public HierarchyItemViewModel CreateBox()
    {
        var entity = new HierarchyItemViewModel(
            "Box",
            "pack://application:,,,/assets/icons/Asset (4).png"
        );

        HierarchyItems.Add(entity);
        return entity;
    }

    public HierarchyItemViewModel CreateCircle()
    {
        var entity = new HierarchyItemViewModel(
            "Circle",
            "pack://application:,,,/assets/icons/Asset (99).png"
        );

        HierarchyItems.Add(entity);
        return entity;
    }
}