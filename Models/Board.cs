namespace AnimalChess.Models;

public sealed class Board
{
    public const int Rows = 9;
    public const int Columns = 7;

    public Board()
    {
        Cells = new Cell[Rows, Columns];
        InitializeTerrain();
        InitializePieces();
    }

    public Cell[,] Cells { get; }

    public List<Piece> Pieces { get; } = new();

    public Cell GetCell(BoardPosition position) => Cells[position.Row, position.Col];

    public Piece? GetPiece(BoardPosition position) => GetCell(position).Piece;

    public IEnumerable<Piece> ActivePieces(PlayerSide side) => Pieces.Where(piece => piece.Side == side && GetPiece(piece.Position) == piece);

    private void InitializeTerrain()
    {
        for (var row = 0; row < Rows; row++)
        {
            for (var col = 0; col < Columns; col++)
            {
                var position = new BoardPosition(row, col);
                Cells[row, col] = new Cell(position, TerrainType.Land);
            }
        }

        SetTerrain(0, 3, TerrainType.Den, PlayerSide.Red);
        SetTerrain(8, 3, TerrainType.Den, PlayerSide.Blue);

        SetTerrain(0, 2, TerrainType.Trap, PlayerSide.Red);
        SetTerrain(0, 4, TerrainType.Trap, PlayerSide.Red);
        SetTerrain(1, 3, TerrainType.Trap, PlayerSide.Red);

        SetTerrain(8, 2, TerrainType.Trap, PlayerSide.Blue);
        SetTerrain(8, 4, TerrainType.Trap, PlayerSide.Blue);
        SetTerrain(7, 3, TerrainType.Trap, PlayerSide.Blue);

        for (var row = 3; row <= 5; row++)
        {
            SetTerrain(row, 1, TerrainType.River);
            SetTerrain(row, 2, TerrainType.River);
            SetTerrain(row, 4, TerrainType.River);
            SetTerrain(row, 5, TerrainType.River);
        }
    }

    private void InitializePieces()
    {
        AddPiece(PieceType.Lion, PlayerSide.Red, 0, 0);
        AddPiece(PieceType.Tiger, PlayerSide.Red, 0, 6);
        AddPiece(PieceType.Dog, PlayerSide.Red, 1, 1);
        AddPiece(PieceType.Cat, PlayerSide.Red, 1, 5);
        AddPiece(PieceType.Rat, PlayerSide.Red, 2, 0);
        AddPiece(PieceType.Leopard, PlayerSide.Red, 2, 2);
        AddPiece(PieceType.Wolf, PlayerSide.Red, 2, 4);
        AddPiece(PieceType.Elephant, PlayerSide.Red, 2, 6);

        AddPiece(PieceType.Elephant, PlayerSide.Blue, 6, 0);
        AddPiece(PieceType.Wolf, PlayerSide.Blue, 6, 2);
        AddPiece(PieceType.Leopard, PlayerSide.Blue, 6, 4);
        AddPiece(PieceType.Rat, PlayerSide.Blue, 6, 6);
        AddPiece(PieceType.Cat, PlayerSide.Blue, 7, 1);
        AddPiece(PieceType.Dog, PlayerSide.Blue, 7, 5);
        AddPiece(PieceType.Tiger, PlayerSide.Blue, 8, 0);
        AddPiece(PieceType.Lion, PlayerSide.Blue, 8, 6);
    }

    private void SetTerrain(int row, int col, TerrainType terrain, PlayerSide owner = PlayerSide.None)
    {
        Cells[row, col] = new Cell(new BoardPosition(row, col), terrain, owner);
    }

    private void AddPiece(PieceType type, PlayerSide side, int row, int col)
    {
        var position = new BoardPosition(row, col);
        var piece = new Piece(type, side, position);
        Pieces.Add(piece);
        Cells[row, col].Piece = piece;
    }
}
