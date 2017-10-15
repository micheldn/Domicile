#include <ESP8266WiFi.h>

// Master SSID
const char* ssid = "Domicile_86dB0zZfCN63EYiD";
const char* pass = "OvH7Hs9KbU3c9bSg";
const char* host = "192.168.100.1";
const int port = 12500;

/* */
const int bufferSize = 1024;

// Initialize a 1024 byte buffer
char buffer[bufferSize];

// Char containing data
char data[bufferSize];

// Predefined char arrays that act as packets
const char deviceName[bufferSize] = "Name=SimpleLEDTest;";
const char deviceVersion[bufferSize] = "Version=0.0.1;";

boolean ledState;

// the setup function runs once when you press reset or power the board
void setup() {
	Serial.begin(115200);
	Serial.print("Connecting to ");
	Serial.println(ssid);

	// initialize digital pin BUILTIN_LED as an output.
	pinMode(BUILTIN_LED, OUTPUT);

	// Set WiFi to WifiClient only mode
	WiFi.mode(WIFI_STA);
	WiFi.begin(ssid, pass);

	// If the device isn't connected yet, keep waiting...
	while (WiFi.status() != WL_CONNECTED) {
		delay(500);
		Serial.print(".");
	}

	Serial.println("");
	Serial.println("WiFi connected");
	Serial.println("IP Address:");
	Serial.println(WiFi.localIP());

	ledState = false;
}

WiFiClient client;

// the loop function runs over and over again forever
void loop() {
	Serial.println(client.connected());

	// If the client isn't connected, retry
	if (!client.connected()) {
		if (client.connect(host, port)) {
			Serial.print("Connected to host on ");
			Serial.print(host);
			Serial.print(":");
			Serial.println(port);
		}
		else {
			Serial.println("Connection failed...");
			delay(2000);
			return;
		}
	}

	// Wait for data
	while (!client.available()) {
		delay(1);
	}

	String line = client.readStringUntil('\r');
	Serial.println(line);

	client.flush();

	Serial.println(millis());
}

