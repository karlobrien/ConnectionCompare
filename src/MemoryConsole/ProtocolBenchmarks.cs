using BenchmarkDotNet.Attributes;
using RawTcp.Protocol;
using System;
using System.IO.Pipelines;
using System.Threading.Tasks;

namespace MemoryConsole
{
    [MemoryDiagnoser]
    public class ProtocolBenchmarks
    {
        [Params(10, 100, 1000)]
        public int MessageSize { get; set; }

        private Pipe dataPipe;
        private PipeWriter _writer;
        private PipeReader _reader;
        private LengthProtocol _lengthProtocol;
        private Message _message;
        private byte[] data;

        [GlobalSetup]
        public void Setup()
        {
            var options = new PipeOptions(useSynchronizationContext: false);
            dataPipe = new Pipe(options);
            _reader = dataPipe.Reader;
            _writer = dataPipe.Writer;
            _lengthProtocol = new LengthProtocol();
            data = new byte[MessageSize];
            data.AsSpan().Fill(1);
            _message = new Message(data);
        }

        [Benchmark]
        public async ValueTask ReadProtocolWithWholeMessageAvailable()
        {
            var m = new Message(data);
            _lengthProtocol.WriteMessage(m, _writer);
            await _writer.FlushAsync();

            var result = await _reader.ReadAsync();
            var rq = result.Buffer;

            SequencePosition consumed = rq.Start;
            SequencePosition examined = rq.Start;
            var readResult = _lengthProtocol.TryParseMessage(rq, ref consumed, ref examined, out var msg);

        }
    }
}
