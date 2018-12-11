using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeuLog.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NeuLog.Storage;
using NeuLog.Barometer;
using System.IO;
using NeuLog.UnitTests.Properties;

namespace NeuLog.UnitTests
{
    [TestClass]
    public class LogicTests
    {
        [TestMethod]
        public async Task GetAndStoreBarometerTest()
        {
            var barometer = new Mock<IBarometerSerial>();
            barometer.Setup(m => m.GetBarometerValue())
                .Returns(101.1M);

            decimal storedValue = 0;
            var storage = new Mock<INeuLogStorage>();
            storage.Setup(m => m.StoreBarometerValue(It.IsAny<decimal>()))
                .Callback((decimal valueToStore) => { storedValue = valueToStore; })
                .Returns(Task.FromResult(default(object)));

            var logic = new NeuLogLogic(barometer.Object, storage.Object);

            await logic.GetAndStoreBarometer();

            barometer.Verify(m => m.GetBarometerValue());
            storage.Verify(m => m.StoreBarometerValue(101.1M));

            Assert.AreEqual(101.1M, storedValue);
        }

        [TestMethod]
        public async Task GetAndStoreBarometer_IntegrationTest()
        {
            var settings = new Settings();

            // Arrange
            var filename = $"c:\\temp\\{nameof(GetAndStoreBarometer_IntegrationTest)}.txt";
            File.Delete(filename);

            var barometer = new BarometerSerial(settings.serialPort);
            var storage = new NeuLogFlatFile(filename);

            var logic = new NeuLogLogic(barometer, storage);

            // Act        
            await logic.GetAndStoreBarometer();

            // Assert
            var result = File.ReadAllLines(filename);
            Assert.AreEqual(1, result.Length);

            var parts = result[0].Split(new char[] { '\t' });
            Assert.AreEqual(2, parts.Length);

            var pressure = decimal.Parse(parts[1]);
            Assert.IsTrue(pressure > 0);
            DateTime dateWritten;
            DateTime.TryParse(parts[0], out dateWritten);

            Assert.IsTrue(DateTime.UtcNow > dateWritten);
            Assert.IsTrue(DateTime.UtcNow.AddHours(-1) < dateWritten);
        }

    }
}
