#ifndef JSONUTILS_TPP
#define JSONUTILS_TPP

#include "JsonUtils.h"

template <typename TKey, typename TValue>
void JsonUtils<TKey, TValue>::addKeysToJsonArray(const std::map<TKey, TValue>& myMap, JsonDocument& jsonDoc, const char* arrayName) {
    JsonArray jsonArray;
    
    // 检查是否已经存在对应的数组字段
    if (!jsonDoc.containsKey(arrayName)) {
        jsonArray = jsonDoc[arrayName].to<JsonArray>();
    } else {
        jsonArray = jsonDoc[arrayName].to<JsonArray>();
    }
    
    // 遍历map对象，将每个键添加到jsonArray中
    for (const auto& pair : myMap) {
        jsonArray.add(pair.first);
    }
}

template <typename TKey, typename TValue>
bool JsonUtils<TKey, TValue>::deserializeJsonFromString(JsonDocument& jsonDoc, const char* jsonString) {
    DeserializationError error = deserializeJson(jsonDoc, jsonString);
    if (error) {
        Serial.print(F("[工具]JsonUtils无法实现 JsonDocument 到 char* 的转换:"));
        Serial.println(error.f_str());
        return false;
    }
    return true;
}

#endif // JSONUTILS_TPP
