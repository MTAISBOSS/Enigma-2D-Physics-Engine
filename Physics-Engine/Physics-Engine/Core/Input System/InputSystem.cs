using OpenTK;
using OpenTK.Input;
using Physics_Engine.Core.Log_System;
using Vector2 = Physics_Engine.Math.Vector2;

namespace Physics_Engine.Core.Input_System;

public struct InputSystem
{
    private static KeyboardState _keyboardState;
    private static MouseState _mouseState;
    private static InputBinding _inputBinding;
    
    private static MouseState _prevMouseState;
    private static KeyboardState _prevKeyboardState;

    private static GameWindow _gameWindow;

    public static void Initialize(GameWindow gameWindow)
    {
        _inputBinding = new InputBinding();
        _gameWindow = gameWindow;

        _gameWindow.MouseDown += OnMouseDown;
        _gameWindow.MouseUp += OnMouseUp;
        _gameWindow.MouseMove += OnMouseMove;
        _gameWindow.MouseWheel += OnMouseWheel;

        _gameWindow.KeyDown += OnKeyDown;
        _gameWindow.KeyUp += OnKeyUp;
        
        _keyboardState = Keyboard.GetState();
        _mouseState = Mouse.GetState();
        Logger.Log("Input system Initialized");
    }

    #region Event Handlers

    private static void OnKeyDown(object sender, KeyboardKeyEventArgs e)
    {
        _prevKeyboardState = _keyboardState;
        _keyboardState = e.Keyboard;
    }

    private static void OnKeyUp(object sender, KeyboardKeyEventArgs e)
    {
        _prevKeyboardState = _keyboardState;
        _keyboardState = e.Keyboard;
    }

    private static void OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        _prevMouseState = _mouseState;
        _mouseState = e.Mouse;
    }

    private static void OnMouseUp(object sender, MouseButtonEventArgs e)
    {
        _prevMouseState = _mouseState;
        _mouseState = e.Mouse;
    }

    private static void OnMouseMove(object sender, MouseMoveEventArgs e)
    {
        _mouseState = e.Mouse;
    }

    private static void OnMouseWheel(object sender, MouseWheelEventArgs e)
    {
        _mouseState = e.Mouse;
    }

    #endregion

    #region Key Logic

    public static bool IsKeyDown(Key key)
    {
        return _keyboardState.IsKeyDown(key);
    }

    public static bool IsKeyPressed(Key key)
    {
        return _keyboardState.IsKeyDown(key) && !_prevKeyboardState.IsKeyDown(key);
    }

    public static bool IsKeyUp(Key key)
    {
        return _keyboardState.IsKeyUp(key);
    }

    public static float GetHorizontal()
    {
        var keys = InputBinding.AllActions["Horizontal"];
        if (_keyboardState.IsKeyDown(keys[0]) || _keyboardState.IsKeyDown(keys[2])) return -1;
        if (_keyboardState.IsKeyDown(keys[1]) || _keyboardState.IsKeyDown(keys[3])) return 1;
        return 0;
    }

    public static float GetVertical()
    {
        var keys = InputBinding.AllActions["Vertical"];
        if (_keyboardState.IsKeyDown(keys[0]) || _keyboardState.IsKeyDown(keys[2])) return -1;
        if (_keyboardState.IsKeyDown(keys[1]) || _keyboardState.IsKeyDown(keys[3])) return 1;
        return 0;
    }

    #endregion

    #region Mouse Logic

    public static Vector2 GetMousePositionCursorState()
    {
        var cursor = Mouse.GetCursorState();
        return new Vector2(cursor.X, cursor.Y);
    }

    public static Vector2 GetMousePosition()
    {
        return new Vector2(_mouseState.X, _mouseState.Y);
    }

    public static bool IsMouseButtonDown(int button)
    {
        MouseButton btn = (MouseButton)button;
        return _mouseState.IsButtonDown(btn) && !_prevMouseState.IsButtonDown(btn);
    }

    public static bool IsMouseButtonUp(int button)
    {
        MouseButton btn = (MouseButton)button;
        return _mouseState.IsButtonUp(btn) && _prevMouseState.IsButtonDown(btn);
    }

    public static bool IsMouseButtonHeld(int button)
    {
        MouseButton btn = (MouseButton)button;
        return _mouseState.IsButtonDown(btn);
    }

    #endregion
}
