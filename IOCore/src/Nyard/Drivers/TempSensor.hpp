
#define PWR_SUPPLY (5)
// 温度传感器与湿度传感器实际是同一传感器 但是表的限制 需要两个采集方案
#ifndef TEMPSENSOR_HPP
#define TEMPSENSOR_HPP
#include <Wire.h>

// SHT30 IIC 7位地址默认为0x44
#define Addr_SHT30 0x44

#include "../Nyard.hpp"
class TempSensor : public Nyard
{
private: // 定义使能和数据
    int scl;
    int sda;

public:
    // 构造函数
    TempSensor(int id)
        : Nyard(id), scl(-1), sda(-1)
    {
        preload = 1000;
        pins = {};
        name = "TempSensor";
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
            Wire.begin(sda, scl);
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
        unsigned int data[6]; // 存储获取到的六个数据
        // 开始IIC
        // 写地址
        Wire.beginTransmission(Addr_SHT30);
        // 发送测量命令 0x2C06,但是因为IIC一次只能发一个8位数据，所以得发两次
        Wire.write(0x2C);
        Wire.write(0x06);
        // 停止IIC
        Wire.endTransmission();
        // 等待500ms是等待SHT30器件测量数据，实际上这个时间可以很短
        delay(50);

        // 请求获取6字节的数据，然后会存到8266的内存里
        Wire.requestFrom(Addr_SHT30, 6);

        // 读取6字节的数据
        // 这六个字节分别为：温度8位高数据，温度8位低数据，温度8位CRC校验数据
        //                湿度8位高数据，湿度8位低数据，湿度8位CRC校验数据
        if (Wire.available() == 6)
        {
            data[0] = Wire.read();
            data[1] = Wire.read();
            data[2] = Wire.read();
            data[3] = Wire.read();
            data[4] = Wire.read();
            data[5] = Wire.read();
        }

        // 然后计算得到的数据，要转化为摄氏度、华氏度、相对湿度
        float cTemp = ((((data[0] * 256.0) + data[1]) * 175) / 65535.0) - 45;
        return cTemp;

        // delay(500);
    }
};

#endif
