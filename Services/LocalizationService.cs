using AnimalChess.Models;

namespace AnimalChess.Services;

public sealed class LocalizationService
{
    private readonly Dictionary<string, Dictionary<string, string>> _resources = new()
    {
        ["en-US"] = new Dictionary<string, string>
        {
            ["AppSubtitle"] = "Minimal Jungle Chess",
            ["Language"] = "Language",
            ["Theme"] = "Theme",
            ["Settings"] = "Settings",
            ["Undo"] = "Undo",
            ["Restart"] = "Restart",
            ["Status"] = "Status",
            ["Captured"] = "Captured",
            ["Rules"] = "Rules",
            ["RulesSummary"] = "Click your piece, then click a highlighted square. Rat can enter river and capture Elephant. Lion and Tiger can jump river if no Rat blocks the path. Enter opponent den to win.",
            ["BuildLabel"] = "v0.1 development build - local two-player only",
            ["CurrentTurn"] = "Current turn",
            ["Selected"] = "Selected",
            ["Blue"] = "Blue",
            ["Red"] = "Red",
            ["BlueLost"] = "Blue lost",
            ["RedLost"] = "Red lost",
            ["None"] = "None",
            ["Rank"] = "Rank",
            ["WinnerTitle"] = "Game Over",
            ["BlueWins"] = "Congratulations! Blue wins!",
            ["RedWins"] = "Congratulations! Red wins!",
            ["ThemeDefaultMinimal"] = "Default",
            ["ThemeAnimeGirl"] = "Anime Girl",
            ["ThemeAnimeJk"] = "Anime JK",
            ["ThemeAnimeSunset"] = "Anime Sunset",
            ["ThemeLittlePrince"] = "Little Prince",
            ["ThemeStarAndMoon"] = "Star and Moon",
            ["ThemeSunflower"] = "Sunflower",
            ["SettingsTitle"] = "Settings",
            ["SettingsSubtitle"] = "Language, theme and future AI options.",
            ["AiOpponent"] = "AI Opponent",
            ["AiComingSoon"] = "Coming soon: Easy, Medium and Hard AI difficulty.",
            ["Cancel"] = "Cancel",
            ["Apply"] = "Apply",
            ["Close"] = "Close",
            ["RulesWindowTitle"] = "Animal Chess Rules",
            ["RulesWindowSubtitle"] = "Learn the board, pieces, terrain and winning conditions.",
            ["OpenRulesWebsite"] = "View Full Illustrated Rules",
            ["RulesDetail"] = "Goal\nEnter the opponent den or capture all opponent pieces.\n\nPiece Rank\n1 Rat, 2 Cat, 3 Dog, 4 Wolf, 5 Leopard, 6 Tiger, 7 Lion, 8 Elephant. A piece normally captures an enemy piece with equal or lower rank.\n\nTurn\nBlue moves first. Players move one piece per turn.\n\nMovement\nMost animals move one square horizontally or vertically. They cannot move diagonally. Rat can enter the river. Lion and Tiger can jump across the river if no Rat blocks the path.\n\nCapture\nA piece can capture an enemy piece with equal or lower rank. Rat can capture Elephant. Elephant cannot capture Rat. A piece in the opponent trap has rank 0.\n\nRiver\nOnly Rat can enter river squares. Lion and Tiger may jump over river lanes, but a Rat in the river blocks the jump.\n\nTrap and Den\nYou cannot enter your own den. Enter the opponent den to win. Traps weaken enemy pieces standing on them.\n\nStrategy Tips\nDo not expose Elephant to the opponent Rat. Use Lion and Tiger jumps to attack quickly. Control traps near the den. Sometimes reaching the den is faster than capturing all pieces.\n\nFull illustrated rules\nhttps://LittleBabyQ.github.io/AnimalChess/"
        },
        ["zh-CN"] = new Dictionary<string, string>
        {
            ["AppSubtitle"] = "极简斗兽棋",
            ["Language"] = "语言",
            ["Theme"] = "主题",
            ["Settings"] = "设置",
            ["Undo"] = "撤销",
            ["Restart"] = "重新开始",
            ["Status"] = "状态",
            ["Captured"] = "被吃棋子",
            ["Rules"] = "规则",
            ["RulesSummary"] = "点击己方棋子，再点击高亮格子移动。鼠可以进河并吃象。狮和虎可以跳河，但河中有鼠会阻挡。进入对方兽穴即可获胜。",
            ["BuildLabel"] = "v0.1 开发版 - 仅支持本地双人",
            ["CurrentTurn"] = "当前回合",
            ["Selected"] = "已选择",
            ["Blue"] = "蓝方",
            ["Red"] = "红方",
            ["BlueLost"] = "蓝方损失",
            ["RedLost"] = "红方损失",
            ["None"] = "无",
            ["Rank"] = "等级",
            ["WinnerTitle"] = "游戏结束",
            ["BlueWins"] = "恭喜！蓝方获胜！",
            ["RedWins"] = "恭喜！红方获胜！",
            ["ThemeDefaultMinimal"] = "默认纯色",
            ["ThemeAnimeGirl"] = "动漫女孩",
            ["ThemeAnimeJk"] = "动漫 JK",
            ["ThemeAnimeSunset"] = "动漫夕阳",
            ["ThemeLittlePrince"] = "小王子",
            ["ThemeStarAndMoon"] = "星与月",
            ["ThemeSunflower"] = "向日葵",
            ["SettingsTitle"] = "设置",
            ["SettingsSubtitle"] = "语言、主题和后续 AI 选项。",
            ["AiOpponent"] = "AI 对手",
            ["AiComingSoon"] = "即将支持：简单、中等、困难三档 AI。",
            ["Cancel"] = "取消",
            ["Apply"] = "应用",
            ["Close"] = "关闭",
            ["RulesWindowTitle"] = "极简斗兽棋规则",
            ["RulesWindowSubtitle"] = "了解棋盘、棋子、地形和胜利条件。",
            ["OpenRulesWebsite"] = "查看完整图文规则",
            ["RulesDetail"] = "胜利目标\n进入对方兽穴，或吃掉对方全部棋子。\n\n棋子等级\n1 鼠，2 猫，3 狗，4 狼，5 豹，6 虎，7 狮，8 象。通常高等级可以吃低等级，同等级可以互吃。\n\n回合\n蓝方先手。双方每回合移动一个棋子。\n\n移动\n大多数动物每次横向或纵向移动一格，不能斜走。鼠可以进入河流。狮和虎可以跳过河流，但路径中有鼠时不能跳。\n\n吃子\n棋子可以吃掉等级小于或等于自己的敌方棋子。鼠可以吃象。象不能吃鼠。进入对方陷阱的棋子等级视为 0。\n\n河流\n只有鼠可以进入河流。狮和虎可以跨河跳跃，但河里有鼠会阻挡跳跃。\n\n陷阱与兽穴\n不能进入己方兽穴。进入对方兽穴即可获胜。陷阱会削弱站在其中的敌方棋子。\n\n新手提示\n不要轻易让象靠近对方鼠。利用狮虎跳河快速进攻。控制兽穴附近的陷阱。有时候进入兽穴比吃光棋子更快获胜。\n\n完整图文规则\nhttps://LittleBabyQ.github.io/AnimalChess/"
        },
        ["zh-TW"] = new Dictionary<string, string>
        {
            ["AppSubtitle"] = "極簡鬥獸棋",
            ["Language"] = "語言",
            ["Theme"] = "主題",
            ["Settings"] = "設定",
            ["Undo"] = "復原",
            ["Restart"] = "重新開始",
            ["Status"] = "狀態",
            ["Captured"] = "被吃棋子",
            ["Rules"] = "規則",
            ["RulesSummary"] = "點擊己方棋子，再點擊高亮格子移動。鼠可以進河並吃象。獅和虎可以跳河，但河中有鼠會阻擋。進入對方獸穴即可獲勝。",
            ["BuildLabel"] = "v0.1 開發版 - 僅支援本地雙人",
            ["CurrentTurn"] = "目前回合",
            ["Selected"] = "已選擇",
            ["Blue"] = "藍方",
            ["Red"] = "紅方",
            ["BlueLost"] = "藍方損失",
            ["RedLost"] = "紅方損失",
            ["None"] = "無",
            ["Rank"] = "等級",
            ["WinnerTitle"] = "遊戲結束",
            ["BlueWins"] = "恭喜！藍方獲勝！",
            ["RedWins"] = "恭喜！紅方獲勝！",
            ["ThemeDefaultMinimal"] = "預設純色",
            ["ThemeAnimeGirl"] = "動漫女孩",
            ["ThemeAnimeJk"] = "動漫 JK",
            ["ThemeAnimeSunset"] = "動漫夕陽",
            ["ThemeLittlePrince"] = "小王子",
            ["ThemeStarAndMoon"] = "星與月",
            ["ThemeSunflower"] = "向日葵",
            ["SettingsTitle"] = "設定",
            ["SettingsSubtitle"] = "語言、主題和後續 AI 選項。",
            ["AiOpponent"] = "AI 對手",
            ["AiComingSoon"] = "即將支援：簡單、中等、困難三檔 AI。",
            ["Cancel"] = "取消",
            ["Apply"] = "套用",
            ["Close"] = "關閉",
            ["RulesWindowTitle"] = "極簡鬥獸棋規則",
            ["RulesWindowSubtitle"] = "了解棋盤、棋子、地形和勝利條件。",
            ["OpenRulesWebsite"] = "查看完整圖文規則",
            ["RulesDetail"] = "勝利目標\n進入對方獸穴，或吃掉對方全部棋子。\n\n棋子等級\n1 鼠，2 貓，3 狗，4 狼，5 豹，6 虎，7 獅，8 象。通常高等級可以吃低等級，同等級可以互吃。\n\n回合\n藍方先手。雙方每回合移動一個棋子。\n\n移動\n大多數動物每次橫向或縱向移動一格，不能斜走。鼠可以進入河流。獅和虎可以跳過河流，但路徑中有鼠時不能跳。\n\n吃子\n棋子可以吃掉等級小於或等於自己的敵方棋子。鼠可以吃象。象不能吃鼠。進入對方陷阱的棋子等級視為 0。\n\n河流\n只有鼠可以進入河流。獅和虎可以跨河跳躍，但河裡有鼠會阻擋跳躍。\n\n陷阱與獸穴\n不能進入己方獸穴。進入對方獸穴即可獲勝。陷阱會削弱站在其中的敵方棋子。\n\n新手提示\n不要輕易讓象靠近對方鼠。利用獅虎跳河快速進攻。控制獸穴附近的陷阱。有時候進入獸穴比吃光棋子更快獲勝。\n\n完整圖文規則\nhttps://LittleBabyQ.github.io/AnimalChess/"
        }
    };

    public string CurrentLanguage { get; private set; } = "en-US";

    public void SetLanguage(string language)
    {
        CurrentLanguage = _resources.ContainsKey(language) ? language : "en-US";
    }

    public string Text(string key)
    {
        if (_resources.TryGetValue(CurrentLanguage, out var languageResources) && languageResources.TryGetValue(key, out var value))
        {
            return value;
        }

        return _resources["en-US"].TryGetValue(key, out var fallback) ? fallback : key;
    }

    public string SideName(PlayerSide side) => side switch
    {
        PlayerSide.Blue => Text("Blue"),
        PlayerSide.Red => Text("Red"),
        _ => Text("None")
    };

    public string WinnerMessage(PlayerSide side) => side switch
    {
        PlayerSide.Blue => Text("BlueWins"),
        PlayerSide.Red => Text("RedWins"),
        _ => Text("WinnerTitle")
    };
}
