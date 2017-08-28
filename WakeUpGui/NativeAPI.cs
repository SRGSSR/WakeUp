using System.Runtime.InteropServices;
using System.Diagnostics;

/// <summary>
/// Calls kernel32.dll and sets the ThredExecutionState
/// </summary>
namespace WakeUpGui
{
    static class NativeAPI
    {
        internal static class NativeMethods
        {
            // Import SetThreadExecutionState Win32 API and necessary flags
            [DllImport("kernel32.dll")]
            public static extern uint SetThreadExecutionState(uint esFlags);
            public const uint ES_CONTINUOUS = 0x80000000;
            public const uint ES_SYSTEM_REQUIRED = 0x00000001;
            public const uint ES_DISPLAY_REQUIRED = 0x00000002;
        }

        public static uint allow = NativeMethods.ES_CONTINUOUS;
        public static uint prevent = NativeMethods.ES_CONTINUOUS |
                                       NativeMethods.ES_DISPLAY_REQUIRED |
                                       NativeMethods.ES_SYSTEM_REQUIRED;

        // Prevent the system from entering sleep and turning off monitor
        public static bool PreventSleepAndMonitorOff(ILogger logger)
        {
            var result =  NativeMethods.SetThreadExecutionState(prevent);

            if (result == 0)
            {
                logger.log("Unexpected return in prevent " + result);

                return false;
            }

            if (result == allow)
            {
                logger.log("Changed from allow to prevent");
            }

            if (result == prevent)
            {
                logger.log("Changed from prevent to prevent");
            }

            return true;
        }

        // Clear EXECUTION_STATE flags to allow the system to sleep and turn off monitor normally
        public static bool AllowSleep(ILogger logger)
        {
            var result = NativeMethods.SetThreadExecutionState(allow);

            if (result == 0)
            {
                logger.log("Unexpected return in allow " + result);

                return false;
            }

            if (result == prevent)
            {
                logger.log("Changed from prevent to allow");
            }

            if (result == allow)
            {
                logger.log("Changed from allow to allow");
            }

            return true;
        }
    }
}
