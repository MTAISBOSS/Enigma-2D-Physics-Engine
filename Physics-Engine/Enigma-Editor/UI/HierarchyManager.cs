using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Brushes = System.Windows.Media.Brushes;
using Image = System.Windows.Controls.Image;
using Orientation = System.Windows.Controls.Orientation;
using TreeView = System.Windows.Controls.TreeView;

namespace Enigma_Editor.UI;

public class HierarchyManager
{
    private readonly TreeView _tree;

    public HierarchyManager(TreeView tree)
    {
        _tree = tree;
    }

    public void AddItem(string name, string icon)
    {
        var item = new TreeViewItem
        {
            Header = CreateHeader(name, icon)
        };

        _tree.Items.Add(item);
    }

    private StackPanel CreateHeader(string title, string iconPath)
    {
        return new StackPanel
        {
            Orientation = Orientation.Horizontal,
            Children =
            {
                new Image
                {
                    Source = new BitmapImage(new Uri(iconPath)),
                    Width = 10,
                    Margin = new Thickness(0, 0, 8, 0)
                },
                new TextBlock
                {
                    Text = title,
                    Foreground = Brushes.Azure
                }
            }
        };
    }
}