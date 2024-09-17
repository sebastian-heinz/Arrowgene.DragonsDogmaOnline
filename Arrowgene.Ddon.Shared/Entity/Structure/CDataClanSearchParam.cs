using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataClanSearchParam
    {
        public CDataClanSearchParam()
        {
            Name = "";
        }

        public byte SearchType { get; set; }
        public string Name { get; set; }
        public ushort Level { get; set; }
        public ushort MemberNum { get; set; }
        public uint Motto { get; set; }
        public uint ActiveDays { get; set; }
        public uint ActiveTime { get; set; }
        public uint Characteristic { get; set; }

        public class Serializer : EntitySerializer<CDataClanSearchParam>
        {
            public override void Write(IBuffer buffer, CDataClanSearchParam obj)
            {
                WriteByte(buffer, obj.SearchType);
                WriteMtString(buffer, obj.Name);
                WriteUInt16(buffer, obj.Level);
                WriteUInt16(buffer, obj.MemberNum);
                WriteUInt32(buffer, obj.Motto);
                WriteUInt32(buffer, obj.ActiveDays);
                WriteUInt32(buffer, obj.ActiveTime);
                WriteUInt32(buffer, obj.Characteristic);
            }

            public override CDataClanSearchParam Read(IBuffer buffer)
            {
                CDataClanSearchParam obj = new CDataClanSearchParam();
                obj.SearchType = ReadByte(buffer);
                obj.Name = ReadMtString(buffer);
                obj.Level = ReadUInt16(buffer);
                obj.MemberNum = ReadUInt16(buffer);
                obj.Motto = ReadUInt32(buffer);
                obj.ActiveDays = ReadUInt32(buffer);
                obj.ActiveTime = ReadUInt32(buffer);
                obj.Characteristic = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
