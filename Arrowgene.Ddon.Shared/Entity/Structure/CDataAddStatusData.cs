using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataAddStatusData
    {
        public byte IsAddStat1;
        public byte IsAddStat2;
        public ushort AdditionalStatus1;
        public ushort AdditionalStatus2;

        public class Serializer : EntitySerializer<CDataAddStatusData>
        {
            public override void Write(IBuffer buffer, CDataAddStatusData obj)
            {
                WriteByte(buffer, obj.IsAddStat1);
                WriteByte(buffer, obj.IsAddStat2);
                WriteUInt16(buffer, obj.AdditionalStatus1);
                WriteUInt16(buffer, obj.AdditionalStatus2);
            }

            public override CDataAddStatusData Read(IBuffer buffer)
            {
                CDataAddStatusData obj = new CDataAddStatusData();
                obj.IsAddStat1 = ReadByte(buffer);
                obj.IsAddStat2 = ReadByte(buffer);
                obj.AdditionalStatus1 = ReadUInt16(buffer);
                obj.AdditionalStatus2 = ReadUInt16(buffer);
                return obj;
            }
        }
    }

}
