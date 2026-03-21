using OpenTK.Graphics;
using System;

namespace Physics_Engine.Utilities;

public struct RandomHelper
{
    private static readonly Random _rand = new Random();

    public static float GetRandomFloat(float min, float max)
    {
        if (min > max)
        {
            float temp = min;
            min = max;
            max = temp;
        }

        return min + (float)(_rand.NextDouble() * (max - min));
    }

    public static int GetRandomInteger(int min, int max)
    {
        if (min > max)
        {
            int temp = min;
            min = max;
            max = temp;
        }

        return _rand.Next(min, max + 1);
    }

    public static Color4 GetRandomColor()
    {
        return new Color4(
            (byte)GetRandomInteger(0, 255),
            (byte)GetRandomInteger(0, 255),
            (byte)GetRandomInteger(0, 255),
            255
        );
    }

    public static bool GetRandomBoolean()
    {
        return _rand.Next(0, 2) == 1;
    }
}