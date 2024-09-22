#include <Arduino.h>
#include "Generic.h"
#include <iostream>
#include <vector>
using namespace std;
void println(String tag,String content){
    Serial.println("["+tag+"]"+content);
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

/*
    // 使用 '#' 作为分隔符分割字符串
    vector<string> parts = splitString(str, "#");

    // 根据索引查询子串
    const char* ptr = getSingleParameter(parts, ind);


*/