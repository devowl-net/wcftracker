using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;

using Wcf.Tracker.Sniffer;

namespace Wcf.Tracker.Log
{
    /// <summary>
    /// Accumulating messages log information. 
    /// </summary>
    internal class LogTracker
    {
        private readonly Dictionary<UniqueId, DateTime> _messageTimestamps;

        private bool _isLogging;

        /// <summary>
        /// Constructor for <see cref="LogTracker"/>.
        /// </summary>
        public LogTracker()
        {
            _messageTimestamps = new Dictionary<UniqueId, DateTime>();
        }

        /// <summary>
        /// Is logging enabled.
        /// </summary>
        public bool IsLogging
        {
            get
            {
                return _isLogging;
            }

            set
            {
                _isLogging = value;
                if (!_isLogging)
                {
                    _messageTimestamps.Clear();
                }
            }
        }

        /// <summary>
        /// Trace frames roster.
        /// </summary>
        public ObservableCollection<TraceFrame> TraceFrames { get; } = new ObservableCollection<TraceFrame>();
        
        /// <summary>
        /// Process new message.
        /// </summary>
        /// <param name="direction">Message direction.</param>
        /// <param name="actionUrl">Service action URL.</param>
        /// <param name="size">Message size.</param>
        /// <param name="messageId">Message identity.</param>
        internal void ProcessMessage(MessageDirection direction, string actionUrl, string size, UniqueId messageId)
        {
            var stackTrace = TrackerUtils.GetStackTrace(2);
            var traceFrame = new TraceFrame(
                direction,
                actionUrl,
                size,
                messageId,
                stackTrace);

            TraceFrames.Add(traceFrame);
        }

        /// <summary>
        /// Save information about message.
        /// </summary>
        /// <param name="messageId">Message identity.</param>
        internal void SaveMessageId(UniqueId messageId)
        {
            _messageTimestamps.Add(messageId, DateTime.Now);
        }
    }
}