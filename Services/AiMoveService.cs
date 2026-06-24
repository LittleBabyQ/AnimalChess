using AnimalChess.Models;

namespace AnimalChess.Services;

public readonly record struct AiMove(Piece Piece, BoardPosition Target);

public sealed class AiMoveService
{
    private readonly GameRuleService _rules;
    private readonly Random _random = new();

    public AiMoveService(GameRuleService rules)
    {
        _rules = rules;
    }

    public AiMove? ChooseMove(Board board, PlayerSide side, AiDifficulty difficulty)
    {
        var moves = GenerateMoves(board, side).ToList();
        if (moves.Count == 0)
        {
            return null;
        }

        return difficulty switch
        {
            AiDifficulty.Easy => moves[_random.Next(moves.Count)],
            AiDifficulty.Medium => ChooseMediumMove(board, side, moves),
            _ => null
        };
    }

    public IEnumerable<AiMove> GenerateMoves(Board board, PlayerSide side)
    {
        foreach (var piece in board.ActivePieces(side))
        {
            foreach (var target in _rules.GetLegalMoves(board, piece))
            {
                yield return new AiMove(piece, target);
            }
        }
    }

    private AiMove ChooseMediumMove(Board board, PlayerSide side, IReadOnlyList<AiMove> moves)
    {
        var bestScore = int.MinValue;
        var bestMoves = new List<AiMove>();

        foreach (var move in moves)
        {
            var score = ScoreMove(board, side, move);
            if (score > bestScore)
            {
                bestScore = score;
                bestMoves.Clear();
                bestMoves.Add(move);
            }
            else if (score == bestScore)
            {
                bestMoves.Add(move);
            }
        }

        return bestMoves[_random.Next(bestMoves.Count)];
    }

    private int ScoreMove(Board board, PlayerSide side, AiMove move)
    {
        var opponent = Opponent(side);
        var targetCell = board.GetCell(move.Target);
        var score = _random.Next(0, 8);

        if (targetCell.Terrain == TerrainType.Den && targetCell.Owner == opponent)
        {
            score += 100_000;
        }

        if (targetCell.Piece is not null && targetCell.Piece.Side == opponent)
        {
            score += 1_000 + targetCell.Piece.Rank * 160;
            if (move.Piece.Type == PieceType.Rat && targetCell.Piece.Type == PieceType.Elephant)
            {
                score += 900;
            }
        }

        score += ProgressTowardOpponentDen(board, side, move.Target) * 25;
        score += CenterControl(move.Target) * 8;

        if (targetCell.Terrain == TerrainType.Trap && targetCell.Owner == opponent)
        {
            score -= 500;
        }

        if (IsThreatenedAfterMove(board, side, move))
        {
            score -= 450 + move.Piece.Rank * 90;
        }

        if (move.Piece.Type is PieceType.Lion or PieceType.Tiger && Math.Abs(move.Target.Row - move.Piece.Position.Row) > 1)
        {
            score += 180;
        }

        if (move.Piece.Type == PieceType.Rat && targetCell.Terrain == TerrainType.River)
        {
            score += 80;
        }

        return score;
    }

    private bool IsThreatenedAfterMove(Board board, PlayerSide side, AiMove move)
    {
        var opponent = Opponent(side);
        var originalPosition = move.Piece.Position;
        var originalSourcePiece = board.GetCell(originalPosition).Piece;
        var targetCell = board.GetCell(move.Target);
        var capturedPiece = targetCell.Piece;

        board.GetCell(originalPosition).Piece = null;
        targetCell.Piece = move.Piece;
        move.Piece.Position = move.Target;

        var threatened = board.ActivePieces(opponent)
            .SelectMany(piece => _rules.GetLegalMoves(board, piece))
            .Any(position => position == move.Target);

        move.Piece.Position = originalPosition;
        board.GetCell(originalPosition).Piece = originalSourcePiece;
        targetCell.Piece = capturedPiece;

        return threatened;
    }

    private static int ProgressTowardOpponentDen(Board board, PlayerSide side, BoardPosition position)
    {
        var den = FindDen(board, Opponent(side));
        return Board.Rows + Board.Columns - Math.Abs(position.Row - den.Row) - Math.Abs(position.Col - den.Col);
    }

    private static int CenterControl(BoardPosition position)
    {
        return 6 - Math.Abs(position.Col - 3);
    }

    private static BoardPosition FindDen(Board board, PlayerSide owner)
    {
        foreach (var cell in board.Cells)
        {
            if (cell.Terrain == TerrainType.Den && cell.Owner == owner)
            {
                return cell.Position;
            }
        }

        return owner == PlayerSide.Blue ? new BoardPosition(8, 3) : new BoardPosition(0, 3);
    }

    private static PlayerSide Opponent(PlayerSide side) => side == PlayerSide.Blue ? PlayerSide.Red : PlayerSide.Blue;
}
