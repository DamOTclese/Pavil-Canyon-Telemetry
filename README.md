# Pavil-Canyon-Telemetry
Meshtastic Network Monitor Tool -- affords the ability to log and display all network traffic that is seen on a Meshtastic device

The Windows application source code is C Sharp and uses Visual Studio Community to build the solution. The code's footprint is small since it uses standard public libraries including the Meshtastic library for C Sharp.

The program has these functional aspects:

* On connection the software obtains information from the Meshtastic device and populates one of the window panels with a list of all of the devices that the radio has heard or, in any event, all of the devices that the connected device remembers. Older nodes get dropped from the connected device's list of known devices over time.

* The software polls the device to obtain its name, hardware revision number, and software revision number -- however the software currently does not have retrieval of the revision information fully linked in, so currently the revision information is left blank on the screen.

* The software establishes the list of known devices on the left panel in a radio button list so that one may click on a device and, using the "send" field at the bottom, send a message typed in to the text field. The default radio button is the broadcast address so one can start the Windows appliation and be ready to broadcast.

* The background network radio strength, GPS coordiates, routing, and other network-related frames are filtered out by default however oe may un-check the checkbox that performs the filtering, and one may see the background networking frames. Note that these diagnostic frames are prefaced with ASCII escape characters to perform color however color is not currently supported in the diaglog panel.

* One may designate a specific remote device in the configuration drop down from the menu bar so that any traffic received -- any telemetry -- is sent to a secondary log file and to a comma-delimited file so that one may log and display telemetry data from a specific, designated device. Tis functionality is not yet fully tested.

* The traffic that gets displayed is logged to a new log file every time the Windows application is started with the file name containing the date and time that the application was started. The comma-delimited file for a specific device to log telemetry likewise gets created at the time the application is started.

There are problems with the user interface in that the "send" text field and the "clear" button are locked in to a gang panel which is ugly and needs to be fixed. The user interface is not good, it needs to be reworked so that the sending of text messages is a lot cleaner on the screen.

If you would like to work on this source code and push changes, please email me! I'll give you access and will check your changes.

Fredric L. Rice
Fred@CrystalLake.Name
