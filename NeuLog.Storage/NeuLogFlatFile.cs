using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NeuLog.Storage
{
    public class NeuLogFlatFile : INeuLogStorage
    {
        private string Filename { get; set; }
        public NeuLogFlatFile(string filename)
        {
            Filename = filename;
        }

        public async Task StoreBarometerValue(decimal currentValue)
        {
            await FileWriteAsync(Filename, GetFormattedLine(currentValue));            
        }

        private string GetFormattedLine(decimal currentValue)
        {
            return DateTime.UtcNow + "\t" + currentValue;
        }

        private async Task FileWriteAsync(string filePath, string messaage, bool append = true)
        {
            var fi = new FileInfo(Filename);
            fi.Directory.Create();

            using (FileStream stream = new FileStream(filePath, append ? FileMode.Append : FileMode.Create, FileAccess.Write, FileShare.None, 4096, true))
            using (StreamWriter sw = new StreamWriter(stream))
            {
                await sw.WriteLineAsync(messaage);
            }
        }
    }
}
