using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using Physics_Engine.Core.Collision;
using Physics_Engine.Core.Input_System;
using Physics_Engine.Core.Log_System;
using Physics_Engine.Core.Physics_2D;
using Physics_Engine.Core.Rigidbody;
using Physics_Engine.Core.Sample_Physic_Objects;
using Physics_Engine.Core.Time;
using Physics_Engine.Graphics;
using Physics_Engine.Graphics.Shapes;
using Physics_Engine.Utilities;
using Circle = Physics_Engine.Graphics.Shapes.Circle;
using Vector2 = Physics_Engine.Math.Vector2;

namespace Physics_Engine
{
    public static class Engine
    {
        private static readonly List<PhysicsObject> AllObjects = new List<PhysicsObject>();
        private static Dictionary<PhysicsObject, List<Circle>> _dots = new Dictionary<PhysicsObject, List<Circle>>();
        private static readonly float moveOffset = 5000;
        private static float _dx;
        private static float _dy;
        private static PhysicsObject _player;
        private static PhysicsWorld _physicsWorld;
        private static PhysicsObjectContainer _physicsObjectContainer;

        public static void Main()
        {
            using var game = new GameWindow(800, 600, GraphicsMode.Default, "Physics Engine");
            var window = new Window(game);
            window.Run();
        }

        public static void Start()
        {
            InputSystem.Initialize();
            Logger.Log("Initialize Engine");
            _physicsWorld = new PhysicsWorld();
            _physicsObjectContainer = new PhysicsObjectContainer();

            // CreateRandomBodiesAndPlayer();
            CreateBackground();
            
            PhysicsObject ground = new PhysicsObject("Ground");
            ground.Transform.Position = new Vector2(0, -50);
            ground.Transform.Scale = new Vector2(200, 10);

            var groundRb =
                new RigidbodyBuilder.Builder<BoxRigidbody2D>()
                    .WithArea(new BoxArea(ground.Transform.Scale.x, ground.Transform.Scale.y))
                    .WithState(true)
                    .WithOwner(ground)
                    .Build();
            var groundCollider = new PolygonCollider()
            {
                Owner = ground
            };
            var groundRenderer = new Rectangle()
            {
                Color = Color4.DarkGray,
                Filled = true,
                Layer = 0,
                Owner = ground
            };
            ground.Components.Add(groundRb);
            ground.Components.Add(groundCollider);
            ground.Components.Add(groundRenderer);
        }

        private static void CreateBackground()
        {
            PhysicsObject background = new PhysicsObject("Background")
            {
                Transform =
                {
                    Scale = new Vector2(ShapeRenderer.Instance.MainCamera.Width,
                        ShapeRenderer.Instance.MainCamera.Height)
                }
            };
            var backgroundRenderer = new Rectangle()
            {
                Color = Color4.DimGray,
                Filled = true,
                Layer = -1,
                Owner = background
            };
            background.Components.Add(backgroundRenderer);
        }

        private static void CreateRandomBodiesAndPlayer()
        {
            _player = new Core.Sample_Physic_Objects.Circle("Player");
            _player.Transform.Position = new Vector2(-60f, 0f);
            _player.Components.Get<Shape2D>().Color = Color4.Gold;
            for (int i = -1; i < 1; i++)
            {
                for (int j = -2; j < 2; j++)
                {
                    Box box = new Box("Box")
                    {
                        Transform =
                        {
                            Position = new Vector2(i * 5, j * 5)
                        }
                    };
                    box.Components.Get<Rectangle>().Color = RandomHelper.GetRandomColor();
                    box.Components.Get<Rigidbody2D>().Body.IsStatic = RandomHelper.GetRandomBoolean();
                    if (box.Components.Get<Rigidbody2D>().Body.IsStatic)
                    {
                        box.Components.Get<Rectangle>().Color = Color4.White;
                    }
                }
            }

            for (int i = 5; i < 8; i++)
            {
                for (int j = -2; j < 2; j++)
                {
                    Core.Sample_Physic_Objects.Circle circle = new Core.Sample_Physic_Objects.Circle("Circle")
                    {
                        Transform =
                        {
                            Position = new Vector2(i * 5, j * 5)
                        }
                    };
                    circle.Components.Get<Circle>().Color = RandomHelper.GetRandomColor();
                    circle.Components.Get<Rigidbody2D>().Body.IsStatic = RandomHelper.GetRandomBoolean();
                    if (circle.Components.Get<Rigidbody2D>().Body.IsStatic)
                    {
                        circle.Components.Get<Circle>().Color = Color4.White;
                    }
                }
            }
        }

        public static void Update(object sender, FrameEventArgs e)
        {
            //ControlPlayer();
            //Logger.Log($"[Cursor State] x :{InputSystem.GetMousePositionCursorState().x} y :{InputSystem.GetMousePositionCursorState().y}");
            if (InputSystem.IsMouseButtonUp(0))
            {
                Logger.LogWarning("Up");
            }

            if (InputSystem.IsMouseButtonDown(0))
            {
                Box box = new Box("Box");
                box.Transform.Position = InputSystem.GetMousePositionCursorState();
                box.Components.Get<Rigidbody2D>().Body.HasGravity = true;
                box.Components.Get<Rectangle>().Color = RandomHelper.GetRandomColor();
            }

            if (InputSystem.IsMouseButtonDown(2))
            {
                Physics_Engine.Core.Sample_Physic_Objects.Circle circle =
                    new Physics_Engine.Core.Sample_Physic_Objects.Circle("Circle");
                circle.Transform.Position = InputSystem.GetMousePositionCursorState();
                circle.Components.Get<Rigidbody2D>().Body.HasGravity = true;
                circle.Components.Get<Circle>().Color = RandomHelper.GetRandomColor();
            }

            if (InputSystem.IsMouseButtonHeld(0))
            {
                Logger.LogError("Hold!!");
            }

            _physicsWorld.Simulate(Time.DeltaTimeFloat);
        }

        private static void ControlPlayer()
        {
            _dx = moveOffset * Time.DeltaTimeFloat * InputSystem.GetHorizontal();
            _dy = moveOffset * Time.DeltaTimeFloat * InputSystem.GetVertical();

            _player.Components.Get<Rigidbody2D>().AddForce(new Vector2(_dx, _dy));
        }
    }
}