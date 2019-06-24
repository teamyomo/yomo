# Hardware Stack
  * Raspberry Pi 3 A+ (Wifi/USB/Uart/GPIO)
  * [NavSpark RTK GPS (UART)](https://www.navspark.com.tw/)
  * WitMotion initia navigation (UART/SPI)
  * H-Bridge DC Motor drivers

# Tech Stack
  * [Raspbian OS](https://www.raspberrypi.org/downloads/raspbian/)
  * [.Net Core 2.2 ASP.Net RESTfullAPI/Service running on Rasbian services](https://blogs.msdn.microsoft.com/david/2017/07/20/setting_up_raspian_and_dotnet_core_2_0_on_a_raspberry_pi/)
  
# Hardware Requirements

These hardware attributes will contribute most to this projects success;

1. Easy to code and debug
1. Has ~100Mb of storage 
1. UART, USB and GPIO for motor driver
1. Web-Hostable
1. Multi-threaded

Notice I didn't say speed, or super cheap.  The web hosting and interface requirements steered me towards the Raspberry Pi.  An ESP32 variant could potentially work as well.  Especially with it's improved debugging support and dual cores.  Future version, we'll see.


# Operating system requirement
  
The vehicle moves slowly and no operation requires clock-cycle accuracy to ensure timely completion.  The PID loops can accomidate interuptions.  So a real-time operating system is not a hard requirement.  For that reason regular Rasbian is the OS choice.

The software running on the Pi breaks down into three major components;

* Operations Interface (OI)
* Command (ACM)
* Navigation (NM)
