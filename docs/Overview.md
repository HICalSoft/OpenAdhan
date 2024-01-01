---
layout: page
title: Overview
sidebar_link: true
---

Open Adhan provides a simple main window that keeps track of all your prayer times for the day:

![Open Adhan Main Window](/img/MainWindow.png?raw=true)

The main window emboldens the current prayer and keeps a running time for how long remains until the next prayer.

When minimized, a notify icon in your system tray can be moused-over to quickly get information about what prayer is currently active, as well as how many hours and minutes remain until the next prayer.

![Open Adhan Notify Icon](/img/NotifyIcon.png?raw=true)

When you need to configure Open Adhan, click "Edit" > "Settings" to open the Settings dialog which allows for all the configuration needed to configure Open Adhan for your current location.

![Open Adhan Settings Dialog](/img/OpenAdhanSettings.png?raw=true)

To change an adhan audio sound from the default, click the button next to the "Normal Adhan" or "Fajr Adhan" labels and select the new adhan audio file from your computer. Note that only WAV files are supported since this application uses the built in Microsoft media player to recite the adhans. Be sure to test the adhan playback after choosing it by pressing the "Test Adhan Playback" button!



### Advanced Implementation Details

* All settings are saved in your system registry and can be manually modified at "HKEY_LOCAL_MACHINE\SOFTWARE\OpenAdhan" - or "HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\OpenAdhan" if you are running on a 64bit machine.
* By default, OpenAdhan installs to the following directory: C:\Program Files (x86)\HICalSoft\OpenAdhan


![Open Adhan](/img/kaaba_icon_96x96.png)
