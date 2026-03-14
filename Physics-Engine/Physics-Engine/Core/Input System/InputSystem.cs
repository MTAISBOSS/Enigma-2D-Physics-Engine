using OpenTK.Input;

namespace Physics_Engine.Core.Input_System;

public static class InputSystem
{
    private static KeyboardState _keyboardState;
    private static InputBinding _inputBinding ;

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
}