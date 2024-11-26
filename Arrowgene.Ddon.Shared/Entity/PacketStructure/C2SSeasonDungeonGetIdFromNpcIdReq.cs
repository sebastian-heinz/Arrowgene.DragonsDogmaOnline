using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SSeasonDungeonGetIdFromNpcIdReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_SEASON_DUNGEON_GET_ID_FROM_NPC_ID_REQ;

        public NpcId NpcId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SSeasonDungeonGetIdFromNpcIdReq>
        {
            public override void Write(IBuffer buffer, C2SSeasonDungeonGetIdFromNpcIdReq obj)
            {
                WriteUInt32(buffer, (uint) obj.NpcId);
            }

            public override C2SSeasonDungeonGetIdFromNpcIdReq Read(IBuffer buffer)
            {
                C2SSeasonDungeonGetIdFromNpcIdReq obj = new C2SSeasonDungeonGetIdFromNpcIdReq();
                obj.NpcId = (NpcId) ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
