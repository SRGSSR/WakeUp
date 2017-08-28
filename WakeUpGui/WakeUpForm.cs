using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using System;

namespace WakeUpGui
{
    public partial class WakeUpForm : Form
    {
        //private CrashLogger logger;
        private ILogger logger;
        private XMLHandler xml;
        private ProcessWatcher watcher;
        private bool nonsleepmode = false;
        private readonly object serialiselock = new object();

        //constructor
        public WakeUpForm()
        {
            InitializeComponent();

            logger = new DebugLogger();
            watcher = new ProcessWatcher();
            xml = new XMLHandler(WakeupSettings.Default.url, Environment.ExpandEnvironmentVariables(WakeupSettings.Default.configPath), logger, WakeupSettings.Default.delayIntervallInMinutes);

            //Subscribe events
            xml.Changed += Changed;
            watcher.Changed += Changed;

            WakeUpIcon.Text = "Bildschirmschoner ist aktiviert!";
        }

        //methode is called when a subscribed event is triggered
        private void Changed(object sender, System.EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new EventHandler(Changed), sender, e);
            }
            else
            {
                lock (serialiselock)
                {
                    Check();
                }
            }
        }

        //verifies if a process is running
        private void Check()
        {
            if (checkIfProcessIsRunning())
            {
                //if no sleep is not set, set it
                if (!nonsleepmode)
                {
                    var result = NativeAPI.PreventSleepAndMonitorOff(logger);

                    //verify the result
                    if (!result )
                    {
                        logger.log("Cannot prevent sleep");
                    }
                    else
                    {
                        logger.log("Sleep prevented");
                        nonsleepmode = true;
                        WakeUpIcon.Icon = WakeNonSleepIcon.Icon;
                        WakeUpIcon.Text = "Bildschirmschoner ist deaktiviert! ";
                    }
                }
            }
            else
            {
                //if no sleep is set, unset it
                if (nonsleepmode)
                {
                    var result = NativeAPI.AllowSleep(logger);
                    if (!result)
                    {
                        logger.log("Cannot allow sleep");
                    }
                    else
                    {
                        logger.log("Sleep allowed");
                        nonsleepmode = false;
                        WakeUpIcon.Icon = WakeUpSleepIcon.Icon;
                        WakeUpIcon.Text = "Bildschirmschoner ist aktiviert!";
                    }
                }
            }
        }

        //checks if a defined process is running on the system
        private bool checkIfProcessIsRunning()
        {
            foreach (var process in Process.GetProcesses())
            {
                if (xml.getProcess().ContainsKey(process.ProcessName))
                {
                    return true;
                }
            }

            return false;
        }

        //default form parameters
        private void WakeUpForm_Load(object sender, System.EventArgs e)
        {
            logger.log("Started");
            Visible = false;

            Check();
            watcher.Start();
            xml.Start();
        }
    }
}
