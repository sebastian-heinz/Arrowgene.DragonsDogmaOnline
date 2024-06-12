using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SQuestGetQuestCompleteListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_QUEST_GET_QUEST_COMPLETE_LIST_REQ;

        public C2SQuestGetQuestCompleteListReq()
        {
        }

        public byte QuestType { get; set; }

        public class Serializer : PacketEntitySerializer<C2SQuestGetQuestCompleteListReq>
        {
            public override void Write(IBuffer buffer, C2SQuestGetQuestCompleteListReq obj)
            {
                WriteByte(buffer, obj.QuestType);
            }

            public override C2SQuestGetQuestCompleteListReq Read(IBuffer buffer)
            {
                C2SQuestGetQuestCompleteListReq obj = new C2SQuestGetQuestCompleteListReq();
                obj.QuestType = ReadByte(buffer);
                return obj;
            }
        }
    }
}

