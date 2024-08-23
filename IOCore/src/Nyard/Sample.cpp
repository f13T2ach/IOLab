/**
 * IOLab - A Full-Stack Experimentation Platform
 *
 * Copyright (C) 2024 Redmaple. All rights reserved.
 *
 * Sample.cpp is part of the IOLab project.
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

#include "Nyard.h"
#include<Arduino.h>
// 实现一个具体的采集器类
class Sample : public Nyard {
public:
    // 构造函数，调用基类构造函数
    Sample(int pin) : Nyard(pin) {}

    // 重写基类的虚函数
    void begin() override {
        pinMode(sensorPin, INPUT);
    }

    int collectData() override {
        // 模拟采集数据
        return analogRead(sensorPin);
    }
};

