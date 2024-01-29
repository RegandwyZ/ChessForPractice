using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(IObjectTweener))]
[RequireComponent(typeof(MaterialSetter))]
public abstract class Piece : MonoBehaviour
{
    private MaterialSetter _materialSetter;
    private IObjectTweener _tweener;
    
    public Board Board { protected get; set; }
    public Vector2Int OccupiedSquare { get; set; }
    public TeamColor Team { get; set; }
    public bool HasMoved { get; private set; }
    public List<Vector2Int> AvaliableMoves;

    public abstract List<Vector2Int> SelectAvailableSquares();


    private void Awake()
    {
        AvaliableMoves = new List<Vector2Int>();
        _tweener = GetComponent<IObjectTweener>();
        _materialSetter = GetComponent<MaterialSetter>();
        HasMoved = false;
    }

    public void SetMaterial(Material material)
    {
        if (material == null)
            _materialSetter = GetComponent<MaterialSetter>();
        _materialSetter.SetSingleMaterial(material);
    }

    public bool IsFromSameTeam(Piece piece)
    {
        return Team == piece.Team;
    }

    public bool CanMoveTo(Vector2Int coords)
    {
        return AvaliableMoves.Contains(coords);
    }

    public virtual void MovePiece(Vector2Int coords)
    {
        Vector3 targetPosition = Board.CalculatePositionFromCoords(coords);
        OccupiedSquare = coords;
        HasMoved = true;
        _tweener.MoveTo(transform, targetPosition);
    }

    protected void TryToAddMove(Vector2Int coords)
    {
        AvaliableMoves.Add(coords);
    }

    public void SetData(Vector2Int coords, TeamColor team, Board board)
    {
        Team = team;
        OccupiedSquare = coords;
        Board = board;
        transform.position = board.CalculatePositionFromCoords(coords);
    }

    public bool IsAttackingPieceOfType<T>() where T: Piece
    {
        foreach (var square in AvaliableMoves)
        {
            if (Board.GetPieceOnSquare(square) is T)
                return true;
        }
        return false;
    }
    
    protected Piece GetPieceInDirection<T>(TeamColor team, Vector2Int direction) where T: Piece
    {
        for (int i = 1; i <= Board.BOARD_SIZE; i++)
        {
            Vector2Int nextCoords = OccupiedSquare + direction * i;
            Piece piece = Board.GetPieceOnSquare(nextCoords);
            if (!Board.CheckIfCoordinatesAreOnBoard(nextCoords))
                return null;
            
            if (piece != null)
            {
                if (piece.Team != team || piece is not T)
                    return null;
                if (piece.Team == team)
                    return piece;
            }
        }
        return null;
    }
}
