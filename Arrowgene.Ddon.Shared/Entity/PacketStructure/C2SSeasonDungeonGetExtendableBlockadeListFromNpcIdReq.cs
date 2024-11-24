using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SSeasonDungeonGetExtendableBlockadeListFromNpcIdReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_SEASON_DUNGEON_GET_EXTENDABLE_BLOCKADE_LIST_FROM_NPC_ID_REQ;

        public NpcId NpcId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SSeasonDungeonGetExtendableBlockadeListFromNpcIdReq>
        {
            public override void Write(IBuffer buffer, C2SSeasonDungeonGetExtendableBlockadeListFromNpcIdReq obj)
            {
                WriteUInt32(buffer, (uint)obj.NpcId);
            }

            public override C2SSeasonDungeonGetExtendableBlockadeListFromNpcIdReq Read(IBuffer buffer)
            {
                C2SSeasonDungeonGetExtendableBlockadeListFromNpcIdReq obj = new C2SSeasonDungeonGetExtendableBlockadeListFromNpcIdReq();
                obj.NpcId = (NpcId)ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
