
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Running;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MemoryConsole
{
    public class Program
    {
        public const int HeaderSize = 4;

        static async Task Main(string[] args)
        {
            //SimpleConsole sc = new SimpleConsole();
            //await sc.RunAgainstProtocol();

            BenchmarkRunner.Run<ProtocolBenchmarks>();
        }

    }


}
