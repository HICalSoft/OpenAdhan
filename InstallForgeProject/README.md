# Developers Note

Install Forge is not particularly portable, so ideally it will be replaced as the OpenAdhan installer project in the future.

Visual Studio Setup Project and WIX were attempted to be used, but my initial attempts to get the registry & install behavior I wanted failed, so I settled for the simpler Install Forge project for the initial release.

Ideally an installer should:
* Install dependencies (.net framework)
* Install application outputs into Program Files\OpenAdhan
* Install resources into Program Files\OpenAdhan
* Run the OpenAdhanRegistrySetupApp on install completion as Administrator (HKLM keys edited)
* Add Registry Keys to startup on boot/login (or use the shortcut approach)
* Add icons to installer & application binary.
* Support clean & easy updating to newer releases of software by simplying running another version of the installer.