using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SQuestGetRewardBoxItemReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_QUEST_GET_REWARD_BOX_ITEM_REQ;

        public C2SQuestGetRewardBoxItemReq()
        {
            GetRewardBoxItemList = new List<CDataGetRewardBoxItem>();
        }

        public uint ListNo { get; set; }
        public List<CDataGetRewardBoxItem> GetRewardBoxItemList { get; set; }

        public class Serializer : PacketEntitySerializer<C2SQuestGetRewardBoxItemReq>
        {
            public override void Write(IBuffer buffer, C2SQuestGetRewardBoxItemReq obj)
            {
                WriteUInt32(buffer, obj.ListNo);
                WriteEntityList(buffer, obj.GetRewardBoxItemList);
            }

            public override C2SQuestGetRewardBoxItemReq Read(IBuffer buffer)
            {
                C2SQuestGetRewardBoxItemReq obj = new C2SQuestGetRewardBoxItemReq();
                obj.ListNo = ReadUInt32(buffer);
                obj.GetRewardBoxItemList = ReadEntityList<CDataGetRewardBoxItem>(buffer);
                return obj;
            }
        }
    }
}
