using Connection.Wcf;
using System;
using System.ServiceModel;

namespace Connection.WcfRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var host = new ServiceHost(typeof(WcfDataService)))
            {
                host.Open();
                Console.WriteLine("Service is running...");
                Console.WriteLine("Service address: " + host.BaseAddresses[0]);
                Console.Read();
            }
        }
    }
}
