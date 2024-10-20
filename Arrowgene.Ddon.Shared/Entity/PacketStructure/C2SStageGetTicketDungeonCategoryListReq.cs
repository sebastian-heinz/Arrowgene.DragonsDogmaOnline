using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SStageGetTicketDungeonCategoryListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_STAGE_GET_TICKET_DUNGEON_CATEGORY_LIST_REQ;

        public class Serializer : PacketEntitySerializer<C2SStageGetTicketDungeonCategoryListReq>
        {

            public override void Write(IBuffer buffer, C2SStageGetTicketDungeonCategoryListReq obj)
            {
            }

            public override C2SStageGetTicketDungeonCategoryListReq Read(IBuffer buffer)
            {
                return new C2SStageGetTicketDungeonCategoryListReq();
            }
        }
    }
}
