using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSeasonDungeonExecuteSoulOrdealRes : ServerResponse
    {
        public S2CSeasonDungeonExecuteSoulOrdealRes()
        {
        }

        public override PacketId Id => PacketId.S2C_SEASON_DUNGEON_EXECUTE_SOUL_ORDEAL_RES;

        public class Serializer : PacketEntitySerializer<S2CSeasonDungeonExecuteSoulOrdealRes>
        {
            public override void Write(IBuffer buffer, S2CSeasonDungeonExecuteSoulOrdealRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CSeasonDungeonExecuteSoulOrdealRes Read(IBuffer buffer)
            {
                S2CSeasonDungeonExecuteSoulOrdealRes obj = new S2CSeasonDungeonExecuteSoulOrdealRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}

