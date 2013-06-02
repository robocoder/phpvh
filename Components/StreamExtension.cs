using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

using Components;

namespace Components
{
    [DebuggerStepThrough]
    public static class StreamExtension
    {
        public static byte[] Read(this Stream SourceStream, int BufferSize)
        {
            byte[] buffer = new byte[BufferSize];

            int len = SourceStream.Read(buffer, 0, BufferSize);

            Array.Resize(ref buffer, len);

            return buffer;
        }

        public static void Write(this Stream DestinationStream, byte[] Buffer)
        {
            DestinationStream.Write(Buffer, 0, Buffer.Length);
        }

        public static void Write(this Stream sourceStream, Stream destinationStream)
        {
            byte[] bytes;

            while ((bytes = sourceStream.Read(8192)).Length != 0)
                destinationStream.Write(bytes);
        }

        public static string ReadString(this Stream DestinationStream, int BufferSize)
        {
            return Read(DestinationStream, BufferSize).GetString();
        }

        public static void WriteString(this Stream DestinationStream, string Buffer)
        {
            Write(DestinationStream, Buffer.GetBytes());
        }
    }
}
