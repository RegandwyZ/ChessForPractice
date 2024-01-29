using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



[RequireComponent(typeof(PieceCreator))]
public class ChessGameController : MonoBehaviour
{
    
    private enum GameState { Init, Play, Finished  }
    
    [SerializeField] private BoardLayout _startingBoardLayout;
    [SerializeField] private Board _board;
    [SerializeField] private ChessUIManager _uiManager;

    private ChessPlayer _whitePlayer;
    private ChessPlayer _blackPlayer;
    private ChessPlayer _activePlayer;
    private PieceCreator _pieceCreator;
   
    private GameState _state;

    private void Awake()
    {
        SetDependencies();
        CreatePlayers();
    }

    private void CreatePlayers()
    {
        _whitePlayer = new ChessPlayer(TeamColor.White, _board);
        _blackPlayer = new ChessPlayer(TeamColor.Black, _board);
    }

    private void SetDependencies()
    {
        _pieceCreator = GetComponent<PieceCreator>();
    }

    private void Start()
    {
        StartNewGame();
    }

    private void StartNewGame()
    {
        _uiManager.HideUI();
        SetGameState(GameState.Init);
        _board.SetDependencies(this);
        CreatePiecesFromLayout(_startingBoardLayout);
        _activePlayer = _whitePlayer;
        GenerateAllPossiblePlayerMoves(_activePlayer);
        SetGameState(GameState.Play);
    }

    public void RestartGame()
    {
        DestroyPieces();
        _board.OnGameRestarted();
        _whitePlayer.OnGameRestarted();
        _blackPlayer.OnGameRestarted();
        StartNewGame();
        
    }

    private void DestroyPieces()
    {
        _whitePlayer.ActivePieces.ForEach(p => Destroy(p.gameObject));
        _blackPlayer.ActivePieces.ForEach(p => Destroy(p.gameObject));
    }

    private void SetGameState(GameState state)
    {
        _state = state;
    }

    public bool IsGameInProgress()
    {
        return _state == GameState.Play;
    }


    private void CreatePiecesFromLayout(BoardLayout layout)
    {
        for (var i = 0; i < layout.GetPiecesCount(); i++)
        {
            var squareCoords = layout.GetSquareCoordsAtIndex(i);
            var teamColor = layout.GetSquareTeamColorAtIndex(i);
            var typeName = layout.GetSquarePieceNameAtIndex(i);

            var type = Type.GetType(typeName);
            CreatePieceAndInitialize(squareCoords, teamColor, type);
        }
    }

    public void CreatePieceAndInitialize(Vector2Int squareCoords, TeamColor teamColor, Type type)
    {
        var newPiece = _pieceCreator.CreatePiece(type).GetComponent<Piece>();
        newPiece.SetData(squareCoords, teamColor, _board);

        var teamMaterial = _pieceCreator.GetTeamMaterial(teamColor);
        newPiece.SetMaterial(teamMaterial);

        _board.SetPieceOnBoard(squareCoords, newPiece);
        
        ChessPlayer currentPlayer = teamColor == TeamColor.White ? _whitePlayer : _blackPlayer;
        currentPlayer.AddPiece(newPiece);
    }
    
    private void GenerateAllPossiblePlayerMoves(ChessPlayer player)
    {
        player.GenerateAllPossibleMoves();
    }

    public bool IsTeamTurnActive(TeamColor pieceTeam)
    {
        return _activePlayer.Team == pieceTeam;
    }

    public void EndTurn()
    {
       GenerateAllPossiblePlayerMoves(_activePlayer);
       GenerateAllPossiblePlayerMoves(GetOpponentToPlayer(_activePlayer));
       if (CheckIsGameIsFinished())
           EndGame();
       else
       {
           ChangeActiveTeam();
       }
       
    }

    private bool CheckIsGameIsFinished()
    {
        Piece[] kingAttackingPiece = _activePlayer.GetPieceAttackingOppositePieceOfType<King>();
        if (kingAttackingPiece.Length > 0)
        {
            ChessPlayer oppositePlayer = GetOpponentToPlayer(_activePlayer);
            Piece attackedKing = oppositePlayer.GetPieceOfType<King>().FirstOrDefault();
            oppositePlayer.RemoveMovesEnablingAttackOnPiece<King>(_activePlayer, attackedKing);

            int availableKingMoves = attackedKing.AvaliableMoves.Count;
            if (availableKingMoves == 0)
            {
                bool canCoverKing = oppositePlayer.CanHidePieceFromAttack<King>(_activePlayer);
                if (!canCoverKing)
                    return true;
            }
        }

        return false;
    }

    private void EndGame()
    {
        _uiManager.OnGameFinished(_activePlayer.Team.ToString());
        SetGameState(GameState.Finished);
    }

    private ChessPlayer GetOpponentToPlayer(ChessPlayer player)
    {
        return player == _whitePlayer ? _blackPlayer : _whitePlayer;
    }
    
    private void ChangeActiveTeam()
    {
        _activePlayer = _activePlayer == _whitePlayer ? _blackPlayer : _whitePlayer;
    }

    public void RemoveMovesEnablingAttackOnPieceOfType<T>(Piece piece) where T : Piece
    {
        _activePlayer.RemoveMovesEnablingAttackOnPiece<T>(GetOpponentToPlayer(_activePlayer), piece);
    }

    public void OnPieceRemoved(Piece piece)
    {
        ChessPlayer pieceOwner = (piece.Team == TeamColor.White) ? _whitePlayer : _blackPlayer;
        pieceOwner.RemovePiece(piece);
        Destroy(piece.gameObject);
    }
}
