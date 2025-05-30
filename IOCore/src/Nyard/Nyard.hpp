/**
 * IOLab - A Full-Stack Experimentation Platform
 *
 * Copyright (C) 2024 Redmaple. All rights reserved.
 *
 * Nyard.h is part of the IOLab project.
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

#ifndef NYARD_HPP
#define NYARD_HPP

#include <Arduino.h>
#include "NyardPorts.h"
#include <vector>

class Nyard
{
public:
    // 构造函数
    Nyard(int id) : id(id) {}

    // 虚析构函数（必须！防止内存泄漏）
    virtual ~Nyard() {}

    // 必须由派生类实现的功能
    virtual void setupPins() = 0;
    virtual void begin() = 0;
    virtual float collectData() = 0;

    // 复制自身（用于 clone 模式）
    virtual Nyard *clone() const = 0;

    // 关闭传感器并释放资源
    virtual void close()
    {
        releasePins();
    }

    // 设置引脚
    virtual void setPins(const std::vector<int> &pins, String pin)
    {
        this->pinName = pin;
        this->pins = pins;
        this->isRunning = true;
    }

    // 获取传感器基本信息
    int getId() const { return id; }
    int getPreload() const { return preload; }
    String getName() const { return name; }
    bool getIsRunning() const { return isRunning; }
    std::vector<int> getPins() const { return pins; }
    String getPinName() const { return pinName; }

protected:
    int id;
    String pinName;
    int preload = 1000; // 默认预加载时间为1000ms
    bool isRunning = false;
    std::vector<int> pins;
    String name = "Unnamed";

    // 子类可用：设置基本信息
    void setName(String sensorName) { name = sensorName; }
    void setPreload(int ms) { preload = ms; }

    // 引脚释放逻辑
    void releasePins()
    {
        for (int pin : pins)
        {
            pinUsage[pin] = false;
        }
        pins.clear();
        isRunning = false;
        pinName = "";
    }
};

#endif
