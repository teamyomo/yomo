using System;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Abstractions;

namespace yomo.Utility
{
    public class PCA9685
    {
        const byte __PCA9685_REG_MODE1 = 0x00;    // Mode register 1
        const byte __PCA9685_REG_MODE2 = 0x01;    // Mode register 2
        const byte __PCA9685_REG_SUBADR1 = 0x02;    // I2C-bus subaddress 1
        const byte __PCA9685_REG_SUBADR2 = 0x03;    // I2C-bus subaddress 2
        const byte __PCA9685_REG_SUBADR3 = 0x04;    // I2C-bus subaddress 3
        const byte __PCA9685_REG_ALLCALLADR = 0x05;    // LED All Call I2C-bus address

        const byte __PCA9685_REG_PWM_BASE = __PCA9685_REG_PWM0_ON_L;    // LED output and brightness control base address
        const byte __PCA9685_REG_PWM_INC = 4;    

        const byte __PCA9685_REG_PWM0_ON_L = 0x06;    // LED0 output and brightness control byte 0
        const byte __PCA9685_REG_PWM0_ON_H = 0x07;    // LED0 output and brightness control byte 1
        const byte __PCA9685_REG_PWM0_OFF_L = 0x08;    // LED0 output and brightness control byte 2
        const byte __PCA9685_REG_PWM0_OFF_H = 0x09;    // LED0 output and brightness control byte 3
        // ... these continue to 4

        const byte __PCA9685_REG_ALL_PWM_ON_L = 0xFA;    // All LED output and brightness control byte 0
        const byte __PCA9685_REG_ALL_PWM_ON_H = 0xFB;    // All LED output and brightness control byte 1
        const byte __PCA9685_REG_ALL_PWM_OFF_L = 0xFC;    // All LED output and brightness control byte 2
        const byte __PCA9685_REG_ALL_PWM_OFF_H = 0xFD;    // All LED output and brightness control byte 3

        const byte __PCA9685_REG_PRE_SCALE = 0xFE;    // Prescaler for output frequency
        const byte __PCA9685_REG_TESTMODE = 0xFF;    // Defines the test mode to be entered

        const byte __PCA9685_MODE1_RESTART_ENABLED = 0b10000000;    // Restart enabled
        const byte __PCA9685_MODE1_RESTART_DISABLED = 0b00000000;    // Restart disabled
        const byte __PCA9685_MODE1_EXTCLK_ENABLED = 0b01000000;    // External clock enabled
        const byte __PCA9685_MODE1_EXTCLK_DISABLED = 0b00000000;    // External clock disabled
        const byte __PCA9685_MODE1_AI_ENABLED = 0b00100000;    // Register Auto-increment enabled
        const byte __PCA9685_MODE1_AI_DISABLED = 0b00000000;    // Register Auto-increment disabled
        const byte __PCA9685_MODE1_SLEEP_ENABLED = 0b00010000;    // Low power mode, oscillator off
        const byte __PCA9685_MODE1_SLEEP_DISABLED = 0b00000000;    // Normal mode
        const byte __PCA9685_MODE1_SUB1_ENABLED = 0b00001000;    // I2C subaddress 1 enabled
        const byte __PCA9685_MODE1_SUB1_DISABLED = 0b00000000;    // I2C subaddress 1 disabled
        const byte __PCA9685_MODE1_SUB2_ENABLED = 0b00000100;    // I2C subaddress 2 enabled
        const byte __PCA9685_MODE1_SUB2_DISABLED = 0b00000000;    // I2C subaddress 2 disabled
        const byte __PCA9685_MODE1_SUB3_ENABLED = 0b00000010;    // I2C subaddress 3 enabled
        const byte __PCA9685_MODE1_SUB3_DISABLED = 0b00000000;    // I2C subaddress 3 disabled
        const byte __PCA9685_MODE1_ALLCALL_ENABLED = 0b00000001;    // I2C All Call address enabled
        const byte __PCA9685_MODE1_ALLCALL_DISABLED = 0b00000000;    // I2C All Call address disabled

