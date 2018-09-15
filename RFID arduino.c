//count = 0
int count = 0;                                          
//character array of size 12 
char input[12];                                         
//flag =0
boolean flag = 0;                                    
void setup()
{
  //begin serial port with baud rate 9600bps
  Serial.begin(9600);                              
}
void loop()
{
  if(Serial.available())
  {
    count = 0;
    //Read 12 characters and store them in input array
    while(Serial.available() && count < 12)          
    {
      input[count] = Serial.read();
      count++;
      delay(5);
    }
    //Print RFID tag number 
    Serial.println(input);                            
  }
}