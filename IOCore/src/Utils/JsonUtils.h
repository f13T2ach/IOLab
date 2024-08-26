#ifndef JSONUTILS_H
#define JSONUTILS_H

#include <ArduinoJson.h>
#include <map>
#include <vector>
#include <string>

template <typename TKey, typename TValue>
class JsonUtils {
public:
    // 将map的键添加到JsonDocument中指定的数组字段
    static void addKeysToJsonArray(const std::map<TKey, TValue>& myMap, JsonDocument& jsonDoc, const char* arrayName);

    // 将字符串反序列化为JsonDocument
    static bool deserializeJsonFromString(JsonDocument& jsonDoc, const char* jsonString);
};

#include "JsonUtils.tpp"  // 包含模板实现文件

#endif // JSONUTILS_H
