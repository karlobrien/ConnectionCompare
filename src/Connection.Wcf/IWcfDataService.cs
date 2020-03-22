using System.ServiceModel;

namespace Connection.Wcf
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract(CallbackContract = typeof(IWcfDataServiceCallback))]
    public interface IWcfDataService
    {
        [OperationContract]
        string GetData(int value);

        [OperationContract]
        void Subscribe();

        [OperationContract]
        void UnSubscribe();
    }

    [ServiceContract]
    public interface IWcfDataServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void GetRisk(string riskData);
    }
}
