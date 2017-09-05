using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

using Wcf.Tracker.Log;

namespace Wcf.Tracker.Sniffer
{
    /// <summary>
    /// Endpoint tracker behavior.
    /// </summary>
    internal class EndpointTrackerBehavior : IEndpointBehavior
    {
        private readonly LogTracker _logTracker;

        /// <summary>
        /// Constructor for <see cref="EndpointTrackerBehavior"/>.
        /// </summary>
        public EndpointTrackerBehavior(LogTracker logTracker)
        {
            _logTracker = logTracker;
        }

        /// <inheritdoc/>
        public void Validate(ServiceEndpoint endpoint)
        {
        }

        /// <inheritdoc/>
        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        /// <inheritdoc/>
        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
        }

        /// <inheritdoc/>
        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            if (!clientRuntime.MessageInspectors.OfType<MessageInspector>().Any())
            {
                clientRuntime.MessageInspectors.Add(new MessageInspector(_logTracker));
            }
        }
    }
}