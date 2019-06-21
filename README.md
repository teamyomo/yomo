# yomo
Autonomous solar-only lawn trimming rover

# WHY?!

I want a 100% autonimous non-stupid set-it-forget-it bulk grass cutting device that I didn't have to babysit, watch, push, tow or remote into.  Setup the parimeter and let it do its thing so long as the sun is shining and the good Lord is willing!

# Technology

- Old beat up Power Chair, preferably without batteries.
- Two Hedge Trimmers; I may evaluate reel mowers too, they cut better (on short, frequently cut grass) and use about the same energy
- Raspbery Pi 3 A+ main board
- ~~Adafruit GPS HAT~~ (I need cm accuracy, regular GPS won't cut it[ouch])
- 2x NavSpark DGPS receievers
- 2x Long ranage 915mhz trasmitter/receiver modules
- 3x H-Bridge to power the motors

# Design

This is the logical and physical layout of the components
![logical and physical design diagram](resources/yomo_design.png)

[The implimenation specification](implimentation.md) details the hardware/software compontents that will make-up the rover and how/what technology they will be built on.

# Steps

## Completed

- bought everything above
- stripped an old beatup chair
- fabricated solar panel mount from the old chair mount
- removed the brake from both motors (it's a parasitic load)
- re-assembled chair
- stripped electric hedge trimmers, built an attachment for them.

## Started
- Coding the Pi (see this repository)
  - Setup Pi
  - GPS Reader
  - INS Reader
  - Wrote PWM Motor controller logic
  - Sourced PID controller for navigation control (heading/speed adjustments)

## RTK GPS

** 6/20/2019 NOTE: this hasn't even arrived yet.  I'll make an update once it's here and I'm using it.

Getting a hyper accurate position sensor is really the key to this project actually working at all.  After a lot of searching I found an  inexpensive RTK GPS setup with good instructional videos and great documentation (not entirely Chineese, so really good).  It's modules only, so there's manual setups and configurations and it needs a seperate LoRa transciever to sync the sat offset data.  But the chips do all the real work and the project gets 1cm accuracy.

The whole RTK setup will "true out" at $250.  Next cheapest "hobby grade" is $600; after that it goes up steaply.  $1500 for the first shrink wrap solution then sky is the limits for survey stuff (2k-5k). 

Watch this video:
[NavSpark Overview/Setup video](https://www.youtube.com/watch?v=17fS9YZC84I)

Source:
[NavSpark Store](http://navspark.mybigcommerce.com/)

## Next steps
  - Test motor control

# Current status

I've got the chair, got the pi, assembling

