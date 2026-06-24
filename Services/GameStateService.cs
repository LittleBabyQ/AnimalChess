using AnimalChess.Models;

namespace AnimalChess.Services;

public sealed class GameStateService
{
    private readonly GameRuleService _rules;
    private Board _board = new();
    private readonly Stack<MoveRecord> _history = new();
    private PlayerSide _currentTurn = PlayerSide.Blue;
    private Piece? _selectedPiece;
    private IReadOnlyList<BoardPosition> _legalMoves = Array.Empty<BoardPosition>();
    private PlayerSide? _winner;

    public GameStateService(GameRuleService rules, GameSettings? settings = null)
    {
        _rules = rules;
        Settings = settings ?? new GameSettings();
    }

    public GameSettings Settings { get; }

    public GameStateSnapshot Snapshot => new(_board, _currentTurn, _selectedPiece, _legalMoves, _winner, _history.Count);

    public void HandleCellClick(BoardPosition position)
    {
        if (_winner is not null)
        {
            return;
        }

        var clickedPiece = _board.GetPiece(position);
        if (clickedPiece?.Side == _currentTurn)
        {
            SelectPiece(clickedPiece);
            return;
        }

        if (_selectedPiece is not null && _legalMoves.Contains(position))
        {
            MoveSelectedPiece(position);
            return;
        }

        ClearSelection();
    }

    public bool TryMovePiece(Piece piece, BoardPosition targetPosition)
    {
        if (_winner is not null || piece.Side != _currentTurn)
        {
            return false;
        }

        var legalMoves = _rules.GetLegalMoves(_board, piece);
        if (!legalMoves.Contains(targetPosition))
        {
            return false;
        }

        _selectedPiece = piece;
        _legalMoves = legalMoves;
        MoveSelectedPiece(targetPosition);
        return true;
    }

    public bool IsAiTurn()
    {
        return Settings.Mode == GameMode.PlayerVsAi
            && Settings.AiDifficulty is AiDifficulty.Easy or AiDifficulty.Medium
            && _winner is null
            && _currentTurn != Settings.HumanSide;
    }

    public void ClearSelectionForAi()
    {
        ClearSelection();
    }

    public void Restart()
    {
        _board = new Board();
        _history.Clear();
        _currentTurn = PlayerSide.Blue;
        _selectedPiece = null;
        _legalMoves = Array.Empty<BoardPosition>();
        _winner = null;
    }

    public void Undo()
    {
        if (_history.Count == 0)
        {
            return;
        }

        var record = _history.Pop();
        var toCell = _board.GetCell(record.To);
        var fromCell = _board.GetCell(record.From);
        var movedPiece = toCell.Piece;
        if (movedPiece is null)
        {
            return;
        }

        toCell.Piece = null;
        movedPiece.Position = record.From;
        fromCell.Piece = movedPiece;

        if (record.CapturedPiece is not null)
        {
            var restored = record.CapturedPiece.Clone();
            restored.Position = record.To;
            toCell.Piece = restored;
            _board.Pieces.Add(restored);
        }

        _currentTurn = record.PreviousTurn;
        _winner = null;
        ClearSelection();
    }

    public string CapturedPieces(PlayerSide side, string emptyText = "None")
    {
        var active = _board.ActivePieces(side).ToHashSet();
        var captured = _board.Pieces
            .Where(piece => piece.Side == side && !active.Contains(piece))
            .Select(piece => piece.ChineseName);
        var text = string.Join(" ", captured);
        return string.IsNullOrWhiteSpace(text) ? emptyText : text;
    }

    private void SelectPiece(Piece piece)
    {
        _selectedPiece = piece;
        _legalMoves = _rules.GetLegalMoves(_board, piece);
    }

    private void MoveSelectedPiece(BoardPosition targetPosition)
    {
        if (_selectedPiece is null)
        {
            return;
        }

        var from = _selectedPiece.Position;
        var fromCell = _board.GetCell(from);
        var targetCell = _board.GetCell(targetPosition);
        var capturedPiece = targetCell.Piece;

        _history.Push(new MoveRecord(from, targetPosition, _selectedPiece, capturedPiece, _currentTurn));

        fromCell.Piece = null;
        targetCell.Piece = _selectedPiece;
        _selectedPiece.Position = targetPosition;

        if (_rules.IsWinner(_board, _currentTurn))
        {
            _winner = _currentTurn;
        }
        else
        {
            _currentTurn = _currentTurn == PlayerSide.Blue ? PlayerSide.Red : PlayerSide.Blue;
        }

        ClearSelection();
    }

    private void ClearSelection()
    {
        _selectedPiece = null;
        _legalMoves = Array.Empty<BoardPosition>();
    }
}
