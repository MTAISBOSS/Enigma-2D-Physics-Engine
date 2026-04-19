using System.Windows.Input;
using Keys = OpenTK.Windowing.GraphicsLibraryFramework.Keys;

namespace Enigma_Editor.Input;

public static class WpfInputAdapter
{
    public static Keys ConvertWpfKeyToOpenTkKey(Key wpfKey)
    {
        return wpfKey switch
        {
            // Alphabet
            Key.A => Keys.A,
            Key.B => Keys.B,
            Key.C => Keys.C,
            Key.D => Keys.D,
            Key.E => Keys.E,
            Key.F => Keys.F,
            Key.G => Keys.G,
            Key.H => Keys.H,
            Key.I => Keys.I,
            Key.J => Keys.J,
            Key.K => Keys.K,
            Key.L => Keys.L,
            Key.M => Keys.M,
            Key.N => Keys.N,
            Key.O => Keys.O,
            Key.P => Keys.P,
            Key.Q => Keys.Q,
            Key.R => Keys.R,
            Key.S => Keys.S,
            Key.T => Keys.T,
            Key.U => Keys.U,
            Key.V => Keys.V,
            Key.W => Keys.W,
            Key.X => Keys.X,
            Key.Y => Keys.Y,
            Key.Z => Keys.Z,

            // Numbers
            Key.D0 => Keys.D0,
            Key.D1 => Keys.D1,
            Key.D2 => Keys.D2,
            Key.D3 => Keys.D3,
            Key.D4 => Keys.D4,
            Key.D5 => Keys.D5,
            Key.D6 => Keys.D6,
            Key.D7 => Keys.D7,
            Key.D8 => Keys.D8,
            Key.D9 => Keys.D9,

            // Arrows
            Key.Up => Keys.Up,
            Key.Down => Keys.Down,
            Key.Left => Keys.Left,
            Key.Right => Keys.Right,

            // Modifiers & Special
            Key.Space => Keys.Space,
            Key.Escape => Keys.Escape,
            Key.Enter => Keys.Enter,
            Key.Back => Keys.Backspace,
            Key.Tab => Keys.Tab,
            Key.LeftShift => Keys.LeftShift,
            Key.RightShift => Keys.RightShift,
            Key.LeftCtrl => Keys.LeftControl,
            Key.RightCtrl => Keys.RightControl,
            Key.LeftAlt => Keys.LeftAlt,
            Key.RightAlt => Keys.RightAlt,

            // F-Keys
            Key.F1 => Keys.F1,
            Key.F2 => Keys.F2,
            Key.F3 => Keys.F3,
            Key.F4 => Keys.F4,
            Key.F5 => Keys.F5,
            Key.F6 => Keys.F6,
            Key.F7 => Keys.F7,
            Key.F8 => Keys.F8,
            Key.F9 => Keys.F9,
            Key.F10 => Keys.F10,
            Key.F11 => Keys.F11,
            Key.F12 => Keys.F12,

            // Default fallback for unmapped keys
            _ => Keys.Unknown
        };
    }
}