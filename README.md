# Animal Chess

[简体中文](README.zh-CN.md) | English

Animal Chess is a minimalist Windows board game inspired by Jungle Chess. It focuses on simple rules, clean visuals, animal piece icons, local play, theme backgrounds, and an illustrated rules page.

## Features

- Classic animal strategy board game rules
- Local two-player gameplay
- Animal avatar pieces
- Multiple background themes
- English, Simplified Chinese, and Traditional Chinese UI
- Separate Settings and Rules windows
- GitHub Pages illustrated rules website source in `docs/`
- Local-first desktop experience

## How to Play

The goal is to enter the opponent's den or capture all opponent pieces.

Piece ranks from low to high:

1. Rat
2. Cat
3. Dog
4. Wolf
5. Leopard
6. Tiger
7. Lion
8. Elephant

Most animals move one square horizontally or vertically. Rat can enter the river. Lion and Tiger can jump across the river if no Rat blocks the path. Rat can capture Elephant, while Elephant cannot capture Rat.

Full illustrated rules URL:

<https://LittleBabyQ.github.io/AnimalChess/>

## Development

Requirements:

- Windows 10/11
- .NET 8 SDK

Run locally:

```powershell
dotnet run
```

Build:

```powershell
dotnet build
```

## GitHub Pages Rules Site

The static illustrated rules site is stored in:

```text
docs/
```

After pushing to GitHub, enable GitHub Pages with:

- Source: Deploy from a branch
- Branch: `main`
- Folder: `/docs`

Expected URL:

<https://LittleBabyQ.github.io/AnimalChess/>

## Privacy

Animal Chess is designed as a local-first desktop game. It does not require an account, does not upload gameplay data, and does not include telemetry in the current development version.

See [PRIVACY.md](PRIVACY.md) for details.

## License

Copyright © BabyQ. All rights reserved.

This project is publicly visible for personal learning and non-commercial use only. Commercial use, republishing, resale, rebranding, paid distribution, or integration into commercial products is not permitted without explicit written permission from the copyright holder.

See [LICENSE](LICENSE) for details.
