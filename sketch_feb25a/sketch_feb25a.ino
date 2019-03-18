
#include <SimpleDHT.h>
#define ON 1 //debug on
#define OFF 0 //debug off


#define DEBUG_MODE ON
#define PHOTORESISTOR A4 // a6 nano
#define SOUNDANALOG 3 //A7 nano
#define SOUNDPWM A0
#define TEMPHUM A3 //a5 nano*
SimpleDHT11 dht11(TEMPHUM); //special classs for dht11




void setup() {
  Serial.begin(9600);
  pinMode(SOUNDPWM,INPUT);
  pinMode(PHOTORESISTOR,INPUT);
}

void loop() {
 uint8_t temperature, humidity,luminosity, sound;
 if(dht11.read(TEMPHUM,&temperature,&humidity,NULL)!=SimpleDHTErrSuccess){
  #if (DEBUG_MODE == ON)
  Serial.println("Waiting for datas!");
  #endif
  }else{
  luminosity=(100*analogRead(PHOTORESISTOR))/255;
  sound=analogRead(A0);//(100*analogRead(SOUNDPWM))/255;
  #if (DEBUG_MODE == ON)
  Serial.print("Temperature = ");
  Serial.println(temperature);
  Serial.print("Humidity = ");
  Serial.println(humidity);
  Serial.print("Light =");
  Serial.println(luminosity); //0-100% light
  Serial.print("Sound=");
  Serial.println(sound); //0-100% sound
  #endif
  
  }
  if(Serial.available() > 0){// verify bluetooth if is activated or not 
    #if (DEBUG_MODE == ON)
    Serial.println("Sending datas!");//afisare desktop
    #endif
    Serial.write(temperature); //afisare temperatura
    delay(250);
    #if (DEBUG_MODE == ON)
    Serial.println("Temperature sent!");
    #endif
    Serial.write(humidity);
    delay(250);
    #if (DEBUG_MODE == ON)
    Serial.println("Humidity sent!");
    #endif
    Serial.write(luminosity);
    delay(250);
    #if (DEBUG_MODE == ON)
    Serial.println("Luminosity sent!");
    #endif
    Serial.write(sound);
    delay(250);
    #if (DEBUG_MODE == ON)
    Serial.println("Sound sent!");
    #endif
  }
  delay(1000);
}
