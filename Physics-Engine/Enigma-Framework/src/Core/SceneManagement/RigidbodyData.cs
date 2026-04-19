using System.Reflection;
using System.Text.Json.Serialization;
using Enigma_Framework.Math;

namespace Enigma_Framework.Core.SceneManagement;

public class RigidbodyData : ComponentData
{
    public Vector2 LinearVelocity { get; set; }
    public float AngularVelocity { get; set; }
    public float Mass { get; set; }
    public float InverseMass { get; set; }
    public float Inertia { get; set; }
    public float InverseInertia { get; set; }
    public bool IsStatic { get; set; }
    public bool HasGravity { get; set; }
}