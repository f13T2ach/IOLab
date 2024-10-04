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

class Nyard {
public:
    // 构造函数
    Nyard(int id) : id(id) {}

    // 虚函数（可被子类重写）
    virtual void setupPins() = 0;
    virtual void begin() = 0;

    // 在关闭传感器时释放引脚
    virtual void close() {
        releasePins();  // 释放引脚
    }

    virtual int collectData() = 0;
    
    // 设置引脚
    virtual void setPins(const std::vector<int>& pins) {
        this->pins = pins;
    }

    // 获取传感器id
    int getId() const {
        return id;
    }

    // 获取预加载状态
    int getPreload() const {
        return preload;
    }

    String getName() const {
        return name;
    }

    // 获取pin
    std::vector<int> getPins() const{
        return this->pins;
    }

protected:
    int id;
    int preload;
    std::vector<int> pins;  // 记录传感器使用的引脚
    String name;

    // 释放引脚（占用引脚在解析时就执行）
    void releasePins() {
        for (int pin : pins) {
            pinUsage[pin] = false;  // 更新引脚状态，标记为未占用
        }
        pins.clear();  // 清空引脚列表
    }
};

#endif


