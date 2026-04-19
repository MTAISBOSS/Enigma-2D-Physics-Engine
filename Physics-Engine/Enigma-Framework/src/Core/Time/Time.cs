using OpenTK.Windowing.Common;

namespace Enigma_Framework.Core.Time;

public struct Time
{
    public static double ElapsedTime { get; private set; }
    public static double DeltaTime { get; private set; }
    public static float DeltaTimeFloat => (float)DeltaTime;

    public static void Initialize(double initialDeltaTime)
    {
        DeltaTime = initialDeltaTime;
        ElapsedTime = 0.0;
    }

    public static void Update(FrameEventArgs e)
    {
        DeltaTime = e.Time;
        ElapsedTime += DeltaTime;
    }
}