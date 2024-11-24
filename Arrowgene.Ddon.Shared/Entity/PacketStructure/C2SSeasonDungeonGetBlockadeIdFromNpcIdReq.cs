using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SSeasonDungeonGetBlockadeIdFromNpcIdReq : IPacketStructure
    {
        public C2SSeasonDungeonGetBlockadeIdFromNpcIdReq()
        {
        }

        public PacketId Id => PacketId.C2S_SEASON_DUNGEON_GET_BLOCKADE_ID_FROM_NPC_ID_REQ;

        public NpcId NpcId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SSeasonDungeonGetBlockadeIdFromNpcIdReq>
        {
            public override void Write(IBuffer buffer, C2SSeasonDungeonGetBlockadeIdFromNpcIdReq obj)
            {
                WriteUInt32(buffer, (uint) obj.NpcId);
            }

            public override C2SSeasonDungeonGetBlockadeIdFromNpcIdReq Read(IBuffer buffer)
            {
                C2SSeasonDungeonGetBlockadeIdFromNpcIdReq obj = new C2SSeasonDungeonGetBlockadeIdFromNpcIdReq();
                obj.NpcId = (NpcId) ReadUInt32(buffer);
                return obj;
            }
        }
    }
}

