using System;
using System.Linq;
using System.Collections.Generic;

namespace GISdotNet.Core.Packets
{
    public class Packet246
    {
  
        private static readonly PacketSerializer<Targets> TargetsSerializer = new PacketSerializer<Targets>();
        private static readonly PacketSerializer<Target>  TargetSerializer  = new PacketSerializer<Target>();

     
        public static List<Target> ReadPacket(byte[] packet)
        {
            //Targets TargetsPacket = TargetsSerializer.RawDeserialize(packet, 0);
            List<Target> targets = new List<Target>();
            Target target;

            byte[][] targetArrays = packet
                    .Select((s, i) => new { Value = s, Index = i })
                    .GroupBy(x => x.Index / 45) // тут 45 - это длинна одного подмассива
                    .Select(grp => grp.Select(x => x.Value).ToArray())
                    .ToArray();
            foreach (byte[] trg in targetArrays)
            {
                target = TargetSerializer.RawDeserialize(trg, 0);
                targets.Add(target);
            }

            return targets;
        }

        public void Dispose()
        {
            ((IDisposable)TargetsSerializer).Dispose();
            ((IDisposable)TargetSerializer).Dispose();
        }
        
    }
}
