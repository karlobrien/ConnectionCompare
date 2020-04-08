using System;
using System.Buffers;
using System.Buffers.Binary;

namespace RawTcp.Protocol
{
    public class LengthProtocol : IMessageProtocol
    {
        const int HeaderSize = 4;
        public void WriteMessage(Message message, IBufferWriter<byte> output)
        {
            var header = output.GetSpan(HeaderSize);
            BinaryPrimitives.WriteUInt32BigEndian(header, (uint)message.Payload.Length);
            output.Advance(HeaderSize);

            foreach (var msg in message.Payload)
                output.Write(msg.Span);
        }

        public bool TryParseMessage(in ReadOnlySequence<byte> input, ref SequencePosition consumed, ref SequencePosition examined, out Message message)
        {
            var reader = new SequenceReader<byte>(input);

            if (input.Length < HeaderSize)
            {
                message = default;
                return false;
            }

            //https://github.com/grpc/grpc-dotnet/blob/6504c26be7ff763f6367daf1a43b8c01eefc3e7a/src/Grpc.AspNetCore.Server/Internal/PipeExtensions.cs#L158-L184
            int length = 0;
            if (input.Length >= HeaderSize)
            {
                var header = input.First.Span.Slice(0, HeaderSize);
                length = BinaryPrimitives.ReadInt32BigEndian(header);
            }
            else
            {
                Span<byte> header = stackalloc byte[HeaderSize];
                input.Slice(0, HeaderSize).CopyTo(header);
                length = BinaryPrimitives.ReadInt32BigEndian(header);
            }

            var t = input.Slice(HeaderSize, length);
            message = new Message(t);

            consumed = t.End;
            examined = consumed;

            return true;
        }
    }
}
