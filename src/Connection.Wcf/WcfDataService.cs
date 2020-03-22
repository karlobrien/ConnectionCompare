using System;
using System.ServiceModel;
using System.Threading;

namespace Connection.Wcf
{

    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant, InstanceContextMode = InstanceContextMode.PerSession)]
    public class WcfDataService : IWcfDataService
    {

        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        static event EventHandler<DataEventArgs> DataArg;
        IWcfDataServiceCallback _callback;

        public void Subscribe()
        {
            Console.WriteLine("Hitting Subscribe");
            _callback = OperationContext.Current.GetCallbackChannel<IWcfDataServiceCallback>();
            Console.WriteLine("Callback created");
            DataArg += new EventHandler<DataEventArgs>(WeatherService_WeatherReporting);
        }

        void WeatherService_WeatherReporting(object sender, DataEventArgs e)
        {
            // Remember check the callback channel's status before using it.
            if (((ICommunicationObject)_callback).State == CommunicationState.Opened)
                _callback.GetRisk(e.Data);
            else
                UnSubscribe();
        }

        public void UnSubscribe()
        {
            DataArg -= new EventHandler<DataEventArgs>(WeatherService_WeatherReporting);
        }

        static WcfDataService()
        {
            ThreadPool.QueueUserWorkItem(
                new WaitCallback(delegate
                {
                    string[] weatherArray = { "Sunny", "Windy", "Snow", "Rainy" };
                    Random rand = new Random();

                    while (true)
                    {
                        Thread.Sleep(5000);
                        Console.WriteLine("Sending");
                        if (DataArg != null)
                            DataArg(
                                null,
                                new DataEventArgs
                                {
                                    Data = weatherArray[rand.Next(weatherArray.Length)]
                                });
                    }
                }));
        }
    }

    class DataEventArgs : EventArgs
    {
        public string Data { set; get; }
    }
}
