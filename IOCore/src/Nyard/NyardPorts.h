#ifndef NYARDPORTS_H
#define NYARDPORTS_H

#include<Arduino.h>
#include<iostream>
#include<map>
#include<vector>
#include <unordered_map>
using namespace std;


extern std::map<String, std::vector<int>> nyardPorts;  // 声明 nyardPorts
extern std::unordered_map<int, bool> pinUsage;         // 声明 pinUsage


// 检查引脚是否已被占用
bool isPinOccupied(int pin);

// 设置引脚为占用状态
void occupyPins(const std::vector<int>& pins);
// 释放引脚
void releasePins(const std::vector<int>& pins);
String findPinName(std::vector<int>&pins);
#endif