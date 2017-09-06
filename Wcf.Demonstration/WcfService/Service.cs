using System.ServiceModel;

namespace Wcf.Demonstration.WcfService
{
    // UseSynchronizationContext = false is required if creation scope is UI thread.
    [ServiceBehavior(UseSynchronizationContext = false, InstanceContextMode = InstanceContextMode.PerCall)]
    internal class Service : IService
    {
        public int Sum(int value1, int value2)
        {
            return value1 + value2;
        }
    }
}
