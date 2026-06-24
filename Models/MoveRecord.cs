namespace AnimalChess.Models;

public sealed class MoveRecord
{
    public MoveRecord(BoardPosition from, BoardPosition to, Piece movedPiece, Piece? capturedPiece, PlayerSide previousTurn)
    {
        From = from;
        To = to;
        MovedPiece = movedPiece.Clone();
        CapturedPiece = capturedPiece?.Clone();
        PreviousTurn = previousTurn;
    }

    public BoardPosition From { get; }

    public BoardPosition To { get; }

    public Piece MovedPiece { get; }

    public Piece? CapturedPiece { get; }

    public PlayerSide PreviousTurn { get; }
}
