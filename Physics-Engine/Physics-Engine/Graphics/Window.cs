using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;

namespace Physics_Engine.Graphics
{
    public class Window
    {
        private readonly GameWindow _window;

        private Camera2D _camera;
        private ShapeRenderer _renderer;

        private float _theta;

        public Window(GameWindow window)
        {
            _window = window;
            Start();
        }

        private void Start()
        {
            _window.Load += OnLoad;
            _window.RenderFrame += OnRender;
            _window.UpdateFrame += OnUpdate;
            _window.Resize += OnResize;
            _window.Run(60);
        }

        private void OnLoad(object sender, System.EventArgs e)
        {
            GL.ClearColor(Color4.Black);
            GL.Disable(EnableCap.DepthTest);

            _camera = new Camera2D();
            _renderer = new ShapeRenderer();

            _renderer.Renderables.Add(new CircleShape
            {
                Position = new Vector2(-15, 0),
                Radius = 5,
                Color = Color4.Green,
                Layer = 1
            });

            _renderer.Renderables.Add(new BoxShape
            {
                Position = new Vector2(10, 0),
                Width = 12,
                Height = 5,
                Rotation = 30,
                Color = Color4.Red,
                Layer = 0,
                Filled = true
            });

            _renderer.Renderables.Add(new PolygonShape
            {
                Position = new Vector2(0, 10),
                Vertices = new[]
                {
                    new Vector2(-3, -2),
                    new Vector2(3, -2),
                    new Vector2(0, 4)
                },
                Color = Color4.Cyan,
                Layer = 2
            });
        }

        private void OnResize(object sender, System.EventArgs e)
        {
            GL.Viewport(0, 0, _window.Width, _window.Height);
        }

        private void OnUpdate(object sender, FrameEventArgs e)
        {
            _theta += 60f * (float)e.Time;

            if (_renderer.Renderables[2] is Shape2D s)
                s.Rotation = _theta;
        }

        private void OnRender(object sender, FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            _camera.Apply(_window.Width, _window.Height);
            _renderer.Render();

            _window.SwapBuffers();
        }
    }
}