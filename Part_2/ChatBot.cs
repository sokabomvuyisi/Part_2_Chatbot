using System;
using System.Collections.Generic;

namespace CybersecurityChatbot
{
    /// <summary>
    /// Central chatbot class. Routes all user input through keyword recognition,
    /// sentiment detection, memory recall, and conversation flow.
    /// MainWindow should only ever call ProcessInput() and GetGreeting() on this class.
    /// </summary>
    public class ChatBot
    {
        private readonly KeywordResponder _keywords;
        private readonly SentimentDetector _sentiment;
        private readonly MemoryStore _memory;
        private readonly Random _random = new();

        private bool _awaitingName = true;
        private string _lastTopic = string.Empty;

        // Follow-up trigger phrases
        private readonly List<string> _followUpPhrases = new()
        {
            "tell me more", "explain more", "give me another tip",
            "more info", "elaborate", "continue", "go on", "more details",
            "anything else", "what else"
        };

        // Fallback responses for unrecognised input
        private readonly List<string> _fallbackResponses = new()
        {
            "I'm not sure I understand. Can you try rephrasing?",
            "Hmm, I didn't quite catch that. Try asking about passwords, phishing, malware, or scams!",
            "I'm still learning! Could you ask me something about a cybersecurity topic?",
            "That's outside my expertise right now. Try asking: 'Tell me about phishing' or 'How do I stay safe online?'"
        };

        public ChatBot()
        {
            _keywords = new KeywordResponder();
            _sentiment = new SentimentDetector();
            _memory = new MemoryStore();
        }

        /// <summary>Returns the opening greeting message shown on app launch.</summary>
        public string GetGreeting()
        {
            return "👋 Hello! I'm CyberBot, your personal cybersecurity assistant.\n\nBefore we begin, what's your name?";
        }

        /// <summary>
        /// Main routing method. Processes user input and returns the bot's response.
        /// Order: name capture → follow-up → sentiment → keyword → special phrases → fallback.
        /// </summary>
        public string ProcessInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "Please type a message so I can help you! 😊";

            input = input.Trim();
            string lowerInput = input.ToLower();

            // ── Step 1: Capture name on first message ──────────────────────────────
            if (_awaitingName)
            {
                string name = ExtractName(input);
                _memory.UserName = name;
                _memory.Store("name", name);
                _awaitingName = false;

                return $"Nice to meet you, {name}! 🛡️\n\n" +
                       $"I'm here to help you stay safe in the digital world. " +
                       $"You can ask me about topics like passwords, phishing, malware, scams, privacy, VPNs, firewalls, and encryption.\n\n" +
                       $"What would you like to know about today, {name}?";
            }

            // ── Step 2: Check for follow-up phrases ────────────────────────────────
            if (IsFollowUp(lowerInput))
            {
                if (!string.IsNullOrWhiteSpace(_lastTopic))
                {
                    string? followUpResponse = _keywords.GetResponseForKeyword(_lastTopic);
                    if (followUpResponse != null)
                    {
                        string opener = _memory.HasName() ? $"{_memory.UserName}, here's more on {_lastTopic}: " : $"Here's more on {_lastTopic}: ";
                        return opener + "\n\n" + followUpResponse;
                    }
                }
                return "I don't have a previous topic to continue from. What cybersecurity topic would you like to explore?";
            }

            // ── Step 3: Check for favourite topic storage ──────────────────────────
            string? detectedTopic = TryExtractFavouriteTopic(lowerInput);
            if (detectedTopic != null && !_memory.HasFavouriteTopic())
            {
                _memory.FavouriteTopic = detectedTopic;
                _memory.Store("favourite_topic", detectedTopic);
                _lastTopic = detectedTopic;

                string? topicResponse = _keywords.GetResponseForKeyword(detectedTopic);
                string topicReply = topicResponse != null ? "\n\n" + topicResponse : string.Empty;

                return $"Great! I'll remember that you're interested in {detectedTopic}. " +
                       $"It's a crucial part of staying safe online.{topicReply}";
            }

            // ── Step 4: Detect sentiment ───────────────────────────────────────────
            Sentiment detectedSentiment = _sentiment.Detect(lowerInput);
            string sentimentOpener = _sentiment.GetSentimentResponse(detectedSentiment);

