using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using yomo.Navigation;
using yomo.Simulator;

namespace YoGoTester
{
    /// Test that simulator works
    public class SimulatorTest
    {
        public Vehicle Vehicle { get; set; }
        public IWheel left = new WheelSimulator(yomo.Navigation.WheelId.LeftRear);
        public IWheel right = new WheelSimulator(yomo.Navigation.WheelId.RightRear);
        public IPosition position = new PositionSimulator();

        [Fact]
        public void Forward()
        {
            bool hit = false;
            left.SetSpeed(200);
            right.SetSpeed(200);

            position.LoopReadPosition(pos =>
            {
                Xunit.Assert.True(pos.position.X > 0.0d);
                Xunit.Assert.Equal(0.0d, pos.position.Y);
                hit = true;
            });

            System.Threading.Thread.Sleep(1500);
            Xunit.Assert.True(hit);
        }


        [Fact]
        public void Left()
        {
            bool hit = false;
            left.SetSpeed(150);
            right.SetSpeed(200);

            position.LoopReadPosition(pos =>
            {
                Xunit.Assert.True(pos.position.X < 0.0d);
                Xunit.Assert.True(pos.position.Y > 0.0d);
                hit = true;
            });

            System.Threading.Thread.Sleep(1500);
            Xunit.Assert.True(hit);
        }

    }
}
