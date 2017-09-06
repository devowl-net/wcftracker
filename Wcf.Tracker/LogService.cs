using Wcf.Tracker.Log;
using Wcf.Tracker.Presentation.Views;

namespace Wcf.Tracker
{
    /// <summary>
    /// Service for simple events registration.
    /// </summary>
    public static class LogService
    {
        private static TraceWindow _traceWindow;

        /// <summary>
        /// Show trace log windows.
        /// </summary>
        public static void ShowWindow()
        {
            if (_traceWindow == null)
            {
                _traceWindow = new TraceWindow();
                _traceWindow.Closed += (a, e) =>
                {
                   _traceWindow = null;
                   LogTracker.Instance.IsLogging = false;
                };

                LogTracker.Instance.IsLogging = true;
                _traceWindow.Show();
            }
        }
    }
}