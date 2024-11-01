using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using IOLab.Core.Generic;
using IOLab.Core.Properties;
using IOLab.Network.API;
using IOLab.Network.Result;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IOLab.Network
{
    public class Auth
    {
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string Email { get; private set; }

        private string _token;
        public string Token
        {
            get => _token;
            private set
            {
                _token = value;
            }
        }

        public int GroupId { get; private set; } // 添加 groupid 属性
        public int Experience { get; private set; } // 添加 exp 属性

        private static readonly HttpClient client = new HttpClient();
        private static readonly string BasicUrl = IONetAPI.Servers[Settings.Default.ServerIndex];

        public Auth(string username, string password, string email = null)
        {
            Username = username;
            Password = password;
            Email = email;
        }

        public Auth(string token)
        {
            Token = token;
        }

        // 使用 token 获取用户信息的异步方法
        public async Task<AuthResult> LoginViaTokenAsync()
        {
            var values = new Dictionary<string, string> { { "token", Token } };
            var content = new StringContent(JsonConvert.SerializeObject(values), Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync(BasicUrl + IONetAPI.AuthFetchPrivately, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var responseData = JObject.Parse(responseString);

                    if (responseData.ContainsKey("code") && responseData["code"].ToObject<int>() == 0)
                    {
                        var data = (JObject)responseData["data"];
                        Username = data["username"].ToString();
                        Email = data["email"].ToString();
                        GroupId = data["groupid"].ToObject<int>();
                        Experience = data["exp"].ToObject<int>();

                        ActionBundle.DoChangeLoginStat(true);

                        Debug.WriteLine("User info retrieved successfully.");
                        return new AuthResult(true, 0, "登录成功", data);
                    }
                    return new AuthResult(false, responseData["code"].ToObject<int>(), responseData["msg"].ToString());
                }
                return new AuthResult(false, -1, "登录失效，请重试", null);
            }
            catch (Exception ex)
            {
                return new AuthResult(false, -1, $"IOLab 自动登录时发生了错误: {ex.Message}", null);
            }
        }

        // 登录方法
        public async Task<AuthResult> LoginAsync()
        {
            var values = new Dictionary<string, string>
            {
                { "username", Username },
                { "password", Password }
            };

            var content = new StringContent(JsonConvert.SerializeObject(values), Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync(BasicUrl + IONetAPI.AuthLogin, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var responseData = JObject.Parse(responseString);

                    if (responseData != null && responseData.ContainsKey("code") && responseData["code"].ToObject<int>() == 0)
                    {
                        // 保存Token、GroupId 和 Experience
                        Email = responseData["data"]["email"].ToString();
                        Token = responseData["data"]["token"].ToString();
                        GroupId = responseData["data"]["groupid"].ToObject<int>(); // 获取 groupid
                        Experience = responseData["data"]["exp"].ToObject<int>(); // 获取 exp

                        // 触发 Action 登录状态改变
                        ActionBundle.DoChangeLoginStat(true);


                        return new AuthResult(true, 0, "登录成功", (JObject)responseData["data"]);
                    }
                    return new AuthResult(false, responseData["code"].ToObject<int>(), responseData["msg"].ToString());
                }
                return new AuthResult(false, -1, "登录失败，检查用户名和密码", null);
            }
            catch (Exception ex)
            {
                return new AuthResult(false, -1, "登录发生错误: " + ex.Message, null);
            }
        }

        /// <summary>
        /// 注册方法
        /// </summary>
        /// <returns>注意，此处是否成功指的是与服务器连接是否成功，而不是业务是否完成</returns>
        public async Task<AuthResult> RegisterAsync()
        {
            var values = new Dictionary<string, string>
            {
                { "username", Username },
                { "password", Password },
                { "email", Email }
            };

            var content = new StringContent(JsonConvert.SerializeObject(values), Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync(BasicUrl + IONetAPI.AuthRegist, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var responseData = JObject.Parse(responseString);

                    if (responseData != null && responseData["code"].ToObject<int>() == 0)
                    {
                        return new AuthResult(true, 0, responseData["msg"].ToString(), null);
                    }
                    return new AuthResult(false, responseData["code"].ToObject<int>(), responseData["msg"].ToString());
                }
                return new AuthResult(false, -1, "无法连接服务器", null);
            }
            catch (Exception ex)
            {
                return new AuthResult(false, -1, "连接服务器注册发生错误: " + ex.Message, null);
            }
        }

        // 注销方法
        public async Task<AuthResult> LogoutAsync()
        {
            var values = new Dictionary<string, string>
            {
                { "token", Token } // 使用登录后保存的Token
            };

            var content = new StringContent(JsonConvert.SerializeObject(values), Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync(BasicUrl + IONetAPI.AuthLogout, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var responseData = JObject.Parse(responseString);

                    if (responseData != null && responseData["code"].ToObject<int>() == 0)
                    {
                        CeaseSessionAsync();
                        return new AuthResult(true, 0, responseData["msg"].ToString()+ "\n无论注销是否成功，IOLab 都会登出账户。", null);
                    }
                    return new AuthResult(false, responseData["code"].ToObject<int>(), responseData["msg"].ToString());
                }
                return new AuthResult(false, -1, "IOLab 无法注销你的账户\n登录状态可能不正常，请重新登录", null);
            }
            catch (Exception ex)
            {
                return new AuthResult(false, -1, "连接服务器出现错误: " + ex.Message, null);
            }
        }

        /// <summary>
        /// 终止会话
        /// </summary>
        /// <returns></returns>
        public async Task<AuthResult> CeaseSessionAsync()
        {
            Token = "";
            Username = "";
            ActionBundle.DoChangeLoginStat(false);
            return new AuthResult(false, -1, "会话终止成功，账户已登出", null);
        }
    }
}
