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
        public TraceFrame(MessageDirection direction, string actionUrl, string size, UniqueId messageId, string callStack)
        {
            Direction = direction;
            ActionUrl = actionUrl;
            Size = size;
            MessageId = messageId;
            CallStack = callStack;
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
    }
}