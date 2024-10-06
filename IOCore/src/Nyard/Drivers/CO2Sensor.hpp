

#define         DC_GAIN                      (8.5)   //define the DC gain of amplifier
/***********************Software Related Macros************************************/
#define         READ_SAMPLE_INTERVAL         (30)    //define how many samples you are going to take in normal operation
#define         READ_SAMPLE_TIMES            (5)     //define the time interval(in milisecond) between each samples in
                                                     //normal operation

/**********************Application Related Macros**********************************/
//These two values differ from sensor to sensor. user should derermine this value.
#define         ZERO_POINT_VOLTAGE           (0.296) //define the output of the sensor in volts when the concentration of CO2 is 400PPM
#define         REACTION_VOLTGAE             (0.027) //define the voltage drop of the sensor when move the sensor from air into 1000ppm CO2

/*****************************Globals***********************************************/
float           CO2Curve[3]  =  {2.602,ZERO_POINT_VOLTAGE,(REACTION_VOLTGAE/(2.602-3))};
                                                     //two points are taken from the curve.
                                                     //with these two points, a line is formed which is
                                                     //"approximately equivalent" to the original curve.
                                                     //data format:{ x, y, slope}; point1: (lg400, 0.324), point2: (lg4000, 0.280)
                                                     //slope = ( reaction voltage ) / (log400 –log1000)

#ifndef CO2SENSOR_HPP
#define CO2SENSOR_HPP

#include "../Nyard.hpp"

class CO2Sensor : public Nyard
{
private: // 定义使能和数据
    int dataPin;

public:
    // 构造函数
    CO2Sensor(int id)
        : Nyard(id), dataPin(-1)
    {
        preload = 2000;
        pins = {};
        name = "CO2Sensor";
    } // 初始引脚设置为空值

    // 动态设置引脚
    void setPins(const std::vector<int> &parg,String pin) override
    {
        
        if (parg.size() == 1)
        {
            dataPin = parg[0]; // 设置引脚
        }
        Nyard::setPins(parg,pin);
    }

    // 设置引脚模式
    void setupPins() override
    {
        if (dataPin != -1)
        {
            //pinMode(dataPin, INPUT);
        }
    }

    void begin() override
    {
        if (dataPin != -1)
        {
            //pinMode(dataPin, INPUT);      // set pin to input
            //digitalWrite(dataPin, HIGH); // turn on pullup resistors
        }
    }

    void close() override
    {
        if (dataPin != -1)
        {
            //digitalWrite(dataPin, LOW); // 关闭传感器（如果不需要使能可不继承）
        }
        Nyard::close();
    }

    float collectData() override
    {
        if (dataPin == -1)
        {
            return -1;
        }

        int percentage;
        float volts;

        volts = MGRead(dataPin);

        percentage = MGGetPercentage(volts, CO2Curve);
        if (percentage == -1)
        {
            return 0.04f;
        }
        else
        {
            float pfloat = (float)percentage;
            float nfloat = 10000.00;
            return pfloat/nfloat; //转为百分比
        }

        //delay(500);
    }

    /*****************************  MGRead *********************************************
    Input:   dataPin - analog channel
    Output:  output of SEN-000007
    Remarks: This function reads the output of SEN-000007
    ************************************************************************************/
    float MGRead(int dataPin)
    {
        int i;
        float v = 0;

        for (i = 0; i < READ_SAMPLE_TIMES; i++)
        {
            v += analogRead(dataPin);
            delay(READ_SAMPLE_INTERVAL);
        }
        v = (v / READ_SAMPLE_TIMES) * 5 / 1024;
        return v;
    }

    /*****************************  MQGetPercentage **********************************
    Input:   volts   - SEN-000007 output measured in volts
             pcurve  - pointer to the curve of the target gas
    Output:  ppm of the target gas
    Remarks: By using the slope and a point of the line. The x(logarithmic value of ppm)
             of the line could be derived if y(MG-811 output) is provided. As it is a
             logarithmic coordinate, power of 10 is used to convert the result to non-logarithmic
             value.
    ************************************************************************************/
    int MGGetPercentage(float volts, float *pcurve)
    {
        if ((volts / DC_GAIN) >= ZERO_POINT_VOLTAGE)
        {
            return -1;
        }
        else
        {
            return pow(10, ((volts / DC_GAIN) - pcurve[1]) / pcurve[2] + pcurve[0]);
        }
    }
};

#endif
