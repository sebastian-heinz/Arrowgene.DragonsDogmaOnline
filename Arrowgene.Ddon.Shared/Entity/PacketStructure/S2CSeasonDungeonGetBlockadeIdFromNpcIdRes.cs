using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSeasonDungeonGetBlockadeIdFromNpcIdRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_SEASON_DUNGEON_GET_BLOCKADE_ID_FROM_NPC_ID_RES;

        public uint EpitaphId { get; set; }
        public bool Unk0 { get; set; }

        public class Serializer : PacketEntitySerializer<S2CSeasonDungeonGetBlockadeIdFromNpcIdRes>
        {
            public override void Write(IBuffer buffer, S2CSeasonDungeonGetBlockadeIdFromNpcIdRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.EpitaphId);
                WriteBool(buffer, obj.Unk0);
            }

            public override S2CSeasonDungeonGetBlockadeIdFromNpcIdRes Read(IBuffer buffer)
            {
                S2CSeasonDungeonGetBlockadeIdFromNpcIdRes obj = new S2CSeasonDungeonGetBlockadeIdFromNpcIdRes();
                ReadServerResponse(buffer, obj);
                obj.EpitaphId = ReadUInt32(buffer);
                obj.Unk0 = ReadBool(buffer);
                return obj;
            }
        }
    }
}
