using System.ServiceModel;

namespace Wcf.Demonstration.WcfService
{
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        int Sum(int value1, int value2);
    }
}
