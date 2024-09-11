// 定义传感器的相关引脚
#define RX_PIN 2 // Arduino的RX引脚
#define TX_PIN 3 // Arduino的TX引脚

#include <SoftwareSerial.h>

// 创建SoftwareSerial对象，用于与传感器通信
SoftwareSerial mySerial(RX_PIN, TX_PIN);

void setup() {
  // 启动硬件串口与软件串口
  Serial.begin(9600);   // 设置Arduino串口
  mySerial.begin(9600); // 设置传感器串口

  // 提示准备就绪
  Serial.println("SH20-CH2O Sensor Ready...");
}

void loop() {
  // 检查是否有可用的传感器数据
  if (mySerial.available() > 0) {
    // 读取传感器的数据
    uint8_t data[9];  // 用于存储9字节的响应数据
    for (int i = 0; i < 9; i++) {
      data[i] = mySerial.read();
    }

    // 校验数据的起始位和校验位
    if (data[0] == 0xFF && data[8] == calculateChecksum(data)) {
      // 计算甲醛浓度，单位是PPB
      uint16_t ch2o_concentration = (data[3] << 8) | data[4];
      Serial.print("CH2O Concentration: ");
      Serial.print(ch2o_concentration);
      Serial.println(" PPB");

      // 如果需要将PPB转换为PPM，可以使用下面的公式
      float ch2o_concentration_ppm = ch2o_concentration / 1000.0;
      Serial.print("CH2O Concentration: ");
      Serial.print(ch2o_concentration_ppm);
      Serial.println(" PPM");
    } else {
      Serial.println("Data error or checksum mismatch.");
    }
  }

  delay(1000); // 1秒读取一次数据
}

// 校验和计算函数
uint8_t calculateChecksum(uint8_t* data) {
  uint8_t sum = 0;
  for (int i = 1; i < 8; i++) {
    sum += data[i];
  }
  return (~sum) + 1;
}
