using System.IO;
using System.Text.Json;
using AnimalChess.Models;

namespace AnimalChess.Services;

public sealed class SettingsStorageService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true
    };

    private readonly string _settingsPath;

    public SettingsStorageService()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var settingsDirectory = Path.Combine(appData, "AnimalChess");
        Directory.CreateDirectory(settingsDirectory);
        _settingsPath = Path.Combine(settingsDirectory, "settings.json");
    }

    public GameSettings Load()
    {
        if (!File.Exists(_settingsPath))
        {
            return new GameSettings();
        }

        try
        {
            var json = File.ReadAllText(_settingsPath);
            return JsonSerializer.Deserialize<GameSettings>(json) ?? new GameSettings();
        }
        catch
        {
            return new GameSettings();
        }
    }

    public void Save(GameSettings settings)
    {
        var json = JsonSerializer.Serialize(settings, JsonOptions);
        File.WriteAllText(_settingsPath, json);
    }
}
