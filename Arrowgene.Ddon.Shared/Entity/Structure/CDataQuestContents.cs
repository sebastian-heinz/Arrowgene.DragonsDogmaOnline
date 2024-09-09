using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataQuestContents
    {
        /// <summary>
        /// 1 = Hunt
        /// 2 = Delivery
        /// </summary>
        public byte Type { get; set; }
        public int Param01 { get; set; }
        public int Param02 { get; set; }
        public int Param03 { get; set; }
        public int Param04 { get; set; }
        public ushort Unk0 { get; set; }
        public ushort Unk1 { get; set; }
    
        public class Serializer : EntitySerializer<CDataQuestContents>
        {
            public override void Write(IBuffer buffer, CDataQuestContents obj)
            {
                WriteByte(buffer, obj.Type);
                WriteInt32(buffer, obj.Param01);
                WriteInt32(buffer, obj.Param02);
                WriteInt32(buffer, obj.Param03);
                WriteInt32(buffer, obj.Param04);
                WriteUInt16(buffer, obj.Unk0);
                WriteUInt16(buffer, obj.Unk1);
            }
        
            public override CDataQuestContents Read(IBuffer buffer)
            {
                CDataQuestContents obj = new CDataQuestContents();
                obj.Type = ReadByte(buffer);
                obj.Param01 = ReadInt32(buffer);
                obj.Param02 = ReadInt32(buffer);
                obj.Param03 = ReadInt32(buffer);
                obj.Param04 = ReadInt32(buffer);
                obj.Unk0 = ReadUInt16(buffer);
                obj.Unk1 = ReadUInt16(buffer);
                return obj;
            }
        }
    }
}
