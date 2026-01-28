using OpenTK;
using OpenTK.Graphics;
using Physics_Engine.Graphics;
using Vector2 = Physics_Engine.Math.Vector2;

namespace Physics_Engine
{
    public class Engine
    {
        public static void Main()
        {
            GameWindow gameWindow = new GameWindow(500, 500);
            var window = new Window(gameWindow);
        }

        private static void Start()
        {
        }

        private static void Update(object sender, FrameEventArgs e)
        {
        }
    }
}