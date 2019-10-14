# yomo
Autonomous solar-only lawn trimming rover
![Autonomous solar-only lawn trimming rover](resources/yomo.jpg)

# WHY?!

I want a 100% autonomous non-stupid set-it-forget-it bulk grass cutting device that I didn't have to babysit, watch, push, tow or remote into.  Setup the perimeter and let it do its thing so long as the sun is shining and the good Lord is willing!

# Technology

- Power chair motors
- Fiskers manual push reel mower
- Raspbery Pi 3 A+ main board
- 2x NavSpark DGPS receievers
- 2x Long ranage 915mhz trasmitter/receiver modules
- 3x H-Bridge to power the motors

# Design

This is the logical and physical layout of the components
![logical and physical design diagram](resources/yomo_design.png)

![brains of the rover](resources/brains.jpg)


[Detailed Design](Design.md)

[The implemenation specification](Implementation.md) details the hardware/software components that will make-up the rover and how/what technology they will be built on.

# Steps

## Current Status
- Fully architected, designed & UX mock-up (see this repo)
- Mounted the power chair motors very slickly on the reel mower (I tied into the existing bolts in the side rails)
- Written much of the control system (Navigation, Kinematics and much of the configuration)
- Assembled and tested a smaller/cheaper testing bot
- Assembled the full-size proto board and tested the h-bridge hardware
- Working with web developer on admin console

## Failures
- Hedge trimmers suck for cutting grass
- Power chairs suck at mower, but the motors are nice

## Next
- Integrate all the system and start navigating a grid 
- Get the system working on the reel mower
- Finish the mower hardware; add the reel driver motor.  This will be the last thing
