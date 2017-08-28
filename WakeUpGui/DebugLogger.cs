using System;
using System.Diagnostics;
using System.Threading;

namespace WakeUpGui
{
    class DebugLogger : ILogger
    {
        public void log(string tolog)
        {
            Debug.WriteLine("Thread " + Thread.CurrentThread.ManagedThreadId.ToString() + " " + (tolog));
        }
    }
}
