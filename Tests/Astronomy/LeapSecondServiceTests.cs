using Galaxon.Astronomy.Algorithms.Services;
using Galaxon.Astronomy.Algorithms.Utilities;

namespace Galaxon.Tests.Astronomy;

[TestClass]
public class LeapSecondServiceTests
{
    [TestMethod]
    public void TestCalcDUT1()
    {
        LeapSecondService leapSecondService = ServiceManager.GetService<LeapSecondService>();
        for (int y = 1972; y <= 2022; y++)
        {
            DateTime dt = new DateTime(y, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            int LSC = leapSecondService.TotalLeapSeconds(dt);
            double deltaT = DeltaTUtility.CalcDeltaTNasa(dt);
            double DUT1 = leapSecondService.CalcDUT1(dt);
            Console.WriteLine($"Year={y}, LSC={LSC}, ∆T={deltaT}, DUT1={DUT1}");
            if (Abs(DUT1) > 0.9)
            {
                Console.WriteLine("Wrong.");
            }
        }
    }
}
