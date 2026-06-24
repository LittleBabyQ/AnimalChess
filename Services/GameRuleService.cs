using AnimalChess.Models;

namespace AnimalChess.Services;

public sealed class GameRuleService
{
    private static readonly (int Row, int Col)[] Directions =
    {
        (-1, 0),
        (1, 0),
        (0, -1),
        (0, 1)
    };

    public IReadOnlyList<BoardPosition> GetLegalMoves(Board board, Piece piece)
    {
        var moves = new List<BoardPosition>();

        foreach (var direction in Directions)
        {
            var next = new BoardPosition(piece.Position.Row + direction.Row, piece.Position.Col + direction.Col);
            if (!next.IsValid)
            {
                continue;
            }

            var targetCell = board.GetCell(next);
            if (CanMoveTo(board, piece, targetCell))
            {
                moves.Add(next);
                continue;
            }

            if (CanJumpRiver(board, piece, direction, out var landing))
            {
                moves.Add(landing);
            }
        }

        return moves;
    }

    public bool CanMoveTo(Board board, Piece piece, Cell targetCell)
    {
        if (targetCell.Terrain == TerrainType.Den && targetCell.Owner == piece.Side)
        {
            return false;
        }

        if (targetCell.Terrain == TerrainType.River && piece.Type != PieceType.Rat)
        {
            return false;
        }

        if (targetCell.Piece is null)
        {
            return true;
        }

        if (targetCell.Piece.Side == piece.Side)
        {
            return false;
        }

        return CanCapture(board, piece, targetCell.Piece, targetCell);
    }

    public bool CanCapture(Board board, Piece attacker, Piece defender, Cell defenderCell)
    {
        var attackerCell = board.GetCell(attacker.Position);
        if (attackerCell.Terrain == TerrainType.River || defenderCell.Terrain == TerrainType.River)
        {
            return attacker.Type == PieceType.Rat && defender.Type == PieceType.Rat;
        }

        if (attacker.Type == PieceType.Rat && defender.Type == PieceType.Elephant)
        {
            return true;
        }

        if (attacker.Type == PieceType.Elephant && defender.Type == PieceType.Rat)
        {
            return false;
        }

        return GetEffectiveRank(defender, defenderCell) <= GetEffectiveRank(attacker, attackerCell);
    }

    public bool IsWinner(Board board, PlayerSide side)
    {
        var opponent = side == PlayerSide.Blue ? PlayerSide.Red : PlayerSide.Blue;
        var opponentDen = FindDen(board, opponent);
        if (opponentDen.Piece?.Side == side)
        {
            return true;
        }

        return !board.ActivePieces(opponent).Any();
    }

    private static int GetEffectiveRank(Piece piece, Cell currentCell)
    {
        if (currentCell.Terrain == TerrainType.Trap && currentCell.Owner != piece.Side)
        {
            return 0;
        }

        return piece.Rank;
    }

    private static Cell FindDen(Board board, PlayerSide owner)
    {
        foreach (var cell in board.Cells)
        {
            if (cell.Terrain == TerrainType.Den && cell.Owner == owner)
            {
                return cell;
            }
        }

        throw new InvalidOperationException("Den not found.");
    }

    private static bool CanJumpRiver(Board board, Piece piece, (int Row, int Col) direction, out BoardPosition landing)
    {
        landing = default;
        if (piece.Type is not (PieceType.Lion or PieceType.Tiger))
        {
            return false;
        }

        var cursor = new BoardPosition(piece.Position.Row + direction.Row, piece.Position.Col + direction.Col);
        if (!cursor.IsValid || board.GetCell(cursor).Terrain != TerrainType.River)
        {
            return false;
        }

        while (cursor.IsValid && board.GetCell(cursor).Terrain == TerrainType.River)
        {
            if (board.GetCell(cursor).Piece?.Type == PieceType.Rat)
            {
                return false;
            }

            cursor = new BoardPosition(cursor.Row + direction.Row, cursor.Col + direction.Col);
        }

        if (!cursor.IsValid)
        {
            return false;
        }

        var landingCell = board.GetCell(cursor);
        if (landingCell.Piece?.Side == piece.Side)
        {
            return false;
        }

        if (landingCell.Piece is not null && !new GameRuleService().CanCapture(board, piece, landingCell.Piece, landingCell))
        {
            return false;
        }

        landing = cursor;
        return true;
    }
}
