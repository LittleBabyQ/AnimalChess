using System.Windows;
using System.Windows.Controls;
using AnimalChess.Models;
using AnimalChess.Services;

namespace AnimalChess;

public partial class SettingsWindow : Window
{
    private readonly LocalizationService _localization;
    private bool _isInitializing;

    public SettingsWindow(LocalizationService localization, string selectedLanguage, ThemeOption selectedTheme)
    {
        InitializeComponent();
        _localization = localization;
        SelectedLanguage = selectedLanguage;
        SelectedTheme = selectedTheme;
        InitializeOptions(selectedLanguage, selectedTheme);
        ApplyLocalization();
    }

    public string SelectedLanguage { get; private set; }

    public ThemeOption SelectedTheme { get; private set; }

    private void InitializeOptions(string selectedLanguage, ThemeOption selectedTheme)
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
    }

    private void ApplyLocalization()
    {
        Title = _localization.Text("SettingsTitle");
        TitleText.Text = _localization.Text("SettingsTitle");
        SubtitleText.Text = _localization.Text("SettingsSubtitle");
        LanguageLabel.Text = _localization.Text("Language");
        ThemeLabel.Text = _localization.Text("Theme");
        AiTitle.Text = _localization.Text("AiOpponent");
        AiComingSoonText.Text = _localization.Text("AiComingSoon");
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

        DialogResult = true;
    }
}
