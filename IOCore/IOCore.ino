#include <Arduino.h>
#include "IOCore.h"
#include ""
void setup(){
  Serial.begin(115200);
  while(true)
  {
    // 收取信息
    if(Serial.available()){
      String content = Serial.readString();
      if(handleAuthentication()){
        break;
      }
    }
    Serial.println(Authentication);
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
  try{
    JsonObject& inputObject = jsonBuffer.parseObject(inputJson);
  }
  catch(){

  }
  return false;
}

// 握手
void sendHandShake(){

}