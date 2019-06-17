# yomo
Autonomous solar-only lawn trimming rover

# WHY?!

I want a 100% autonimous non-stupid set-it-forget-it bulk grass cutting device that I didn't have to babysit, watch, push, tow or remote into.  Setup the parimeter and let it do its thing so long as the sun is shining and the good Lord is willing!

# Technology

- Old beat up Power Chair, preferably without batteries.
- Two Hedge Trimmers
- Raspbery Pi 3 A+ main board
- Adafruit GPS HAT
- H-Bridge to power the motors

# Design

This is the logical and physical layout of the components
![logical and physical design diagram](resources/yomo_design.png)

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

## Next steps
  - Test motor control

# Current status

I've got the chair, got the pi, assembling

