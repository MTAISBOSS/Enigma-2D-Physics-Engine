using OpenTK;

namespace Physics_Engine.Graphics
{
    public static class Time
    {
        public static double ElapsedTime { get; private set; } = 0.0;
        public static double DeltaTime { get; private set; } = 0.0;
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
}