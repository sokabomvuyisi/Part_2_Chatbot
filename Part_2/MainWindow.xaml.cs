using System;
using System.IO;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CybersecurityChatbot
{
    public partial class MainWindow : Window
    {
        private readonly ChatBot _chatBot;

        public MainWindow()
        {
            InitializeComponent();
            _chatBot = new ChatBot();
            LoadLogo();
            PlayVoiceGreeting();
            Loaded += (s, e) => NameInputBox.Focus();
        }

        // ── PAGE 1 ─────────────────────────────────────

        private void NameInputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                GoToChatPage();
        }

        private void GoToChatPage()
        {
            string name = NameInputBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                NameInputBox.BorderBrush = new SolidColorBrush(Colors.Red);
                NameInputBox.Focus();
                return;
            }

            ChatSubtitle.Text = $"Hello, {CapitaliseFirst(name)}! How can I help you today?";
            WelcomePage.Visibility = Visibility.Collapsed;
            ChatPage.Visibility = Visibility.Visible;

            string greeting = _chatBot.ProcessInput(name);
            AppendBotMessage(greeting);
            UserInputBox.Focus();
        }

        // ── PAGE 2 ─────────────────────────────────────

        private void SendButton_Click(object sender, RoutedEventArgs e) => SendMessage();

        private void UserInputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                SendMessage();
        }

        private void SendMessage()
        {
            string userInput = UserInputBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(userInput)) return;

            AppendUserMessage(userInput);
            UserInputBox.Clear();
            UserInputBox.Focus();

            StatusText.Text = "CyberBot is thinking...";
            string response = _chatBot.ProcessInput(userInput);
            AppendBotMessage(response);
            StatusText.Text = "Type a message and press Enter or click Send";
            ScrollToBottom();
        }

        // ── Chat bubbles ───────────────────────────────

        private void AppendUserMessage(string message)
        {
            var container = new Border
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(80, 4, 0, 4),
                Background = new SolidColorBrush(Color.FromRgb(0, 73, 105)),
                CornerRadius = new CornerRadius(14, 2, 14, 14),
                Padding = new Thickness(14, 10, 14, 10),
                MaxWidth = 580
            };

            var label = new TextBlock
            {
                Text = $"You  •  {DateTime.Now:HH:mm}",
                Foreground = new SolidColorBrush(Color.FromRgb(100, 200, 240)),
                FontSize = 10,
                Margin = new Thickness(0, 0, 0, 4)
            };

            var text = new TextBlock
            {
                Text = message,
                Foreground = Brushes.White,
                FontSize = 13,
                TextWrapping = TextWrapping.Wrap
            };

            var stack = new StackPanel();
            stack.Children.Add(label);
            stack.Children.Add(text);
            container.Child = stack;
            ChatPanel.Children.Add(container);
            ScrollToBottom();
        }

        private void AppendBotMessage(string message)
        {
            var container = new Border
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(0, 4, 80, 4),
                Background = new SolidColorBrush(Color.FromRgb(22, 27, 34)),
                BorderBrush = new SolidColorBrush(Color.FromRgb(33, 38, 45)),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(2, 14, 14, 14),
                Padding = new Thickness(14, 10, 14, 10),
                MaxWidth = 640
            };

            var label = new TextBlock
            {
                Text = $"🤖 CyberBot  •  {DateTime.Now:HH:mm}",
                Foreground = new SolidColorBrush(Color.FromRgb(0, 191, 255)),
                FontSize = 10,
                Margin = new Thickness(0, 0, 0, 4)
            };

            var text = new TextBlock
            {
                Text = message,
                Foreground = new SolidColorBrush(Color.FromRgb(230, 237, 243)),
                FontSize = 13,
                TextWrapping = TextWrapping.Wrap,
                LineHeight = 20
            };

            var stack = new StackPanel();
            stack.Children.Add(label);
            stack.Children.Add(text);
            container.Child = stack;
            ChatPanel.Children.Add(container);
            ScrollToBottom();
        }

        private void ScrollToBottom()
        {
            ChatScrollViewer.UpdateLayout();
            ChatScrollViewer.ScrollToBottom();
        }

        // ── Startup helpers ────────────────────────────

        private void LoadLogo()
        {
            try
            {
                string logoPath = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory, "cyberbot_logo.png");

                if (File.Exists(logoPath))
                {
                    var bmp = new BitmapImage(new Uri(logoPath, UriKind.Absolute));
                    WelcomeLogo.Source = bmp;
                    HeaderLogo.Source = bmp;
                }
            }
            catch (Exception) { }
        }

        private void PlayVoiceGreeting()
        {
            try
            {
                string wavPath = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory, "greeting.wav");

                if (File.Exists(wavPath))
                    new SoundPlayer(wavPath).Play();
            }
            catch (Exception) { }
        }

        private static string CapitaliseFirst(string s)
        {
            if (string.IsNullOrEmpty(s)) return s;
            return char.ToUpper(s[0]) + s.Substring(1).ToLower();
        }

        private void UserInputBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}