using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipelines;
using System.Text;

namespace RawTcp.Server
{
    public class MessageReader
    {
        private readonly PipeReader _pipeReader;

        public MessageReader(PipeReader pipeReader)
        {
            _pipeReader = pipeReader;
        }

        //public MessageReader(Stream stream) : this(PipeReader.Create(stream)){}

    }
}
