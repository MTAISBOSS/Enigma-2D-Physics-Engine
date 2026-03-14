using System.Collections.Generic;
using OpenTK.Input;

namespace Physics_Engine.Core.Input_System;

internal class InputBinding
{
    public static readonly Dictionary<string, Key[]> AllActions = new Dictionary<string, Key[]>();
    public InputBinding()
    {
        AllActions.Add("Horizontal", new[] { Key.A, Key.D, Key.Left, Key.Right });
        AllActions.Add("Vertical", new[] { Key.S, Key.W, Key.Down, Key.Up });
    }
}