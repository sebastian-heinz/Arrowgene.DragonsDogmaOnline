using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSeasonDungeonGetIdFromNpcIdRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_SEASON_DUNGEON_GET_ID_FROM_NPC_ID_RES;

        public uint DungeonId { get; set; } // Set as 3 in packet capture?


        public class Serializer : PacketEntitySerializer<S2CSeasonDungeonGetIdFromNpcIdRes>
        {
            public override void Write(IBuffer buffer, S2CSeasonDungeonGetIdFromNpcIdRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.DungeonId);
            }

            public override S2CSeasonDungeonGetIdFromNpcIdRes Read(IBuffer buffer)
            {
                S2CSeasonDungeonGetIdFromNpcIdRes obj = new S2CSeasonDungeonGetIdFromNpcIdRes();
                ReadServerResponse(buffer, obj);
                obj.DungeonId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
