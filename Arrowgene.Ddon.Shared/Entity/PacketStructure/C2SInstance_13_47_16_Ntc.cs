using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SInstance_13_47_16_Ntc : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_INSTANCE_13_47_16_NTC;

        public C2SInstance_13_47_16_Ntc()
        {
        }

        public uint Unk0 { get; set; }
        public byte Unk1 { get; set; }
        public uint Unk2 { get; set; }
        public uint Unk3 { get; set; }
        public uint Unk4 { get; set; }
        public uint Unk5 { get; set; }


        public class Serializer : PacketEntitySerializer<C2SInstance_13_47_16_Ntc>
        {
            public override void Write(IBuffer buffer, C2SInstance_13_47_16_Ntc obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteByte(buffer, obj.Unk1);
                WriteUInt32(buffer, obj.Unk2);
                WriteUInt32(buffer, obj.Unk3);
                WriteUInt32(buffer, obj.Unk4);
                WriteUInt32(buffer, obj.Unk5);

            }

            public override C2SInstance_13_47_16_Ntc Read(IBuffer buffer)
            {
                C2SInstance_13_47_16_Ntc obj = new C2SInstance_13_47_16_Ntc();
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1 = ReadByte(buffer);
                obj.Unk2 = ReadUInt32(buffer);
                obj.Unk3 = ReadUInt32(buffer);
                obj.Unk4 = ReadUInt32(buffer);
                obj.Unk5 = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
