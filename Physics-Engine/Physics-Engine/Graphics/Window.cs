using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;
using Physics_Engine.Core.Time;

namespace Physics_Engine.Graphics
{
    public class Window
    {
        private readonly GameWindow _window;
        private Camera2D _camera;
        private ShapeRenderer _shapeRenderer;

        public Window(GameWindow window)
        {
            _window = window;
            _window.Load += OnLoad;
            _window.RenderFrame += OnRender;
            _window.UpdateFrame += OnUpdate;
            _window.Resize += OnResize;
        }
        public void Run()
        {
            _window.Load += (sender, e) => Time.Initialize(0.0);
            _window.Run(60);
        }
        private void OnLoad(object sender, System.EventArgs e)
        {
            GL.ClearColor(Color4.Black);
            GL.Disable(EnableCap.DepthTest);
            _camera = new Camera2D();
            _shapeRenderer = ShapeRenderer.Instance;
            _shapeRenderer.SetCamera(_camera);
            _shapeRenderer.MainCamera.Apply(_window.Width, _window.Height);
            Engine.Start();
        }
        private void OnResize(object sender, System.EventArgs e)
        {
            GL.Viewport(0, 0,
                _window.WindowState == WindowState.Fullscreen
                    ? GraphicsMode.Default.Buffers
                    : _window.Width,
                _window.WindowState == WindowState.Fullscreen
                    ? GraphicsMode.Default.Buffers
                    : _window.Height);
        }
        private void OnUpdate(object sender, FrameEventArgs e)
        {
            Time.Update(e);
            Engine.Update(sender, e);
            _shapeRenderer.Renderables.ForEach(o => o.Draw());
        }
        private void OnRender(object sender, FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            _shapeRenderer.MainCamera.Apply(_window.Width, _window.Height);
            _shapeRenderer.Render();
            _window.SwapBuffers();
        }
    }
}