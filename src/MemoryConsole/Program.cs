using System;
using System.Buffers;
using System.IO.Pipelines;
using System.Text;
using System.Threading.Tasks;

namespace MemoryConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var data = Encoding.UTF8.GetBytes("Hello World");
            var options = new PipeOptions(useSynchronizationContext: false);

            var pipe = new Pipe(options);

            var writer = pipe.Writer;

            WriteMe(new ReadOnlySequence<byte>(data), writer);
            //await writer.WriteAsync(data);


            var reader = pipe.Reader;

            var result = await reader.ReadAsync();

            var rq = result.Buffer;

        }

        public static void Write(IBufferWriter<byte> wt, ReadOnlySpan<byte> message)
        {
            wt.Write(message);
        }

        public static void WriteMe(ReadOnlySequence<Byte> messge, IBufferWriter<byte> writer)
        {
            foreach(var item in messge)
            {
                writer.Write(item.Span);
            }
        }
    }
}
