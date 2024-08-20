#include <Arduino.h>
#include "IOCore.h"
void setup(){
  Serial.begin(115200);
  // 如果没有收到认证消息 就持续输出
}

void loop(){
}

// 发送认证消息
void sendAuthentication(){

}

// 处理认证消息
bool handleAuthentication(String content){

}

// 握手
void sendHandShake(){

}