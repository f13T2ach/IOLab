
#define PWR_SUPPLY (5)

#ifndef PHSENSOR_HPP
#define PHSENSOR_HPP

#include "../Nyard.hpp"
int pHSense = 32;
int samples = 10;
float adc_resolution = 4095.0;
class pHSensor : public Nyard
{
private: // 定义使能和数据
    int dataPin;

public:
    // 构造函数
    pHSensor(int id)
        : Nyard(id), dataPin(-1)
    {
        preload = 1000;
        pins = {};
        name = "pHSensor";
    } // 初始引脚设置为空值

    // 动态设置引脚
    void setPins(const std::vector<int> &parg, String pin) override
    {

        if (parg.size() == 1)
        {
            dataPin = parg[0]; // 设置引脚
        }
        Nyard::setPins(parg, pin);
    }

    // 设置引脚模式
    void setupPins() override
    {
        if (dataPin != -1)
        {
            // pinMode(dataPin, INPUT);
        }
    }

    void begin() override
    {
        if (dataPin != -1)
        {
            // pinMode(dataPin, INPUT);      // set pin to input
            // digitalWrite(dataPin, HIGH); // turn on pullup resistors
        }
    }

    void close() override
    {
        if (dataPin != -1)
        {
            // digitalWrite(dataPin, LOW); // 关闭传感器（如果不需要使能可不继承）
        }
        Nyard::close();
    }

    float collectData() override
    {
        float measurings = 0;

        for (int i = 0; i < samples; i++)
        {
            measurings += analogRead(pHSense);
            delay(10);
        }

        float voltage = 4.7 / adc_resolution * measurings / samples;
        return ph(voltage);

        // delay(500);
    }

    float ph(float voltage)
    {
        return 7 + ((2.5 - voltage) / 0.18);
    }

    Nyard *clone() const override
    {
        return new pHSensor(*this);
    }
};

#endif
