using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;

using Wcf.Tracker.Log;
using Wcf.Tracker.Sniffer;

namespace Wcf.Tracker
{
    /// <summary>
    /// Log service extensions.
    /// </summary>
    public static class LogExtentions
    {
        /// <summary>
        /// Attach tracker to existing <see cref="ServiceEndpoint"/>.
        /// </summary>
        /// <param name="serviceEndpoint">Service point.</param>
        public static ServiceEndpoint AttachTracker(this ServiceEndpoint serviceEndpoint)
        {
            if (serviceEndpoint == null)
            {
                throw new ArgumentNullException(nameof(serviceEndpoint));
            }

            if (!serviceEndpoint.Behaviors.OfType<EndpointTrackerBehavior>().Any())
            {
                serviceEndpoint.Behaviors.Add(new EndpointTrackerBehavior(LogTracker.Instance));
            }

            return serviceEndpoint;
        }

        /// <summary>
        /// Attach tracker to existing <see cref="ServiceEndpoint"/>.
        /// </summary>
        /// <param name="factory">Channel factory.</param>
        public static ChannelFactory<TChannel> AttachTracker<TChannel>(this ChannelFactory<TChannel> factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            AttachTracker(factory.Endpoint);
            return factory;
        }
    }
}