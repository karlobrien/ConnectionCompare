# ConnectionCompare
Trying to test out various connectivity libs

## WCF
Use callback to pass messages from server to client

## Raw Tcp

 * Prefix every message with an integer that tells the length of the message.
 * All messages be fixed length. And both client and server must know the length before the message is sent.
 * Append every message with a delimiter to show where it ends. And both client and server must know what the delimiter is before the message is sent.

### Borrowing Heavily from the following
 * https://github.com/grpc/grpc-dotnet/blob/6504c26be7ff763f6367daf1a43b8c01eefc3e7a/src/Grpc.AspNetCore.Server/Internal/PipeExtensions.cs#L158-L184
 * https://stackoverflow.com/questions/56046176/system-io-pipelines-length-field-based-tcp-decoding
 * https://github.com/davidfowl/TcpEcho/blob/master/src/Server/Program.cs

