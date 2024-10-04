#include <unordered_map>
#include "NyardPorts.h"
#include <vector>
#include <map>
#include <iostream>
#include <Arduino.h>
using namespace std;

std::map<String, std::vector<int>> nyardPorts = {
        {"AL", {A0}},
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
