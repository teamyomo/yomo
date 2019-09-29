using System;
using Xunit;
using yomo.Navigation;

namespace YoGoTester
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            // Use simulator, not real board
            yomo.Simulator.SimulatorGlue.Glue();

            var nav = new Navigator();

            nav.SetTarget(new yomo.Utility.Vector(0, 100), 100);

            for(int i = 0; i < 10; i++)
                nav.NavigationLoop();


        }
    }
}
