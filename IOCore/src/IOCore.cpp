/**
 * IOLab - A Full-Stack Experimentation Platform
 *
 * Copyright (C) 2024 Redmaple. All rights reserved.
 *
 * IOCore.cpp is part of the IOLab project.
 *
 * IOLab is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * IOLab is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with IOLab. If not, see <https://www.gnu.org/licenses/>.
 */

#include <Arduino.h>
#include <ArduinoJson.h>

#include "IOCore.h"
#include "Utils/Convert.h"
#include "Utils/Generic.h"
#include "Utils/JsonUtils.h"
// Nyard
#include "Nyard/Nyard.hpp"
#include "Nyard/NyardPorts.h"
#include "Nyard/NyardDrivers.h"

#include "System.h"

using namespace std;

// 预定义最大采集器数量
#define MAX_SENSORS 10

// 创建一个指针数组来存储所有的采集器
Nyard *sensors[MAX_SENSORS];
int sensorCount = 0;

// 载入Nyard
void initNyard()
{
  sensors[sensorCount++] = new SampleSensor(100000);
}

std::vector<int> getPinsByName(String name)
{
  auto item = nyardPorts.find(name);
  if (item == nyardPorts.end())
  {
    return {};
  }
  return nyardPorts[name];
}

// 载入认证消息
void initAuthentication()
{
}

// 处理认证消息
bool handleAuthentication(String content)
{
  content.trim();
  println("验证", content);

  if (content != ReceivedRequest)
  {
    return false;
  }
  return true;
}

// 握手消息
void sendHandShake()
{
  JsonDocument doc;
  deserializeJson(doc, ShankeHanded);
  JsonUtils<String, std::vector<int>>::addKeysToJsonArray(nyardPorts, doc, "ports");

  // 使用工具类方法将JsonDocument序列化并输出到串口
  serializeJson(doc, Serial);
  Serial.println(); // 换行
}

// 处理引脚配置指令
void handlePinConfig(const JsonDocument doc)
{

  int commandId = doc["id"]; // 模拟接收到的指令id

  // 模拟接收到的引脚配置
  std::vector<int> pins = getPinsByName(doc["port"]);
  if (pins.size() == 0)
  {
  }

  // 遍历所有传感器并匹配id
  for (int i = 0; i < sensorCount; i++)
  {
    if (sensors[i]->getId() == commandId)
    {
      sensors[i]->setPins(pins); // 动态设置引脚
      sensors[i]->setupPins();   // 配置引脚
      sensors[i]->begin();       // 启动传感器
      break;                     // 找到匹配的传感器后退出循环
    }
  }

  delay(1000);
}


void handleCommands(String command)
{
  if (command == Disconnect)
  {
    resetFunc();
  }
  else if (command.startsWith(EnablePort))
  {
     vector<string> parts = splitMyCommand(command.c_str(), "#");
     const char* id = getSingleParameter(parts, 1);
  }
}


void setup()
{
  Serial.begin(115200);
  initAuthentication();

  while (true)
  {
    Serial.println(Authentication);
    // 收取信息
    if (Serial.available())
    {
      String content = Serial.readString();
      if (handleAuthentication(content))
      {
        break;
      } // 失败就晾着它
    }

    delay(200);
  }
  // 发送本机详情
  sendHandShake();
}

void loop()
{
  if (Serial.available())
  {
    handleCommands(Serial.readString());
  }
}
