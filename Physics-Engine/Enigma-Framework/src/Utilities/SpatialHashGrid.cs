using Enigma_Framework.Core.Collision;

namespace Enigma_Framework.Utilities;

public class SpatialHashGrid
{
    private readonly Dictionary<int, List<int>> cells = new();
    private readonly float cellSize;
    private readonly Stack<List<int>> listPool = new();

    public SpatialHashGrid(float cellSize)
    {
        this.cellSize = cellSize;
    }

    public void Clear()
    {
        foreach (var cell in cells.Values)
        {
            cell.Clear();
            listPool.Push(cell);
        }

        cells.Clear();
    }

    public void Insert(int bodyIndex, AABBCollision aabb)
    {
        var startX = (int)System.Math.Floor(aabb.Min.x / cellSize);
        var startY = (int)System.Math.Floor(aabb.Min.y / cellSize);
        var endX = (int)System.Math.Floor(aabb.Max.x / cellSize);
        var endY = (int)System.Math.Floor(aabb.Max.y / cellSize);

        for (var x = startX; x <= endX; x++)
        for (var y = startY; y <= endY; y++)
        {
            var hash = GetHash(x, y);

            if (!cells.TryGetValue(hash, out var cell))
            {
                cell = listPool.Count > 0 ? listPool.Pop() : new List<int>(8);
                cells[hash] = cell;
            }

            cell.Add(bodyIndex);
        }
    }

    public Dictionary<int, List<int>>.ValueCollection GetActiveCells()
    {
        return cells.Values;
    }

    private int GetHash(int x, int y)
    {
        unchecked
        {
            var hash = 17;
            hash = hash * 31 + x;
            hash = hash * 31 + y;
            return hash;
        }
    }
}