namespace AnimalChess.Models;

public sealed class GameSettings
{
    public GameMode Mode { get; set; } = GameMode.LocalTwoPlayers;

    public AiDifficulty AiDifficulty { get; set; } = AiDifficulty.None;

    public PlayerSide HumanSide { get; set; } = PlayerSide.Blue;

    public string Language { get; set; } = "en-US";

    public string Theme { get; set; } = "DefaultMinimal";

    public bool ShowLegalMoves { get; set; } = true;
}
