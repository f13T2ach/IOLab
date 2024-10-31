#include <Arduino.h>

int pHSense = 32;
int samples = 10;
float adc_resolution = 4095.0;

void setup()
{
    Serial.begin(300);
    delay(100);
    Serial.println("cimpleo pH Sense");
}

float ph (float voltage) {
    return 7 + ((2.5 - voltage) / 0.18);
}

void loop()
{
    float measurings=0;

    for (int i = 0; i < samples; i++)
    {
        measurings += analogRead(pHSense);
        delay(10);

    }

    float voltage = 5 / adc_resolution * measurings/samples;
    Serial.print("pH= ");
    Serial.println(ph(voltage));
    delay(1000);
}