using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISdotNet.Core.Packets
{
    class Packet243
    {
        private static readonly PacketSerializer<StandingPoint> StandingPointSerializer = new PacketSerializer<StandingPoint>();

        public static StandingPoint ReadPacket(byte[] packet)
        {
            
            StandingPoint standingPoint; //= StandingPointSerializer.RawDeserialize(packet, 0);
            standingPoint = StandingPointSerializer.RawDeserialize(packet, 0);
                
            return standingPoint;
        }
    }
}
