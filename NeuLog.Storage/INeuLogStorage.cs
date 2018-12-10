using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuLog.Storage
{
    public interface INeuLogStorage
    {
        Task StoreBarometerValue(decimal currentValue);
    }
}
