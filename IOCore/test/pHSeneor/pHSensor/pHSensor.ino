/*
 # This sample code is used to test the pH meter Pro V1.0 on DFROBOT ESP32E.
*/
#include<Arduino.h>
#define SensorPin 10            //
#define Offset 21.677           // deviation compensate
#define LED 2                   // ESP32 onboard LED is usually on GPIO 2
#define samplingInterval 20
#define printInterval 800
#define ArrayLenth  40          // times of collection
int pHArray[ArrayLenth];        // Store the average value of the sensor feedback
int pHArrayIndex = 0;

void setup(void)
{
  pinMode(LED, OUTPUT);  
  Serial.begin(115200);         // ESP32 typically uses a higher baud rate like 115200
  Serial.println("pH meter experiment on ESP32!");    // Test the serial monitor
}

void loop(void)
{
  static unsigned long samplingTime = millis();
  static unsigned long printTime = millis();
  static float pHValue, voltage;
  
  if (millis() - samplingTime > samplingInterval)
  {
    
      pHArray[pHArrayIndex++] = analogRead(SensorPin);  // Read from the sensor
      if (pHArrayIndex == ArrayLenth) pHArrayIndex = 0;
      
      // ESP32's analog reference voltage is 3.3V, so adjust the voltage calculation
      voltage = avergearray(pHArray, ArrayLenth) * 3.3 / 4095;  
      pHValue = -5.8887 * voltage + Offset;
      
      // Clamp the pH value between 0 and 14
      if (pHValue <= 0.0) { pHValue = 0.0; }
      if (pHValue > 14.0) { pHValue = 14.0; }
      
      samplingTime = millis();
  }
  
  if (millis() - printTime > printInterval)   // Every 800 milliseconds, print a numerical, and toggle the LED
  {
    Serial.print("Voltage: ");
    Serial.print(voltage, 2);
    Serial.print("    pH value: ");
    Serial.println(pHValue, 2);
    
    digitalWrite(LED, !digitalRead(LED));  // Toggle LED state
    printTime = millis();
  }
}

double avergearray(int* arr, int number)
{
  int i, max, min;
  double avg;
  long amount = 0;
  
  if (number <= 0) {
    Serial.println("Error: number for the array to average is invalid!");
    return 0;
  }
  
  if (number < 5) {   // Less than 5, calculate directly
    for (i = 0; i < number; i++) {
      amount += arr[i];
    }
    avg = amount / number;
    return avg;
  } else {
    // Find the min and max values in the array
    if (arr[0] < arr[1]) {
      min = arr[0]; max = arr[1];
    } else {
      min = arr[1]; max = arr[0];
    }
    
    for (i = 2; i < number; i++) {
      if (arr[i] < min) {
        amount += min;        // arr < min
        min = arr[i];
      } else if (arr[i] > max) {
        amount += max;        // arr > max
        max = arr[i];
      } else {
        amount += arr[i];     // min <= arr <= max
      }
    }
    
    avg = (double)amount / (number - 2);  // Exclude min and max
  }
  
  return avg;
}
