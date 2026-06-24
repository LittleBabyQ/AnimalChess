namespace AnimalChess.Models;

public sealed class ThemeOption
{
    public ThemeOption(string key, string displayKey, string? backgroundFileName)
    {
        Key = key;
        DisplayKey = displayKey;
        BackgroundFileName = backgroundFileName;
    }

    public string Key { get; }

    public string DisplayKey { get; }

    public string? BackgroundFileName { get; }

    public bool UsesImage => !string.IsNullOrWhiteSpace(BackgroundFileName);

    public static IReadOnlyList<ThemeOption> All { get; } = new[]
    {
        new ThemeOption("DefaultMinimal", "ThemeDefaultMinimal", null),
        new ThemeOption("AnimeGirl", "ThemeAnimeGirl", "anime-girl.png"),
        new ThemeOption("AnimeJk", "ThemeAnimeJk", "anime-jk.png"),
        new ThemeOption("AnimeSunset", "ThemeAnimeSunset", "anime-sunset.png"),
        new ThemeOption("LittlePrince", "ThemeLittlePrince", "little-prince.png"),
        new ThemeOption("StarAndMoon", "ThemeStarAndMoon", "star-and-moon.png"),
        new ThemeOption("Sunflower", "ThemeSunflower", "sunflower.png")
    };
}
