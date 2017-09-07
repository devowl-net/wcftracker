using System;
using System.Xml;

namespace Wcf.Tracker.Log
{
    /// <summary>
    /// Contains full information about message.
    /// </summary>
    internal class TraceFrame
    {
        /// <summary>
        /// Constructor for <see cref="TraceFrame"/>.
        /// </summary>
        public TraceFrame(
            MessageDirection direction,
            string actionUrl,
            string size,
            UniqueId messageId,
            string callStack,
            string replyDuration,
            DateTime eventTime,
            string sourceText)
        {
            Direction = direction;
            ActionUrl = actionUrl;
            Size = size;
            MessageId = messageId;
            CallStack = callStack;
            ReplyDuration = replyDuration;
            EventTime = eventTime;
            SourceText = sourceText;
        }

        /// <summary>
        /// Message direction.
        /// </summary>
        public MessageDirection Direction { get; private set; }

        /// <summary>
        /// Service action URL.
        /// </summary>
        public string ActionUrl { get; private set; }

        /// <summary>
        /// Message size.
        /// </summary>
        public string Size { get; private set; }

        /// <summary>
        /// Unique message id.
        /// </summary>
        internal UniqueId MessageId { get; private set; }

        /// <summary>
        /// Calling stack.
        /// </summary>
        public string CallStack { get; private set; }

        /// <summary>
        /// Reply time for response messages.
        /// </summary>
        public string ReplyDuration { get; private set; }

        /// <summary>
        /// Message event time.
        /// </summary>
        public DateTime EventTime { get; private set; }

        /// <summary>
        /// Source message text.
        /// </summary>
        public string SourceText { get; private set; }

        /// <summary>
        /// Is trace frame is breakpoint.
        /// </summary>
        public bool IsBreakpoint { get; set; }
    }
}