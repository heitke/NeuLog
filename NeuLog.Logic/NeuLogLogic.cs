using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuLog.Barometer;
using NeuLog.Storage;

namespace NeuLog.Logic
{
    public class NeuLogLogic
    {
        private IBarometerSerial Barometer { get; set; }
        private INeuLogStorage Storage { get; set; }

        public NeuLogLogic(IBarometerSerial barometer, INeuLogStorage storage)
        {
            Barometer = barometer;
            Storage = storage;
        }
        public async Task GetAndStoreBarometer()
        {             
            var currentValue = Barometer.GetBarometerValue();

            await Storage.StoreBarometerValue(currentValue);

        }
    }
}
