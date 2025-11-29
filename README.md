<p align="center">
  <img src="https://github.com/adarshmani30-cloud/SomniaStreamsTreasure/raw/main/banner.png" 
       width="100%" alt="Somnia Treasure Banner"/>
</p>

<p align="center">
  <img src="https://img.shields.io/badge/Unity-6000%2B-black?logo=unity&style=for-the-badge" />
  <img src="https://img.shields.io/badge/Platform-WebGL-blue?style=for-the-badge&logo=google-chrome" />
  <img src="https://img.shields.io/badge/Web3-Somnia%20Streams-purple?style=for-the-badge&logo=ethereum" />
  <img src="https://img.shields.io/badge/Language-C%23-239120?style=for-the-badge&logo=c-sharp" />
  <img src="https://img.shields.io/badge/Status-Live-success?style=for-the-badge" />
</p>

<h1 align="center">💰 Somnia Treasure — On-Chain Gamble & Grow Arcade</h1>
<h3 align="center">Unity • Somnia Data Streams • WebGL • On-Chain Event Logging</h3>

---

## 🚀 About the Project

**Somnia Treasure** is a fast, luck-driven arcade game where players gamble, invest, and grow their in-game wealth through quick decisions.  
Every bet, multiplier, and earning is streamed **on-chain** via **Somnia Streams**, enabling:

- 🔒 Cheat-proof gameplay  
- 🌐 Full transparency  
- 📊 Immutable run history  
- 🤝 Trustless competition  

---

## 🎮 Core Gameplay

- Choose your gamble (Rock–Paper–Scissors style spins)
- Win or lose money based on multipliers
- Upgrade income sources:  
  - 📁 Income  
  - 🏠 Real Estate  
  - 💼 Businesses  
  - 🔮 Somnia Coin  
- Raise your yield rate
- Try again… or cash out
- Mint your earnings on-chain

---

## 🔗 Powered by Somnia Streams

Somnia Streams records in real time:

- Bet amount  
- Gamble outcome  
- Multiplier result  
- Session earnings  
- Upgrades purchased  
- Mint trigger events  

Everything is stored **decentralized and verifiable**.

---

## 🧠 Architecture

```
/Core
   GameManager.cs
   BetController.cs
   MoneySystem.cs
   UpgradeManager.cs
   YieldSystem.cs

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
```

---

## 🏗️ Technical Flow

```
[Player Action]
     ↓
Select Bet Type
     ↓
Calculate Win/Loss
     ↓
Update Money + Yield Rate
     ↓
UI Refresh
     ↓
Send Event → Somnia Streams
     ↓
Stored On-Chain
     ↓
[Optional] Mint Earnings
```

---

## 📂 Folder Structure

```
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
```

---

## 🖼️ Screenshots

```
/screenshots
   gameplay1.png
   gameplay2.png
   gameplay3.png
```

---

## ▶️ Play the Game

🔗 **Live WebGL Build:**  
https://somniastreamstreasure.netlify.app  

---

## 🛠️ Tech Used

- Unity 6
- Somnia Data Streams (SDS)
- WebGL
- C#
- On-chain event streaming

---

## 👤 Developer

Solo-built with ❤️ for the Somnia Streams Mini Hackathon.
