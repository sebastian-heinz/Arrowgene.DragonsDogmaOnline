using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CStampBonusCheckRes : ServerResponse
    {
        public S2CStampBonusCheckRes()
        {
            //From InGameDump.Dump_95
            Unk0 = 1;
            Unk1 = 0;
            Unk2 = 1;
            Unk3 = 77;
            Unk4 = 257;
        }

        public override PacketId Id => PacketId.S2C_STAMP_BONUS_CHECK_RES;

        //The structure of this is probably mostly wrong and totally differs from what's in the debug symbols.
        //Take these data types with a grain of salt.
        public uint Unk0 { get; set; }
        public ushort Unk1 { get; set; }
        public bool SuppressDaily { get; set; }
        public byte Unk2 { get; set; }
        public uint Unk3 { get; set; }
        public bool SuppressTotal { get; set; }
        public ushort Unk4 { get; set; }

        public class Serializer : PacketEntitySerializer<S2CStampBonusCheckRes>
        {
            public override void Write(IBuffer buffer, S2CStampBonusCheckRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.Unk0);
                WriteUInt16(buffer, obj.Unk1);
                WriteBool(buffer, obj.SuppressDaily);
                WriteByte(buffer, obj.Unk2);
                WriteUInt32(buffer, obj.Unk3);
                WriteBool(buffer, obj.SuppressTotal);
                WriteUInt16(buffer, obj.Unk4);
            }

            public override S2CStampBonusCheckRes Read(IBuffer buffer)
            {
                S2CStampBonusCheckRes obj = new S2CStampBonusCheckRes();
                ReadServerResponse(buffer, obj);
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1 = ReadUInt16(buffer);
                obj.SuppressDaily = ReadBool(buffer);
                obj.Unk2 = ReadByte(buffer);
                obj.Unk3 = ReadUInt16(buffer); 
                obj.SuppressTotal = ReadBool(buffer);
                obj.Unk4 = ReadUInt16(buffer);

                return obj;
            }
        }
    }
}
