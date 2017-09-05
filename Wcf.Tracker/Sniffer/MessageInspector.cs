using System;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

using Wcf.Tracker.Log;

namespace Wcf.Tracker.Sniffer
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
                LogMessage(request);
                _logTracker.SaveMessageId(request.Headers.MessageId);
            }

            return null;
        }

        /// <inheritdoc/>
        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            if (reply != null && _logTracker.IsLogging)
            {
                LogMessage(reply);
            }
        }

        private void LogMessage(Message request)
        {
            var actionUrl = request.Headers.Action;
            var size = request.GetHumanSize();
            var messageId = request.Headers.MessageId;
            var direction = MessageDirection.Outgoing;

            if (messageId == null)
            {
                messageId = request.Headers.RelatesTo;
                direction = MessageDirection.Incomming;
            }

            _logTracker.ProcessMessage(direction, actionUrl, size, messageId);
        }
    }
}