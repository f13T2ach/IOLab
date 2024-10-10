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
#include "Nyard/Nyard.cpp"
#include "Nyard/NyardPorts.h"
#include "Nyard/NyardDrivers.h"

#include "System.h"

using namespace std;

// 预定义最大采集器数量
#define MAX_SENSORS 10

// 创建一个指针数组来存储所有的采集器
Nyard *sensors[MAX_SENSORS];
int sensorCount = 0;
unsigned int lastUpdate = 0;

// 载入Nyard
void initNyard()
{
  sensors[sensorCount++] = new SampleSensor(100000);
  sensors[sensorCount++] = new CO2Sensor(100001);
  sensors[sensorCount++] = new pHSensor(100002);
  sensors[sensorCount++] = new ORPSensor(100003);
  
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
void handlePinConfig(String port, int serviceID)
{
    println("配置", "接到对 " + port + " 采用方案 " + serviceID + " 监听的配置");

    // 模拟接收到的引脚配置
    std::vector<int> pins = getPinsByName(port);
    if (pins.size() == 0)
    {
        throwError("固件异常", 10010, "配置端口");
        return;  // 添加返回以避免继续执行
    }

    for (int pin : pins)
    {
        if (isPinOccupied(pin))
        {
            throwError("引脚已被占用", 10010, "配置端口");
            return;
        }
    }

    // 遍历所有传感器并匹配id
    for (int i = 0; i < sensorCount; i++)
    {
        //println("Debug", "第" + String(i) + "个传感器是" + String(sensors[i]->getId()));
        if (sensors[i]->getId() == serviceID)
        {
            // 动态设置引脚
            sensors[i]->setPins(pins,port); 

            // 配置引脚
            sensors[i]->setupPins();  
            sensors[i]->begin();       // 启动传感器
            occupyPins(pins);

            // 确保成功设置引脚后再获取引脚
            println("配置后的引脚: " + vectorToString(sensors[i]->getPins()), "Debug");

            throwCallback("成功配置 "+sensors[i]->getName() + " 到插口 "+port, 10010, "配置端口");
            break; // 找到匹配的传感器后退出循环
        }
    }
}


//根据引脚释放服务
void removePinConfig(String port){
  std::vector<int> pins = getPinsByName(port);
  bool flag = false;
  //Serial.println(String(sensorCount));
  for (int i = 0; i < sensorCount; i++)
  {
    //println(vectorToString(sensors[i]->getPins()),String(i));
    if (sensors[i]->getPins() == pins)
    {
      sensors[i]->close();
      flag = true;
      throwCallback("成功移除 "+port+" 上的传感器",10011,"移除");
      break;                     // 找到匹配的传感器后退出循环
    }
  }
  
  /*
  if(!flag){
    throwError("引脚"+port+"没有被占用",10011,"移除");
  }*/
}

void handleCommands(String command)
{
  if (command == Disconnect)
  {
    resetFunc();
  }
  else if (command.startsWith(EnablePort)) // 启用端口
  {
    vector<string> parts = splitMyCommand(command.c_str(), "#");
    String id = getSingleParameter(parts, 1);
    int sid = -1;
    try
    {
      sid = std::stoi(getSingleParameter(parts, 2));
    }
    catch (const std::invalid_argument &e)
    {
      // 输入不是有效整数
      throwError("给定服务 ID 异常", 10010, "配置端口");
    }
    handlePinConfig(id, sid);
  }
  else if (command.startsWith(DisablePort)) // 关闭端口和端口上跑的传感器
  {
    vector<string> parts = splitMyCommand(command.c_str(), "#");
    String id = getSingleParameter(parts, 1);
    removePinConfig(id);
  }
}

void setup()
{
  Serial.begin(115200);
  initAuthentication();
  initNyard();

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

    delay(500);
  }
  // 发送本机详情
  //Serial.println("传感器已添加: " + String(sensorCount));
  sendHandShake();
}

void loop()
{
  if (Serial.available())
  {
    String command =Serial.readString();
    command.replace("\n", "");  //必须替换,蜜汁bug
    handleCommands(command);

  }
  // 采集并回报
  for(Nyard* sensor : sensors){
    if(sensor==NULL){
      continue;
    }
    if(!sensor->getIsRunning()){
      continue; // 忽略未在运行的监视器
    }
    if(millis()-lastUpdate >= sensor->getPreload()){ //如果达到更新时间
      lastUpdate = millis(); // 更新
      outdata(sensor);
    }
  }

}
