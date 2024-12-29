using System.Runtime.InteropServices;
using Hypercube.Mathematics;
using Hypercube.Mathematics.Vectors;

namespace Hypercube.Graphics.Rendering.Batching;

[StructLayout(LayoutKind.Sequential)]
public readonly struct Vertex
{
    public const int Size = 9;
    private static readonly Color DefaultColor = Color.White;

    public readonly Vector3 Position;
    public readonly Color Color;
    public readonly Vector2 UVCoords;
    
    public Vertex(Vector3 position, Vector2 uvCoords, Color color)
    {
        Position = position;
        UVCoords = uvCoords;
        Color = color;
    }
    
    public Vertex(Vector2 position, Vector2 uvCoords, Color color)
    {
        Position = new Vector3(position, 0.0f);
        UVCoords = uvCoords;
        Color = color;
    }

    public Vertex(Vector3 position, Vector2 uvCoords)
    {
        Position = position;
        UVCoords = uvCoords;
        Color = DefaultColor;
    }

    public IEnumerable<float> ToVertices()
    {
        yield return Position.X;
        yield return Position.Y;
        yield return Position.Z;
        yield return Color.R;
        yield return Color.G;
        yield return Color.B;
        yield return Color.A;
        yield return UVCoords.X;
        yield return UVCoords.Y;
    }
}