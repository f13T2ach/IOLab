#include<Arduino.h>
#include<iostream>
#include<map>
#include<vector>
using namespace std;

std::map<String, std::vector<int>> ports = {
        {"pH", {A0}},
        {"I2C", {A0,1}},
};