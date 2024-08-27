#include <avr/io.h>

int freeMemory() {
  extern int __heap_start;
  extern void *__brkval;
  int v;
  return (int)&v - ((int)__brkval + sizeof(int));
}

void setup() {
  Serial.begin(9600);
  delay(1000); // 等待串口初始化完成

  // 输出系统启动时的内存量
  Serial.print("Total SRAM: ");
  Serial.print(1024); // 以字节为单位的总内存量（示例值，以实际硬件为准）
  Serial.println(" bytes");

  Serial.print("Free memory at startup: ");
  Serial.print(freeMemory());
  Serial.println(" bytes");
}

void loop() {
  // 主循环代码
}
