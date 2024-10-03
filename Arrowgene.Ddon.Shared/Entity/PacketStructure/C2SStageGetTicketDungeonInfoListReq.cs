using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SStageGetTicketDungeonInfoListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_STAGE_GET_TICKET_DUNGEON_INFO_LIST_REQ;

        public uint CategoryId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SStageGetTicketDungeonInfoListReq>
        {
            public override void Write(IBuffer buffer, C2SStageGetTicketDungeonInfoListReq obj)
            {
                WriteUInt32(buffer, obj.CategoryId);
            }

            public override C2SStageGetTicketDungeonInfoListReq Read(IBuffer buffer)
            {
                C2SStageGetTicketDungeonInfoListReq obj = new C2SStageGetTicketDungeonInfoListReq();
                obj.CategoryId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
