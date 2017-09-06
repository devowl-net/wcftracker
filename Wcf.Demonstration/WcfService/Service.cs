using System;
using System.ServiceModel;
using System.Threading;

namespace Wcf.Demonstration.WcfService
{
    // UseSynchronizationContext = false is required if creation scope is UI thread.
    [ServiceBehavior(UseSynchronizationContext = false, InstanceContextMode = InstanceContextMode.PerCall)]
    internal class Service : IService
    {
        public int Sum(int value1, int value2)
        {
            var r = new Random();
            var random = r.Next(0, 10);
            if (random < 3)
            {
                Thread.Sleep(TimeSpan.FromSeconds(random));
            }

            return value1 + value2;
        }
    }
}
