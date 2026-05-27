# CyberBot — Cybersecurity Awareness Chatbot

CyberBot is a Windows desktop chatbot built with C# and WPF that educates users about cybersecurity topics through natural, interactive conversation. It features keyword recognition, sentiment detection, personalised memory, and a polished two-page GUI.

---

## Table of Contents

- Features
- Topic that Cyberbot knows
- Project Structure
- Author

---

## Features

- **Keyword Recognition** — Responds to 8 cybersecurity topics with detailed, structured advice and follow-up drill-downs
- **Sentiment Detection** — Detects 5 emotional tones (worried, curious, frustrated, happy, neutral) and adapts responses accordingly
- **Memory & Recall** — Remembers your name and favourite topic throughout the conversation for personalised responses
- **Conversation Flow** — Multi-step flow with name capture, follow-up handling ("tell me more"), and graceful fallbacks
- **Random Responses** — Varied fallback and sentiment opener responses to keep conversation natural
- **Voice Greeting** — Plays an audio greeting (`greeting.wav`) on launch if present
- **Polished GUI** — Two-page WPF interface: a light welcome page and a dark chat page with styled message bubbles, timestamps, and a status bar

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
Part_2/
├── ChatBot.cs              # Core routing logic — processes all user input
├── KeywordResponder.cs     # Keyword detection, responses, and follow-up drill-downs
├── SentimentDetector.cs    # Emotion detection and empathetic response openers
├── MemoryStore.cs          # Stores and recalls user name and favourite topic
├── MainWindow.xaml         # WPF UI layout — welcome page and chat page
├── MainWindow.xaml.cs      # UI code-behind — message rendering and event handlers
├── cyberbot_logo.png       # Application logo (displayed on both pages)
└── CybersecurityChatbot.csproj
```

---


## Author
```
Mvuyisi Sokabo
ST10490509
Rosebank International
```
