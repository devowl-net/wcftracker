using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

using Wcf.Tracker.Log;

namespace Wcf.Tracker.Behaviors
{
    /// <summary>
    /// WCF message inspector.
    /// </summary>
    internal class MessageInspector : IClientMessageInspector
    {
        private readonly LogTracker _logTracker;

        /// <summary>
        /// Constructor for <see cref="MessageInspector"/>.
        /// </summary>
        /// <param name="logTracker"></param>
        public MessageInspector(LogTracker logTracker)
        {
            _logTracker = logTracker;
        }

        /// <inheritdoc/>
        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            if (request != null && _logTracker.IsLogging)
            {
                LogMessage(ref request);
                _logTracker.SaveMessageId(request.Headers.MessageId);
            }

            return null;
        }

        /// <inheritdoc/>
        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            if (reply != null && _logTracker.IsLogging)
            {
                LogMessage(ref reply);
            }
        }

        private void LogMessage(ref Message request)
        {
            var stackTrace = TrackerUtils.GetStackTrace(3);
            DebuggerBreakCheck(stackTrace, request.Headers.Action);

            var bufferedCopy = request.CreateBufferedCopy(int.MaxValue);
            request = bufferedCopy.CreateMessage();
            var requestCopy = bufferedCopy.CreateMessage();

            var actionUrl = requestCopy.Headers.Action;
            var size = requestCopy.GetHumanSize();
            var messageId = requestCopy.Headers.MessageId;
            var direction = MessageDirection.Outgoing;

            if (messageId == null)
            {
                messageId = requestCopy.Headers.RelatesTo;
                direction = MessageDirection.Incomming;
            }

            _logTracker.ProcessMessage(direction, actionUrl, size, messageId, request.ToString(), stackTrace);
        }

        private void DebuggerBreakCheck(string stackTrace, string action)
        {
            var collection = LogTracker.Instance.TraceFrames.ToArray();
            if (collection.Any(frame => frame.IsBreakpoint && frame.CallStack == stackTrace && frame.ActionUrl == action))
            {
                Debugger.Break();
            }
        }
    }
}