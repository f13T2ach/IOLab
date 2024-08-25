#include <Arduino.h>
#include "Generic.h"
void println(String tag,String content){
    Serial.println("["+tag+"]"+content);
}