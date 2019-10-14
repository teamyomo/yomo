# RTK GPS

Getting a hyper accurate position sensor is really the key to this project actually working at all.  After a lot of searching I found an  inexpensive RTK GPS setup with good instructional videos and great documentation (really good).  It's modules only, so there's manual setups and configurations and it needs a separate LoRa transciever to sync the sat offset data.  But the chips do all the real work and the project gets 1cm accuracy.

The whole RTK setup cost ~$250.  Next cheapest "hobby grade" is $600; after that it goes up steeply.  $1500 for the first shrink wrap solution then sky is the limit for survey stuff (2k-5k). 

6/27 UPDATE: I had some bother setting up my own USB-UART dongle (not the one they sell). Once I sorted those issues out the NavStar RTK GPS setup _EXACTLY_ like the video, very impressive, very straight forward.  I haven't tested the correction serial links yet, but I'll get those in soon.

Watch this video:
[NavSpark Overview/Setup video](https://www.youtube.com/watch?v=17fS9YZC84I)

Source:
[NavSpark Store](http://navspark.mybigcommerce.com/)
