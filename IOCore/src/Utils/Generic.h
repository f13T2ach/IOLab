#include<Arduino.h>
#ifndef SPLIT_MY_COMMAND_H
#define SPLIT_MY_COMMAND_H

#include <vector>
#include <string>
using namespace std;
void println(String content,String tag = "");
vector<string> splitMyCommand(const char* str, const char* delimiter);
#endif
const char* getSingleParameter(const vector<string>& parts, int index);