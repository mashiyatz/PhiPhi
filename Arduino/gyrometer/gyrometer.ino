#include "Arduino_LSM6DS3.h"
#include "MadgwickAHRS.h"

Madgwick filter;
const float sensorRate = 104.00;

float roll = 0.0;
float pitch = 0.0;
float yaw = 0.0;

void setup() {
  Serial.begin(9600);
  if (!IMU.begin()) {
    Serial.println("Failed to initialize IMU");
    while(true);
  }

  filter.begin(sensorRate);
}

void loop() {
  float xAcc, yAcc, zAcc;
  float xGyro, yGyro, zGyro;

  if (IMU.accelerationAvailable() && IMU.gyroscopeAvailable()) {
    IMU.readAcceleration(xAcc, yAcc, zAcc);
    IMU.readGyroscope(xGyro, yGyro, zGyro);

    filter.updateIMU(xGyro, yGyro, zGyro, xAcc, yAcc, zAcc);
    roll = filter.getRoll();
    pitch = filter.getPitch();
    yaw = filter.getYaw();
  }

  if (Serial.available()) {
    char input = Serial.read();
    Serial.print(yaw);
    Serial.print(",");
    Serial.print(pitch);
    Serial.print(",");
    Serial.println(roll);
  }
}