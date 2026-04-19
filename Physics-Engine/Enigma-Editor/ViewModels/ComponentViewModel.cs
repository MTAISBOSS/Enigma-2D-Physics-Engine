using System.Diagnostics;
using System.Windows.Input;
using Enigma_Editor.Commands;

namespace Enigma_Editor.ViewModels;

public abstract class ComponentViewModel : ViewModelBase
{
    protected ComponentViewModel(string title, string docsUrl, ViewModelBase bodyVm)
    {
        Title = title;
        DocumentationUrl = docsUrl;
        BodyViewModel = bodyVm;

        OpenDocumentationCommand = new RelayCommand(OpenDocs);
        RemoveCommand = new RelayCommand(Remove);
    }

    public string Title { get; }
    public string DocumentationUrl { get; }

    public ViewModelBase BodyViewModel { get; }

    public ICommand OpenDocumentationCommand { get; }
    public ICommand RemoveCommand { get; }

    public event Action<ComponentViewModel>? OnRemoveRequested;

    private void OpenDocs()
    {
        Process.Start(new ProcessStartInfo(DocumentationUrl) { UseShellExecute = true });
    }

    private void Remove()
    {
        OnRemoveRequested?.Invoke(this);
    }
}