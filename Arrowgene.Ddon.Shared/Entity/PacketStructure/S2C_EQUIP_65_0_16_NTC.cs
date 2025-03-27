using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2C_EQUIP_65_0_16_NTC : IPacketStructure
    {
        public S2C_EQUIP_65_0_16_NTC()
        {
            AddStatusParamList = new List<CDataAddStatusParam>();
        }

        public PacketId Id => PacketId.S2C_EQUIP_65_0_16_NTC;

        public uint Unk0 { get; set; }
        public uint Unk1 { get; set; }
        public byte Unk2 { get; set; }
        public byte Unk3 { get; set; }
        public uint Unk4 { get; set; }
        public List<CDataAddStatusParam> AddStatusParamList { get; set; }

        public class Serializer : PacketEntitySerializer<S2C_EQUIP_65_0_16_NTC>
        {
            public override void Write(IBuffer buffer, S2C_EQUIP_65_0_16_NTC obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteUInt32(buffer, obj.Unk1);
                WriteByte(buffer, obj.Unk2);
                WriteByte(buffer, obj.Unk3);
                WriteUInt32(buffer, obj.Unk4);
                WriteEntityList(buffer, obj.AddStatusParamList);
            }

            public override S2C_EQUIP_65_0_16_NTC Read(IBuffer buffer)
            {
                S2C_EQUIP_65_0_16_NTC obj = new S2C_EQUIP_65_0_16_NTC();
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1 = ReadUInt32(buffer);
                obj.Unk2 = ReadByte(buffer);
                obj.Unk3 = ReadByte(buffer);
                obj.Unk4 = ReadUInt32(buffer);
                obj.AddStatusParamList = ReadEntityList<CDataAddStatusParam>(buffer);
                return obj;
            }
        }
    }
}
