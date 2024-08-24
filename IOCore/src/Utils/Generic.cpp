#include <Arduino.h>
#include "Generic.h"
void println(String content,String tag){
    Serial.println("["+tag+"]"+content);
}