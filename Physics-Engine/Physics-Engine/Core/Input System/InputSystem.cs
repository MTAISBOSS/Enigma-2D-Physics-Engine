using OpenTK.Input;
using Physics_Engine.Math;

namespace Physics_Engine.Core.Input_System;

public static class InputSystem
{
    private static KeyboardState _keyboardState;
    private static MouseState _mouseState;
    private static InputBinding _inputBinding;
    private static bool _isMouseButtonPressed;

    public static void Initialize()
    {
        _inputBinding = new InputBinding();
    }

    public static bool IsKeyDown(Key key)
    {
        _keyboardState = Keyboard.GetState();
        return _keyboardState.IsKeyDown(key);
    }

    public static bool IsKeyUp(Key key)
    {
        _keyboardState = Keyboard.GetState();
        return _keyboardState.IsKeyUp(key);
    }

    public static float GetHorizontal()
    {
        var keys = InputBinding.AllActions["Horizontal"];
        _keyboardState = Keyboard.GetState();
        if (_keyboardState.IsKeyDown(keys[0]) || _keyboardState.IsKeyDown(keys[2]))
        {
            return -1;
        }

        if (_keyboardState.IsKeyDown(keys[1]) || _keyboardState.IsKeyDown(keys[3]))
        {
            return 1;
        }

        return 0;
    }

    public static float GetVertical()
    {
        var keys = InputBinding.AllActions["Vertical"];
        _keyboardState = Keyboard.GetState();
        if (_keyboardState.IsKeyDown(keys[0]) || _keyboardState.IsKeyDown(keys[2]))
        {
            return -1;
        }

        if (_keyboardState.IsKeyDown(keys[1]) || _keyboardState.IsKeyDown(keys[3]))
        {
            return 1;
        }

        return 0;
    }

    public static Vector2 GetMousePositionCursorState()
    {
        _mouseState = Mouse.GetCursorState();
        return new Vector2(_mouseState.X, _mouseState.Y);
    }

    public static Vector2 GetMousePosition()
    {
        _mouseState = Mouse.GetState();
        return new Vector2(_mouseState.X, _mouseState.Y);
    }

    /// <summary>
    /// returns the state of the pressed button on mouse
    /// 0 is lmb
    /// 1 is mmb
    /// 2 is rmb
    /// </summary>
    /// <param name="button"></param>
    /// <returns></returns>
    public static bool IsMouseButtonDown(int button)
    {
        if (_isMouseButtonPressed)
        {
            return false;
        }
        _mouseState = Mouse.GetCursorState();
        if (button == 0 && _mouseState.LeftButton == ButtonState.Pressed)
        {
            _isMouseButtonPressed = true;
            return true;
        }

        if (button == 1 && _mouseState.MiddleButton == ButtonState.Pressed)
        {
            _isMouseButtonPressed = true;
            return true;
        }

        if (button == 2 && _mouseState.RightButton == ButtonState.Pressed)
        {
            _isMouseButtonPressed = true;
            return true;
        }

        return false;
    }

    /// <summary>
    /// returns the state of the released button on mouse
    /// 0 is lmb
    /// 1 is mmb
    /// 2 is rmb
    /// </summary>
    /// <param name="button"></param>
    /// <returns></returns>
    public static bool IsMouseButtonUp(int button)
    {
        if (!_isMouseButtonPressed)
        {
            return false;
        }
        _mouseState = Mouse.GetCursorState();
        if (button == 0 && _mouseState.LeftButton == ButtonState.Released)
        {
            _isMouseButtonPressed = false;
            return true;
        }

        if (button == 1 && _mouseState.MiddleButton == ButtonState.Released)
        {
            _isMouseButtonPressed = false;
            return true;
        }

        if (button == 2 && _mouseState.RightButton == ButtonState.Released)
        {
            _isMouseButtonPressed = false;
            return true;
        }

        return false;
    }  /// <summary>
    /// returns the state of the released button on mouse
    /// 0 is lmb
    /// 1 is mmb
    /// 2 is rmb
    /// </summary>
    /// <param name="button"></param>
    /// <returns></returns>
    public static bool IsMouseButtonHeld(int button)
    {
        _mouseState = Mouse.GetCursorState();
        if (button == 0 && _mouseState.LeftButton == ButtonState.Pressed)
        {
            return true;
        }

        if (button == 1 && _mouseState.MiddleButton == ButtonState.Pressed)
        {
            return true;
        }

        if (button == 2 && _mouseState.RightButton == ButtonState.Pressed)
        {
            return true;
        }
        return false;
    }
}