        const byte __PCA9685_MODE2_INVRT_ENABLED = 0b00010000;    // Output logic state inverted when OE=0
        const byte __PCA9685_MODE2_INVRT_DISABLED = 0b00000000;    // Output logic state not inverted when OE=0
        const byte __PCA9685_MODE2_OCH_STOP = 0b00000000;    // Outputs change on STOP command
        const byte __PCA9685_MODE2_OCH_ACK = 0b00001000;    // Outputs change on ACK command
        const byte __PCA9685_MODE2_OUTDRV_OPENDRAIN = 0b00000000;    // Outputs are configured to open drain structure
        const byte __PCA9685_MODE2_OUTDRV_TOTEMPOLE = 0b00000100;    // Outputs are configured to totem pole structure
        const byte __PCA9685_MODE2_OUTNE_0 = 0b00000000;    // LEDn=0 when OE=1
        const byte __PCA9685_MODE2_OUTNE_1 = 0b00000001;    // LEDn=1 when OE=1 and OUTDRV=1 or high-impedance when OUTDRV=0
        const byte __PCA9685_MODE2_OUTNE_2 = 0b00000010;    // LEDn=high-impedance

        II2CDevice device;
        int i2cAddress;

        public PCA9685(int busId = 2, int address = 0x46)
        {
            device = Pi.I2C.AddDevice(i2cAddress = address);

            Setup();
        }

        public void Setup()
        {
            var config = __PCA9685_MODE1_RESTART_DISABLED
                | __PCA9685_MODE1_EXTCLK_DISABLED
                | __PCA9685_MODE1_AI_DISABLED
                | __PCA9685_MODE1_SLEEP_DISABLED
                | __PCA9685_MODE1_SUB1_DISABLED
                | __PCA9685_MODE1_SUB1_DISABLED
                | __PCA9685_MODE1_SUB3_DISABLED
                | __PCA9685_MODE1_ALLCALL_DISABLED
                ;

            device.WriteAddressByte(__PCA9685_REG_MODE1, (byte)config);

            config = __PCA9685_MODE2_INVRT_DISABLED
                    | __PCA9685_MODE2_OCH_STOP
                    | __PCA9685_MODE2_OUTDRV_TOTEMPOLE
                    | __PCA9685_MODE2_OUTNE_0
                    ;

            device.WriteAddressByte(__PCA9685_REG_MODE2, (byte)config);

            SetAllPwm(0, 0);
        }

        public void SetPwm(int ch, int on, int off)
        {
            var regAddress = __PCA9685_REG_PWM_BASE + __PCA9685_REG_PWM_INC * ch;
            SetPwmCore(regAddress, on, off);
        }

        public void SetAllPwm(int on, int off)
        {
            SetPwmCore(__PCA9685_REG_ALL_PWM_ON_L, on, off);
        }

        private void SetPwmCore(int regAddress, int on, int off)
        {
            device.WriteAddressByte(regAddress + 0, LO(on));
            device.WriteAddressByte(regAddress + 1, HI(on));
            device.WriteAddressByte(regAddress + 2, LO(off));
            device.WriteAddressByte(regAddress + 3, HI(off));
        }
        private static byte HI(int v) { return (byte)((v >> 8) & 0x0F); }
        private static byte LO(int v) { return (byte)(v & 0xFF); }

        public void SetPwmDuty(int ch, int duty)
        {
            SetPwm(ch, 0, duty * 4096);
        }

        public void SetPreScaler(int prescaler)
        {
            var mode1Save = device.ReadAddressByte(__PCA9685_REG_MODE1);

            var config = mode1Save | __PCA9685_MODE1_SLEEP_ENABLED;

            device.WriteAddressByte(__PCA9685_REG_MODE1, (byte)config);         // sleep
            device.WriteAddressByte(__PCA9685_REG_PRE_SCALE, (byte)prescaler);   // set prescaler
            device.WriteAddressByte(__PCA9685_REG_MODE1, mode1Save);      // restore settings
        }

        public void SetPwmClock(int clk)
        {
            clk = Math.Max(100, Math.Min(1000, clk));
            var prescaler = (int)Math.Round(25E6 / (4096 * clk) - 1);
            SetPreScaler(prescaler);
        }
    }
}
