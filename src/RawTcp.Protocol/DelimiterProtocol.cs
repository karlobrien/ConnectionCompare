using System;
using System.Buffers;

namespace RawTcp.Protocol
{
    public class DelimiterProtocol : IMessageProtocol
    {
        private readonly byte _delimiter;

        public DelimiterProtocol(char delimiter)
        {
            _delimiter = (byte)delimiter;
        }

        public bool TryParseMessage(in ReadOnlySequence<byte> input, ref SequencePosition consumed, ref SequencePosition examined, out Message message)
        {
            throw new NotImplementedException();
        }

        public void WriteMessage(Message message, IBufferWriter<byte> output)
        {
            throw new NotImplementedException();
        }
    }
}
