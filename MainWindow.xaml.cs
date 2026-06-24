using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AnimalChess.Models;
using AnimalChess.Services;

namespace AnimalChess;

public partial class MainWindow : Window
{
    private readonly GameRuleService _rules = new();
    private readonly LocalizationService _localization = new();
    private readonly SettingsStorageService _settingsStorage = new();
    private readonly AiMoveService _ai;
    private readonly GameStateService _game;
    private PlayerSide? _lastAnnouncedWinner;
    private ThemeOption _currentTheme = ThemeOption.All[0];

    public MainWindow()
    {
        InitializeComponent();
        var savedSettings = _settingsStorage.Load();
        _game = new GameStateService(_rules, savedSettings);
        _ai = new AiMoveService(_rules);
        _currentTheme = FindThemeByKey(savedSettings.Theme);
        _localization.SetLanguage(savedSettings.Language);
        InitializeThemeOptions();
        SelectLanguageInMainComboBox(savedSettings.Language);
        SelectCurrentThemeInMainComboBox();
        ApplyLocalization();
        ApplyTheme(_currentTheme);
        RenderBoard();
        UpdateStatus();
        PlayAiTurnIfNeeded();
    }

    private void InitializeThemeOptions()
    {
        ThemeComboBox.Items.Clear();
        foreach (var theme in ThemeOption.All)
        {
            ThemeComboBox.Items.Add(new ComboBoxItem
            {
                Content = _localization.Text(theme.DisplayKey),
                Tag = theme
            });
        }
    }

    private void RefreshThemeOptionLabels()
    {
        foreach (ComboBoxItem item in ThemeComboBox.Items)
        {
            if (item.Tag is ThemeOption theme)
            {
                item.Content = _localization.Text(theme.DisplayKey);
            }
        }
    }

    private void RenderBoard()
    {
        var state = _game.Snapshot;
        BoardGrid.Children.Clear();
        for (var row = 0; row < Board.Rows; row++)
        {
            for (var col = 0; col < Board.Columns; col++)
            {
                var position = new BoardPosition(row, col);
                var cell = state.Board.GetCell(position);
                var button = CreateCellButton(cell, state);
                BoardGrid.Children.Add(button);
            }
        }
    }

    private Button CreateCellButton(Cell cell, GameStateSnapshot state)
    {
        var isLegalMove = state.LegalMoves.Contains(cell.Position);
        var isSelected = state.SelectedPiece?.Position == cell.Position;
        var button = new Button
        {
            Margin = new Thickness(4),
            Padding = new Thickness(4),
            BorderThickness = new Thickness(isSelected ? 3 : isLegalMove ? 2 : 1),
            BorderBrush = isSelected
                ? Brushes.White
                : isLegalMove
                    ? new SolidColorBrush(Color.FromRgb(34, 211, 238))
                    : new SolidColorBrush(Color.FromRgb(55, 65, 81)),
            Background = GetCellBrush(cell),
            Cursor = _game.IsAiTurn() ? Cursors.Arrow : Cursors.Hand,
            IsEnabled = !_game.IsAiTurn(),
            Tag = cell.Position
        };
        button.Click += CellButton_Click;

        var panel = new Grid();
        if (cell.Piece is not null)
        {
            panel.Children.Add(CreatePieceView(cell.Piece));
            button.ToolTip = CreatePieceTooltip(cell.Piece);
        }
        else
        {
            panel.Children.Add(CreateTerrainLabel(cell));
            button.ToolTip = cell.Terrain == TerrainType.Land ? null : cell.Terrain.ToString();
        }

        button.Content = panel;
        return button;
    }

    private static UIElement CreatePieceView(Piece piece)
    {
        var border = new Border
        {
            CornerRadius = new CornerRadius(16),
            Background = piece.Side == PlayerSide.Blue
                ? new SolidColorBrush(Color.FromArgb(190, 37, 99, 235))
                : new SolidColorBrush(Color.FromArgb(190, 220, 38, 38)),
            BorderBrush = piece.Side == PlayerSide.Blue
                ? new SolidColorBrush(Color.FromRgb(147, 197, 253))
                : new SolidColorBrush(Color.FromRgb(252, 165, 165)),
            BorderThickness = new Thickness(2),
            Padding = new Thickness(7)
        };

        var image = new Image
        {
            Source = LoadAnimalImage(piece.Type),
            Stretch = Stretch.Uniform,
            SnapsToDevicePixels = true,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };

        border.Child = image;
        return border;
    }

    private static BitmapImage LoadAnimalImage(PieceType type)
    {
        var fileName = type.ToString().ToLowerInvariant();
        return new BitmapImage(new Uri($"pack://application:,,,/Assets/Animals/{fileName}.png", UriKind.Absolute));
    }

