#include <Arduino.h>
#include "IOCore.h"
void setup(){
  Serial.begin(115200);
  // 如果没有收到认证消息 就持续输出
  while(!Serial.available())
  {
    Serial.println("ok");
    delay(150);
  }
}

void loop(){
}

// 发送认证消息
void sendAuthentication(){

}

// 处理认证消息
bool handleAuthentication(String content){
  return false;
}

// 握手
void sendHandShake(){

}