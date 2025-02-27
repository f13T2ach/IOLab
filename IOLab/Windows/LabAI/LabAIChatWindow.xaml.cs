using IOLab.Network.API;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace IOLab.Windows.LabAI
{
    public partial class LabAIChatWindow : Window
    {
        public ObservableCollection<Message> Messages { get; set; }
        private static readonly HttpClient client = new HttpClient();

        public LabAIChatWindow()
        {
            InitializeComponent();
            Messages = new ObservableCollection<Message>();
            MessagesListBox.ItemsSource = Messages;
        }

        // 发送按钮点击事件
        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            await SendMessageAsync();
        }

        // 监听回车键发送消息
        private async void UserInputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                await SendMessageAsync();
            }
        }

        // 发送消息
        private async Task SendMessageAsync()
        {
            string userMessage = UserInputTextBox.Text.Trim();
            if (!string.IsNullOrEmpty(userMessage))
            {
                // 添加用户消息
                Messages.Add(new Message { Content = userMessage, BackgroundColor = "Blue" });

                // 调用 DeepSeek API 获取流式回复
                await GetAIResponseStreamAsync(userMessage);

                // 清空输入框
                UserInputTextBox.Clear();

                // 滚动到最底部
                MessagesListBox.ScrollIntoView(Messages[Messages.Count - 1]);
            }
        }

        // 使用流式接收 DeepSeek API 回复（修复版）
        public async Task GetAIResponseStreamAsync(string userMessage)
        {
            string apiKey = ThirdParty.DeepseekAIToken;
            string apiUrl = "https://api.deepseek.com/v1/chat/completions";

            var requestBody = new
            {
                model = "deepseek-chat",
                messages = new[] { new { role = "user", content = userMessage } },
                stream = true // 启用流式传输
            };

            // 创建请求
            using var request = new HttpRequestMessage(HttpMethod.Post, apiUrl)
            {
                Content = new StringContent(
                    Newtonsoft.Json.JsonConvert.SerializeObject(requestBody),
                    Encoding.UTF8,
                    "application/json")
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            // 创建用于跟踪AI消息的对象
            Message aiMessage = new Message { Content = "", BackgroundColor = "Gray" };

            try
            {
                using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();

                using var stream = await response.Content.ReadAsStreamAsync();
                using var reader = new StreamReader(stream);

                // 读取SSE格式的流数据
                StringBuilder messageBuffer = new StringBuilder();
                DateTime lastUpdateTime = DateTime.Now;

                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync();
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    // 处理SSE数据格式：data: {...}
                    if (line.StartsWith("data:"))
                    {
                        var jsonData = line.Substring(5).Trim();
                        if (jsonData == "[DONE]") break;

                        try
                        {
                            var responseObject = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(jsonData);
                            string deltaContent = responseObject.choices[0].delta?.content;

                            if (!string.IsNullOrEmpty(deltaContent))
                            {
                                // 更新消息内容
                                messageBuffer.Append(deltaContent);

                                // 控制UI更新频率，每隔200毫秒更新一次
                                if ((DateTime.Now - lastUpdateTime).TotalMilliseconds > 200)
                                {
                                    lastUpdateTime = DateTime.Now;

                                    // 使用Dispatcher保证UI线程更新
                                    Dispatcher.Invoke(() =>
                                    {
                                        // 首次添加消息
                                        if (aiMessage.Content.Length == 0)
                                        {
                                            Messages.Add(aiMessage);
                                        }

                                        // 更新消息内容
                                        aiMessage.Content = messageBuffer.ToString();

                                        // 强制刷新UI
                                        MessagesListBox.Items.Refresh();
                                        MessagesListBox.ScrollIntoView(aiMessage);
                                    });
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Dispatcher.Invoke(() =>
                                Messages.Add(new Message
                                {
                                    Content = $"解析错误: {ex.Message}",
                                    BackgroundColor = "Red"
                                }));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() =>
                    Messages.Add(new Message
                    {
                        Content = $"请求失败: {ex.Message}",
                        BackgroundColor = "Red"
                    }));
            }
        }


    }

    public class Message : INotifyPropertyChanged
    {
        private string _content;
        public string Content
        {
            get => _content;
            set { _content = value; OnPropertyChanged(); }
        }

        public string BackgroundColor { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
