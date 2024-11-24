using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSeasonDungeonInterruptSoulOrdealRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_SEASON_DUNGEON_INTERRUPT_SOUL_ORDEAL_RES;

        public S2CSeasonDungeonInterruptSoulOrdealRes()
        {
        }

        public class Serializer : PacketEntitySerializer<S2CSeasonDungeonInterruptSoulOrdealRes>
        {
            public override void Write(IBuffer buffer, S2CSeasonDungeonInterruptSoulOrdealRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CSeasonDungeonInterruptSoulOrdealRes Read(IBuffer buffer)
            {
                S2CSeasonDungeonInterruptSoulOrdealRes obj = new S2CSeasonDungeonInterruptSoulOrdealRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
