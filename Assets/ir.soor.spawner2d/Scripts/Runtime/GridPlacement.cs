using System;
using UnityEngine;

namespace Soor.Spawner2d
{
    /// <summary>
    /// Manages the placement of objects in a grid-based layout.
    /// This component calculates positions based on rows, columns, and cell size.
    /// </summary>
    [Serializable]
    public class GridPlacement
    {
        #region SERIALIZED_FIELDS

        /// <summary>
        /// The base size of each spawnable object to be used for grid calculations.
        /// </summary>
        [SerializeField] private Vector2 _spawnableSize = Vector2.zero;
        
        /// <summary>
        /// Additional space added between each cell in the grid.
        /// </summary>
        [SerializeField] private Vector2 _padding = Vector2.zero;
        
        /// <summary>
        /// The number of rows in the grid. A value of -1 indicates that it should be calculated automatically.
        /// </summary>
        [SerializeField] private int _rowsCount = -1;
        
        /// <summary>
        /// The number of columns in the grid. A value of -1 indicates that it should be calculated automatically.
        /// </summary>
        [SerializeField] private int _columnsCount = -1;

        #endregion SERIALIZED_FIELDS


        #region FIELDS

        /// <summary>
        /// The calculated size of each cell including padding.
        /// </summary>
        private Vector2 _cellSizeWithPadding = Vector2.zero;

        #endregion FIELDS

        #region METHODS

        /// <summary>
        /// Calculates the final cell size by adding padding to the spawnable size.
        /// </summary>
        public void CalculateCellSizeWithPadding()
        {
            _cellSizeWithPadding = _padding + _spawnableSize;
        }

        /// <summary>
        /// Sets the size of a given spawnable object to the configured <see cref="_spawnableSize"/>.
        /// </summary>
        /// <param name="spawnable">The spawnable object whose size will be set.</param>
        public void SetSpawnableSize(Spawnable spawnable)
        {
            spawnable.SetSize(_spawnableSize);
        }

        /// <summary>
        /// Places a spawnable object at the correct grid position based on its index.
        /// </summary>
        /// <param name="spawnable">The object to be placed.</param>
        /// <param name="index">The index of the object in the spawn queue.</param>
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
        
        #endregion METHODS
    }
}