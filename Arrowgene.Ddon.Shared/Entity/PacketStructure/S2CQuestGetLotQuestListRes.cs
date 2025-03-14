using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestGetLotQuestListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_QUEST_GET_LOT_QUEST_LIST_RES;

        public S2CQuestGetLotQuestListRes()
        {
        }

        public uint LotQuestType { get; set; }
        public List<CDataLotQuestList> LotQuestList { get; set; } = new();

        public class Serializer : PacketEntitySerializer<S2CQuestGetLotQuestListRes>
        {
            public override void Write(IBuffer buffer, S2CQuestGetLotQuestListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.LotQuestType);
                WriteEntityList(buffer, obj.LotQuestList);
            }

            public override S2CQuestGetLotQuestListRes Read(IBuffer buffer)
            {
                S2CQuestGetLotQuestListRes obj = new S2CQuestGetLotQuestListRes();
                ReadServerResponse(buffer, obj);
                ReadUInt32(buffer);
                ReadEntityList<CDataLotQuestList>(buffer);
                return obj;
            }
        }
    }
}
