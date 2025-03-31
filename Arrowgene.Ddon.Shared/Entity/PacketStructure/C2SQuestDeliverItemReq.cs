using System;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SQuestDeliverItemReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_QUEST_DELIVER_ITEM_REQ;

        public C2SQuestDeliverItemReq()
        {
            ItemUIDList = new List<CDataItemUIDList>();
        }

        public UInt32 QuestScheduleId { get; set; }
        public UInt16 ProcessNo { get; set; }
        public List<CDataItemUIDList> ItemUIDList { get; set; }

        public class Serializer : PacketEntitySerializer<C2SQuestDeliverItemReq>
        {
            public override void Write(IBuffer buffer, C2SQuestDeliverItemReq obj)
            {
                WriteUInt32(buffer, obj.QuestScheduleId);
                WriteUInt16(buffer, obj.ProcessNo);
                WriteEntityList<CDataItemUIDList>(buffer, obj.ItemUIDList);
            }

            public override C2SQuestDeliverItemReq Read(IBuffer buffer)
            {
                C2SQuestDeliverItemReq obj = new C2SQuestDeliverItemReq();
                obj.QuestScheduleId = ReadUInt32(buffer);
                obj.ProcessNo = ReadUInt16(buffer);
                obj.ItemUIDList = ReadEntityList<CDataItemUIDList>(buffer);
                return obj;
            }
        }
    }
}
