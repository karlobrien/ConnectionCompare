using System;
using System.Buffers;
using System.Buffers.Binary;
using System.IO.Pipelines;
using System.Text;
using System.Threading.Tasks;

namespace MemoryConsole
{
    class Program
    {
        public const int HeaderSize = 4;

        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var data = Encoding.UTF8.GetBytes("Hello World");
            Console.WriteLine(data.Length);
            var options = new PipeOptions(useSynchronizationContext: false);

            var pipe = new Pipe(options);

            var writer = pipe.Writer;

            CreateHeader(writer, data.Length);

            WriteMe(new ReadOnlySequence<byte>(data), writer);

            await writer.FlushAsync();
           
            var reader = pipe.Reader;

            var result = await reader.ReadAsync();
            var rq = result.Buffer;

            if (TryReadMessage(ref rq, out var response))
            {
                var readResult = Encoding.UTF8.GetString(response);
                Console.WriteLine(readResult);
            }

            
            
            //foreach(var item in rq)
            //{
            //    var readResult = Encoding.UTF8.GetString(item.Span);
            //    Console.WriteLine(readResult);
            //}
            //convert back


        }

        public static bool TryReadMessage(ref ReadOnlySequence<byte> buffer, out byte[] message)
        {
            if (!TryReadHeader(ref buffer, out int length))
            {
                message = null;
                return false;
            }

            message = buffer.Slice(HeaderSize, length).ToArray();
            return true;
        }

        public static bool TryReadHeader(ref ReadOnlySequence<byte> buffer, out int messageLength)
        {
            if (buffer.Length < HeaderSize)
            {
                messageLength = default;
                return false;
            }

            if (buffer.Length >= HeaderSize)
            {
                var header = buffer.First.Span.Slice(0, HeaderSize);
                messageLength = BinaryPrimitives.ReadInt32BigEndian(header);
            }
            else
            {
                Span<byte> header = stackalloc byte[HeaderSize];
                buffer.Slice(0, HeaderSize).CopyTo(header);
                messageLength = BinaryPrimitives.ReadInt32BigEndian(header);
            }

            return true;
        }


        public static void CreateHeader(IBufferWriter<byte> wt, int length)
        {
            var header = wt.GetSpan(HeaderSize);

            BinaryPrimitives.WriteUInt32BigEndian(header, (uint)length);
            wt.Advance(HeaderSize);
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
