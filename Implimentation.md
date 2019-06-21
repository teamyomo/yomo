# Hardware Stack
  * Raspberry Pi 3 A+ (Wifi/USB/Uart/GPIO)
  * [NavSpark RTK GPS (UART)](https://www.navspark.com.tw/)
  * WitMotion initia navigation (UART/SPI)
  * H-Bridge DC Motor drivers

# Tech Stack
  * [Raspbian OS](https://www.raspberrypi.org/downloads/raspbian/)
  * [.Net Core 2.2 ASP.Net RESTfullAPI/Service running on Rasbian services](https://blogs.msdn.microsoft.com/david/2017/07/20/setting_up_raspian_and_dotnet_core_2_0_on_a_raspberry_pi/)
  
# Operating system requirement
  
The vehicle moves slowly and no operation requires clock-cycle accuracy to ensure timely completion.  The PID loops can accomidate interuptions.  So a real-time operating system is not a hard requirement.  For that reason regular Rasbian is the OS choice.

The software running on the Pi breaks down into three major components;

* Operations Interface (OI)
* Command (ACM)
* Navigation (NM)

# Application

There is one primary application all of the software is running under.  It is responsible for all of the software responsibilities listed directly above.  It is a single instance of a .Net Core ASP.Net 2.2 WebAPI with several background processes sharing a common Application Layer Context (ALC) between "modules" (really classes).  The RESTfull API controller is mearly presenting aspects of the ALC as json to the web client which interprets that json into presentation for the operator. 

Meanwhile in the background several threads are running the modules listed below to keep the vehicle in motion and responding physically as requested.

# Operations Interface (OI)

The operator is required to configure YoMo and manually command it to relocate it and demarkate operating regions and non-operating regions. To do this he will also need to see the critical parameters kept by application layer context. 

Potential configurations are as follows;

_MVP Configurations_
* Mowing/No-Mow region selection
* Operating speed
* Turning speed
* Deck width
* Overlap
* Notify on Alerts (unscheduled stop, low-power, etc)

_Direct User Commands (all MVP)_
* All Stop (big read octagon button)
* Manual commands
  * Steer (two thumb sliders)
  * Mower on/off
  * Start/Stop Region Tracking (to designate new mow regions)
  * Start/Stop Route Tracking (for safe inter-region navigation)
* Select region to start auto mowing

_Future Configurations_
* How often do you want to mow (Continous, pausing, etc)
* Black-Out times (Sunday mornings)
* What kind of gridding pattern (up-down,left-right, diagonal, diagonal swapped, spiral-in, spiral-out, mixed)
* Steering PID tuning (P-I-D)

The configurations and directing commands will be done via an HTML page running client-side javascript talking to a RESTful WebAPI service running on the Raspberry Pi (see Application description above).


# Background Services

All modules listed below are actually running in a single .Net Core 2.2 instance as background threads.  The modules described below are actually just logical groupings of classes.  Collectively these jobs are loosely refered to as "background services".

The startup/loop of the Application will also generate at a low-level the monitoring statistics;
* System status 
* Last mowed
* Days/Hours in service
* Battery voltage
* Solar voltage

These will all be provided for and displayed in the OI.

## Automated Command Module(ACM)

All request to physically do anything on the vehicle go through the automated command module (ACM).  It is responsible for interpretting commands in the context of configuration and current location and generate routes of travel for the navigation module (NM).  

**The operating modes are:**

* Stand-By - No motion, no command, stand-still
* Haulted - non-commanded non-motion, in distress
* Manual - Forward commands from the operator to the motor controllers
* AutoRoute - Navigate a route, starting at the closest and going to the next passing them to the NM
* AutoRegion - Given a region, grid it and generate routes for each pass for the NM

**Gridding Algorithm** - The ACM is responsible for grid generation, it will be handed a region.  The MVP grid algorithm is as follows;
1. Generate sub-regions as nessary for "islanding"
1. AutoRoute to Region initial start point
1. Sweep parimeter twice
1. Fill region with diagonal zig-zags in the opposite diagonal as last mow of this region
1. AutoRoute to Region termination point

The ADM finally is responsible for turning on/off the mower itself via GPIO.

## Navigation Module (NM)

The navigation module(NM) is responsible for translating routing, possition and attitude into wheels being steered.

**Position** is recieved by a 10hz 10mm precision RTK GPS update.  The NM must receive the fastest possible feed of the GPS data. It's feed rate is second only to the Attitude sensor.

**Attitude** sensor data is read in via a USB-UART and provides the most time-critical feedback to the Steering PID control.

**A route request contains;**
 * Initial point
 * Terminus point
 * Requested Velocity

From all of the above information the NM calculates the ideal track-line, the cross-track error, the current heading, the correction heading and the actual velocity.

The NM hands the correction track, ideal speed, actual heading, actual speed to the Kinetics Module (KM). 

If the NM has a velocity bellow the configured minumum or above the configured maximum or the cross-track error is outside the maximum configured cross track error, it will place the KM into a "haulted" state.  It will report a faulted state to the ACM.

## Kinetics (KM) 

The Kinetics module (KM) is fed directly by the NM and is soley responsible for actually moving the vehicle.  Fundamentally this is a Steering PID loop implimentation.  Given the heading and speed we want and the heading and speed we have, tell me what to send to each motor.

The modes of operation are;
* Stand-By - Told to stop
* Haulted - non-commanded/non-movement
* Moving - Normal operation


If the pid parameters exceed configured limits, the KM will automatically go into a "Haulted" mode, which will stop and await instructions from the NM.

The KM is responsible for initializing and feeding appropriate values (between minimum/maximum) to the hardware PWM GPIOs that control the H-Bridge motor controllers. 

## Unhappy path

Conditions that must generate a "hault and catch fire" situation (stop and notify the operator);

* If the ACM can not determine a contigous route from the current location to the next region
* If the requested navigation does not happen withing a configured timeout
* If the vehicle reports an anomious sensing (future sensors for clogging or obstacle)
* If the vehicle emergency "All Stop" button is down
