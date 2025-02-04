using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSeasonDungeonSoulOrdealReadyRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_SEASON_DUNGEON_SOUL_ORDEAL_READY_RES;

        public S2CSeasonDungeonSoulOrdealReadyRes()
        {
        }

        public class Serializer : PacketEntitySerializer<S2CSeasonDungeonSoulOrdealReadyRes>
        {
            public override void Write(IBuffer buffer, S2CSeasonDungeonSoulOrdealReadyRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CSeasonDungeonSoulOrdealReadyRes Read(IBuffer buffer)
            {
                S2CSeasonDungeonSoulOrdealReadyRes obj = new S2CSeasonDungeonSoulOrdealReadyRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
