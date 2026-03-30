using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using Physics_Engine.Audio;
using Physics_Engine.Core.Collision;
using Physics_Engine.Core.Entity_Component_System;
using Physics_Engine.Core.Input_System;
using Physics_Engine.Core.Log_System;
using Physics_Engine.Core.Physics_2D;
using Physics_Engine.Core.Raycast;
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
        private static readonly List<Entity> AllObjects = new List<Entity>();
        private static Dictionary<Entity, List<Circle>> _dots = new Dictionary<Entity, List<Circle>>();
        private static readonly float moveOffset = 10000;
        private static float _dx;
        private static float _dy;
        private static Entity _player;
        private static PhysicsContext _physicsContext;
        private static EntityContainer _entityContainer;
        private static Vector2 _playerInitialScale;
        private static readonly Stopwatch SampleTimer = new Stopwatch();
        private static readonly Stopwatch Watch = new();
        private static double _totalStepTime;
        private static int _totalSampleCount;
        private static int _totalSBodyCount;

        private static string _bodyCountString = String.Empty;
        private static string _wordStepTimeString = String.Empty;
        private static GameWindow _game;
        private static Entity _slope;
        private static Line _line;

        public static void Main()
        {
            using var game = new GameWindow(800, 800, GraphicsMode.Default, "Physics Engine");
            _game = game;
            var window = new Window(game);
            window.Run();
        }

        public static void Start()
        {
            InputSystem.Initialize(_game);
            AudioManager.Initialize();
            Logger.Log("Initialize Engine");

            _physicsContext = new PhysicsContext();
            _entityContainer = new EntityContainer();

            // CreateRandomBodiesAndPlayer();


            AudioManager.LoadSound("0100_00061", @"../../Assets/Audio/0100_00061.wav");
            AudioManager.CreateSource("Music Source");
            //  AudioManager.Play("Music Source","0100_00061");

            SampleTimer.Start();
            //CreatePlayer();
            CreateGround();
            CreateSlope();
            CreateText();
            
            _line = new Line()
            {
                Color = Color4.Red,
                Layer = LayerMask.Debug
            };
        }

        private static void CreateSlope()
        {
            _slope = new Entity("Slope");
            _slope.Transform.Position = new Vector2(-20, 20);
            _slope.Transform.Rotation = -45;
            _slope.Transform.Scale = new Vector2(50, 10);
            Rectangle slopeRenderer = new Rectangle()
            {
                Color = Color4.Gold,
                Filled = true,
                Layer = LayerMask.World,
                Entity = _slope,
                RenderOrder = 3
            };
            Rigidbody2D slopeRigidbody = new RigidbodyBuilder.Builder<Rigidbody2D>()
                .WithOwner(_slope)
                .WithState(true)
                .WithAngularVelocity(50)
                .WithLinearVelocity(new Vector2(1, 0))
                .Build();
            PolygonCollider slopeCollider = new PolygonCollider()
            {
                Entity = _slope,
            };
            _slope.Components.Add(slopeCollider);
            _slope.Components.Add(slopeRigidbody);
            _slope.Components.Add(slopeRenderer);
        }

        private static void CreatePlayer()
        {
            _player = new Entity("Player");

            _player.Transform.Scale = new Vector2(20, 20);
            _player.Transform.Position = new Vector2(-60f, 0f);
            _player.Transform.Rotation = 180;
            Sprite sprite =
                new Sprite(@"../../Assets/Texture/Test.png")
                {
                    Entity = _player,
                    RenderOrder = 5
                };
            _player.Components.Add(sprite);

            Rigidbody2D playerRigidbody2D = new RigidbodyBuilder.Builder<Rigidbody2D>()
                .WithOwner(_player)
                .WithGravityState(true)
                .WithState(false)
                .WithMass(1)
                .Build();

            PolygonCollider playerCollider = new PolygonCollider()
            {
                Entity = _player,
                IsTrigger = true
            };
            _player.Components.Add(playerCollider);
            _player.Components.Add(playerRigidbody2D);
            _playerInitialScale = _player.Transform.Scale;
        }

        private static void CreateText()
        {
            Entity text = new Entity("Text");
            text.Transform.Position = new Vector2(0, 0);
            Text2D textRenderer = new Text2D()
            {
                Alignment = TextAlignment.Center,
                Color = Color4.White,
                FontFamily = "Arial",
                FontSize = 6,
                TextContent = "Hello World",
                RenderOrder = 100,
                Entity = text,
                Layer = LayerMask.UI
            };
            text.Components.Add(textRenderer);
        }

        private static void CreateGround()
        {
            Entity ground = new Entity("Ground");
            ground.Transform.Position = new Vector2(0, -50);
            ground.Transform.Scale = new Vector2(200, 10);

            var groundRb =
                new RigidbodyBuilder.Builder<Rigidbody2D>()
                    .WithState(true)
                    .WithOwner(ground)
                    .Build();
            var groundCollider = new PolygonCollider()
            {
                Entity = ground
            };
            var groundRenderer = new Rectangle()
            {
                Color = Color4.DarkGray,
                Filled = true,
                RenderOrder = 0,
                Entity = ground
            };
            ground.Components.Add(groundCollider);
            ground.Components.Add(groundRb);
            ground.Components.Add(groundRenderer);
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
            _slope.Components.Get<Rigidbody2D>().RotateByAmount(Time.DeltaTimeFloat * 10);
            //TODO: Loop through all components in scene graph and call update method on all of them
            if (InputSystem.IsMouseButtonDown(2))
            {
                if (RandomHelper.GetRandomBoolean())
                {
                    Physics_Engine.Core.Sample_Physic_Objects.Circle circle =
                        new Physics_Engine.Core.Sample_Physic_Objects.Circle("Circle");
                    circle.Transform.Position = ShapeRenderer.Instance.MainCamera.ScreenToWorldPoint(
                        new OpenTK.Vector2(_game.Width, _game.Height),
                        InputSystem.GetMousePosition().ConvertFromOpenTk());
                    circle.Components.Get<Rigidbody2D>().Body.HasGravity = true;
                    circle.Components.Get<Circle>().Color = RandomHelper.GetRandomColor();
                }
                else
                {
                    Box box = new Box("Box");
                    box.Transform.Position = ShapeRenderer.Instance.MainCamera.ScreenToWorldPoint(
                        new OpenTK.Vector2(_game.Width, _game.Height),
                        InputSystem.GetMousePosition().ConvertFromOpenTk());
                    box.Components.Get<Rigidbody2D>().Body.HasGravity = true;
                    box.Components.Get<Rectangle>().Color = RandomHelper.GetRandomColor();
                }
            }

            if (SampleTimer.Elapsed.TotalSeconds > 1d)
            {
                _bodyCountString = System.Math.Round(_totalSBodyCount / (double)_totalSampleCount, 4)
                    .ToString(CultureInfo.InvariantCulture);
                _wordStepTimeString = System.Math.Round(_totalStepTime / (double)_totalSampleCount, 4)
                    .ToString(CultureInfo.InvariantCulture);
                //Logger.LogWarning($"Body Count : {_bodyCountString}");
                //Logger.LogWarning($"Sample Time Count : {_totalSampleCount}");
                //Logger.LogWarning($"Step Time: {_totalStepTime}");

                _totalSBodyCount = 0;
                _totalStepTime = 0;
                _totalSampleCount = 0;
                SampleTimer.Restart();
            }

//Logger.Log($"X: {_player.Transform.Position.x} Y: {_player.Transform.Position.y}");
            Vector2 origin = Vector2.Right * 10;
            Vector2 direction = Vector2.One * -1;
            float length = 50;
            Ray2D ray2D = new Ray2D(origin, direction, length);
            if (Raycast2D.Raycast(ray2D, _physicsContext.GetColliders(), out RaycastHit2D hit2D))
            {
                Logger.LogWarning(
                    $"Raycast hit:{hit2D.Collider.Entity.Name} at position: {hit2D.Point} with normal: {hit2D.Normal} with distance: {hit2D.Distance}");
                _line.EndPosition = hit2D.Point.ConvertFromOpenTk();
                _line.StartPosition = origin.ConvertFromOpenTk();
            }

            Watch.Restart();
            _entityContainer.Update();
            _physicsContext.Simulate(Time.DeltaTimeFloat);
            Watch.Stop();

            _totalStepTime += Watch.Elapsed.TotalMilliseconds;
            _totalSBodyCount += _physicsContext.BodyCount;
            _totalSampleCount++;

            // DrawContactPoints();
        }

        private static void ControlPlayer()
        {
            _dx = moveOffset * Time.DeltaTimeFloat * InputSystem.GetHorizontal();
            _dy = moveOffset * Time.DeltaTimeFloat * InputSystem.GetVertical();

            _player.Components.Get<Rigidbody2D>().AddForce(new Vector2(_dx, _dy));
            if (InputSystem.GetHorizontal() > 0)
            {
                _player.Transform.Scale = new Vector2(-_playerInitialScale.x, _player.Transform.Scale.y);
            }
            else if (InputSystem.GetHorizontal() < 0)
            {
                _player.Transform.Scale = new Vector2(_playerInitialScale.x, _player.Transform.Scale.y);
            }
        }
    }
}