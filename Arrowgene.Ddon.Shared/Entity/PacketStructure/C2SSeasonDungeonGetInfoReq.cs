using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SSeasonDungeonGetInfoReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_SEASON_DUNGEON_GET_INFO_REQ;

        public uint DungeonId { get; set; } // Value returned from S2C_SEASON_DUNGEON_GET_ID_FROM_NPC_ID_RES

        public class Serializer : PacketEntitySerializer<C2SSeasonDungeonGetInfoReq>
        {
            public override void Write(IBuffer buffer, C2SSeasonDungeonGetInfoReq obj)
            {
                WriteUInt32(buffer, obj.DungeonId);
            }

            public override C2SSeasonDungeonGetInfoReq Read(IBuffer buffer)
            {
                C2SSeasonDungeonGetInfoReq obj = new C2SSeasonDungeonGetInfoReq();
                obj.DungeonId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
