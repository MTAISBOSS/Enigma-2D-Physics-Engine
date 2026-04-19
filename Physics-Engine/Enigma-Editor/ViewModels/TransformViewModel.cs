namespace Enigma_Editor.ViewModels;

public class TransformViewModel : ComponentViewModel
{
    private double _positionX;

    private double _positionY;

    private double _rotationX;

    private double _rotationY;

    private double _scaleX;

    private double _scaleY;

    public TransformViewModel() : base("Transform", "https://yourdocs.com/transform",
        new Body())
    {
    }

    public double PositionX
    {
        get => _positionX;
        set => SetProperty(ref _positionX, value);
    }

    public double PositionY
    {
        get => _positionY;
        set => SetProperty(ref _positionY, value);
    }

    public double RotationX
    {
        get => _rotationX;
        set => SetProperty(ref _rotationX, value);
    }

    public double RotationY
    {
        get => _rotationY;
        set => SetProperty(ref _rotationY, value);
    }

    public double ScaleX
    {
        get => _scaleX;
        set => SetProperty(ref _scaleX, value);
    }

    public double ScaleY
    {
        get => _scaleY;
        set => SetProperty(ref _scaleY, value);
    }

    public class Body : ViewModelBase
    {
    }
}