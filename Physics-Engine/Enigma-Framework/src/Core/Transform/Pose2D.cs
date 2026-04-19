using Enigma_Framework.Math;

namespace Enigma_Framework.Core.Transform;

public readonly struct Pose2D
{
    public readonly float PositionX;
    public readonly float PositionY;
    public readonly float Sin;
    public readonly float Cos;
    public static readonly Pose2D Zero = new(0f, 0f, 0f);

    public Pose2D(Vector2 position, float angle)
    {
        PositionX = position.x;
        PositionY = position.y;
        Sin = (float)System.Math.Sin(angle * System.Math.PI / 180);
        Cos = (float)System.Math.Cos(angle * System.Math.PI / 180);
    }

    public Pose2D(float x, float y, float angle)
    {
        PositionX = x;
        PositionY = y;
        Sin = (float)System.Math.Sin(angle * System.Math.PI / 180);
        Cos = (float)System.Math.Cos(angle * System.Math.PI / 180);
    }
}