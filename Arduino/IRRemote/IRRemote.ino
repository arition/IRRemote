#define RECVPINID 11

#include <IRremote.h> //include the library
IRrecv irrecv(RECVPINID); //create a new instance of receiver
decode_results results;
IRsend irsend;
bool on = true;

void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
  pinMode(RECVPINID, OUTPUT);
  irrecv.enableIRIn();
}

void loop() {
  //dumpRemote();
  switch(Serial.parseInt()){
    case 1:
      sendIR(0xFE50AF); // TV ON
      break;
    case 2:
      sendIR(0xFECA35); // change TV Input
      break;
    case 3:
      sendIR(0xFF02FD); // SoundBar ON
      break;
    case 4:
      sendIR(0xFF827D); // SoundBar Vol+
      break;
    case 5:
      sendIR(0xFFA25D); // SoundBar Vol-
      break;
    case 6:
      sendIR(0x1FE40BF); // HDMI Switch 1
      break;
    case 7:
      sendIR(0x1FE20DF); // HDMI Switch 2
      break;
    case 8:
      sendIR(0x1FE609F); // HDMI Switch 3
      break;
    case 9:
      sendIR(0xFE0AF5); // TV Enter
      break;
  }
}

void sendIR(unsigned long code) {
    irsend.sendNEC(code, 32);
    delay(40);
}

void dump(decode_results *results) {
  // Dumps out the decode_results structure.
  // Call this after IRrecv::decode()

  int count = results->rawlen;

  if (results->decode_type == UNKNOWN) {
    Serial.print("Unknown encoding: ");
  }

  else if (results->decode_type == NEC) {
    Serial.print("Decoded NEC: ");
  }

  else if (results->decode_type == SONY) {
    Serial.print("Decoded SONY: ");
  }

  else if (results->decode_type == RC5) {
    Serial.print("Decoded RC5: ");
  }

  else if (results->decode_type == RC6) {
    Serial.print("Decoded RC6: ");
  }

  else if (results->decode_type == PANASONIC) {
    Serial.print("Decoded PANASONIC - Address: ");
    Serial.print(results->address, HEX);
    Serial.print(" Value: ");
  }

  else if (results->decode_type == LG) {
    Serial.print("Decoded LG: ");
  }

  else if (results->decode_type == JVC) {
    Serial.print("Decoded JVC: ");
  }

  else if (results->decode_type == AIWA_RC_T501) {
    Serial.print("Decoded AIWA RC T501: ");
  }

  else if (results->decode_type == WHYNTER) {
    Serial.print("Decoded Whynter: ");
  }

  Serial.print(results->value, HEX);
  Serial.print(" (");
  Serial.print(results->bits, DEC);
  Serial.println(" bits)");
  Serial.print("Raw (");
  Serial.print(count, DEC);
  Serial.print("): ");

  for (int i = 1; i < count; i++) {
    if (i & 1) {
      Serial.print(results->rawbuf[i]*USECPERTICK, DEC);
    }
    else {
      Serial.write('-');
      Serial.print((unsigned long) results->rawbuf[i]*USECPERTICK, DEC);
    }
    Serial.print(" ");
  }
  Serial.println();
}

void dumpRemote() {
  if (irrecv.decode(&results)) { //we have received an IR
    Serial.println (results.value, HEX); //display HEX
    dump(&results);
    irrecv.resume(); //next value
  }
}
