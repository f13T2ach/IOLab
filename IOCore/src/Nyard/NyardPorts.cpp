#include <unordered_map>
#include "NyardPorts.h"
#include <vector>
#include <map>
#include <iostream>
#include <Arduino.h>
using namespace std;

std::map<String, std::vector<int>> nyardPorts = {
        {"AL", {32}},
        {"AL_1", {33}},
        {"AL_2", {25}},
        {"I2C",{22,21}}
};

std::unordered_map<int, bool> pinUsage;  // 记录引脚占用状态

// 检查引脚是否已被占用
bool isPinOccupied(int pin) {
    return pinUsage.find(pin) != pinUsage.end() && pinUsage[pin];
}

// 设置引脚为占用状态
void occupyPins(const std::vector<int>& pins) {
    for (int pin : pins) {
        pinUsage[pin] = true;  // 标记引脚为占用
    }
}

// 释放引脚
void releasePins(const std::vector<int>& pins) {
    for (int pin : pins) {
        pinUsage[pin] = false;  // 标记引脚为释放
    }
}

// 查询引脚名
String findPinName(const std::vector<int>& pins){
    std::map<String, std::vector<int>>::iterator iter;
    for (iter = nyardPorts.begin(); iter != nyardPorts.end(); ++iter) 
    {
        if(iter->second == pins){
            return iter->first;
        }
    }
    // TODO 处理警告
}