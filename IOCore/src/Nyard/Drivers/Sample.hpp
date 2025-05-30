/**
 * IOLab - A Full-Stack Experimentation Platform
 *
 * Copyright (C) 2024 Redmaple. All rights reserved.
 *
 * Sample.hpp is part of the IOLab project.
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

#ifndef SAMPLE_HPP
#define SAMPLE_HPP

#include "../Nyard.hpp"


class SampleSensor : public Nyard {
private: // 定义使能和数据
    int dataPin;
    int powerPin;
    
public:
    // 构造函数
    SampleSensor(int id)
        : Nyard(id), dataPin(-1), powerPin(-1) {
            preload = 20000; //preload 是延迟时间
            pins = {};
            name = "ExampleSensor";
        }  // 初始引脚设置为空值

    // 动态设置引脚
    void setPins(const std::vector<int>& parg,String pin) override {
        if (parg.size() == 2) {
            dataPin = parg[0];  // 设置引脚
            powerPin = parg[1];
        }
        Nyard::setPins(parg,pin); // 调用基类
    }

    // 设置引脚模式
    void setupPins() override {
        if (dataPin != -1 && powerPin != -1) {
            pinMode(dataPin, INPUT);
            pinMode(powerPin, OUTPUT);
        }
    }

    void begin() override {
        if (powerPin != -1) {
            digitalWrite(powerPin, HIGH);  // 打开传感器（如果不需要使能可不继承）
        }
    }

    void close() override {
        if (powerPin != -1) {
            digitalWrite(powerPin, LOW);  // 关闭传感器（如果不需要使能可不继承）
        }
        Nyard::close(); // 必须销毁端口占用
    }

    float collectData() override {
        // 采样允许小段时间的delay 但是总等待长度不能超过250ms(通常最多8个引脚接入)
        if (dataPin != -1) {
            return analogRead(dataPin);  // 模拟采集数据
        }
        return -1; // 错误码，表示未正确配置引脚
    }

    Nyard *clone() const override
    {
        return new SampleSensor(*this);
    }
};

#endif
