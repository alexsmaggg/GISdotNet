using System;
using System.Linq;
using System.Collections.Generic;

namespace GISdotNet.Core.Packets
{
    public class PacketReader
    {
        private static readonly PacketSerializer<Header> HeaderSerializer = new PacketSerializer<Header>();
             
        public static Header ReadHeader(byte[] PacketBytes)
        {
            byte[] headerBytes = Get12Bytes(PacketBytes);
            Header header = HeaderSerializer.RawDeserialize(headerBytes, 0);
            return header;
        }

        private static byte[] Get12Bytes(byte[] PacketBytes)
        {
            byte[] headerBytes = PacketBytes.Take(12).ToArray();
            return headerBytes;
        }
        
        private static byte[] GetRestPacketBytes(byte[] PacketBytes)
        {
            byte[] BodyBytes = PacketBytes.Skip(12).ToArray();           
            return BodyBytes;
        }

        public static byte[] GetPacketBody(byte[] PacketBytes) 
        {
            return GetRestPacketBytes(PacketBytes);
        }

        public static byte[] AppendPacketIdByte(byte[] PacketBytes, byte PacketId)
        {
            byte[] PacketBodyWithPacketId = new byte[PacketBytes.Length + 1];
            PacketBytes.CopyTo(PacketBodyWithPacketId, 1);
            PacketBodyWithPacketId[0] = PacketId;
            return PacketBodyWithPacketId;

        }
       
        public static List<Target> ReadTargets(byte[] bytes)
        {
            byte[] targetBytes = bytes.Skip(1).ToArray(); // Первый байт количество целей в пакете
            return Packet246.ReadPacket(targetBytes);
        }

        public static StandingPoint ReadStandingPoints(byte[] bytes) 
        {
            return Packet243.ReadPacket(bytes);
        }

        public static Azimuth ReadAzimuth(byte[] bytes) 
        {
            return Packet241.ReadPacket(bytes);
        }
      
    }
}
