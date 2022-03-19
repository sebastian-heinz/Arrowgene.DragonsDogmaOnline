using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SInstanceGetGatheringItemReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_INSTANCE_GET_GATHERING_ITEM_REQ;

        public uint StageId { get; set; }
        public byte LayerNo { get; set; }
        public uint GroupId { get; set; }
        public uint PosId { get; set; }
        public byte Unk0 { get; set; }
        public uint LengthTest { get; set; }
        public uint Length { get; set; }
        public uint ItemNo { get; set; }
        public uint Quantity { get; set; }
        public byte Unk1 { get; set; }

        public C2SInstanceGetGatheringItemReq()
        {
            StageId = 0;
            LayerNo = 0;
            GroupId = 0;
            PosId = 0;
            Unk0 = 0;
            LengthTest = 0;
            Length = 0;
            ItemNo = 0;
            Quantity = 0;
            Unk1 = 0;
        }


        public class Serializer : PacketEntitySerializer<C2SInstanceGetGatheringItemReq>
        {
            public override void Write(IBuffer buffer, C2SInstanceGetGatheringItemReq obj)
            {
                WriteUInt32(buffer, obj.StageId);
                WriteByte(buffer, obj.LayerNo);
                WriteUInt32(buffer, obj.GroupId);
                WriteUInt32(buffer, obj.PosId);
                WriteByte(buffer, obj.Unk0);
                WriteUInt32(buffer, obj.LengthTest);
                WriteUInt32(buffer, obj.Length);
                WriteUInt32(buffer, obj.ItemNo);
                WriteUInt32(buffer, obj.Quantity);
                WriteByte(buffer, obj.Unk1);
            }

            public override C2SInstanceGetGatheringItemReq Read(IBuffer buffer)
            {
                C2SInstanceGetGatheringItemReq obj = new C2SInstanceGetGatheringItemReq();
                obj.StageId = ReadUInt32(buffer);
                obj.LayerNo = ReadByte(buffer);
                obj.GroupId = ReadUInt32(buffer);
                obj.PosId = ReadUInt32(buffer);
                obj.Unk0 = ReadByte(buffer);
                obj.LengthTest = ReadUInt32(buffer);
                obj.Length = ReadUInt32(buffer);
                obj.ItemNo = ReadUInt32(buffer);
                obj.Quantity = ReadUInt32(buffer);
                obj.Unk1 = ReadByte(buffer);
                return obj;
            }
        }
    }
}
