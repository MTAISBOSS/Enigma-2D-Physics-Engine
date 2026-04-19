namespace Enigma_Editor.ViewModels;

public class CameraComponentViewModel : ComponentViewModel
{
    private double _orthoSize = 5;
    private double _px;
    private double _py;

    public CameraComponentViewModel()
        : base("Camera", "https://yourdocs.com/camera",
            new Body())
    {
    }

    public double OrthoSize
    {
        get => _orthoSize;
        set => SetProperty(ref _orthoSize, value);
    }

    public double PositionX
    {
        get => _px;
        set => SetProperty(ref _px, value);
    }

    public double PositionY
    {
        get => _py;
        set => SetProperty(ref _py, value);
    }

    public class Body : ViewModelBase
    {
    }
}