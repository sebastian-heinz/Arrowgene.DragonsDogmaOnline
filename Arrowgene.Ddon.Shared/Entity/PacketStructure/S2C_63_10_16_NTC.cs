using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2C_63_10_16_NTC : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_63_10_16_NTC;

        public S2C_63_10_16_NTC()
        {
            Unk1String = string.Empty;
        }

        public uint Unk0 {  get; set; }
        public string Unk1String {  get; set; }
        public byte Unk2 {  get; set; }

        public class Serializer : PacketEntitySerializer<S2C_63_10_16_NTC>
        {
            public override void Write(IBuffer buffer, S2C_63_10_16_NTC obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteMtString(buffer, obj.Unk1String);
                WriteByte(buffer, obj.Unk2);
            }

            public override S2C_63_10_16_NTC Read(IBuffer buffer)
            {
                S2C_63_10_16_NTC obj = new S2C_63_10_16_NTC();
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1String = ReadMtString(buffer);
                obj.Unk2 = ReadByte(buffer);
                return obj;
            }
        }
    }
}
