using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure;

public class CDataDeliveredItemRecord
{
    public CDataDeliveredItemRecord()
    {
        DeliveredItemList = new List<CDataDeliveredItem>();
    }

    public uint CharacterId { get; set; }
    public uint QuestScheduleId { get; set; }
    public ushort ProcessNo { get; set; }
    public List<CDataDeliveredItem> DeliveredItemList { get; set; } 
    
    public class Serializer : EntitySerializer<CDataDeliveredItemRecord>
    {
        public override void Write(IBuffer buffer, CDataDeliveredItemRecord obj)
        {
            WriteUInt32(buffer, obj.CharacterId);
            WriteUInt32(buffer, obj.QuestScheduleId);
            WriteUInt16(buffer, obj.ProcessNo);
            WriteEntityList<CDataDeliveredItem>(buffer, obj.DeliveredItemList);
        }

        public override CDataDeliveredItemRecord Read(IBuffer buffer)
        {
            CDataDeliveredItemRecord obj = new CDataDeliveredItemRecord();
            obj.CharacterId = ReadUInt32(buffer);
            obj.QuestScheduleId = ReadUInt32(buffer);
            obj.ProcessNo = ReadUInt16(buffer);
            obj.DeliveredItemList = ReadEntityList<CDataDeliveredItem>(buffer);
            return obj;
        }
    }
}
