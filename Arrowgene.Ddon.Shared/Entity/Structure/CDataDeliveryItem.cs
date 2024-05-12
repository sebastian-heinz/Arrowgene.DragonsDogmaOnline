using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure;

public class CDataDeliveryItem
{
    public uint ItemId { get; set; }
    // One of these is item num, another probably need num
    public ushort Unk0 { get; set; }
    public ushort Unk1 { get; set; }
    public ushort Unk2 { get; set; }
    
    public class Serializer : EntitySerializer<CDataDeliveryItem>
    {
        public override void Write(IBuffer buffer, CDataDeliveryItem obj)
        {
            WriteUInt32(buffer, obj.ItemId);
            WriteUInt16(buffer, obj.Unk0);
            WriteUInt16(buffer, obj.Unk1);
            WriteUInt16(buffer, obj.Unk2);
        }

        public override CDataDeliveryItem Read(IBuffer buffer)
        {
            CDataDeliveryItem obj = new CDataDeliveryItem();
            obj.ItemId = ReadUInt32(buffer);
            obj.Unk0 = ReadUInt16(buffer);
            obj.Unk1 = ReadUInt16(buffer);
            obj.Unk2 = ReadUInt16(buffer);
            return obj;
        }
    }
}
