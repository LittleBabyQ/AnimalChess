namespace AnimalChess.Models;

public sealed class GameStateSnapshot
{
    public GameStateSnapshot(
        Board board,
        PlayerSide currentTurn,
        Piece? selectedPiece,
        IReadOnlyList<BoardPosition> legalMoves,
        PlayerSide? winner,
        int historyCount)
    {
        Board = board;
        CurrentTurn = currentTurn;
        SelectedPiece = selectedPiece;
        LegalMoves = legalMoves;
        Winner = winner;
        HistoryCount = historyCount;
    }

    public Board Board { get; }

    public PlayerSide CurrentTurn { get; }

    public Piece? SelectedPiece { get; }

    public IReadOnlyList<BoardPosition> LegalMoves { get; }

    public PlayerSide? Winner { get; }

    public int HistoryCount { get; }
}
