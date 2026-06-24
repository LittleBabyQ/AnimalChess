namespace AnimalChess.Models;

public sealed class Piece
{
    public Piece(PieceType type, PlayerSide side, BoardPosition position)
    {
        Type = type;
        Side = side;
        Position = position;
    }

    public PieceType Type { get; }

    public PlayerSide Side { get; }

    public BoardPosition Position { get; set; }

    public int Rank => Type switch
    {
        PieceType.Rat => 1,
        PieceType.Cat => 2,
        PieceType.Dog => 3,
        PieceType.Wolf => 4,
        PieceType.Leopard => 5,
        PieceType.Tiger => 6,
        PieceType.Lion => 7,
        PieceType.Elephant => 8,
        _ => 0
    };

    public string EnglishName => Type.ToString();

    public string ChineseName => Type switch
    {
        PieceType.Rat => "鼠",
        PieceType.Cat => "猫",
        PieceType.Dog => "狗",
        PieceType.Wolf => "狼",
        PieceType.Leopard => "豹",
        PieceType.Tiger => "虎",
        PieceType.Lion => "狮",
        PieceType.Elephant => "象",
        _ => string.Empty
    };

    public Piece Clone() => new(Type, Side, Position);
}
