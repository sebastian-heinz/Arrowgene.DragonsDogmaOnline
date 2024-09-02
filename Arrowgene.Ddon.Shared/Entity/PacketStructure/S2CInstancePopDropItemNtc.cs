using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CInstancePopDropItemNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_INSTANCE_POP_DROP_ITEM_NTC;

        public S2CInstancePopDropItemNtc()
        {
            LayoutId = new CDataStageLayoutId();
        }

        public CDataStageLayoutId LayoutId { get; set; }
        public uint SetId { get; set; }
        public byte MdlType { get; set; }
        public double PosX { get; set; }
        public float PosY { get; set; }
        public double PosZ { get; set; }

        public class Serializer : PacketEntitySerializer<S2CInstancePopDropItemNtc>
        {
            public override void Write(IBuffer buffer, S2CInstancePopDropItemNtc obj)
            {
                WriteEntity<CDataStageLayoutId>(buffer, obj.LayoutId);
                WriteUInt32(buffer, obj.SetId);
                WriteByte(buffer, obj.MdlType);
                WriteDouble(buffer, obj.PosX);
                WriteFloat(buffer, obj.PosY);
                WriteDouble(buffer, obj.PosZ);
            }

            public override S2CInstancePopDropItemNtc Read(IBuffer buffer)
            {
                S2CInstancePopDropItemNtc obj = new S2CInstancePopDropItemNtc();
                obj.LayoutId = ReadEntity<CDataStageLayoutId>(buffer);
                obj.SetId = ReadUInt32(buffer);
                obj.MdlType = ReadByte(buffer);
                obj.PosX = ReadDouble(buffer);
                obj.PosY = ReadFloat(buffer);
                obj.PosZ = ReadDouble(buffer);
                return obj;
            }
        }

    }
}