using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Piece
{
    private readonly Vector2Int[] _direction = new Vector2Int[]
    {
        new Vector2Int(1, 1),
        new Vector2Int(1, -1),
        new Vector2Int(-1, 1),
        new Vector2Int(-1, -1)
    };
        
    public override List<Vector2Int> SelectAvailableSquares()
    {
        AvaliableMoves.Clear();
        float range = Board.BOARD_SIZE;
        foreach (var direction in _direction)
        {
            for (int i = 1; i <= range; i++)
            {
                Vector2Int nextCoords = OccupiedSquare + direction * i;
                Piece piece = Board.GetPieceOnSquare(nextCoords);
                if(!Board.CheckIfCoordinatesAreOnBoard(nextCoords))
                    break;
                if(piece == null)
                    TryToAddMove(nextCoords);
                else if (!piece.IsFromSameTeam(this))
                {
                    TryToAddMove(nextCoords);
                    break;
                }
                else if(piece.IsFromSameTeam(this))
                    break;
            }
        }

        return AvaliableMoves;
    }
}
