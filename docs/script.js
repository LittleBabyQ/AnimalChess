const translations = {
  en: {
    eyebrow: "Minimal strategy board game",
    title: "Animal Chess Rules",
    intro: "Learn the board, pieces, terrain, captures and winning conditions for Animal Chess.",
    goalTitle: "Goal",
    goalBody: "Enter the opponent den or capture all opponent pieces. Reaching the den is often faster than capturing every piece.",
    piecesTitle: "Piece Rank",
    rat: "Rat",
    cat: "Cat",
    dog: "Dog",
    wolf: "Wolf",
    leopard: "Leopard",
    tiger: "Tiger",
    lion: "Lion",
    elephant: "Elephant",
    piecesBody: "A piece normally captures an enemy piece with equal or lower rank. Rat can capture Elephant, but Elephant cannot capture Rat.",
    moveTitle: "Movement",
    moveBody: "Most animals move one square horizontally or vertically. They cannot move diagonally and cannot move onto a friendly piece.",
    captureTitle: "Capture",
    captureBody: "Higher-ranked animals can capture lower-ranked animals. Equal rank can capture equal rank. Traps reduce enemy rank to 0.",
    riverTitle: "River",
    riverBody: "Only Rat can enter river squares. Lion and Tiger can jump across river lanes, but a Rat in the river blocks the jump.",
    denTitle: "Trap & Den",
    denBody: "You cannot enter your own den. Enter the opponent den to win. Traps weaken enemy pieces standing on them.",
    tipsTitle: "Beginner Tips",
    tip1: "Do not expose Elephant to the opponent Rat.",
    tip2: "Use Lion and Tiger jumps to attack quickly.",
    tip3: "Control traps near the den.",
    tip4: "Sometimes entering the den is faster than capturing all pieces."
  },
  zhHans: {
    eyebrow: "极简策略棋盘游戏",
    title: "极简斗兽棋规则",
    intro: "了解极简斗兽棋的棋盘、棋子、地形、吃子和胜利条件。",
    goalTitle: "胜利目标",
    goalBody: "进入对方兽穴，或吃掉对方全部棋子。有时候进入兽穴比吃光棋子更快获胜。",
    piecesTitle: "棋子等级",
    rat: "鼠",
    cat: "猫",
    dog: "狗",
    wolf: "狼",
    leopard: "豹",
    tiger: "虎",
    lion: "狮",
    elephant: "象",
    piecesBody: "通常高等级可以吃低等级，同等级可以互吃。鼠可以吃象，但象不能吃鼠。",
    moveTitle: "移动规则",
    moveBody: "大多数动物每次横向或纵向移动一格，不能斜走，也不能移动到己方棋子所在格。",
    captureTitle: "吃子规则",
    captureBody: "高等级动物可以吃低等级动物，同等级可以互吃。陷阱会让敌方棋子的等级变为 0。",
    riverTitle: "河流",
    riverBody: "只有鼠可以进入河流。狮和虎可以跨河跳跃，但河里有鼠会阻挡跳跃。",
    denTitle: "陷阱与兽穴",
    denBody: "不能进入己方兽穴。进入对方兽穴即可获胜。陷阱会削弱站在其中的敌方棋子。",
    tipsTitle: "新手提示",
    tip1: "不要轻易让象靠近对方鼠。",
    tip2: "利用狮虎跳河快速进攻。",
    tip3: "控制兽穴附近的陷阱。",
    tip4: "有时候进入兽穴比吃光棋子更快获胜。"
  },
  zhHant: {
    eyebrow: "極簡策略棋盤遊戲",
    title: "極簡鬥獸棋規則",
    intro: "了解極簡鬥獸棋的棋盤、棋子、地形、吃子和勝利條件。",
    goalTitle: "勝利目標",
    goalBody: "進入對方獸穴，或吃掉對方全部棋子。有時候進入獸穴比吃光棋子更快獲勝。",
    piecesTitle: "棋子等級",
    rat: "鼠",
    cat: "貓",
    dog: "狗",
    wolf: "狼",
    leopard: "豹",
    tiger: "虎",
    lion: "獅",
    elephant: "象",
    piecesBody: "通常高等級可以吃低等級，同等級可以互吃。鼠可以吃象，但象不能吃鼠。",
    moveTitle: "移動規則",
    moveBody: "大多數動物每次橫向或縱向移動一格，不能斜走，也不能移動到己方棋子所在格。",
    captureTitle: "吃子規則",
    captureBody: "高等級動物可以吃低等級動物，同等級可以互吃。陷阱會讓敵方棋子的等級變為 0。",
    riverTitle: "河流",
    riverBody: "只有鼠可以進入河流。獅和虎可以跨河跳躍，但河裡有鼠會阻擋跳躍。",
    denTitle: "陷阱與獸穴",
    denBody: "不能進入己方獸穴。進入對方獸穴即可獲勝。陷阱會削弱站在其中的敵方棋子。",
    tipsTitle: "新手提示",
    tip1: "不要輕易讓象靠近對方鼠。",
    tip2: "利用獅虎跳河快速進攻。",
    tip3: "控制獸穴附近的陷阱。",
    tip4: "有時候進入獸穴比吃光棋子更快獲勝。"
  }
};

function setLanguage(language) {
  const dictionary = translations[language] || translations.en;
  document.documentElement.lang = language === "zhHans" ? "zh-CN" : language === "zhHant" ? "zh-TW" : "en";
  document.querySelectorAll("[data-i18n]").forEach((element) => {
    const key = element.getAttribute("data-i18n");
    element.textContent = dictionary[key] || translations.en[key] || key;
  });
  document.querySelectorAll(".langs button").forEach((button) => {
    button.classList.toggle("active", button.dataset.lang === language);
  });
}

document.querySelectorAll(".langs button").forEach((button) => {
  button.addEventListener("click", () => setLanguage(button.dataset.lang));
});

setLanguage("en");
