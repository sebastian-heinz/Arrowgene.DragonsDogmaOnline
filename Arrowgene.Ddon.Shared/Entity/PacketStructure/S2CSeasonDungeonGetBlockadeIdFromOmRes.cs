using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSeasonDungeonGetBlockadeIdFromOmRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_SEASON_DUNGEON_GET_BLOCKADE_ID_FROM_OM_RES;

        public uint EpitaphId { get; set; }

        public class Serializer : PacketEntitySerializer<S2CSeasonDungeonGetBlockadeIdFromOmRes>
        {
            public override void Write(IBuffer buffer, S2CSeasonDungeonGetBlockadeIdFromOmRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.EpitaphId);
            }

            public override S2CSeasonDungeonGetBlockadeIdFromOmRes Read(IBuffer buffer)
            {
                S2CSeasonDungeonGetBlockadeIdFromOmRes obj = new S2CSeasonDungeonGetBlockadeIdFromOmRes();
                ReadServerResponse(buffer, obj);
                obj.EpitaphId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
