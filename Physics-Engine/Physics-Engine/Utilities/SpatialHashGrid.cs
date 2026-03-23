using System.Collections.Generic;
using Physics_Engine.Core.Collision;

namespace Physics_Engine.Utilities
{
    public class SpatialHashGrid
    {
        private readonly float _cellSize;
        private readonly Dictionary<int, List<int>> _cells = new();
        private readonly Stack<List<int>> _listPool = new();

        public SpatialHashGrid(float cellSize)
        {
            _cellSize = cellSize;
        }

        public void Clear()
        {
            foreach (var cell in _cells.Values)
            {
                cell.Clear();
                _listPool.Push(cell);
            }
            _cells.Clear();
        }

        public void Insert(int bodyIndex, AABBCollision aabb)
        {
            int startX = (int)System.Math.Floor(aabb.Min.x / _cellSize);
            int startY = (int)System.Math.Floor(aabb.Min.y / _cellSize);
            int endX = (int)System.Math.Floor(aabb.Max.x / _cellSize);
            int endY = (int)System.Math.Floor(aabb.Max.y / _cellSize);

            for (int x = startX; x <= endX; x++)
            {
                for (int y = startY; y <= endY; y++)
                {
                    int hash = GetHash(x, y);

                    if (!_cells.TryGetValue(hash, out List<int> cell))
                    {
                        cell = _listPool.Count > 0 ? _listPool.Pop() : new List<int>(8);
                        _cells[hash] = cell;
                    }

                    cell.Add(bodyIndex);
                }
            }
        }

        public Dictionary<int, List<int>>.ValueCollection GetActiveCells()
        {
            return _cells.Values;
        }

        private int GetHash(int x, int y)
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 31 + x;
                hash = hash * 31 + y;
                return hash;
            }
        }
    }
}
