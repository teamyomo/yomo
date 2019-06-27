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
            var nav = new Navigator();

            nav.SetTarget(new yomo.Utility.Vector(0, 100), 100);

            for(int i = 0; i < 10; i++)
                nav.NavigationLoop();


        }
    }
}
