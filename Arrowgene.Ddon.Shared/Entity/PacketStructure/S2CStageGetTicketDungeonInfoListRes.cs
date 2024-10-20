using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CStageGetTicketDungeonInfoListRes : ServerResponse
    {
        public S2CStageGetTicketDungeonInfoListRes()
        {
            CategoryInfoList = new List<CDataStageTicketDungeonCategoryInfo>();
        }

        public override PacketId Id => PacketId.S2C_STAGE_GET_TICKET_DUNGEON_INFO_LIST_RES;

        public List<CDataStageTicketDungeonCategoryInfo> CategoryInfoList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CStageGetTicketDungeonInfoListRes>
        {
            public override void Write(IBuffer buffer, S2CStageGetTicketDungeonInfoListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList(buffer, obj.CategoryInfoList);
            }

            public override S2CStageGetTicketDungeonInfoListRes Read(IBuffer buffer)
            {
                S2CStageGetTicketDungeonInfoListRes obj = new S2CStageGetTicketDungeonInfoListRes();
                ReadServerResponse(buffer, obj);
                obj.CategoryInfoList = ReadEntityList<CDataStageTicketDungeonCategoryInfo>(buffer);
                return obj;
            }
        }
    }
}
