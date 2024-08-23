#include <Arduino.h>
#include "IOCore.h"
#include <ArduinoJson.h>
using namespace std;

void loop(){
}

// 发送认证消息
void sendAuthentication(){

}

// 处理认证消息
bool handleAuthentication(String content){
  JsonDocument document;
  DeserializationError error = deserializeJson(document, content);
  if (error) {
    return false;
  }
  return true;
}

// 握手
void sendHandShake(){

}

void setup(){
  Serial.begin(115200);
  while(true)
  {
    // 收取信息
    if(Serial.available()){
      String content = Serial.readString();
      if(handleAuthentication(content)){
        break;
      }
    }
    Serial.println(Authentication);
    delay(150);
  }
}
