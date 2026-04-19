using Enigma_Framework.Core.LogSystem;
using Enigma_Framework.Math;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;


namespace Enigma_Framework.Core.Input;

public struct InputSystem
{
    private static KeyboardState? _keyboardState;
    private static MouseState? _mouseState;
    private static InputBinding _inputBinding;

    private static MouseState? _prevMouseState;
    private static KeyboardState? _prevKeyboardState;

    private static GameWindow? _gameWindow;

    private static Vector2 mousePosition;
    public static void Initialize(GameWindow? gameWindow)
    {
        _inputBinding = new InputBinding();
        _gameWindow = gameWindow;

        _keyboardState = _gameWindow?.KeyboardState.GetSnapshot();
        _mouseState = _gameWindow?.MouseState.GetSnapshot();
        _prevKeyboardState = _keyboardState;
        _prevMouseState = _mouseState;
        
        Logger.Log("Input system Initialized");
    }

    public static void Update()
    {
        if (_gameWindow == null) return;

        _prevKeyboardState = _keyboardState;
        _prevMouseState = _mouseState;

        _keyboardState = _gameWindow.KeyboardState.GetSnapshot();
        _mouseState = _gameWindow.MouseState.GetSnapshot();
    }

    #region Key Logic

    public static bool IsKeyDown(Keys key)
    {
        return _keyboardState != null && _keyboardState.IsKeyDown(key);
    }

    public static bool IsKeyPressed(Keys key)
    {
        return _prevKeyboardState != null && _keyboardState != null && _keyboardState.IsKeyDown(key) && !_prevKeyboardState.IsKeyDown(key);
    }

    public static bool IsKeyUp(Keys key)
    {
        return _keyboardState != null && !_keyboardState.IsKeyDown(key);
    }

    public static float GetHorizontal()
    {
        var keys = InputBinding.AllActions["Horizontal"];
        if (_keyboardState != null && (_keyboardState.IsKeyDown((Keys)keys[0]) || _keyboardState.IsKeyDown((Keys)keys[2]))) return -1;
        if (_keyboardState != null && (_keyboardState.IsKeyDown((Keys)keys[1]) || _keyboardState.IsKeyDown((Keys)keys[3]))) return 1;
        return 0;
    }

    public static float GetVertical()
    {
        var keys = InputBinding.AllActions["Vertical"];
        if (_keyboardState != null && (_keyboardState.IsKeyDown((Keys)keys[0]) || _keyboardState.IsKeyDown((Keys)keys[2]))) return -1;
        if (_keyboardState != null && (_keyboardState.IsKeyDown((Keys)keys[1]) || _keyboardState.IsKeyDown((Keys)keys[3]))) return 1;
        return 0;
    }

    #endregion

    #region Mouse Logic

    public static Vector2 GetMousePositionCursorState()
    {
        return new Vector2(_gameWindow.MouseState.X, _gameWindow.MouseState.Y);
    }

    public static Vector2 GetMousePosition()
    {
         return new Vector2(_mouseState.X, _mouseState.Y);
         return mousePosition;
    }

    public static bool IsMouseButtonDown(int button)
    {
        var btn = (MouseButton)button;
        return _prevMouseState != null && _mouseState != null && _mouseState.IsButtonDown(btn) && !_prevMouseState.IsButtonDown(btn);
    }

    public static bool IsMouseButtonUp(int button)
    {
        var btn = (MouseButton)button;
        return _prevMouseState != null && _mouseState != null && !_mouseState.IsButtonDown(btn) && _prevMouseState.IsButtonDown(btn);
    }

    public static bool IsMouseButtonHeld(int button)
    {
        var btn = (MouseButton)button;
        return _mouseState != null && _mouseState.IsButtonDown(btn);
    }

    #endregion

    public static void SetMousePosition(double positionX, double glY)
    {
        mousePosition = new Vector2((float)positionX, (float)glY);
    }
}
