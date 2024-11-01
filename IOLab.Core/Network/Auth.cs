using System;
using System.Collections.Generic;
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
                // 触发 Action 登录状态改变
                ActionBundle.DoChangeLoginStat(value);
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
                        Token = responseData["data"]["token"].ToString();
                        GroupId = responseData["data"]["groupid"].ToObject<int>(); // 获取 groupid
                        Experience = responseData["data"]["exp"].ToObject<int>(); // 获取 exp
                        return new AuthResult(true, 0, "Login successful.", (JObject)responseData["data"]);
                    }
                    return new AuthResult(false, responseData["code"].ToObject<int>(), responseData["msg"].ToString());
                }
                return new AuthResult(false, -1, "Login request failed.", null);
            }
            catch (Exception ex)
            {
                return new AuthResult(false, -1, "An error occurred during login: " + ex.Message, null);
            }
        }

        // 注册方法
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
                        return new AuthResult(true, 0, "Registration successful.", null);
                    }
                    return new AuthResult(false, responseData["code"].ToObject<int>(), responseData["msg"].ToString());
                }
                return new AuthResult(false, -1, "Registration request failed.", null);
            }
            catch (Exception ex)
            {
                return new AuthResult(false, -1, "An error occurred during registration: " + ex.Message, null);
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
                        Token = ""; // 注销后清空Token
                        return new AuthResult(true, 0, "Logout successful.", null);
                    }
                    return new AuthResult(false, responseData["code"].ToObject<int>(), responseData["msg"].ToString());
                }
                return new AuthResult(false, -1, "Logout request failed.", null);
            }
            catch (Exception ex)
            {
                return new AuthResult(false, -1, "An error occurred during logout: " + ex.Message, null);
            }
        }

        /// <summary>
        /// 终止会话
        /// </summary>
        /// <returns></returns>
        public async Task<AuthResult> CeaseSessionAsync()
        {
            Token = "";
            return new AuthResult(false, -1, "Success In Ceasing Session", null);
        }
    }
}
