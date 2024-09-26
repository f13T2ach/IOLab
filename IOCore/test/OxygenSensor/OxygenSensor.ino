/*
=====引脚说明======
温湿度8号引脚
二氧化碳A0号引脚
氧气A1号引脚
光照SCL-A5~SDA-A4
*/
#define         MG_PIN                       (A0)
#define         BOOL_PIN                     (2)
#define         DC_GAIN                      (0.1035)
#define         READ_SAMPLE_TIMES            (5)
#define         ZERO_POINT_VOLTAGE           (0.296)
#define         REACTION_VOLTGAE             (0.027)
#include <Wire.h>
#define ADDR 0b0100011
#include <DHT.h>
#define DHTPIN 8 
#define DHTTYPE DHT11
DHT dht(DHTPIN, DHTTYPE);

float           CO2Curve[3]  =  {2.602,ZERO_POINT_VOLTAGE,(REACTION_VOLTGAE/(2.602-3))};

int analogInPin = A1; // 定义模拟输入引脚o2
float concentration;
int sensorValue = 0; // 存储模拟输入的值
float voltage = 0;
void setup()
{
    Serial.begin(9600);
    dht.begin();

    pinMode(BOOL_PIN, INPUT);
    digitalWrite(BOOL_PIN, HIGH);

    while (!Serial) {;}
    Wire.begin();
    Wire.beginTransmission(ADDR);
    Wire.write(0b00000001);
    Wire.endTransmission();

    Serial.println();

    Serial.println("设备上线！");
    Serial.println("请按回车键继续...");
    while(!Serial.available()){
    }
    Serial.println();
}

void loop()
{
    sensorValue = analogRead(analogInPin);                             // 读取模拟输入的值
    voltage = sensorValue * (5.0 / 1023.0);                            // 将模拟输入的值转换为电压值
    concentration=(voltage*48.046)+0.7;
    
    //获取数据
    int percentage;
    float volts;
    Serial.println();
    Serial.print("氧气浓度 ");
    Serial.print(concentration);
    Serial.print("%");
    Serial.println();

  // 读温度或湿度要用250毫秒
    float h = dht.readHumidity();//读湿度
    float t = dht.readTemperature();//读温度，默认为摄氏度


    int val = 0;    //初始化光照值
  Wire.beginTransmission(ADDR);
  Wire.write(0b00000111);
  Wire.endTransmission();
  Wire.beginTransmission(ADDR);
  Wire.write(0b00100000);
  Wire.endTransmission();
  delay(120);
  /*计算光照*/
  Wire.requestFrom(ADDR, 2);      //每次2byte
  for (val = 0; Wire.available() >= 1; ) {
    char c = Wire.read();
    val = (val << 8) + (c & 0xFF);
  }
  val = val / 1.2;
  /*输出光照数值*/
  

    volts = MGRead(MG_PIN);
    percentage = MGGetPercentage(volts,CO2Curve);
    delay(1000);



}

float MGRead(int mg_pin)
{
    int i;
    float v=0;

    for (i=0;i<READ_SAMPLE_TIMES;i++) {
        v += analogRead(mg_pin);
        delay(50);
    }
    v = (v/READ_SAMPLE_TIMES) *5/1024 ;
    return v;
}
int  MGGetPercentage(float volts, float *pcurve)
{
   if ((volts/DC_GAIN )>=ZERO_POINT_VOLTAGE) {
      return -1;
   } else {
      return pow(10, ((volts/DC_GAIN)-pcurve[1])/pcurve[2]+pcurve[0]);
   }
}
