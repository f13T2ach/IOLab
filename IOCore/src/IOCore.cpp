/**
 * IOLab - A Full-Stack Experimentation Platform
 *
 * Copyright (C) 2024 Redmaple. All rights reserved.
 *
 * IOCore.cpp is part of the IOLab project.
 *
 * IOLab is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * IOLab is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with IOLab. If not, see <https://www.gnu.org/licenses/>.
 */

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
    return false; // 解析失败
  }
  const int* versionID = document["version"];
  if(versionID > &coreVersion){
    return false; // 协议太屌
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
  // 
}
