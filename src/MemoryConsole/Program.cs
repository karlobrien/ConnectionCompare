
using RawTcp.Protocol;
using System;
using System.Buffers;
using System.IO.Pipelines;
using System.Text;
using System.Threading.Tasks;

namespace MemoryConsole
{
    public class Program
    {
        public const int HeaderSize = 4;

        static async Task Main(string[] args)
        {
            Console.WriteLine("Server!");

            LengthProtocol pl = new LengthProtocol();
            var data = Encoding.UTF8.GetBytes("Hello World");
            Console.WriteLine(data.Length);
            var options = new PipeOptions(useSynchronizationContext: false);

            var pipe = new Pipe(options);

            var writer = pipe.Writer;

            pl.WriteMessage(new Message(data), writer);

            await writer.FlushAsync();

            var reader = pipe.Reader;

            var result = await reader.ReadAsync();
            var rq = result.Buffer;

            SequencePosition consumed = rq.Start;
            SequencePosition examined = rq.Start;
            if (pl.TryParseMessage(rq, ref consumed, ref examined, out var msg))
            {
                var readResult = Encoding.UTF8.GetString(msg.Payload.ToArray());
                Console.WriteLine(readResult);
            }

        }


    }


}
