using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unosquare.RaspberryIO.Abstractions;

namespace yomo.Utility
{
    public static class I2CHelper
    {
        public static short ReadAddressWordSwapped(this II2CDevice device, short address)
        {
            // Read the conversion results
            // Shift 12-bit results right 4 bits for the ADS1015
            return (short)SwapWord(device.ReadAddressWord(address));
        }

        /// <summary>
        /// Write to the I2C register, ADS1x15 is byte swapped.
        /// </summary>
        /// <param name="registery">register address to write to</param>
        /// <param name="value">value to set</param>
        public static void WriteAddressWordSwapped(this II2CDevice device, byte registery, ushort value)
        {
            device.WriteAddressWord(registery, SwapWord(value));
        }

        /// <summary>
        /// Swap the low and high bytes.
        /// </summary>
        /// <param name="value">value to swap</param>
        /// <returns>16-bit unsigned value with high and low bytes swapped</returns>
        public static ushort SwapWord(ushort value)
        {
            return (ushort)((value >> 8) | (value << 8));
        }
    }
}
