using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;
using Physics_Engine.Core.Collision;
using Physics_Engine.Core.Input_System;
using Physics_Engine.Core.Physics_2D;
using Physics_Engine.Core.Rigidbody;
using Physics_Engine.Math;
using Vector2 = Physics_Engine.Math.Vector2;

namespace Physics_Engine.Graphics
{
    public class Window
    {
        private readonly GameWindow _window;

        private Camera2D _camera;

        private ShapeRenderer _renderer;
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
            _window.Load += (sender, e) => InputSystem.Initialize();
            _window.Run(60);
        }


        private void OnLoad(object sender, System.EventArgs e)
        {
            GL.ClearColor(Color4.Black);
            GL.Disable(EnableCap.DepthTest);

            _camera = new Camera2D();
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
            ShapeRenderer.Instance.Renderables.ForEach(o => o.Draw());
        }

        private void OnRender(object sender, FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            _camera.Apply(_window.Width, _window.Height);
            ShapeRenderer.Instance.Render();
            _window.SwapBuffers();
        }
    }
}