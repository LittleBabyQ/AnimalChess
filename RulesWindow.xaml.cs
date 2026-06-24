using System.Diagnostics;
using System.Windows;
using AnimalChess.Services;

namespace AnimalChess;

public partial class RulesWindow : Window
{
    private const string RulesWebsiteUrl = "https://LittleBabyQ.github.io/AnimalChess/";
    private readonly LocalizationService _localization;

    public RulesWindow(LocalizationService localization)
    {
        InitializeComponent();
        _localization = localization;
        ApplyLocalization();
    }

    private void ApplyLocalization()
    {
        Title = _localization.Text("RulesWindowTitle");
        TitleText.Text = _localization.Text("RulesWindowTitle");
        SubtitleText.Text = _localization.Text("RulesWindowSubtitle");
        RulesDetailText.Text = _localization.Text("RulesDetail");
        OpenWebsiteButton.Content = _localization.Text("OpenRulesWebsite");
        CloseButton.Content = _localization.Text("Close");
    }

    private void OpenWebsiteButton_Click(object sender, RoutedEventArgs e)
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = RulesWebsiteUrl,
            UseShellExecute = true
        });
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
