namespace AnimalChess.Models;

public sealed class Cell
{
    public Cell(BoardPosition position, TerrainType terrain, PlayerSide owner = PlayerSide.None)
    {
        Position = position;
        Terrain = terrain;
        Owner = owner;
    }

    public BoardPosition Position { get; }

    public TerrainType Terrain { get; }

    public PlayerSide Owner { get; }

    public Piece? Piece { get; set; }
}
