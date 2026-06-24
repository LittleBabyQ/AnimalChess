namespace AnimalChess.Models;

public readonly record struct BoardPosition(int Row, int Col)
{
    public bool IsValid => Row is >= 0 and < Board.Rows && Col is >= 0 and < Board.Columns;
}
