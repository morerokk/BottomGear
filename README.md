# Bottom Gear
A simple program that uses VRChat/ChilloutVR OSC to integrate with your PiShock.

# Installation
Grab the latest release zip from the Releases page and extract it to a new folder. Edit `config.json` and fill out your device's share code and device ID, as well as your username and API key from the PiShock website.

You can also fill out the `Strength` and `Duration` parameters. By default the shock strength is set to 5%, with the duration set to 1 second.

Open VRChat or ChilloutVR, make sure OSC is enabled. You need an animator parameter to control the app. The easiest way to accomplish this is by making a Contact Receiver on your avatar, with a bool parameter called `ShockMe` (adjustable in `config.json`). If you add this parameter after the first upload, you may need to delete the OSC config for your avatar in AppData/LocalLow/VRChat/VRChat/OSC/Avatars, or manually edit this config to add the parameter.

# Building from source
If you wish to build this from source, simply open the solution in Visual Studio (2019 preferred). You must have the .NET Core 3.1 SDK installed, which comes with most Visual Studio installations. Restore packages for the solution and build the solution.

You can also publish the `BottomGear` project to a folder, which will generate a single .exe file as well as copy the default config.json.
