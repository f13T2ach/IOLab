#include <Arduino.h>
#include "Generic.h"
#include <iostream>
#include <ArduinoJson.h>
#include <vector>
#include "Nyard/Nyard.hpp"
#include "Nyard/NyardPorts.h"
using namespace std;
void println(String tag,String content){
    Serial.println("["+tag+"]"+content);
}

// 此处与MonitorPageViewModel对应
void throwError(String error,int id,String title){
    // 创建 JSON 格式的字符串
    JsonDocument doc;
    doc["command"] = "Message";
    doc["type"] = "error";
    doc["title"] = title;
    doc["id"] = id;
    doc["msg"] = error;
    
    // 使用 Serial.println 输出 JSON
    serializeJson(doc, Serial);
    Serial.println();
}

void throwCallback(String content,int id,String title){
    JsonDocument doc;
    doc["command"] = "Message";
    doc["type"] = "callback";
    doc["title"] = title;
    doc["id"] = id;
    doc["msg"] = content;
    
    // 使用 Serial.println 输出 JSON
    serializeJson(doc, Serial);
    Serial.println();
}

void outdata( Nyard* sensor){
    JsonDocument doc;
    
    doc["command"] = "Collect";
    doc["pin"] = sensor->getPinName();
    doc["data"] = sensor->collectData();
    serializeJson(doc, Serial);
    Serial.println();
    
}


// 分割函数：将字符串按照指定的分隔符进行分割并返回一个 vector
vector<string> splitMyCommand(const char* str, const char* delimiter)
{
    vector<string> result;
    char* pc = strdup(str); // 复制字符串以避免修改原字符串
    if (!pc) return result; // 检查内存分配

    char* token = strtok(pc, delimiter); // 使用分隔符分割字符串
    while (token != NULL)
    {
        result.push_back(string(token)); // 将每个分割结果存储到 vector 中
        token = strtok(NULL, delimiter);
    }

    free(pc); // 释放内存
    return result;
}

// 查询函数：根据索引返回 vector 中的某个子串
const char* getSingleParameter(const vector<string>& parts, int index)
{
    if (index >= 0 && static_cast<size_t>(index) < parts.size())
    {
        return parts[index].c_str(); // 返回索引对应的子串
    }
    return NULL; // 如果索引超出范围，返回 NULL
}