            // ── Step 5: Keyword recognition ────────────────────────────────────────
            string? keywordResponse = _keywords.GetResponse(lowerInput);
            string? matchedKeyword = _keywords.GetMatchedKeyword(lowerInput);

            if (keywordResponse != null)
            {
                _lastTopic = matchedKeyword ?? string.Empty;
                string personalisedOpener = BuildPersonalisedOpener(matchedKeyword);

                string fullResponse = sentimentOpener + personalisedOpener + keywordResponse;
                fullResponse += "\n\n💡 Type 'tell me more' if you'd like another tip on this topic.";
                return fullResponse;
            }

            // ── Step 6: Special command phrases ───────────────────────────────────
            if (lowerInput.Contains("how are you"))
            {
                string name = _memory.HasName() ? $", {_memory.UserName}" : "";
                return $"I'm running at full capacity{name}! 🤖 Ready to help you stay safe online. What cybersecurity topic can I help with?";
            }

            if (lowerInput.Contains("what can you do") || lowerInput.Contains("help") || lowerInput.Contains("purpose"))
            {
                var keywords = _keywords.GetAllKeywords();
                return $"I'm a cybersecurity awareness chatbot! Here's what I can help with:\n\n" +
                       $"🔐 Topics I know: {string.Join(", ", keywords)}\n\n" +
                       $"Just ask me anything about these topics, or type something like:\n" +
                       $"• 'Tell me about phishing'\n• 'How do I create a strong password?'\n• 'I'm worried about scams'";
            }

            if (lowerInput.Contains("who are you") || lowerInput.Contains("your name"))
            {
                return "I'm CyberBot 🤖 — your cybersecurity awareness assistant! I'm here to help you understand online threats and how to stay safe.";
            }

            if (lowerInput.Contains("bye") || lowerInput.Contains("goodbye") || lowerInput.Contains("exit") || lowerInput.Contains("thank you"))
            {
                string name = _memory.HasName() ? $", {_memory.UserName}" : "";
                return $"Stay safe online{name}! 🛡️ Remember: Think before you click. Goodbye!";
            }

            // ── Step 7: Fallback ───────────────────────────────────────────────────
            return sentimentOpener + _fallbackResponses[_random.Next(_fallbackResponses.Count)];
        }

        // ── Private helpers ────────────────────────────────────────────────────────

        private static string ExtractName(string input)
        {
            // Strip common lead-in phrases like "my name is" or "I am"
            string lower = input.ToLower();
            foreach (string phrase in new[] { "my name is ", "i am ", "i'm ", "call me ", "it's ", "its " })
            {
                if (lower.Contains(phrase))
                {
                    int idx = lower.IndexOf(phrase) + phrase.Length;
                    string extracted = input.Substring(idx).Trim().Split(' ')[0];
                    return CapitaliseFirst(extracted.TrimEnd('.', ',', '!', '?'));
                }
            }
            // Fallback: use the first word of whatever they typed
            return CapitaliseFirst(input.Split(' ')[0].TrimEnd('.', ',', '!', '?'));
        }

        private bool IsFollowUp(string lowerInput)
        {
            foreach (string phrase in _followUpPhrases)
            {
                if (lowerInput.Contains(phrase))
                    return true;
            }
            return false;
        }

        private string? TryExtractFavouriteTopic(string lowerInput)
        {
            // Detect "I am interested in X" or "I like X"
            var patterns = new[] { "interested in ", "i like ", "i love ", "passionate about " };
            foreach (string pattern in patterns)
            {
                if (lowerInput.Contains(pattern))
                {
                    int idx = lowerInput.IndexOf(pattern) + pattern.Length;
                    string remainder = lowerInput.Substring(idx).Split(' ')[0].TrimEnd('.', ',', '!', '?');
                    // Check if the extracted word is a known keyword
                    if (_keywords.GetAllKeywords().Contains(remainder))
                        return remainder;
                }
            }
            return null;
        }

        private string BuildPersonalisedOpener(string? keyword)
        {
            if (_memory.HasFavouriteTopic() && keyword == _memory.FavouriteTopic)
                return $"As someone interested in {_memory.FavouriteTopic}, here's something especially relevant: ";

            if (_memory.HasName())
                return $"{_memory.UserName}, ";

            return string.Empty;
        }

        private static string CapitaliseFirst(string s)
        {
            if (string.IsNullOrEmpty(s)) return s;
            return char.ToUpper(s[0]) + s.Substring(1).ToLower();
        }
    }
}
