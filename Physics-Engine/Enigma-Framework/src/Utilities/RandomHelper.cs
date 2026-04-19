using OpenTK.Mathematics;

namespace Enigma_Framework.Utilities;

public struct RandomHelper
{
    private static readonly Random Rand = new();

    public static float GetRandomFloat(float min, float max)
    {
        if (min > max)
        {
            (min, max) = (max, min);
        }

        return min + (float)(Rand.NextDouble() * (max - min));
    }

    public static int GetRandomInteger(int min, int max)
    {
        if (min > max)
        {
            (min, max) = (max, min);
        }

        return Rand.Next(min, max + 1);
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
        return Rand.Next(0, 2) == 1;
    }
}