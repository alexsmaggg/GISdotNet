using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISdotNet.Core.Packets
{
    class Packet241
    {
        private static readonly PacketSerializer<Azimuth> AzimuthSerializer = new PacketSerializer<Azimuth>();

        public static Azimuth ReadPacket(byte[] packet)
        {
            Azimuth Azmth = AzimuthSerializer.RawDeserialize(packet, 0);
            return Azmth;
        }
    }
}
