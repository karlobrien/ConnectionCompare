using Connection.WcfClient.ServiceReference1;
using System;
using System.ServiceModel;

namespace Connection.WcfClient
{
    public class Program : IWcfDataServiceCallback
    {
        static void Main(string[] args)
        {
            //ChannelFactory<IWcfDataServiceCallback> factory = new ChannelFactory<IWcfDataServiceCallback>("CallbackClientEndpoint");
            //sIWcfDataServiceCallback callback = factory.CreateChannel(new EndpointAddress("net.tcp://localhost:8523/TcpService"));

            var test = new Runner();

            Console.ReadLine();
        }

        public void GetRisk(string riskData)
        {
            Console.WriteLine(riskData);
        }
    }

    public class Runner : IWcfDataServiceCallback
    {
        public Runner()
        {
            var dataServiceClient = new WcfDataServiceClient(new InstanceContext(this));
            dataServiceClient.Subscribe();
        }

        public void GetRisk(string riskData)
        {
            Console.WriteLine(riskData);
        }

    }
}
