using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
    public override List<Vector2Int> SelectAvailableSquares()
    {
        AvaliableMoves.Clear();
        Vector2Int direction = Team == TeamColor.White ? Vector2Int.up : Vector2Int.down;
        float range = HasMoved ? 1 : 2;
        for (int i = 1; i <= range; i++)
        {
            Vector2Int nextCoords = OccupiedSquare + direction * i;
            Piece piece = Board.GetPieceOnSquare(nextCoords);
            if (!Board.CheckIfCoordinatesAreOnBoard(nextCoords))
            {
                break;
            }

            if (piece == null)
            {
                TryToAddMove(nextCoords);
            }
            else if(piece.IsFromSameTeam(this))
                break;
        }

        Vector2Int[] takeDirection = new Vector2Int[] { new Vector2Int(1, direction.y), new Vector2Int(-1, direction.y) };
        for (int i = 0; i < takeDirection.Length; i++)
        {
            Vector2Int nextCoords = OccupiedSquare + takeDirection[i];
            Piece piece = Board.GetPieceOnSquare(nextCoords);
            if (!Board.CheckIfCoordinatesAreOnBoard(nextCoords))
            {
                continue;
            }

            if (piece != null && !piece.IsFromSameTeam(this))
            {
                TryToAddMove(nextCoords);
            }
        }

        return AvaliableMoves;
    }

    public override void MovePiece(Vector2Int coords)
    {
        base.MovePiece(coords);
        CheckPromotion();
    }

    private void CheckPromotion()
    {
        int endOfBoardYCoord = Team == TeamColor.White ? Board.BOARD_SIZE - 1 : 0;
        if (OccupiedSquare.y == endOfBoardYCoord)
            Board.PromotePiece(this);
    }
}
