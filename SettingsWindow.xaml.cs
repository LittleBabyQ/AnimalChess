using System.Windows;
using System.Windows.Controls;
using AnimalChess.Models;
using AnimalChess.Services;

namespace AnimalChess;

public partial class SettingsWindow : Window
{
    private readonly LocalizationService _localization;
    private bool _isInitializing;

    public SettingsWindow(LocalizationService localization, GameSettings settings, ThemeOption selectedTheme)
    {
        InitializeComponent();
        _localization = localization;
        SelectedLanguage = settings.Language;
        SelectedTheme = selectedTheme;
        SelectedGameMode = settings.Mode;
        SelectedAiDifficulty = settings.AiDifficulty == AiDifficulty.Hard ? AiDifficulty.Medium : settings.AiDifficulty;
        InitializeOptions(settings.Language, selectedTheme, settings.Mode, SelectedAiDifficulty);
        ApplyLocalization();
    }

    public string SelectedLanguage { get; private set; }

    public ThemeOption SelectedTheme { get; private set; }

    public GameMode SelectedGameMode { get; private set; }

    public AiDifficulty SelectedAiDifficulty { get; private set; }

    private void InitializeOptions(string selectedLanguage, ThemeOption selectedTheme, GameMode selectedGameMode, AiDifficulty selectedAiDifficulty)
    {
        _isInitializing = true;

        for (var index = 0; index < LanguageComboBox.Items.Count; index++)
        {
            if (LanguageComboBox.Items[index] is ComboBoxItem { Tag: string language } && language == selectedLanguage)
            {
                LanguageComboBox.SelectedIndex = index;
                break;
            }
        }

        RebuildThemeOptions(selectedTheme);
        RebuildGameModeOptions(selectedGameMode);
        RebuildAiDifficultyOptions(selectedAiDifficulty);
        AiDifficultyComboBox.IsEnabled = selectedGameMode == GameMode.PlayerVsAi;
        _isInitializing = false;
    }

    private void RebuildThemeOptions(ThemeOption selectedTheme)
    {
        ThemeComboBox.Items.Clear();
        var selectedIndex = 0;
        for (var index = 0; index < ThemeOption.All.Count; index++)
        {
            var theme = ThemeOption.All[index];
            ThemeComboBox.Items.Add(new ComboBoxItem
            {
                Content = _localization.Text(theme.DisplayKey),
                Tag = theme
            });

            if (theme.Key == selectedTheme.Key)
            {
                selectedIndex = index;
            }
        }

        ThemeComboBox.SelectedIndex = selectedIndex;
    }

    private void RebuildGameModeOptions(GameMode selectedGameMode)
    {
        GameModeComboBox.Items.Clear();
        var options = new[]
        {
            (Mode: GameMode.LocalTwoPlayers, Key: "ModeLocalTwoPlayers"),
            (Mode: GameMode.PlayerVsAi, Key: "ModePlayerVsAi")
        };

        var selectedIndex = 0;
        for (var index = 0; index < options.Length; index++)
        {
            GameModeComboBox.Items.Add(new ComboBoxItem
            {
                Content = _localization.Text(options[index].Key),
                Tag = options[index].Mode
            });

            if (options[index].Mode == selectedGameMode)
            {
                selectedIndex = index;
            }
        }

        GameModeComboBox.SelectedIndex = selectedIndex;
    }

    private void RebuildAiDifficultyOptions(AiDifficulty selectedDifficulty)
    {
        AiDifficultyComboBox.Items.Clear();
        var options = new[]
        {
            (Difficulty: AiDifficulty.Easy, Key: "AiEasy"),
            (Difficulty: AiDifficulty.Medium, Key: "AiMedium")
        };

        var selectedIndex = 0;
        for (var index = 0; index < options.Length; index++)
        {
            AiDifficultyComboBox.Items.Add(new ComboBoxItem
            {
                Content = _localization.Text(options[index].Key),
                Tag = options[index].Difficulty
            });

            if (options[index].Difficulty == selectedDifficulty)
            {
                selectedIndex = index;
            }
        }

        AiDifficultyComboBox.SelectedIndex = selectedIndex;
    }

    private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (_isInitializing || LanguageComboBox.SelectedItem is not ComboBoxItem { Tag: string language })
        {
            return;
        }

        SelectedLanguage = language;
        _localization.SetLanguage(language);
        ApplyLocalization();
        RebuildThemeOptions(SelectedTheme);
        RebuildGameModeOptions(SelectedGameMode);
        RebuildAiDifficultyOptions(SelectedAiDifficulty);
    }

    private void GameModeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (_isInitializing || GameModeComboBox.SelectedItem is not ComboBoxItem { Tag: GameMode mode })
        {
            return;
        }

        SelectedGameMode = mode;
        AiDifficultyComboBox.IsEnabled = mode == GameMode.PlayerVsAi;
    }

    private void ApplyLocalization()
    {
        Title = _localization.Text("SettingsTitle");
        TitleText.Text = _localization.Text("SettingsTitle");
        SubtitleText.Text = _localization.Text("SettingsSubtitle");
        LanguageLabel.Text = _localization.Text("Language");
        ThemeLabel.Text = _localization.Text("Theme");
        GameModeLabel.Text = _localization.Text("GameMode");
        AiDifficultyLabel.Text = _localization.Text("AiDifficulty");
        AiHintText.Text = _localization.Text("AiHint");
        CancelButton.Content = _localization.Text("Cancel");
        ApplyButton.Content = _localization.Text("Apply");
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
    }

    private void ApplyButton_Click(object sender, RoutedEventArgs e)
    {
        if (LanguageComboBox.SelectedItem is ComboBoxItem { Tag: string language })
        {
            SelectedLanguage = language;
        }

        if (ThemeComboBox.SelectedItem is ComboBoxItem { Tag: ThemeOption theme })
        {
            SelectedTheme = theme;
        }

        if (GameModeComboBox.SelectedItem is ComboBoxItem { Tag: GameMode mode })
        {
            SelectedGameMode = mode;
        }

        if (AiDifficultyComboBox.SelectedItem is ComboBoxItem { Tag: AiDifficulty difficulty })
        {
            SelectedAiDifficulty = difficulty;
        }

        if (SelectedGameMode == GameMode.LocalTwoPlayers)
        {
            SelectedAiDifficulty = AiDifficulty.None;
        }
        else if (SelectedAiDifficulty == AiDifficulty.None || SelectedAiDifficulty == AiDifficulty.Hard)
        {
            SelectedAiDifficulty = AiDifficulty.Easy;
        }

        DialogResult = true;
    }
}
