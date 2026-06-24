# 极简斗兽棋

简体中文 | [English](README.md)

极简斗兽棋是一款受经典斗兽棋启发的 Windows 极简棋盘游戏。它强调简单规则、清爽视觉、动物头像棋子、本地游玩、主题背景，以及图文并茂的规则说明页面。

## 功能特点

- 经典斗兽棋规则
- 本地双人对战
- 动物头像棋子
- 多款背景主题
- English / 简体中文 / 繁體中文界面
- 独立的设置窗口和规则窗口
- `docs/` 内置 GitHub Pages 图文规则网站源码
- 本地优先的桌面游戏体验

## 玩法简介

游戏目标是进入对方兽穴，或吃掉对方全部棋子。

棋子等级从低到高：

1. 鼠
2. 猫
3. 狗
4. 狼
5. 豹
6. 虎
7. 狮
8. 象

大多数动物每次横向或纵向移动一格。鼠可以进入河流。狮和虎可以跳过河流，但河中有鼠会阻挡跳跃。鼠可以吃象，象不能吃鼠。

完整图文规则网址：

<https://LittleBabyQ.github.io/AnimalChess/>

## 本地开发

环境要求：

- Windows 10/11
- .NET 8 SDK

本地运行：

```powershell
dotnet run
```

构建：

```powershell
dotnet build
```

## GitHub Pages 规则网站

图文规则静态网站源码位于：

```text
docs/
```

推送到 GitHub 后，在仓库中开启 GitHub Pages：

- Source: Deploy from a branch
- Branch: `main`
- Folder: `/docs`

预计访问地址：

<https://LittleBabyQ.github.io/AnimalChess/>

## 隐私

极简斗兽棋是本地优先的桌面游戏。当前开发版本不需要账号，不上传游戏数据，也不包含遥测。

详见 [PRIVACY.md](PRIVACY.md)。

## 许可

Copyright © BabyQ. All rights reserved.

本项目公开仅供个人学习和非商业使用。未经版权方明确书面许可，禁止商业使用、重新发布、转售、改名发布、收费分发，或集成到商业产品中。

详见 [LICENSE](LICENSE)。
