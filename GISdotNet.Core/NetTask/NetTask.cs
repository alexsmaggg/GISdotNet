using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Threading.Channels;
using System.Threading.Tasks;
using GISdotNet.Core.Packets;
using GISdotNet.Core.Net;


namespace GISdotNet.Core.NetTask
{
    public delegate void MapWork<T>(T val);
    public delegate T Reader<T>(byte[] PacketByte);

    public class NetTask<T>
    {
        private Channel<T> channel;
        private readonly ChannelWriter<T> _writer;
        private readonly ChannelReader<T> _reader;

        public NetTask()
        {
            channel = Channel.CreateUnbounded<T>();
            _writer = channel.Writer;
            _reader = channel.Reader;
        }



        public async Task RecieveBytesEndWriteToChannel(ushort port, Reader<T> packetReader)
        {
            UdpClient client = UdpSocket.CreateUdpClient(port);
            var from = new IPEndPoint(0, 0);
            while (true)
            {
                byte[] recvBuffer = client.Receive(ref from);
                T result = packetReader(recvBuffer);
                await _writer.WriteAsync(result);
            }

        }


        public async Task WriteToChannel(ChannelWriter<T> writer, Reader<T> packetReader, byte[] packetBytes) 
        {
            T result = packetReader(packetBytes);
            await _writer.WriteAsync(result);
        } 

        public async Task Read(MapWork<T> work)
        {
            while (await _reader.WaitToReadAsync())
            {
                while (_reader.TryRead(out T data))
                {  
                    work(data);     // some map work on data              
                }
            }
        }

    }
}
