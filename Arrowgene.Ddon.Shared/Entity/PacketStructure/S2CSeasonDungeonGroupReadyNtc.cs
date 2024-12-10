using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSeasonDungeonGroupReadyNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_SEASON_62_23_16_NTC;

        public S2CSeasonDungeonGroupReadyNtc()
        {
        }

        public bool Ready { get; set; }

        public class Serializer : PacketEntitySerializer<S2CSeasonDungeonGroupReadyNtc>
        {
            public override void Write(IBuffer buffer, S2CSeasonDungeonGroupReadyNtc obj)
            {
                WriteBool(buffer, obj.Ready);
            }

            public override S2CSeasonDungeonGroupReadyNtc Read(IBuffer buffer)
            {
                S2CSeasonDungeonGroupReadyNtc obj = new S2CSeasonDungeonGroupReadyNtc();
                obj.Ready = ReadBool(buffer);
                return obj;
            }
        }
    }
}
