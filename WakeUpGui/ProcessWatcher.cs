using System;
using System.Management;

namespace WakeUpGui
{
    class ProcessWatcher
    {
        private ManagementEventWatcher startWatcher;
        private ManagementEventWatcher stopWatcher;

        private static string scope = @"\\.\root\CIMV2"; // The dot in the scope means use the current machine
        private static string queryStart = "SELECT TargetInstance" +
                    "  FROM __InstanceCreationEvent " +
                    "WITHIN  " + WakeupSettings.Default.refreshInvervallInSeconds +
                    " WHERE TargetInstance ISA 'Win32_Process' ";
        private static string queryStop =
                "SELECT TargetInstance" +
                "  FROM __InstanceDeletionEvent " +
                "WITHIN  " + WakeupSettings.Default.refreshInvervallInSeconds +
                " WHERE TargetInstance ISA 'Win32_Process' ";

        public delegate void ChangedEventHandler(object sender, EventArgs e);
        public event ChangedEventHandler Changed;
        
        //constructor
        public ProcessWatcher()
        {
            startWatcher = new ManagementEventWatcher(scope, queryStart);
            stopWatcher = new ManagementEventWatcher(scope, queryStop);

            startWatcher.EventArrived += ProcessChanged;
            stopWatcher.EventArrived += ProcessChanged;
        }

        //this methode is called when something has changed
        public void OnChanged(EventArgs e)
        {
            Changed?.Invoke(this, e);
        }

        //start the two watchers
        public void Start()
        {
            startWatcher.Start();
            stopWatcher.Start();
        }

        //if a process has changed locally fire methode
        private void ProcessChanged(object sender, EventArrivedEventArgs e)
        {
            OnChanged(EventArgs.Empty);
        }
    }
}