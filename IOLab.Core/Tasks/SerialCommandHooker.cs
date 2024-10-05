using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using IOLab.Core.Config;
using IOLab.Core.Generic;
using Newtonsoft.Json.Linq;

public class SerialCommandHooker
{
    private readonly ConcurrentDictionary<int, TaskCompletionSource<(string msg, bool isSuccess)>> _pendingResponses = new();

    public SerialCommandHooker()
    {
        // 订阅 ActionBundle.NewestSerialCommand 事件
        ActionBundle.NewestSerialCommand += OnNewestSerialCommand;
    }

    public async Task<(string msg, bool isSuccess)> SendMessageAsync(string message, int expectedId)
    {
        // 发送消息
        InstanceBundle.connectTool._SerialPort.WriteLine(message);

        // 创建 TaskCompletionSource 来等待响应
        var tcs = new TaskCompletionSource<(string msg, bool isSuccess)>();
        _pendingResponses.TryAdd(expectedId, tcs);

        try
        {
            // 等待响应，设置超时机制
            var completedTask = await Task.WhenAny(tcs.Task, Task.Delay(InternalConfig.OperationTimeout)); // 5秒超时
            if (completedTask == tcs.Task)
            {
                return await tcs.Task; // 返回成功的消息和状态
            }
            else
            {
                throw new TimeoutException($"未收到 ID 为 {expectedId} 的响应.");
            }
        }
        finally
        {
            // 清理已完成的任务
            _pendingResponses.TryRemove(expectedId, out _);
        }
    }

    private void OnNewestSerialCommand(JObject message)
    {
        // 解析 JObject，获取 ID 和消息
        int id = (int)message["id"];
        var msg = message["msg"]?.Value<string>();
        var type = message["type"]?.Value<string>();

        // 判断是否成功 (根据 "type" 字段)
        bool isSuccess = type != "error";

        if ( id!=null && _pendingResponses.TryGetValue(id, out var tcs))
        {
            // 如果 ID 匹配，设置任务完成结果 (返回消息和状态)
            tcs.SetResult((msg, isSuccess));
        }
    }
}
