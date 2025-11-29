🎰 Somnia Treasure — On-Chain Gamble & Grow Game
<p align="center"> <img src="https://github.com/adarshmani30-cloud/SomniaStreamsTreasure/raw/main/banner.png" width="100%" /> </p> <p align="center"> <img src="https://img.shields.io/badge/Unity-6000.0+-black?logo=unity&style=for-the-badge" /> <img src="https://img.shields.io/badge/WebGL-Browser%20Game-blue?style=for-the-badge&logo=google-chrome" /> <img src="https://img.shields.io/badge/Web3-Somnia%20Streams-purple?style=for-the-badge&logo=ethereum" /> <img src="https://img.shields.io/badge/Language-C%23-239120?style=for-the-badge&logo=c-sharp" /> <img src="https://img.shields.io/badge/Status-Live-success?style=for-the-badge" /> </p>
🪙 About the Game

Somnia Treasure is a fast, luck-driven arcade game where players gamble, invest, and multiply their in-game money through quick decisions and strategic risk-taking.
Every bet, win, loss, yield upgrade, and multiplier is logged on-chain using Somnia Data Streams, making the entire game:

✔ Tamper-proof
✔ Transparent
✔ Impossible to cheat
✔ Community-verifiable

Web2 gameplay × Web3 trust.
A perfect hybrid.

🎮 Gameplay Overview

🎲 Spin, choose, or gamble

💵 Earn money based on multipliers

📈 Increase yield rate through upgrades

🏦 Buy income, real estate, businesses, and coins

🔁 Press your luck again — or cash out

🪙 Mint your earned tokens

Every session becomes part of your permanent on-chain history.

🔗 On-Chain SDS Integration

Somnia Treasure uses Somnia Streams to capture and decentralize:

Bet amount

Gamble outcome

Final session money

Upgrades purchased

Mint trigger events

Timestamped runs

This builds a fully auditable trail of your gameplay.

🧠 Game Architecture
/Core
   GameManager.cs
   MoneySystem.cs
   YieldSystem.cs
   UpgradeManager.cs
   BetController.cs

/UI
   HUDController.cs
   ShopUI.cs
   ResultScreen.cs

/SDS
   SDSWriter.cs
   SDSReader.cs
   OnChainEventBuilder.cs

/Player
   PlayerWallet.cs
   TokenMinting.cs

🏗️ Technical Flow (How It Works)
[Player Bet]
     ↓
Calculate Win / Loss
     ↓
Update Money + Yield Rate
     ↓
UI Refresh
     ↓
Send Gameplay Event → Somnia Stream
     ↓
Store On-Chain (Immutable)
     ↓
If Player Chooses → Mint Tokens

📂 Project Structure
SomniaStreamsTreasure/
│── Assets/
│   ├── Scripts/
│   │   ├── Core/
│   │   ├── UI/
│   │   ├── SDS/
│   ├── Prefabs/
│   ├── UI/
│── ProjectSettings/
│── README.md

🖼️ Screenshots
/screenshots
   1.png
   2.png
   3.png

▶️ Play the Game

👉 Live WebGL Build:
https://somniastreamstreasure.netlify.app

🛠️ Built With

Unity 6

Somnia Data Streams (SDS)

WebGL

C#

On-chain event streaming architecture

👤 Developer

Solo-built with ❤️ by a Unity/Web3 indie dev.
