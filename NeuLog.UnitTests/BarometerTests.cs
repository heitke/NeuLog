using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeuLog.Barometer;
using NeuLog.UnitTests.Properties;

namespace NeuLog.UnitTests
{
    [TestClass]
    public class BarometerTests
    {
        [TestMethod]
        public async Task IsDeviceListening_IntegrationTest()
        {
            var settings = new Settings();
            
            using (var barometer = new BarometerSerial(settings.serialPort))
            {
                var result = await barometer.IsDeviceActive();

                Assert.AreEqual(true, result);
            }
        }

        [TestMethod]
        public async Task GetBarometer_IntegrationTest()
        {
            var settings = new Settings();

            using (var barometer = new BarometerSerial(settings.serialPort))
            {
                var result = await barometer.GetBarometerValue();

                Assert.IsTrue(result > 0);


            }

            System.Threading.Thread.Sleep(1000);

            using (var barometer = new BarometerSerial(settings.serialPort))
            {
                var result = await barometer.GetBarometerValue();

                Assert.IsTrue(result > 0);
            }
        }
    }
}