    private string CreatePieceTooltip(Piece piece)
    {
        return $"{_localization.SideName(piece.Side)}\n{piece.EnglishName} / {piece.ChineseName}\n{_localization.Text("Rank")} {piece.Rank}";
    }

    private static UIElement CreateTerrainLabel(Cell cell)
    {
        var text = cell.Terrain switch
        {
            TerrainType.River => "河",
            TerrainType.Trap => "陷",
            TerrainType.Den => "穴",
            _ => string.Empty
        };

        return new TextBlock
        {
            Text = text,
            Foreground = cell.Terrain == TerrainType.Land
                ? Brushes.Transparent
                : new SolidColorBrush(Color.FromRgb(243, 244, 246)),
            FontSize = 18,
            FontWeight = FontWeights.SemiBold,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };
    }

    private static Brush GetCellBrush(Cell cell)
    {
        return cell.Terrain switch
        {
            TerrainType.River => new SolidColorBrush(Color.FromArgb(130, 30, 64, 175)),
            TerrainType.Trap => new SolidColorBrush(Color.FromArgb(130, 127, 29, 29)),
            TerrainType.Den => new SolidColorBrush(Color.FromArgb(150, 146, 64, 14)),
            _ => new SolidColorBrush(Color.FromArgb(95, 31, 41, 55))
        };
    }

    private void CellButton_Click(object sender, RoutedEventArgs e)
    {
        if (_game.IsAiTurn() || sender is not Button { Tag: BoardPosition position })
        {
            return;
        }

        _game.HandleCellClick(position);
        RenderBoard();
        UpdateStatus();
        ShowWinnerDialogIfNeeded();
        PlayAiTurnIfNeeded();
    }

    private void UndoButton_Click(object sender, RoutedEventArgs e)
    {
        _game.Undo();
        _lastAnnouncedWinner = null;
        RenderBoard();
        UpdateStatus();
        PlayAiTurnIfNeeded();
    }

    private void RestartButton_Click(object sender, RoutedEventArgs e)
    {
        _game.Restart();
        _lastAnnouncedWinner = null;
        RenderBoard();
        UpdateStatus();
        PlayAiTurnIfNeeded();
    }

    private void SettingsButton_Click(object sender, RoutedEventArgs e)
    {
        var settingsWindow = new SettingsWindow(_localization, _game.Settings, _currentTheme)
        {
            Owner = this
        };

        if (settingsWindow.ShowDialog() != true)
        {
            _localization.SetLanguage(_game.Settings.Language);
            return;
        }

        _localization.SetLanguage(settingsWindow.SelectedLanguage);
        _game.Settings.Language = settingsWindow.SelectedLanguage;
        _currentTheme = settingsWindow.SelectedTheme;
        _game.Settings.Theme = _currentTheme.Key;
        _game.Settings.Mode = settingsWindow.SelectedGameMode;
        _game.Settings.AiDifficulty = settingsWindow.SelectedAiDifficulty;
        _game.Settings.HumanSide = PlayerSide.Blue;

        ApplyLocalization();
        RefreshThemeOptionLabels();
        SelectLanguageInMainComboBox(_game.Settings.Language);
        SelectCurrentThemeInMainComboBox();
        ApplyTheme(_currentTheme);
        _settingsStorage.Save(_game.Settings);
        RenderBoard();
        UpdateStatus();
        PlayAiTurnIfNeeded();
    }

    private void RulesButton_Click(object sender, RoutedEventArgs e)
    {
        var rulesWindow = new RulesWindow(_localization)
        {
            Owner = this
        };

        rulesWindow.ShowDialog();
    }

    private void SelectCurrentThemeInMainComboBox()
    {
        for (var index = 0; index < ThemeComboBox.Items.Count; index++)
        {
            if (ThemeComboBox.Items[index] is ComboBoxItem { Tag: ThemeOption theme } && theme.Key == _currentTheme.Key)
            {
                ThemeComboBox.SelectedIndex = index;
                return;
            }
        }
    }

    private void SelectLanguageInMainComboBox(string language)
    {
        for (var index = 0; index < LanguageComboBox.Items.Count; index++)
        {
            if (LanguageComboBox.Items[index] is ComboBoxItem { Tag: string itemLanguage } && itemLanguage == language)
            {
                LanguageComboBox.SelectedIndex = index;
                return;
            }
        }
    }

    private static ThemeOption FindThemeByKey(string key)
    {
        return ThemeOption.All.FirstOrDefault(theme => theme.Key == key) ?? ThemeOption.All[0];
    }

