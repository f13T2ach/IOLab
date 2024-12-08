using IOLab.Core.Config;
using IOLab.Core.Properties;
using IOLab.Network.API;
using IOLab.Network.Result;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;

// 注意
// (JObject)responseData["data"] 不一定正确


/* Example
var auth = new Auth("username", "password");
await auth.LoginAsync();

if (auth.Token != null)
{
    var workshop = new Workshop(auth);

    // 上传文案
    var uploadResult = await workshop.UploadWorkshopItemAsync("path/to/file.json", "我的文案", "这是文案内容", new List<string> { "标签1", "标签2" }, DateTime.Now);
    Console.WriteLine(uploadResult.Message);

    // 浏览文案
    var browseResult = await workshop.BrowseWorkshopItemsAsync(1, 10);
    Console.WriteLine(browseResult.Message);

    // 下载文件
    var downloadResult = await workshop.DownloadWorkshopItemAsync(1, "path/to/save/file.json");
    Console.WriteLine(downloadResult.Message);
}
else
{
    Console.WriteLine("登录失败，无法继续操作。");
}

*/
namespace IOLab.Network
{
    public class Workshop
    {
        private readonly Auth _auth;
        private HttpClient client = new HttpClient();
        private string BasicUrl = IONetAPI.Servers[Settings.Default.ServerIndex];

        public Workshop(Auth auth)
        {
            _auth = auth;
            client.Timeout = TimeSpan.FromMilliseconds(InternalConfig.OperationTimeout);
        }

        /// <summary>
        /// 上传文案
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="title">标题</param>
        /// <param name="content">文案内容</param>
        /// <param name="tags">标签列表</param>
        /// <param name="createdAt">创建时间</param>
        /// <returns></returns>
        public async Task<AuthResult> UploadWorkshopItemAsync(string filePath, string title, string content, List<string> tags, DateTime createdAt)
        {
            using (var formData = new MultipartFormDataContent())
            {
                // 添加文件
                var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                var fileContent = new StreamContent(fileStream);
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/octet-stream");
                formData.Add(fileContent, "json_file", Path.GetFileName(filePath));

                // 添加其他表单数据
                formData.Add(new StringContent(_auth.Token), "token");
                formData.Add(new StringContent(title), "title");
                formData.Add(new StringContent(content), "content");
                formData.Add(new StringContent(createdAt.ToString("o")), "created_at"); // ISO 8601 格式
                formData.Add(new StringContent(JsonConvert.SerializeObject(tags)), "tags");

                try
                {
                    var response = await client.PostAsync(BasicUrl + IONetAPI.WorkshopUpload, formData);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        var responseData = JObject.Parse(responseString);

                        if (responseData.ContainsKey("code") && responseData["code"].ToObject<int>() == 0)
                        {
                            return new AuthResult(true, 0, "上传成功", (JObject)responseData["data"]);
                        }
                        return new AuthResult(false, responseData["code"].ToObject<int>(), responseData["msg"].ToString());
                    }
                    return new AuthResult(false, -1, "上传失败: " + response.StatusCode + " - " + response.ReasonPhrase, null);
                }
                catch (Exception ex)
                {
                    return new AuthResult(false, -1, "上传时发生错误: " + ex.Message, null);
                }
            }
        }

        /// <summary>
        /// 浏览文案
        /// </summary>
        /// <param name="page">页数</param>
        /// <param name="perPage">每页的文案数</param>
        /// <returns></returns>
        public async Task<AuthResult> BrowseWorkshopItemsAsync(int page = 1, int perPage = 10)
        {
            var values = new Dictionary<string, string>
            {
                { "pages", page.ToString() },
                { "per_page", perPage.ToString() }
            };

            var content = new StringContent(JsonConvert.SerializeObject(values), Encoding.UTF8, "application/json");

            try
            {
                var response = await client.GetAsync($"{BasicUrl}{IONetAPI.WorkshopBrowse}?pages={page}&per_page={perPage}");

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var responseData = JObject.Parse(responseString);

                    if (responseData.ContainsKey("code") && responseData["code"].ToObject<int>() == 0)
                    {
                        return new AuthResult(true, 0, "浏览成功", responseData);
                    }
                    return new AuthResult(false, responseData["code"].ToObject<int>(), responseData["msg"].ToString());
                }
                return new AuthResult(false, -1, "浏览失败: " + response.StatusCode + " - " + response.ReasonPhrase, null);
            }
            catch (Exception ex)
            {
                return new AuthResult(false, -1, "浏览时发生错误: " + ex.Message, null);
            }
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="itemId">文案ID</param>
        /// <param name="savePath">保存路径</param>
        /// <returns></returns>
        public async Task<AuthResult> DownloadWorkshopItemAsync(int itemId, string savePath)
        {
            try
            {
                var response = await client.GetAsync($"{BasicUrl}{IONetAPI.WorkshopDownload.Replace("{item_id}", itemId.ToString())}");

                if (response.IsSuccessStatusCode)
                {
                    var fileStream = await response.Content.ReadAsStreamAsync();
                    using (var fs = new FileStream(savePath, FileMode.Create, FileAccess.Write))
                    {
                        await fileStream.CopyToAsync(fs);
                    }
                    return new AuthResult(true, 0, "下载成功", null);
                }
                return new AuthResult(false, -1, "下载失败: " + response.StatusCode + " - " + response.ReasonPhrase, null);
            }
            catch (Exception ex)
            {
                return new AuthResult(false, -1, "下载时发生错误: " + ex.Message, null);
            }
        }
    }
}
