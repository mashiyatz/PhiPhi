#include <EncoderStepCounter.h>

// encoder pins:
const int pin1 = 3;
const int pin2 = 2;
// Create encoder instance:
EncoderStepCounter encoder(pin1, pin2);

// encoder previous position:
int oldPosition = 0;
const int buttonPin = 4;
int lastButtonState = LOW;
int debounceDelay = 5; // what is this?

void setup() {
  pinMode(buttonPin, INPUT_PULLUP);
  Serial.begin(9600);
  // Initialize encoder
  encoder.begin();
  // Initialize interrupts
  attachInterrupt(digitalPinToInterrupt(pin1), interrupt, CHANGE);
  attachInterrupt(digitalPinToInterrupt(pin2), interrupt, CHANGE);
}

void interrupt() {
  encoder.tick();
}

void loop() {
  // read encoder position:
  int position = encoder.getPosition();
  int buttonState = digitalRead(buttonPin);
  
  if (buttonState != lastButtonState) {
    delay(debounceDelay);
  }

  lastButtonState = buttonState;

  // if (position > 100) encoder.setPosition(100);
  // if (position < 0) encoder.setPosition(0);
  if (abs(position) % 24 == 0) {
    encoder.reset();
    position = encoder.getPosition();
  }

  if (Serial.available()) {
    char input = Serial.read();
    Serial.print(position);
    Serial.print(",");
    Serial.println(buttonState);
  }
}