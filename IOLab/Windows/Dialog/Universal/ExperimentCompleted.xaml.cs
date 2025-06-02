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

using iNKORE.UI.WPF.Modern.Controls;
using IOLab.Core.Models;
using IOLab.Core.Properties;
using IOLab.Core.Util.LabAI;
using IOLab.Network.API;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace IOLab.Windows.Dialog.Universal
{
    /// <summary>
    /// ExperimentCompleted.xaml 的交互逻辑
    /// </summary>
    public partial class ExperimentCompleted : ContentDialog
    {
        private static readonly HttpClient client = new HttpClient();

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
            string apiUrl;
            string modelName;

            if (Settings.Default.RunLocalModel)
            {
                apiUrl = ThirdParty.LocalURL;
                modelName = ThirdParty.LocalModelName;
            }
            else
            {
                apiUrl = ThirdParty.AliyunDeepseekURL;
                modelName = ThirdParty.RemoteModelName;
            }



            var requestBody = new
            {
                model = modelName,  // 请根据实际情况替换模型名称
                messages = new[] {
                    new { role = "system", content =  TextPrompts.ROLE_PROMPT },
                    new { role = "user", content = userMessage }
                },
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


            try
            {
                using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();

                using var stream = await response.Content.ReadAsStreamAsync();
                using var reader = new StreamReader(stream);

                // 读取SSE格式的流数据
                StringBuilder messageBuffer = new StringBuilder();
                DateTime lastUpdateTime = DateTime.Now;

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
                                var responseObject = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(jsonData);
                                string deltaContent = responseObject.choices[0].delta?.content;

                                if (!string.IsNullOrEmpty(deltaContent))
                                {
                                    // 更新消息内容
                                    messageBuffer.Append(deltaContent);

                                    // 控制UI更新频率，每隔10毫秒更新一次
                                    if ((DateTime.Now - lastUpdateTime).TotalMilliseconds > 10)
                                    {
                                        lastUpdateTime = DateTime.Now;

                                        // 使用Dispatcher保证UI线程更新
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
                                    AISummary.Text = $"解析错误 {ex.Message}");
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() =>
                                    AISummary.Text = $"请求错误 {ex.Message}");
            }
        }




    }
}
