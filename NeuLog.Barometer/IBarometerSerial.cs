﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuLog.Barometer
{
    public interface IBarometerSerial
    {
        bool IsDeviceActive();
        decimal GetBarometerValue();
    }
}
