using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece
{
    private readonly Vector2Int[] _offset = new Vector2Int[]
    {
        new Vector2Int(2, 1),
        new Vector2Int(2, -1),
        new Vector2Int(1, 2),
        new Vector2Int(1, -2),
        new Vector2Int(-2, 1),
        new Vector2Int(-2, -1),
        new Vector2Int(-1, 2),
        new Vector2Int(-1, -2),
        
    };
    public override List<Vector2Int> SelectAvailableSquares()
    {
        AvaliableMoves.Clear();
        for (int i = 0; i < _offset.Length; i++)
        {
            Vector2Int nextCoord = OccupiedSquare + _offset[i];
            Piece piece = Board.GetPieceOnSquare(nextCoord);
            if (!Board.CheckIfCoordinatesAreOnBoard(nextCoord))
            {
                continue;
            }

            if (piece == null || !piece.IsFromSameTeam(this))
            {
                TryToAddMove(nextCoord);
            }

        }

        return AvaliableMoves;
    }
}
