using RawTcp.Protocol;
using System;
using System.Buffers;
using System.IO.Pipelines;
using System.Text;
using System.Threading.Tasks;

namespace MemoryConsole
{
    public class SimpleConsole
    {
        private PipeReader _pipeReader;
        private PipeWriter _pipeWriter;
        private IMessageProtocol _lengthProtocol;

        public async Task RunAgainstProtocol()
        {
            _ = ReadPipeAsync();


            for (int i = 0; i < 100; i++)
            {
                Console.WriteLine($"Writer: {i}");
                await WritePipeAsync(i);
                await Task.Delay(TimeSpan.FromMilliseconds(500));
            }

            _pipeWriter.Complete();

        }

        public SimpleConsole()
        {
            var options = new PipeOptions(useSynchronizationContext: false);
            var pipe = new Pipe(options);
            _lengthProtocol = new LengthProtocol();

            _pipeWriter = pipe.Writer;
            _pipeReader = pipe.Reader;
        }

        public SimpleConsole(Pipe pipe, IMessageProtocol messageProtocol)
        {
            _pipeReader = pipe.Reader;
            _pipeWriter = pipe.Writer;
            _lengthProtocol = messageProtocol;
        }

        private async Task WritePipeAsync(int i)
        {
            var msg = $"Message Number: {i}";
            var data = Encoding.UTF8.GetBytes(msg);

            _lengthProtocol.WriteMessage(new Message(data), _pipeWriter);
            await _pipeWriter.FlushAsync();
        }

        private async Task ReadPipeAsync()
        {
            Console.WriteLine("Starting to read pipe");

            while (true)
            {
                var result = await _pipeReader.ReadAsync();
                var rq = result.Buffer;

                SequencePosition consumed = rq.Start;
                SequencePosition examined = rq.Start;
                if (_lengthProtocol.TryParseMessage(rq, ref consumed, ref examined, out var msg))
                {
                    ProcessMessage(msg);
                }

                if (result.IsCompleted)
                {
                    break;
                }

                //TODO: Investigate how to use examine
                //need to advance#
                _pipeReader.AdvanceTo(consumed);
            }
        }

        private void ProcessMessage(Message msg)
        {
            var readResult = Encoding.UTF8.GetString(msg.Payload.ToArray());
            Console.WriteLine(readResult);
        }


    }
}
