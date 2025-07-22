using System;
using UnityEngine;

[Serializable]
public class GridPlacement
{
    [SerializeField] private Vector2 _spawnableSize = Vector2.zero;

    public Vector2 SpawnableSize => _spawnableSize;
    
    [SerializeField] private Vector2 _padding = Vector2.zero;

    [SerializeField] private int _rowsCount = -1;
    [SerializeField] private int _columnsCount = -1;

    private Vector2 _cellSizeWithPadding = Vector2.zero;

    public void CalculateCellSizeWithPadding()
    {
        _cellSizeWithPadding = _padding + _spawnableSize;
    }

    public void PlaceSpawnableGridly(Spawnable spawnable, int index)
    {
        var row = 1;
        var column = 1;

//        index++;

        if (_columnsCount > 0)
        {
//            row = (int) (index / _columnsCount) + 1;
            row = (int) (index / _columnsCount);
            column = index % _columnsCount;
//            if (column == 0) column = _columnsCount;
//            if (column == 0) row = row - 1;
        }
        else
        {
            column = (int) (index / _rowsCount);
            row = index % _rowsCount;
//            if (row == 0) row = _rowsCount;
//            if (row == 0) column = column - 1;
        }

        var position = new Vector2(_cellSizeWithPadding.x * column, _cellSizeWithPadding.y * row);
//        var position = new Vector3(_cellSizeWithPadding.x * column, _cellSizeWithPadding.y * row, 0f);

        spawnable.transform.position = position;
        
        
        Debug.Log("spawnable.transform.position      ::::   "+spawnable.transform.position);

    }
}
