/**
 * IOLab - A Full-Stack Experimentation Platform
 *
 * Copyright (C) 2024 Redmaple. All rights reserved.
 *
 * ExperimentCompleted.xaml.cs is part of the IOLab project.
 *
 * IOLab is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * IOLab is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with IOLab. If not, see <https://www.gnu.org/licenses/>.
 */

using System.IO;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System;
using iNKORE.UI.WPF.Modern.Controls;
using IOLab.Core.Models;
using IOLab.Core.Util.LabAI;
using IOLab.Network.API;
using Newtonsoft.Json;
using Windows.Media.Protection.PlayReady;
using System.Threading.Tasks;

namespace IOLab.Windows.Dialog.Universal
{
    /// <summary>
    /// ExperimentCompleted.xaml 的交互逻辑
    /// </summary>
    public partial class ExperimentCompleted : ContentDialog
    {
        public ExperimentCompleted(IOExperiment experiment)
        {
            InitializeComponent();

            PromptGenerator generator = new PromptGenerator();

            // 生成提示
            string prompt = generator.GeneratePrompt();

            // 显示提示
            ReadyForTest(prompt);
        }

        public ExperimentCompleted(string testStr)
        {
            InitializeComponent();

            ReadyForTest(testStr);
        }


        public async void ReadyForTest(string testStr)
        {
            await CallAiStreamAndUpdateUI(testStr);
        }
        public async Task CallAiStreamAndUpdateUI(string userMessage)
        {
            string apiKey = ThirdParty.DeepseekAIAliyunToken;  // 请替换为实际API密钥
            string apiUrl = ThirdParty.AliyunDeepseekURL;

            var requestBody = new
            {
                model = "qwen-plus",  // 请根据实际情况替换模型名称
                messages = new[] { new { role = "user", content = userMessage } },
                stream = true // 启用流式传输
            };

            // 创建请求
            using var request = new HttpRequestMessage(HttpMethod.Post, apiUrl)
            {
                Content = new StringContent(
                    JsonConvert.SerializeObject(requestBody),
                    Encoding.UTF8,
                    "application/json")
            };

            HttpClient client = new HttpClient();
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            try
            {
                using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();

                using var stream = await response.Content.ReadAsStreamAsync();
                using var reader = new StreamReader(stream);

                // 读取SSE格式的流数据
                StringBuilder messageBuffer = new StringBuilder();
                DateTime lastUpdateTime = DateTime.Now;

                // 异步读取流数据
                await Task.Run(async () =>
                {
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
                                var responseObject = JsonConvert.DeserializeObject<dynamic>(jsonData);
                                string deltaContent = responseObject.choices[0].delta?.content;

                                if (!string.IsNullOrEmpty(deltaContent))
                                {
                                    // 更新消息内容
                                    messageBuffer.Append(deltaContent);

                                    // 控制UI更新频率，每隔200毫秒更新一次
                                    if ((DateTime.Now - lastUpdateTime).TotalMilliseconds > 200)
                                    {
                                        lastUpdateTime = DateTime.Now;

                                        // 使用Dispatcher确保UI线程更新
                                        Dispatcher.Invoke(() =>
                                        {
                                            // 首次添加消息
                                            AISummary.Text = messageBuffer.ToString();
                                        });
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Dispatcher.Invoke(() =>
                                    AISummary.Text = $"解析失败：{ex.Message}");
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() =>
                    AISummary.Text = $"请求失败：{ex.Message}");
            }
        }



    }
}
