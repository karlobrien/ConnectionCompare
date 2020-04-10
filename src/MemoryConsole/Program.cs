
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

            var options = new PipeOptions(useSynchronizationContext: false);
            LengthProtocol lp = new LengthProtocol();
            var pipe = new Pipe(options);

            var writer = pipe.Writer;
            var reader = pipe.Reader;
            _ = ReadPipeAsync(reader, lp);


            for (int i = 0; i < 100; i++)
            {
                Console.WriteLine($"Writer: {i}");
                await WritePipeAsync(writer, lp, i);
                await Task.Delay(TimeSpan.FromMilliseconds(500));
            }

            writer.Complete();

        }

        private static async Task WritePipeAsync(PipeWriter writer, IMessageProtocol messageProtocol, int i)
        {
            var 
                msg = $"Message Number: {i}";
            var data = Encoding.UTF8.GetBytes(msg);

            messageProtocol.WriteMessage(new Message(data), writer);
            await writer.FlushAsync();
        }

        private static async Task ReadPipeAsync(PipeReader reader, IMessageProtocol messageProtocol)
        {
            Console.WriteLine("Starting to read pipe");

            while (true)
            {
                var result = await reader.ReadAsync();
                var rq = result.Buffer;

                SequencePosition consumed = rq.Start;
                SequencePosition examined = rq.Start;
                if (messageProtocol.TryParseMessage(rq, ref consumed, ref examined, out var msg))
                {
                    ProcessMessage(msg);
                }

                if (result.IsCompleted)
                {
                    break;
                }

                //TODO: Investigate how to use examine
                //need to advance#
                reader.AdvanceTo(consumed);
            }
        }

        private static void ProcessMessage(Message msg)
        {
            var readResult = Encoding.UTF8.GetString(msg.Payload.ToArray());
            Console.WriteLine(readResult);
        }


    }


}
