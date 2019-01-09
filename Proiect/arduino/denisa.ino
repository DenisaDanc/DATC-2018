#define NR_SENSORS 4

int trigPin1 = 12;
int echoPin1 = 13;
int trigPin2 = 8;
int echoPin2 = 9;
int trigPin3 = 2;
int echoPin3 = 3;
int trigPin4 = 5;
int echoPin4 = 6;
long duration, cm;

int senzor(int trig, int echo) {

  digitalWrite(trig, LOW);
  delayMicroseconds(5);
  digitalWrite(trig, HIGH);
  delayMicroseconds(10);
  digitalWrite(trig, LOW);
  pinMode(echo, INPUT);

  duration = pulseIn(echo, HIGH); //reads the echo pin, sound of wave in ms
  cm = (duration / 2) / 29.1;

  return cm;

}


void setup() {

  pinMode(trigPin1, OUTPUT);
  pinMode(echoPin1, INPUT);
  pinMode(trigPin3, OUTPUT);
  pinMode(echoPin3, INPUT);
  pinMode(trigPin2, OUTPUT);
  pinMode(echoPin2, INPUT);
  pinMode(trigPin4, OUTPUT);
  pinMode(echoPin4, INPUT);

  Serial.begin(9600);
}

void setStates(int *values, int *ocupat) {
  for (int i = 0; i < NR_SENSORS ; i++) {
    if (values[i] > 1 && values[i] <= 7)
    {
      ocupat[i] = 1;
    }
    else
    {
      ocupat[i] = 0;
    }
  }
}

void loop() {

  int values[NR_SENSORS];
  values[0] = senzor(trigPin1, echoPin1);

  values[1] = senzor(trigPin2, echoPin2);

  values[2] = senzor(trigPin3, echoPin3);
  values[3] = senzor(trigPin4, echoPin4);

  int ocupat[NR_SENSORS];
  String message;

  setStates(values, ocupat);

  for (int i = 1; i < 5; i++) {
    message += "P";
    message += i;
    message += ":";
    message += ocupat[i - 1];
    message += " ";
  }
  Serial.println(message);

  delay(1000);

}
