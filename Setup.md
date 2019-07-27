# Raspberry Pi Setup Instructions

* [Download a 'Raspbian Buster Light' image from here](https://www.raspberrypi.org/downloads)
* [Use the Raspberry Pi installation guide](https://www.raspberrypi.org/documentation/installation/installing-images/README.md), burn it to an SD card using your burner of choice (Etcher, Win32DiskImage)
* touch e:\ssh.
* open a text editor enter...

```
country=US
ctrl_interface=DIR=/var/run/wpa_supplicant GROUP=netdev
update_config=1
network={
	ssid="MyWiFiNetwork"
	psk="aVeryStrongPassword"
	key_mgmt=WPA-PSK
}
```

* set End Of Line to "Unix"
* Save as: e:\wpa_supplicant.conf
* Insert SD Card into Pi and Boot
* Wait a minute
* Browse to your router usually... http://192.168.1.1
* Find the attached or connected devices, one should be called "raspberrypi", note the ip address we'll call it "192.168.1.{pi}"
* ssh pi@192.168.1.{pi}

NOTE:  this is accurate as of 7/27, [please see this page](https://dotnet.microsoft.com/download/dotnet-core) to ensure .Net Core ASP 2.2.6 is still the latest.  If not, update the wget and tar commands

```
sudo apt-get -y update
sudo apt-get -y install curl libunwind8 gettext apt-transport-https
wget https://download.visualstudio.microsoft.com/download/pr/13798f38-c14e-4944-83c9-4f5b7c535f4d/1e1c3414f3ad791098d1f654640f9bcf/aspnetcore-runtime-2.2.6-linux-arm.tar.gz
sudo mkdir -p /opt/dotnet
sudo tar -zxf aspnetcore-runtime-2.2.6-linux-arm.tar.gz -C /opt/dotnet
sudo ln -s /opt/dotnet/dotnet /usr/bin
dotnet --info
```

If the last command works, you should be good to roll with yomo

# Build Setup 

* Install Visual Studio 2019
* Clone [this repo](https://github.com/bruceme/yomo)
* Open yomo.sln
* Right click on "yomo" and select 'Edit Project File'
* Update the elements "SshDeployHost" and "SshDeployPassword" to match your Pi setup.
* Press F5 and build

If succesfful you should have a folder "yomo" on your pi root directory.

# Running Yomo

_If you're debugging..._
```
cd yomo
sudo dotnet yomo.dll
```

_To "autoboot" yomo put the command above at the end of..._

` sudo nano /etc/rc.local `

# Sources

* (Raspberry Pi Download)[https://www.raspberrypi.org/downloads]
* (Setup Raspberry Pi WiFi)[https://core-electronics.com.au/tutorials/raspberry-pi-zerow-headless-wifi-setup.html]
* (Asp.Net Core Runtime Install)[https://blog.technitium.com/2019/01/quick-and-easy-guide-to-install-net.html] - Updated to 2.2.6
