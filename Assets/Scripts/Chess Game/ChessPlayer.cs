using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChessPlayer 
{
    public TeamColor Team { get; set; }
    public Board Board { get; set; }
    public List<Piece> ActivePieces { get; private set; }

    public ChessPlayer(TeamColor team, Board board)
    {
        Team = team;
        Board = board;
        ActivePieces = new List<Piece>();
    }

    public void AddPiece(Piece piece)
    {
        if(!ActivePieces.Contains(piece))
            ActivePieces.Add(piece);
    }

    public void RemovePiece(Piece piece)
    {
        if(ActivePieces.Contains(piece))
            ActivePieces.Remove(piece);
    }

    public void GenerateAllPossibleMoves()
    {
        foreach (var piece in ActivePieces)
        {
            if (Board.HasPiece(piece))
                piece.SelectAvailableSquares();
        }
    }

    public Piece[] GetPieceAttackingOppositePieceOfType<T>() where T: Piece
    {
        return ActivePieces.Where(p => p.IsAttackingPieceOfType<King>()).ToArray();
    }

    public Piece[] GetPieceOfType<T>() where T: Piece
    {
        return ActivePieces.Where(p => p is T).ToArray();
    }

    public void RemoveMovesEnablingAttackOnPiece<T>(ChessPlayer opponent, Piece selectedPiece) where T: Piece
    {
        List<Vector2Int> coordToRemove = new List<Vector2Int>();
        foreach (var coors in selectedPiece.AvaliableMoves)
        {
            Piece pieceOnSquare = Board.GetPieceOnSquare(coors);
            Board.UpdateBoardOnPieceMove(coors, selectedPiece.OccupiedSquare, selectedPiece, null);
            opponent.GenerateAllPossibleMoves();
            if (opponent.CheckIsAttackingPiece<T>())
                coordToRemove.Add(coors);
            Board.UpdateBoardOnPieceMove(selectedPiece.OccupiedSquare, coors, selectedPiece, pieceOnSquare);
        }

        foreach (var coords in coordToRemove)
        {
            selectedPiece.AvaliableMoves.Remove(coords);
        }
    }

    private bool CheckIsAttackingPiece<T>() where T: Piece
    {
        return ActivePieces.Any(piece => Board.HasPiece(piece) && piece.IsAttackingPieceOfType<T>());
    }

    public bool CanHidePieceFromAttack<T>(ChessPlayer opponent) where T: Piece
    {
        foreach (var piece in ActivePieces)
        {
            foreach (var coords  in piece.AvaliableMoves)
            {
                Piece pieceOnCoords = Board.GetPieceOnSquare(coords);
                Board.UpdateBoardOnPieceMove(coords, piece.OccupiedSquare, piece, null);
                opponent.GenerateAllPossibleMoves();
                if (!opponent.CheckIsAttackingPiece<T>())
                {
                    Board.UpdateBoardOnPieceMove(piece.OccupiedSquare, coords, piece, pieceOnCoords);
                    return true;
                }
                Board.UpdateBoardOnPieceMove(piece.OccupiedSquare, coords, piece, pieceOnCoords);
            }
        }

        return false;
    }

    public void OnGameRestarted()
    {
        ActivePieces.Clear();
    }
}
