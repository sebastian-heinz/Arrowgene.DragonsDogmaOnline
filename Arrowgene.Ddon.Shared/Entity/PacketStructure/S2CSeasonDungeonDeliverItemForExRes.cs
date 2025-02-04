using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSeasonDungeonDeliverItemForExRes : ServerResponse
    {
        public S2CSeasonDungeonDeliverItemForExRes()
        {
        }

        public override PacketId Id => PacketId.S2C_SEASON_DUNGEON_DELIVER_ITEM_FOR_EX_RES;

        public class Serializer : PacketEntitySerializer<S2CSeasonDungeonDeliverItemForExRes>
        {
            public override void Write(IBuffer buffer, S2CSeasonDungeonDeliverItemForExRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CSeasonDungeonDeliverItemForExRes Read(IBuffer buffer)
            {
                S2CSeasonDungeonDeliverItemForExRes obj = new S2CSeasonDungeonDeliverItemForExRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}

