
#define PWR_SUPPLY (5)

#ifndef ORPSENSOR_HPP
#define ORPSENSOR_HPP

#include "../Nyard.hpp"
#define VOLTAGE 5.0 // system voltage   //32 可用！ 已修改
#define OFFSET 0    // zero drift voltage
#define LED 13      // operating instructions

double orpValue;

#define ArrayLenth 5 // times of collection

int orpArray[ArrayLenth];
int orpArrayIndex = 0;

class ORPSensor : public Nyard
{
private: // 定义使能和数据
    int dataPin;

public:
    // 构造函数
    ORPSensor(int id)
        : Nyard(id), dataPin(-1)
    {
        preload = 1000;
        pins = {};
        name = "ORPSensor";
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
        for(int i =0;i<ArrayLenth;i++)
        {
            orpArray[orpArrayIndex++] = analogRead(dataPin); // read an analog value every 20ms
            if (orpArrayIndex == ArrayLenth)
            {
                orpArrayIndex = 0;
            }
            //    orpValue=((30*(double)VOLTAGE*1000)-(75*avergearray(orpArray, ArrayLenth)*VOLTAGE*1000/1024))/75-OFFSET;   //convert the analog value to orp according the circuit
            
            delay(20);
        }

        orpValue = 2000 - (avergearray(orpArray, ArrayLenth) * VOLTAGE * 1000 / 4095) - OFFSET;

        return (float)orpValue;

    }

    double avergearray(int *arr, int number)
    {
        int i;
        int max, min;
        double avg;
        long amount = 0;
        if (number <= 0)
        {
            printf("Error number for the array to avraging!/n");
            return 0;
        }
        if (number < 5)
        { // less than 5, calculated directly statistics
            for (i = 0; i < number; i++)
            {
                amount += arr[i];
            }
            avg = amount / number;
            return avg;
        }
        else
        {
            if (arr[0] < arr[1])
            {
                min = arr[0];
                max = arr[1];
            }
            else
            {
                min = arr[1];
                max = arr[0];
            }
            for (i = 2; i < number; i++)
            {
                if (arr[i] < min)
                {
                    amount += min; // arr<min
                    min = arr[i];
                }
                else
                {
                    if (arr[i] > max)
                    {
                        amount += max; // arr>max
                        max = arr[i];
                    }
                    else
                    {
                        amount += arr[i]; // min<=arr<=max
                    }
                } // if
            } // for
            avg = (double)amount / (number - 2);
        } // if
        return avg;
    }

    Nyard *clone() const override
    {
        return new ORPSensor(*this);
    }
};

#endif
