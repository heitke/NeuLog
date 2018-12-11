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
        public void IsDeviceListening_IntegrationTest()
        {
            var settings = new Settings();
            
            using (var barometer = new BarometerSerial(settings.serialPort))
            {
                var result = barometer.IsDeviceActive();

                Assert.AreEqual(true, result);
            }
        }

        [TestMethod]
        public void GetBarometer_IntegrationTest()
        {
            var settings = new Settings();

            using (var barometer = new BarometerSerial(settings.serialPort))
            {
                var result = barometer.GetBarometerValue();

                Assert.IsTrue(result > 0);


            }

            System.Threading.Thread.Sleep(1000);

            using (var barometer = new BarometerSerial(settings.serialPort))
            {
                var result = barometer.GetBarometerValue();

                Assert.IsTrue(result > 0);
            }
        }

        [TestMethod]
        public void ConvertBytesToBarometer_Test()
        {
            var settings = new Settings();

            using (var barometer = new BarometerSerial(settings.serialPort))
            {
                var result = barometer.ConvertBytesToBarometer(new byte[] { 0x55, 0x12, 0x01, 0x31, 0xD1, 0x02, 0xA0, 0x0C });

                Assert.AreEqual(102.0M, result);

                var result2 = barometer.ConvertBytesToBarometer(new byte[] { 0x55, 0x12, 0x01, 0x31, 0xD1, 0x01, 0xA9, 0x0C });

                Assert.AreEqual(101.9M, result2);
            }
            
        }
    }
}
