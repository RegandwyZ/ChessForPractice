using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Board/Layout")]
public class BoardLayout : ScriptableObject
{
    [Serializable]
    private class BoardSquareSetup
    {
        public Vector2Int Position;
        public PieceType PieceType;
        public TeamColor TeamColor;
    }

    [SerializeField] private BoardSquareSetup[] _boardSquares;

    public int GetPiecesCount()
    {
        return _boardSquares.Length;
    }

    public Vector2Int GetSquareCoordsAtIndex(int index)
    {
        if (_boardSquares.Length <= index)
        {
            return new Vector2Int(-1, -1);
        }
        
        return new Vector2Int(_boardSquares[index].Position.x - 1, _boardSquares[index].Position.y - 1);
    }

    public string GetSquarePieceNameAtIndex(int index)
    {
        if (_boardSquares.Length <= index)
        {
            return "";
        }

        return _boardSquares[index].PieceType.ToString();
    }

    public TeamColor GetSquareTeamColorAtIndex(int index)
    {
        if (_boardSquares.Length <= index)
        {
            return TeamColor.Black;
        }

        return _boardSquares[index].TeamColor;
    }
}
