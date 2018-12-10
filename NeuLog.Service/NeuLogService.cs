using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using NeuLog.Logic;
using NeuLog.Barometer;
using NeuLog.Storage;
using NeuLog.Service.Properties;

namespace NeuLog.Service
{
    public partial class NeuLogService : ServiceBase
    {
        private EventLog NeuLogEventLog { get; set; }
        private Timer Timer { get; set; }
        public NeuLogService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            CreateEventLog();
            this.NeuLogEventLog.WriteEntry($"{nameof(NeuLogService)} starting up");

            SetupTimer();
        }

        private void SetupTimer()
        {
            Timer = new Timer()
            {
                Enabled = true,
                AutoReset = true,
                Interval = 60 * 1000,
            };
            Timer.Elapsed += Timer_Elapsed;            
        }
        private void CreateEventLog()
        {
            NeuLogEventLog = new EventLog()
            {
                Log = "Application",
                Source = this.ServiceName
            };

            ((ISupportInitialize)(this.NeuLogEventLog)).BeginInit();
            if (!EventLog.SourceExists(this.NeuLogEventLog.Source))
            {
                EventLog.CreateEventSource(this.NeuLogEventLog.Source, this.NeuLogEventLog.Log);
            }
            ((ISupportInitialize)(this.NeuLogEventLog)).EndInit();
        }

        private async void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                var settings = new Settings();

                var barcode = new BarometerSerial(settings.serialPort);
                var storage = new NeuLogFlatFile(settings.filename);
                
                var logic = new NeuLogLogic(barcode, storage);
                await logic.GetAndStoreBarometer();

            }
            catch (Exception ee)
            {
                this.NeuLogEventLog.WriteEntry(ee.Message + ee.StackTrace, EventLogEntryType.Error);
            }
            
        }

        protected override void OnStop()
        {
            Timer.Stop();

            this.NeuLogEventLog.WriteEntry($"{nameof(NeuLogService)} stopping");
        }
    }
}
