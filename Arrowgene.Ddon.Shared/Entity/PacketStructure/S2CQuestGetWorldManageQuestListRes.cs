using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestGetWorldManageQuestListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_QUEST_GET_WORLD_MANAGE_QUEST_LIST_RES;

        public S2CQuestGetWorldManageQuestListRes()
        {
            WorldManageQuestList = new List<CDataWorldManageQuestList>();
        }

        public List<CDataWorldManageQuestList> WorldManageQuestList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CQuestGetWorldManageQuestListRes>
        {
            public override void Write(IBuffer buffer, S2CQuestGetWorldManageQuestListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataWorldManageQuestList>(buffer, obj.WorldManageQuestList);
            }

            public override S2CQuestGetWorldManageQuestListRes Read(IBuffer buffer)
            {
                S2CQuestGetWorldManageQuestListRes obj = new S2CQuestGetWorldManageQuestListRes();
                ReadServerResponse(buffer, obj);
                obj.WorldManageQuestList = ReadEntityList<CDataWorldManageQuestList>(buffer);
                return obj;
            }
        }
    }
}