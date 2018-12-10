# NeuLog
A service for collecting data from NeuLog sensor devices via the serial ports. Currently only supports the Barometer sensor.

The NeuLog software that is included with the device is required to be running all the time to retrieve data.  The provided API also works this way.  API documentation can be found here:
https://neulog.com/wp-content/uploads/2014/06/NeuLog-API-version-7.pdf

This project includes a windows service written in C# that will poll the barometer device once a minute and write the current result to a text file.

# Quickstart
1) Set the com port for your device in the app.config file for the neuLog.Service
2) Set the output file location in the app.config file for the neuLog.Service
3) Install the windows service by running the .net installutil.exe utility and passing the path to your NeuLog.Service.exe
4) Start the service (net start NeuLogBarometerService) and set it to automatic if you'd like

# Known issues
I'm still attempting to figure out the format of the packet returned from the device.  Requesting the current barometer pressure can be done by sending the following command:
55 12 01 31 00 00 00 99

Which returns this:
55 12 01 31 D1 01 A7 12	

I know that this maps to the following data:
101.7 kPa

So it looks like the first 4 bytes returned match my header.  But have yet to figure out how to interpret the final 4 bytes.

# Other technical information
I used a com port sniffer and the API to determine the request and responses to make for these devices.  To connect, use the following com settings:
Speed: 115,200
Data bits: 8
Stop bits: 2
Parity: none

It appears that all request and responses are in an 8 byte format. 

You can get the status of the device by passing this:
55 4E 65 75 4C 6F 67 21                           UNeuLog!
