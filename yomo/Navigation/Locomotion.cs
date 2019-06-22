﻿using System;
using Unosquare.RaspberryIO.Abstractions;

namespace yomo.Navigation
{
	public class Locomotion
	{
        // The wheels we intend to drive and which pin they are attached to.
        Wheel left = new Wheel(BcmPin.Gpio13);
        Wheel right = new Wheel(BcmPin.Gpio18);

        private PidController headingPid = new PidController(1.1, 0.1, 0.1, 180, -180);

        float idealHeading;
        float correctingHeading;
        float courseDeviation; // In meters (right positive, left negative)

        float distanceToTarget;
        float timeToTarget;

        float lastLat;
        float lastLng;
        float lastSpeed;
        float lastHeading;

        float targetLat;
        float targetLng;
        float targetSpeed = 0;

        public Locomotion ()
		{
		}

        public void SetTarget(float lat, float lng, float speed)
        {
            targetLat = lat;
            targetLng = lng;
            targetSpeed = speed;
        }

        public void RefreshPosition(float lat, float lng, float speed, float heading)
        {
            lastLat = lat;
            lastLng = lng;
            lastSpeed = speed;
            lastHeading = heading;

            //CalculateDeviations();

            //PidControlHeading();

            //SteerWheels();
        }
	}
}

