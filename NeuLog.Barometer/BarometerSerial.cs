using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace NeuLog.Barometer
{
    public class BarometerSerial : IBarometerSerial, IDisposable
    {
        private SerialPort Port { get; set; }
        private string PortName { get; set; }

        public BarometerSerial(string portName)
        {
            PortName = portName;
        }

        private void Open()
        {
            Port = new SerialPort(PortName, 115200, Parity.None, 8, StopBits.Two);
            Port.Open();
        }


        public bool IsDeviceActive()
        {            
            try
            {
                Open();

                Port.Write(Encoding.ASCII.GetBytes("UNeuLog!"), 0, 8);

                for (int i = 0; i < 5; i++)
                {
                    if (Port.BytesToRead >= 7) break;
                    System.Threading.Thread.Sleep(1000);
                }
                if (Port.BytesToRead < 7) throw new Exception("Timeout waiting for response");

                byte[] result = new byte[7];
                Port.Read(result, 0, 7);

                var resultString = Encoding.Default.GetString(result);

                if (resultString.StartsWith("OK-V")) return true;

                return false;
            }
            finally
            {
                Close();
            }
        }

        public decimal GetBarometerValue()
        {            
            try
            {
                Open();

                Port.Write(new byte[] { 0x55, 0x12, 0x01, 0x31, 0, 0, 0, 0x99 }, 0, 8);

                for (int i = 0; i < 5; i++)
                {
                    if (Port.BytesToRead >= 8) break;
                    System.Threading.Thread.Sleep(1000);
                }

                if (Port.BytesToRead < 8) throw new Exception("Timeout waiting for response");

                byte[] result = new byte[8];
                Port.Read(result, 0, 8);

                return ConvertBytesToBarometer(result);
            }
            finally
            {
                Close();
            }
        }

        public decimal ConvertBytesToBarometer(byte[] data)
        {
            var hundreds = ((int)data[4]) & 0x0F;
            var tens = (int)data[5];
            var dec = data[6] & 0x0F;

            var result = hundreds * 100 + tens + (decimal)dec / 10;

            return result;

        }


        private void Close()
        {
            Port?.Close();
        }

        public void Dispose()
        {
            Close();
        }
    }
}
