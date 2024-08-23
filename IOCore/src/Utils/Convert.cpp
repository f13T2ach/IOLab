#include <Arduino.h>
#include <vector>
#include <ArduinoJson.h>

String vectorToString(const std::vector<int>& vec) {
    String result = "";
    for (size_t i = 0; i < vec.size(); ++i) {
        result += String(vec[i]);
        if (i < vec.size() - 1) {
            result += ",";  // 添加分隔符（如逗号）
        }
    }
    return result;
}

std::vector<int> jsonArrayToVector(const JsonArray& jsonArray) {
    std::vector<int> vec;
    
    for (const auto& item : jsonArray) {
        if (item.is<int>()) {  // 检查 item 是否是整数
            vec.push_back(item.as<int>());
        } else {
            // 如果不是整数，可以选择抛出错误、记录警告或处理其他类型
        }
    }
    
    return vec;
}
