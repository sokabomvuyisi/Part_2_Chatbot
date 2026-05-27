# 🤖 CyberBot — Cybersecurity Awareness Chatbot

CyberBot is a Windows desktop chatbot built with C# and WPF that educates users about cybersecurity topics through natural, interactive conversation. It features keyword recognition, sentiment detection, personalised memory, and a polished two-page GUI.

---

## 📋 Table of Contents

- [Features]
- [Requirements]
- [Project Structure]
- [Example Interactions]

---

## ✨ Features

- **Keyword Recognition** — Responds to 8 cybersecurity topics with detailed, structured advice and follow-up drill-downs
- **Sentiment Detection** — Detects 5 emotional tones (worried, curious, frustrated, happy, neutral) and adapts responses accordingly
- **Memory & Recall** — Remembers your name and favourite topic throughout the conversation for personalised responses
- **Conversation Flow** — Multi-step flow with name capture, follow-up handling ("tell me more"), and graceful fallbacks
- **Random Responses** — Varied fallback and sentiment opener responses to keep conversation natural
- **Voice Greeting** — Plays an audio greeting (`greeting.wav`) on launch if present
- **Polished GUI** — Two-page WPF interface: a light welcome page and a dark chat page with styled message bubbles, timestamps, and a status bar

---

## 🖥️ Requirements

- Windows 10 or Windows 11
- [.NET 6.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) or later
- Visual Studio 2022 (Community edition or higher) with the **.NET desktop development** workload installed

---

## ⚙️ Setup & Installation

1. **Clone the repository:**
   ```bash
   git clone https://github.com/YOUR_USERNAME/CybersecurityChatbot.git
   cd CybersecurityChatbot
   ```

2. **Open the project in Visual Studio:**
   - Double-click `CybersecurityChatbot.csproj`, or
   - Open Visual Studio → File → Open → Project/Solution → select the `.csproj` file

3. **Build the project:**
   - Press `Ctrl + Shift + B` or go to Build → Build Solution

4. **Run the application:**
   - Press `F5` to run with debugging, or `Ctrl + F5` to run without

> **Note:** The logo (`cyberbot_logo.png`) and optional voice greeting (`greeting.wav`) must be in the same directory as the compiled executable. They are already included in the project and set to copy on build.

---


## Topics CyberBot Knows

| Topic | Example Prompt |
|---|---|
| Passwords | "How do I create a strong password?" |
| Phishing | "Tell me about phishing" |
| Privacy | "How do I protect my privacy online?" |
| Scams | "What are common online scams in South Africa?" |
| Malware | "What is malware?" |
| Firewall | "What does a firewall do?" |
| Encryption | "Explain encryption to me" |
| VPN | "What is a VPN and do I need one?" |

Each topic also supports deeper follow-up questions. For example, after asking about phishing you can ask:
- *"What do I do if I clicked a phishing link?"*
- *"How do I report a phishing email?"*
- *"How do I check if an email is real?"*

---

## Project Structure

```
CybersecurityChatbot/
├── ChatBot.cs              # Core routing logic — processes all user input
├── KeywordResponder.cs     # Keyword detection, responses, and follow-up drill-downs
├── SentimentDetector.cs    # Emotion detection and empathetic response openers
├── MemoryStore.cs          # Stores and recalls user name and favourite topic
├── MainWindow.xaml         # WPF UI layout — welcome page and chat page
├── MainWindow.xaml.cs      # UI code-behind — message rendering and event handlers
├── cyberbot_logo.png       # Application logo (displayed on both pages)
├── greeting.wav            # Optional voice greeting played on launch
└── CybersecurityChatbot.csproj
```

---

## Example Interactions

```
CyberBot: Hello! I'm CyberBot. Before we begin, what's your name?

You: Sarah

CyberBot: Nice to meet you, Sarah! You can ask me about passwords,
          phishing, malware, scams, privacy, VPNs, firewalls, and encryption.

You: I'm worried about phishing emails

CyberBot: I can hear that you're concerned, and that's actually a smart
          instinct. Here's what you need to know to stay safe.
          Phishing is when scammers pretend to be trusted organisations...

You: tell me more

CyberBot: Sarah, here's more on phishing: [follow-up tip]

You: what do I do if I clicked a phishing link?

CyberBot: You Clicked a Phishing Link — Act Fast!
          Step 1: Disconnect from the internet immediately...
```

---

## Disclaimer

CyberBot is an educational tool built for awareness purposes. For incidents involving financial loss or criminal activity, always contact the relevant authorities directly (e.g. SAPS: 10111, SABRIC: 011 847 3000).

---

*Built with C# and WPF · The Independent Institute of Education · 2026*
