# Test bot

A small test bot will help prove the code/concepts and understand the hardware an how it all works together in your own shop instead of trying to trying to work out glitches in your back yard.

Parts:
* Raspberry Pi 3A+
* [Raspberry Pi Motor Hat/Raspberry Pi Full Function Motor HAT](https://www.ebay.com/itm/183810223243)
* [Robot Chasis / 2WD Smart Robot Car Chassis Kit](https://www.ebay.com/itm/383143792928)

# DGPS

Getting a hyper accurate position sensor is really the key to this project actually working at all.  After a lot of searching I found an  inexpensive RTK GPS setup with good instructional videos and great documentation (really good).  It's modules only, so there's manual setups and configurations and it needs a separate LoRa transciever to sync the sat offset data.  But the chips do all the real work and the project gets 1cm accuracy.

The whole RTK setup cost ~$250.  Next cheapest "hobby grade" is $600; after that it goes up steeply.  $1500 for the first shrink wrap solution then sky is the limit for survey stuff (2k-5k). 

After you've got the basics of the wheels working, add the high precision DGPS.

* 2x GPS Receivers
* 2x GPS Antenna + extension
* 2x 915mhz Transceiver

One unit will be setup as the "base" unit.  Connect the transciever Rx pin to the TX2 pin of the GPS unit and power it using any reasonable source (USB power supply, battery or solar panel).

The second unit is the "Roving" unit.  Connnect the second transceiver's Tx pin to the GPS's RX2 pin.  Power the unit off the Raspberry Pi's 5v supply and connect the pins from the GPS's TX1/RX1 to the opposite/swapped RX/TX pins on the Raspberry Pi's serial port.

6/27 UPDATE: I had some bother setting up my own USB-UART dongle (not the one they sell). Once I sorted those issues out the NavStar RTK GPS setup _EXACTLY_ like the video, very impressive, very straight forward.  I haven't tested the correction serial links yet, but I'll get those in soon.

Watch this video:
[NavSpark Overview/Setup video](https://www.youtube.com/watch?v=17fS9YZC84I)

Source:
[NavSpark Store](http://navspark.mybigcommerce.com/)

# Full Build

This will get into some heavy metal fabrication. 

What you need;

1. A Fisker push reel mower
1. A set of power-chair motors (most will do, but I used Jazzy motors)

## Powerchair Motor

You'll have to remove the brakes from each motor.  It's a simple dissaembly and wire snipping.

## Mower Reel prep

1. Remove the handle
1. Dissasemble the wheels, this is done by removing the wheels.  Then remove the chain and sproket.  Finally removing cotter pins on the axle should free the axle from sliding out the welded-in bearings.  I recall this wasn't trivial, but I didn't have to cut or break anything to get them off, I did have to use force, but nothing broke.
1. Using a cutting wheel, hack-saw or a plasma cutter, cut the aft tabs of the mower (behind where the wheel axle was) off, in a shape that will receive the motors.

## Mounting the motors the wheels

1. Fabricate 6x aluminum spacer/bushings.  Approximate 1" diameter with a 1/2" dia ID.
1. Using large washers, bolt the motor to the mower using bushings as "stand-offs" and 6x 1/2" bolt/nuts to secure.

## Solar Panel Assembly

1. Strip down the seat post from Jazzy scooter
1. Get a large solar panel ~(1.6m x 1m), rivet 30mm X 30mm x 2mm aluminum angle centered on the center of mass of the solar panels such that it secures the solar to the seat post.
1. Attach a PTTV solar charge controller to the aluminum angle.

## Finally assembly

[TBD] - Need to mount the brain-box to the mower + a weld a post to hold the solar panel

