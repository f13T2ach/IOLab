#include<Arduino.h>
#ifndef GENERIC_H
#define GENERIC_H

#include <vector>
#include <string>
#include "Nyard/Nyard.hpp"
#include "Nyard/NyardPorts.h"
using namespace std;
void println(String content,String tag = "");
void throwError(String error,int id = 0,String title = "异常");
void throwCallback(String content,int id = 0,String title = "消息");
vector<string> splitMyCommand(const char* str, const char* delimiter);
void outdata( Nyard* sensor);
#endif
const char* getSingleParameter(const vector<string>& parts, int index);