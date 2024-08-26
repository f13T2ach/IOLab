#include<Arduino.h>
#include<iostream>
#include<map>
#include<vector>
using namespace std;

std::map<String, std::vector<int>> nyardPorts = {
        {"pH", {A0}},
        {"I2C", {A0,1}},
};