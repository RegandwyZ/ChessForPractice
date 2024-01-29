using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SquareSelectorCreator))]
public class Board : MonoBehaviour
{

    public const int BOARD_SIZE = 8;
    
    [SerializeField] private Transform _bottomLeftSquareTransform;
    [SerializeField] private float _squareSize;

    private Piece[,] _grid;
    private Piece _selectedPiece;

    private ChessGameController _chessController;
    private SquareSelectorCreator _squareSelector;

    private void Awake()
    {
        _squareSelector = GetComponent<SquareSelectorCreator>();
        CreateGrid();
    }

    public void SetDependencies(ChessGameController chessController)
    {
        _chessController = chessController;
    }

    private void CreateGrid()
    {
        _grid = new Piece[BOARD_SIZE, BOARD_SIZE];
    }

    public Vector3 CalculatePositionFromCoords(Vector2Int coords)
    {
        return _bottomLeftSquareTransform.position + new Vector3(coords.x * _squareSize, 0f, coords.y * _squareSize);
    }

    public bool HasPiece(Piece piece)
    {
        for (var i = 0; i < BOARD_SIZE; i++)
        {
            for (var j = 0; j < BOARD_SIZE; j++)
            {
                if (_grid[i, j] == piece)
                    return true;
            }
        }

        return false;
    }

    public void OnSquareSelected(Vector3 inputPosition)
    {
        if(!_chessController.IsGameInProgress())
            return;
        Vector2Int coords = CalculateCoordsFromPosition(inputPosition);
        Piece piece = GetPieceOnSquare(coords);
        if (_selectedPiece )
        {
            if (piece != null && _selectedPiece == piece)
                DeselectPiece();
            else if (piece != null && _selectedPiece != piece && _chessController.IsTeamTurnActive(piece.Team))
                SelectedPiece(piece);
            else if (_selectedPiece.CanMoveTo(coords))
                OnSelectedPieceMoved(coords, _selectedPiece);
        }
        else
        {
            
            if (piece != null  && _chessController.IsTeamTurnActive(piece.Team))
            {
                SelectedPiece(piece);
            }
        }
    }

    private void OnSelectedPieceMoved(Vector2Int coord, Piece piece)
    {
        TryToTakeOppositePiece(coord);
        UpdateBoardOnPieceMove(coord, piece.OccupiedSquare, piece, null);
        _selectedPiece.MovePiece(coord);
        DeselectPiece();
        EndTurn();
    }

    private void TryToTakeOppositePiece(Vector2Int coord)
    {
        Piece piece = GetPieceOnSquare(coord);
        if (piece != null && !_selectedPiece.IsFromSameTeam(piece))
            TakePiece(piece);
    }

    private void TakePiece(Piece piece)
    {
        if (piece)
        {
            _grid[piece.OccupiedSquare.x, piece.OccupiedSquare.y] = null;
            _chessController.OnPieceRemoved(piece);
        }
    }

    private void EndTurn()
    {
        _chessController.EndTurn();
    }

    public void UpdateBoardOnPieceMove(Vector2Int newCoords, Vector2Int oldCoords, Piece newPiece, Piece oldPiece)
    {
        _grid[oldCoords.x, oldCoords.y] = oldPiece;
        _grid[newCoords.x, newCoords.y] = newPiece;
    }

    private void SelectedPiece(Piece piece)
    {
        _chessController.RemoveMovesEnablingAttackOnPieceOfType<King>(piece);
        _selectedPiece = piece;
        List<Vector2Int> selection = _selectedPiece.AvaliableMoves;
        ShowSelectionSquare(selection);
        
    }

    private void ShowSelectionSquare(List<Vector2Int> selection)
    {
        Dictionary<Vector3, bool> squareData = new Dictionary<Vector3, bool>();
        for (int i = 0; i < selection.Count; i++)
        {
            Vector3 position = CalculatePositionFromCoords(selection[i]);
            bool isSquareFree = GetPieceOnSquare(selection[i]) == null;
            squareData.Add(position,isSquareFree);
        }
        _squareSelector.ShowSelection(squareData);
    }

    private void DeselectPiece()
    {
        _selectedPiece = null; 
        _squareSelector.ClearSelection();
    }

    public Piece GetPieceOnSquare(Vector2Int coord)
    {
        if (CheckIfCoordinatesAreOnBoard(coord))
            return _grid[coord.x, coord.y];
        return null;
    }

    public bool CheckIfCoordinatesAreOnBoard(Vector2Int coords)
    {
        if (coords.x < 0 || coords.y < 0 || coords.x >= BOARD_SIZE || coords.y >= BOARD_SIZE)
            return false;
        return true;
    }

    private Vector2Int CalculateCoordsFromPosition(Vector3 inputPosition)
    {
        int x = Mathf.RoundToInt(transform.InverseTransformPoint(inputPosition).x / _squareSize);
        int y = Mathf.RoundToInt(transform.InverseTransformPoint(inputPosition).z / _squareSize);
        Vector2Int coordinates = new Vector2Int(x, y);

        return coordinates;
    }

    public void SetPieceOnBoard(Vector2Int coords, Piece piece)
    {
        if (CheckIfCoordinatesAreOnBoard(coords))
            _grid[coords.x, coords.y] = piece;

    }

    public void OnGameRestarted()
    {
        _selectedPiece = null;
        CreateGrid();
    }

    public void PromotePiece(Piece piece)
    {
        TakePiece(piece);
        _chessController.CreatePieceAndInitialize(piece.OccupiedSquare, piece.Team, typeof(Queen));
    }
}
