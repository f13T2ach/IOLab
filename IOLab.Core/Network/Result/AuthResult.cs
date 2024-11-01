using Newtonsoft.Json.Linq;

namespace IOLab.Network.Result
{
    public class AuthResult
    {
        public bool Success { get; set; }
        public int Code { get; set; }
        public string Message { get; set; }
        public JObject Data { get; set; } // 存储请求的响应数据

        public AuthResult(bool success, int code, string message, JObject data = null)
        {
            Success = success;
            Code = code;
            Message = message;
            Data = data;
        }
    }
}
