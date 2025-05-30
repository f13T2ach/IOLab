/**
 * IOLab - A Full-Stack Experimentation Platform
 *
 * Copyright (C) 2024 Redmaple
 * IOCore.cpp
 */

#include <Arduino.h>
#include <ArduinoJson.h>

#include "IOCore.h"
#include "Utils/Convert.h"
#include "Utils/Generic.h"
#include "Utils/JsonUtils.h"

#include "Nyard/Nyard.hpp"
#include "Nyard/NyardPorts.h"
#include "Nyard/NyardDrivers.h"
#include "System.h"

using namespace std;

#define MAX_SENSORS 10

// 静态注册传感器模板
Nyard *sensors[MAX_SENSORS];
int sensorCount = 0;

// 动态运行中的传感器
std::vector<Nyard *> runningSensors;

unsigned int lastUpdate = 0;

// ========== 工具函数 ==========
std::vector<int> getPinsByName(String name)
{
    auto item = nyardPorts.find(name);
    if (item == nyardPorts.end())
    {
        return {};
    }
    return item->second;
}

void addRunningSensor(Nyard *sensor)
{
    runningSensors.push_back(sensor);
}

void removeRunningSensorByPort(const String &port)
{
    for (auto it = runningSensors.begin(); it != runningSensors.end(); ++it)
    {
        if ((*it)->getPinName() == port)
        {
            delete *it; // 释放资源
            runningSensors.erase(it);
            break;
        }
    }
}

// ========== 初始化 ==========
void initNyard()
{
    sensors[sensorCount++] = new SampleSensor(100000);
    sensors[sensorCount++] = new CO2Sensor(100001);
    sensors[sensorCount++] = new pHSensor(100002);
    sensors[sensorCount++] = new ORPSensor(100003);
    sensors[sensorCount++] = new TempSensor(100004);
    sensors[sensorCount++] = new LightSensor(100005);
}

void initAuthentication()
{
    
}

// ========== 协议通信 ==========
bool handleAuthentication(String content)
{
    content.trim();
    println("验证", content);
    return content == ReceivedRequest;
}

void sendHandShake()
{
    JsonDocument doc;
    deserializeJson(doc, ShankeHanded);
    JsonUtils<String, std::vector<int>>::addKeysToJsonArray(nyardPorts, doc, "ports");
    serializeJson(doc, Serial);
    Serial.println();
}

// ========== 端口配置 ==========
void handlePinConfig(String port, int serviceID)
{
    println("配置", "接到对 " + port + " 采用方案 " + serviceID + " 监听的配置");

    std::vector<int> pins = getPinsByName(port);
    if (pins.empty())
    {
        throwError("固件异常", 10010, "配置端口");
        return;
    }

    for (int pin : pins)
    {
        if (isPinOccupied(pin))
        {
            throwError("引脚已被占用", 10010, "配置端口");
            return;
        }
    }

    for (int i = 0; i < sensorCount; i++)
    {
        if (sensors[i]->getId() == serviceID)
        {
            Nyard *sensor = sensors[i]->clone(); // clone 实例

            sensor->setPins(pins, port);
            sensor->setupPins();
            sensor->begin();
            occupyPins(pins); // 占用引脚

            addRunningSensor(sensor);

            println("配置后的引脚: " + vectorToString(sensor->getPins()), "Debug");

            throwCallback("成功配置 " + sensor->getName() + " 到插口 " + port, 10010, "配置端口");
            return;
        }
    }

    throwError("未找到服务ID " + String(serviceID), 10010, "配置端口");
}

void removePinConfig(String port)
{
    std::vector<int> pins = getPinsByName(port);
    for (Nyard *sensor : runningSensors)
    {
        if (sensor->getPins() == pins)
        {
            sensor->close();
            releasePins(pins);
            removeRunningSensorByPort(port);
            throwCallback("成功移除 " + port + " 上的传感器", 10011, "移除");
            return;
        }
    }

    throwError("引脚 " + port + " 没有被占用", 10011, "移除");
}

// ========== 命令处理 ==========
void handleCommands(String command)
{
    if (command == Disconnect)
    {
        resetFunc();
    }
    else if (command.startsWith(EnablePort))
    {
        vector<string> parts = splitMyCommand(command.c_str(), "#");
        String id = getSingleParameter(parts, 1);
        int sid = -1;
        try
        {
            sid = std::stoi(getSingleParameter(parts, 2));
        }
        catch (...)
        {
            throwError("给定服务 ID 异常", 10010, "配置端口");
            return;
        }
        handlePinConfig(id, sid);
    }
    else if (command.startsWith(DisablePort))
    {
        vector<string> parts = splitMyCommand(command.c_str(), "#");
        String id = getSingleParameter(parts, 1);
        removePinConfig(id);
    }
}

// ========== Arduino 主程序 ==========
void setup()
{
    Serial.begin(115200);
    initAuthentication();
    initNyard();

    while (true)
    {
        Serial.println(Authentication);
        if (Serial.available())
        {
            String content = Serial.readString();
            if (handleAuthentication(content))
            {
                break;
            }
        }
        delay(500);
    }

    sendHandShake();
}

void loop()
{
    if (Serial.available())
    {
        String command = Serial.readString();
        command.replace("\n", "");
        handleCommands(command);
    }

    bool flag = false;
    for (Nyard *sensor : runningSensors)
    {
        if (sensor == nullptr || !sensor->getIsRunning())
            continue;

        if (millis() - lastUpdate >= sensor->getPreload())
        {
            flag = true;
            outdata(sensor);
            continue;
        }
    }
    if (flag)   //todo 为每个实例单独分配更新时间，而不是一概而论
    {
        lastUpdate = millis();
    }
    

    
}
