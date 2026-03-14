using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;
using Physics_Engine.Core.Collision;
using Physics_Engine.Core.Input_System;
using Physics_Engine.Core.Physics_2D;
using Physics_Engine.Core.Rigidbody;
using Physics_Engine.Graphics;
using Physics_Engine.Math;

namespace Physics_Engine
{
    public class Engine
    {
        private static readonly List<PhysicsObject> AllObjects = new List<PhysicsObject>();
        private Dictionary<PhysicsObject, List<CircleShape>> _dots = new Dictionary<PhysicsObject, List<CircleShape>>();
        private static readonly float moveOffset = 50;
        private static float _dx;
        private static float _dy;
        private static PhysicsObject _boxPlayer;

        public static void Main()
        {
            using var game = new GameWindow(800, 600, GraphicsMode.Default, "Physics Engine");
            var window = new Window(game);
            window.Run();
        }

        public static void Start()
        {
            
            for (int i = 0; i < 1; i++)
            {
                for (int j = 0; j < 1; j++)
                {
                    PhysicsObject box = new PhysicsObject("Box");
                    BoxRigidbody2D boxRigidbody2D = new Rigidbody2D.Builder<BoxRigidbody2D>()
                        .WithArea(new BoxShapeArea() { Width = 4, Height = 4 })
                        .WithPosition(new Math.Vector2((i + 1) * 6, (j + 1) * 6))
                        .Build();
                    BoxShape boxRenderer = new BoxShape
                    {
                        Position = boxRigidbody2D.Body.Position.ConvertFromOpenTk(),
                        Width = 4,
                        Height = 4,
                        Color = Color4.Red,
                        Layer = 0,
                        Filled = true
                    };

                    box.AddComponent(boxRigidbody2D);
                    box.AddComponent(boxRenderer);
                    ShapeRenderer.Instance.Renderables.Add(boxRenderer);
                    AllObjects.Add(box);
                }
            }

            BoxShape background = new BoxShape()
            {
                Color = Color4.Blue,
                Filled = true,
                Height = 100,
                Width = 100,
                Layer = -1,
                Owner = null,
                Position = OpenTK.Vector2.Zero,
                Rotation = 0
            };
            ShapeRenderer.Instance.Renderables.Add(background);

            _boxPlayer = new PhysicsObject("Box Player");
            BoxRigidbody2D boxPlayerRb = new Rigidbody2D.Builder<BoxRigidbody2D>()
                .WithArea(new BoxShapeArea() { Width = 4, Height = 4 })
                .WithPosition(new Math.Vector2(0, 0))
                .Build();
            BoxShape boxPlayerRenderer = new BoxShape
            {
                Position = boxPlayerRb.Body.Position.ConvertFromOpenTk(),
                Width = 4,
                Height = 4,
                Color = Color4.White,
                Layer = 0,
                Filled = true
            };

            _boxPlayer.AddComponent(boxPlayerRb);
            _boxPlayer.AddComponent(boxPlayerRenderer);
            ShapeRenderer.Instance.Renderables.Add(boxPlayerRenderer);
            AllObjects.Add(_boxPlayer);
        }

        public static void Update(object sender, FrameEventArgs e)
        {
            _dx += moveOffset * Time.DeltaTimeFloat * InputSystem.GetHorizontal();
            _dy += moveOffset * Time.DeltaTimeFloat * InputSystem.GetVertical();

            _boxPlayer.GetComponent<BoxRigidbody2D>()?.MoveTowards(new Math.Vector2(_dx, _dy));

            _boxPlayer.GetComponent<BoxShape>()!.Position =
                _boxPlayer.GetComponent<BoxRigidbody2D>()!.Body.Position.ConvertFromOpenTk();

            AllObjects.ForEach(o =>
                o.GetComponent<BoxRigidbody2D>()?.Rotate((float)System.Math.PI * Time.DeltaTimeFloat * 10));

            for (int i = 0; i < AllObjects.Count - 1; i++)
            {
                BoxRigidbody2D boxA = AllObjects[i].GetComponent<BoxRigidbody2D>();
                for (int j = i + 1; j < AllObjects.Count; j++)
                {
                    BoxRigidbody2D boxB = AllObjects[j].GetComponent<BoxRigidbody2D>();
                    if (boxB != null && boxA != null && Collider2D.IntersectPolygons(boxA.GetTransformedVertices(),
                            boxB.GetTransformedVertices(), out Math.Vector2 normal, out float depth))
                    {
                        Console.WriteLine("There is collision!");
                        boxA.Move(-normal * depth / 2f);
                        boxB.Move(normal * depth / 2f);
                    }
                }
            }


            AllObjects.ForEach(o =>
                o.GetComponent<BoxShape>()!.Rotation = o.GetComponent<BoxRigidbody2D>()!.Body.Rotation);
            AllObjects.ForEach(o =>
                o.GetComponent<BoxShape>()!.Position =
                    o.GetComponent<BoxRigidbody2D>()!.Body.Position.ConvertFromOpenTk());

        }
    }
}