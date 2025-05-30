#include <Wire.h>
#define ADDR 0b0100011 // 规定了地址后其实根本不需要给出的引脚，因为esp32只有一个i2c通道

#define PWR_SUPPLY (5)
// 温度传感器与湿度传感器实际是同一传感器 但是表的限制 需要两个采集方案
#ifndef LIGHTSENSOR_HPP
#define LIGHTSENSOR_HPP
#include <Wire.h>

#include "../Nyard.hpp"
class LightSensor : public Nyard
{
private: // 定义使能和数据
    int scl;
    int sda;

public:
    // 构造函数
    LightSensor(int id)
        : Nyard(id), scl(-1), sda(-1)
    {
        preload = 1000;
        pins = {};
        name = "LightSensor";
    } // 初始引脚设置为空值

    // 动态设置引脚
    void setPins(const std::vector<int> &parg, String pin) override
    {

        if (parg.size() == 2)
        {
            scl = parg[0]; // 设置引脚
            sda = parg[1];
            Nyard::setPins(parg, pin);
        }
    }

    // 设置引脚模式
    void setupPins() override
    {
        if (sda != -1)
        {
            // pinMode(dataPin, INPUT);
        }
    }

    void begin() override
    {
        if (sda != -1)
        {
            Wire.begin();
            Wire.beginTransmission(ADDR);
            Wire.write(0b00000001);
            Wire.endTransmission();
        }
    }

    void close() override
    {
        if (sda != -1)
        {
            // digitalWrite(dataPin, LOW); // 关闭传感器（如果不需要使能可不继承）
        }
        Nyard::close();
    }

    float collectData() override
    {
        int val = 0;
        // reset
        Wire.beginTransmission(ADDR);
        Wire.write(0b00000111);
        Wire.endTransmission();

        Wire.beginTransmission(ADDR);
        Wire.write(0b00100000);
        Wire.endTransmission();
        // typical read delay 120ms
        delay(20);
        Wire.requestFrom(ADDR, 2); // 2byte every time
        for (val = 0; Wire.available() >= 1;)
        {
            char c = Wire.read();
            // Serial.println(c, HEX);
            val = (val << 8) + (c & 0xFF);
        }
        val = val / 1.2;
        return (float)val;
    }

    Nyard *clone() const override
    {
        return new LightSensor(*this);
    }
};

#endif
