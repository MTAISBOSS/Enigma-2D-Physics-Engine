using System.Diagnostics;
using System.Drawing.Imaging;
using System.Globalization;
using Enigma_Framework.Audio;
using Enigma_Framework.Core.Collision;
using Enigma_Framework.Core.ECS;
using Enigma_Framework.Core.Input;
using Enigma_Framework.Core.LogSystem;
using Enigma_Framework.Core.Physics2D;
using Enigma_Framework.Core.Raycast;
using Enigma_Framework.Core.Rigidbody;
using Enigma_Framework.Core.SampleEntities;
using Enigma_Framework.Core.SceneManagement;
using Enigma_Framework.Core.Time;
using Enigma_Framework.Graphics;
using Enigma_Framework.Graphics.Shapes;
using Enigma_Framework.Utilities;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Common.Input;
using OpenTK.Windowing.Desktop;
using Physics_Engine.Graphics;
using Physics_Engine.Graphics.Shapes;
using Circle = Enigma_Framework.Graphics.Shapes.Circle;
using Vector2 = Enigma_Framework.Math.Vector2;

namespace Enigma_Framework;

public static class Engine
{
    private static readonly List<Entity> AllObjects = new();
    private static Dictionary<Entity, List<Circle>> _dots = new();
    private static readonly float moveOffset = 10000;
    private static float _dx;
    private static float _dy;
    private static Entity? _player;
    private static Vector2 _playerInitialScale;
    private static readonly Stopwatch SampleTimer = new();
    private static readonly Stopwatch Watch = new();
    private static double _totalStepTime;
    private static int _totalSampleCount;
    private static int _totalSBodyCount;

    private static string _bodyCountString = string.Empty;
    private static string _wordStepTimeString = string.Empty;
    private static GameWindow? _game;
    private static Entity? _slope;
    private static Line _line;

    public static void Main()
    {
        Game game = new Game();
        game.Initialize();
        game.Run();
        game.Destroy();
    }

    public static void Start()
    {
      
        Logger.Log("Initialize Engine");
        
        var scene = SceneManager.CreateNewScene("Main");
        var box = scene.CreateEntity("Box");
        box.Components.Add(new CircleCollider());
        box.Components.Add(new Rigidbody2D());
        box.Components.Add(new Rectangle());
        
        SceneSerializer.Save(scene,"Main.scene");
        // CreateRandomBodiesAndPlayer();


        //AudioManager.LoadSound(new AudioClip("0100_00061", @"../../../assets/Audio/0100_00061.wav"));
        //AudioManager.CreateSource(new AudioSource("Music Source"));
        //AudioManager.Play(new AudioSource("Music Source"));

        SampleTimer.Start();
        //CreatePlayer();
        CreateGround();
        CreateSlope();
        CreateText();

        _line = new Line
        {
            Color = Color4.Red,
            Layer = LayerMask.Debug
        };
    }

    private static void CreateSlope()
    {
        _slope = new Entity("Slope");
        _slope.Transform.WorldPosition = new Vector2(-20, 20);
        _slope.Transform.WorldRotation = -45;
        _slope.Transform.WorldScale = new Vector2(50, 10);
        var slopeRenderer = new Rectangle
        {
            Color = Color4.Gold,
            Filled = true,
            Layer = LayerMask.World,
            Entity = _slope,
            RenderOrder = 3
        };
        var slopeRigidbody = new Rigidbody2D.Builder()
            .WithOwner(_slope)
            .WithState(true)
            .WithAngularVelocity(50)
            .WithLinearVelocity(new Vector2(1, 0))
            .Build();
        var slopeCollider = new PolygonCollider
        {
            Entity = _slope
        };
        _slope.Components.Add(slopeCollider);
        _slope.Components.Add(slopeRigidbody);
        _slope.Components.Add(slopeRenderer);
    }

    private static void CreatePlayer()
    {
        _player = new Entity("Player");

        _player.Transform.WorldScale = new Vector2(20, 20);
        _player.Transform.WorldPosition = new Vector2(-60f, 0f);
        _player.Transform.WorldRotation = 180;
        var sprite =
            new Sprite(@"../../Assets/Texture/Test.png")
            {
                Entity = _player,
                RenderOrder = 5
            };
        _player.Components.Add(sprite);

        var playerRigidbody2D = new Rigidbody2D.Builder()
            .WithOwner(_player)
            .WithGravityState(true)
            .WithState(false)
            .WithMass(1)
            .Build();

        var playerCollider = new PolygonCollider
        {
            Entity = _player,
            IsTrigger = true
        };
        _player.Components.Add(playerCollider);
        _player.Components.Add(playerRigidbody2D);
        _playerInitialScale = _player.Transform.WorldScale;
    }