    private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (_game is null || LanguageComboBox.SelectedItem is not ComboBoxItem { Tag: string language })
        {
            return;
        }

        _localization.SetLanguage(language);
        _game.Settings.Language = language;
        ApplyLocalization();
        RefreshThemeOptionLabels();
        _settingsStorage.Save(_game.Settings);
        RenderBoard();
        UpdateStatus();
    }

    private void ThemeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (_game is null || ThemeComboBox.SelectedItem is not ComboBoxItem { Tag: ThemeOption theme })
        {
            return;
        }

        _currentTheme = theme;
        _game.Settings.Theme = theme.Key;
        ApplyTheme(theme);
        _settingsStorage.Save(_game.Settings);
    }

    private void ApplyLocalization()
    {
        SubtitleText.Text = _localization.Text("AppSubtitle");
        ThemeLabel.Text = _localization.Text("Theme");
        LanguageLabel.Text = _localization.Text("Language");
        SettingsButton.Content = _localization.Text("Settings");
        RulesButton.Content = _localization.Text("Rules");
        UndoButton.Content = _localization.Text("Undo");
        RestartButton.Content = _localization.Text("Restart");
        StatusTitle.Text = _localization.Text("Status");
        CapturedTitle.Text = _localization.Text("Captured");
        RulesTitle.Text = _localization.Text("Rules");
        RulesSummaryText.Text = _localization.Text("RulesSummary");
        BuildLabel.Text = _localization.Text("BuildLabel");
    }

    private void ApplyTheme(ThemeOption theme)
    {
        if (!theme.UsesImage || theme.BackgroundFileName is null)
        {
            ThemeBackgroundImage.Source = null;
            ThemeBackgroundImage.Visibility = Visibility.Collapsed;
            BackgroundLayer.Background = new SolidColorBrush(Color.FromRgb(17, 24, 39));
            return;
        }

        ThemeBackgroundImage.Source = new BitmapImage(new Uri($"pack://application:,,,/Assets/Backgrounds/{theme.BackgroundFileName}", UriKind.Absolute));
        ThemeBackgroundImage.Visibility = Visibility.Visible;
        BackgroundLayer.Background = Brushes.Black;
    }

    private void UpdateStatus()
    {
        var state = _game.Snapshot;
        if (state.Winner is not null)
        {
            TurnText.Text = _localization.WinnerMessage(state.Winner.Value);
        }
        else
        {
            TurnText.Text = $"{_localization.Text("CurrentTurn")}: {_localization.SideName(state.CurrentTurn)}";
            if (_game.Settings.Mode == GameMode.PlayerVsAi)
            {
                var difficultyKey = _game.Settings.AiDifficulty == AiDifficulty.Medium ? "AiMedium" : "AiEasy";
                TurnText.Text += $"\n{_localization.Text("GameMode")}: {_localization.Text("ModePlayerVsAi")} / {_localization.Text(difficultyKey)}";
            }

            if (state.SelectedPiece is not null)
            {
                TurnText.Text += $"\n{_localization.Text("Selected")}: {state.SelectedPiece.EnglishName} {state.SelectedPiece.Rank}";
            }
        }

        CapturedText.Text = $"{_localization.Text("BlueLost")}: {_game.CapturedPieces(PlayerSide.Blue, _localization.Text("None"))}\n{_localization.Text("RedLost")}: {_game.CapturedPieces(PlayerSide.Red, _localization.Text("None"))}";
        UndoButton.IsEnabled = state.HistoryCount > 0 && !_game.IsAiTurn();
    }

    private async void PlayAiTurnIfNeeded()
    {
        if (!_game.IsAiTurn())
        {
            return;
        }

        BoardGrid.IsEnabled = false;
        UndoButton.IsEnabled = false;
        await Task.Delay(450);

        var state = _game.Snapshot;
        var move = _ai.ChooseMove(state.Board, state.CurrentTurn, _game.Settings.AiDifficulty);
        if (move is not null)
        {
            _game.TryMovePiece(move.Value.Piece, move.Value.Target);
        }
        else
        {
            _game.ClearSelectionForAi();
        }

        BoardGrid.IsEnabled = true;
        RenderBoard();
        UpdateStatus();
        ShowWinnerDialogIfNeeded();
    }

    private void ShowWinnerDialogIfNeeded()
    {
        var winner = _game.Snapshot.Winner;
        if (winner is null || winner == _lastAnnouncedWinner)
        {
            return;
        }

        _lastAnnouncedWinner = winner;
        MessageBox.Show(this, _localization.WinnerMessage(winner.Value), _localization.Text("WinnerTitle"), MessageBoxButton.OK, MessageBoxImage.Information);
    }
}
