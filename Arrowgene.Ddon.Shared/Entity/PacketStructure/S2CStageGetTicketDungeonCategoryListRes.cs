using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CStageGetTicketDungeonCategoryListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_STAGE_GET_TICKET_DUNGEON_CATEGORY_LIST_RES;

        public S2CStageGetTicketDungeonCategoryListRes()
        {
            CategoryList = new List<CDataStageTicketDungeonCategory>();
        }

        public List<CDataStageTicketDungeonCategory> CategoryList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CStageGetTicketDungeonCategoryListRes>
        {

            public override void Write(IBuffer buffer, S2CStageGetTicketDungeonCategoryListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList(buffer, obj.CategoryList);
            }

            public override S2CStageGetTicketDungeonCategoryListRes Read(IBuffer buffer)
            {
                S2CStageGetTicketDungeonCategoryListRes obj = new S2CStageGetTicketDungeonCategoryListRes();
                ReadServerResponse(buffer, obj);
                obj.CategoryList = ReadEntityList<CDataStageTicketDungeonCategory>(buffer);
                return obj;
            }
        }
    }
}
