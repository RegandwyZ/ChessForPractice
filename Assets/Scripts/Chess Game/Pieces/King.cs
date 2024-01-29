using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Piece
{
    
    private readonly Vector2Int[] _direction = new Vector2Int[]
    {
        new Vector2Int(-1, 1),
        new Vector2Int(0, 1),
        new Vector2Int(1, 1),
        new Vector2Int(-1, 0),
        new Vector2Int(1, 0),
        new Vector2Int(-1, -1),
        new Vector2Int(0, -1),
        new Vector2Int(1, -1),
        
    };

    private Vector2Int _leftCastlingMove;
    private Vector2Int _rightCastlingMove;
    
    private Piece _leftRook;
    private Piece _rightRook;

    public override List<Vector2Int> SelectAvailableSquares()
    {
        AvaliableMoves.Clear();
        AssignStandardMoves();
        AssignCastlingMoves();

        return AvaliableMoves;
    }

    private void AssignCastlingMoves()
    {
        if(HasMoved)
            return;

        _leftRook = GetPieceInDirection<Rook>(Team, Vector2Int.left);
        if (_leftRook && !_leftRook.HasMoved)
        {
            _leftCastlingMove = OccupiedSquare + Vector2Int.left * 2;
            AvaliableMoves.Add(_leftCastlingMove);
        }
        
        _rightRook = GetPieceInDirection<Rook>(Team, Vector2Int.right);
        if (_rightRook && !_rightRook.HasMoved)
        {
            _rightCastlingMove = OccupiedSquare + Vector2Int.right * 2;
            AvaliableMoves.Add(_rightCastlingMove);
        }
    }
    

    private void AssignStandardMoves()
    {
        float range = 1;
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
    }

    public override void MovePiece(Vector2Int coords)
    {
        base.MovePiece(coords);
        if (coords == _leftCastlingMove)
        {
            Board.UpdateBoardOnPieceMove(coords + Vector2Int.right, _leftRook.OccupiedSquare, _leftRook, null);
            _leftRook.MovePiece(coords + Vector2Int.right);
        }
        else if (coords == _rightCastlingMove)
        {
            Board.UpdateBoardOnPieceMove(coords + Vector2Int.left, _rightRook.OccupiedSquare, _rightRook, null);
            _rightRook.MovePiece(coords + Vector2Int.left);
        }
    }
}
