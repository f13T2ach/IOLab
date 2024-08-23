#include <Arduino.h>
#include <ArduinoJson.h>
#include <vector>

String vectorToString(const std::vector<int>& vec);
std::vector<int> jsonArrayToVector(const JsonArray& jsonArray);