    private static void CreateText()
    {
        var text = new Entity("Text");
        text.Transform.WorldPosition = new Vector2(0, 0);
        var textRenderer = new Text2D
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
        var ground = new Entity("Ground");
        ground.Transform.WorldPosition = new Vector2(0, -50);
        ground.Transform.WorldScale = new Vector2(200, 10);

        var groundRb =
            new Rigidbody2D.Builder()
                .WithState(true)
                .WithOwner(ground)
                .Build();
        var groundCollider = new PolygonCollider
        {
            Entity = ground
        };
        var groundRenderer = new Rectangle
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
        _player = new Core.SampleEntities.Circle("Player");
        _player.Transform.WorldPosition = new Vector2(-60f, 0f);
        _player.Components.Get<Shape2D>().Color = Color4.Gold;
        for (var i = -1; i < 1; i++)
        for (var j = -2; j < 2; j++)
        {
            var box = new Box("Box")
            {
                Transform =
                {
                    WorldPosition = new Vector2(i * 5, j * 5)
                }
            };
            box.Components.Get<Rectangle>().Color = RandomHelper.GetRandomColor();
            box.Components.Get<Rigidbody2D>().Body.IsStatic = RandomHelper.GetRandomBoolean();
            if (box.Components.Get<Rigidbody2D>().Body.IsStatic) box.Components.Get<Rectangle>().Color = Color4.White;
        }

        for (var i = 5; i < 8; i++)
        for (var j = -2; j < 2; j++)
        {
            var circle = new Core.SampleEntities.Circle("Circle")
            {
                Transform =
                {
                    WorldPosition = new Vector2(i * 5, j * 5)
                }
            };
            circle.Components.Get<Circle>().Color = RandomHelper.GetRandomColor();
            circle.Components.Get<Rigidbody2D>().Body.IsStatic = RandomHelper.GetRandomBoolean();
            if (circle.Components.Get<Rigidbody2D>().Body.IsStatic)
                circle.Components.Get<Circle>().Color = Color4.White;
        }
    }

    public static void Update()
    {
        //ControlPlayer();
        //Logger.Log($"[Cursor State] x :{InputSystem.GetMousePositionCursorState().x} y :{InputSystem.GetMousePositionCursorState().y}");
        _slope.Components.Get<Rigidbody2D>().RotateByAmount(Time.DeltaTimeFloat * 10);
        //TODO: Loop through all components in scene graph and call update method on all of them
        if (InputSystem.IsMouseButtonDown(0))
        {
            if (RandomHelper.GetRandomBoolean())
            {
                var circle =
                    new Core.SampleEntities.Circle("Circle");
                circle.Transform.WorldPosition = ShapeRenderer.Instance.MainCamera.ScreenToWorldPoint(
                    new OpenTK.Mathematics.Vector2(_game.ClientSize.X, _game.ClientSize.Y),
                    InputSystem.GetMousePosition().ConvertFromOpenTk()).ConvertToOpenTk();
                circle.Components.Get<Rigidbody2D>().Body.HasGravity = true;
                circle.Components.Get<Circle>().Color = RandomHelper.GetRandomColor();
            }
            else
            {
                var box = new Box("Box");
                box.Transform.WorldPosition = ShapeRenderer.Instance.MainCamera.ScreenToWorldPoint(
                    new OpenTK.Mathematics.Vector2(_game.ClientSize.X, _game.ClientSize.Y),
                    InputSystem.GetMousePosition().ConvertFromOpenTk()).ConvertToOpenTk();
                box.Components.Get<Rigidbody2D>().Body.HasGravity = true;
                box.Components.Get<Rectangle>().Color = RandomHelper.GetRandomColor();
            }
        }

        if (SampleTimer.Elapsed.TotalSeconds > 1d)
        {
            _bodyCountString = System.Math.Round(_totalSBodyCount / (double)_totalSampleCount, 4)
                .ToString(CultureInfo.InvariantCulture);
            _wordStepTimeString = System.Math.Round(_totalStepTime / _totalSampleCount, 4)
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
        var origin = Vector2.Right * 10;
        var direction = Vector2.One * -1;
        float length = 50;
        var ray2D = new Ray2D(origin, direction, length);
        if (Raycast2D.Raycast(ray2D, PhysicsContext.GetColliders(), out var hit2D))
        {
            Logger.LogWarning(
                $"Raycast hit:{hit2D.Collider.Entity.Name} at position: {hit2D.Point} with normal: {hit2D.Normal} with distance: {hit2D.Distance}");
            _line.EndPosition = hit2D.Point.ConvertFromOpenTk();
            _line.StartPosition = origin.ConvertFromOpenTk();
        }

        Watch.Restart();
        PhysicsContext.Simulate(Time.DeltaTimeFloat);
        Watch.Stop();

        _totalStepTime += Watch.Elapsed.TotalMilliseconds;
        _totalSBodyCount += PhysicsContext.BodyCount;
        _totalSampleCount++;
        // DrawContactPoints();
    }

    private static void ControlPlayer()
    {
        _dx = moveOffset * Time.DeltaTimeFloat * InputSystem.GetHorizontal();
        _dy = moveOffset * Time.DeltaTimeFloat * InputSystem.GetVertical();

        _player.Components.Get<Rigidbody2D>().AddForce(new Vector2(_dx, _dy));
        if (InputSystem.GetHorizontal() > 0)
            _player.Transform.WorldScale = new Vector2(-_playerInitialScale.x, _player.Transform.WorldScale.y);
        else if (InputSystem.GetHorizontal() < 0)
            _player.Transform.WorldScale = new Vector2(_playerInitialScale.x, _player.Transform.WorldScale.y);
    }
}