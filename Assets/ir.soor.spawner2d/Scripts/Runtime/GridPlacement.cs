using System;
using UnityEngine;

namespace Soor.Spawner2d
{
    [Serializable]
    public class GridPlacement
    {
        [SerializeField] private Vector2 _spawnableSize = Vector2.zero;
        [SerializeField] private Vector2 _padding = Vector2.zero;
        [SerializeField] private int _rowsCount = -1;
        [SerializeField] private int _columnsCount = -1;

        public Vector2 SpawnableSize => _spawnableSize;
    
        private Vector2 _cellSizeWithPadding = Vector2.zero;

        public void CalculateCellSizeWithPadding()
        {
            _cellSizeWithPadding = _padding + _spawnableSize;
        }

        public void PlaceSpawnableGridly(Spawnable spawnable, int index)
        {
            var row = 1;
            var column = 1;

            if (_columnsCount > 0)
            {
                row = (int) (index / _columnsCount);
                column = index % _columnsCount;
            }
            else
            {
                column = (int) (index / _rowsCount);
                row = index % _rowsCount;
            }

            var position = new Vector2(_cellSizeWithPadding.x * column, _cellSizeWithPadding.y * row);
            spawnable.transform.position = position;
        }
    }
}
