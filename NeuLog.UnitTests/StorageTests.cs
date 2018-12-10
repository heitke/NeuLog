using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeuLog.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NeuLog.UnitTests
{
    [TestClass]
    public class StorageTests
    {
        [TestMethod]
        public async Task FlatFileStorage_IntegrationTest()
        {
            var filename = $"c:\\temp\\{nameof(FlatFileStorage_IntegrationTest)}.txt";
            File.Delete(filename);

            var neuLogFlatFile = new NeuLogFlatFile(filename);

            await neuLogFlatFile.StoreBarometerValue(101.1M);

            var result = File.ReadAllLines(filename);
            Assert.AreEqual(1, result.Length);

            var parts = result[0].Split(new char[] { '\t' });
            Assert.AreEqual(2, parts.Length);

            Assert.AreEqual("101.1", parts[1]);
            DateTime dateWritten;
            DateTime.TryParse(parts[0], out dateWritten);

            Assert.IsTrue(DateTime.UtcNow > dateWritten);
            Assert.IsTrue(DateTime.UtcNow.AddHours(-1) < dateWritten);

        }
    }
}
