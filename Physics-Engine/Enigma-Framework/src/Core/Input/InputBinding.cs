using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Enigma_Framework.Core.Input;

internal struct InputBinding
{
    public static readonly Dictionary<string, Keys[]> AllActions = new();

    public InputBinding()
    {
        AllActions.Add("Horizontal", new[] { Keys.A, Keys.D, Keys.Left, Keys.Right });
        AllActions.Add("Vertical", new[] { Keys.S, Keys.W, Keys.Down, Keys.Up });
    }
}