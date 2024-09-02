using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestDeliverItemNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_QUEST_DELIVER_ITEM_NTC;

        public S2CQuestDeliverItemNtc()
        {
            DeliveredItemRecord = new CDataDeliveredItemRecord();
        }

        public CDataDeliveredItemRecord DeliveredItemRecord {  get; set; }

        public class Serializer : PacketEntitySerializer<S2CQuestDeliverItemNtc>
        {
            public override void Write(IBuffer buffer, S2CQuestDeliverItemNtc obj)
            {
                WriteEntity<CDataDeliveredItemRecord>(buffer, obj.DeliveredItemRecord);
            }

            public override S2CQuestDeliverItemNtc Read(IBuffer buffer)
            {
                S2CQuestDeliverItemNtc obj = new S2CQuestDeliverItemNtc();
                obj.DeliveredItemRecord = ReadEntity<CDataDeliveredItemRecord>(buffer);
                return obj;
            }
        }
    }
}
