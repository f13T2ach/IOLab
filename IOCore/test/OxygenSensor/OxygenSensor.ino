const int sensorPin = 32; // 定义传感器引脚

void setup() {
  Serial.begin(9600); // 初始化串口通信
}

void loop() {
  int sum = 0;
  int count = 50;

  // 读取传感器数据
  for (int i = 0; i < count; i++) {
    int sensorValue = analogRead(sensorPin);
    sum += sensorValue;
    delay(2);
  }

  int avgValue = sum / count;
  float voltage = avgValue * (5.0 / 4096.0);
  float o2Concentration = voltage * 48.046 - 3.00;
  int displayValue = (int)(o2Concentration * 10);

  // 通过串口输出浓度值
  Serial.print("ADC=");
  Serial.print(avgValue);
  Serial.print(" O2=");
  Serial.print(displayValue / 100);
  Serial.print(displayValue % 100 / 10);
  Serial.print(".");
  Serial.print(displayValue % 10);
  Serial.println("%");

  delay(1000);
}
