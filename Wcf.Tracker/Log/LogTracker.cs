using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;

namespace Wcf.Tracker.Log
{
    /// <summary>
    /// Accumulating messages log information. 
    /// </summary>
    internal class LogTracker
    {
        private static LogTracker _instance;

        private readonly Dictionary<UniqueId, DateTime> _messageTimestamps;

        private bool _isLogging;

        /// <summary>
        /// Constructor for <see cref="LogTracker"/>.
        /// </summary>
        private LogTracker()
        {
            _messageTimestamps = new Dictionary<UniqueId, DateTime>();
        }

        /// <summary>
        /// Singleton instance reference.
        /// </summary>
        public static LogTracker Instance
        {
            get
            {
                return _instance ?? (_instance = new LogTracker());
            }
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
        internal void ProcessMessage(MessageDirection direction, string actionUrl, string size, UniqueId messageId, string sourceText)
        {
            var stackTrace = TrackerUtils.GetStackTrace(3);

            DateTime lastMessageTime;
            string replyDuration = string.Empty;
            if (messageId != null && _messageTimestamps.TryGetValue(messageId, out lastMessageTime))
            {
                var timeDiff = DateTime.Now - lastMessageTime;
                if ((int)timeDiff.TotalSeconds > 0)
                {
                    replyDuration = timeDiff.ToString("hh':'mm':'ss'.'fff");
                }
            }

            var traceFrame = new TraceFrame(
                direction,
                actionUrl,
                size,
                messageId,
                stackTrace,
                replyDuration,
                DateTime.Now,
                sourceText);

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