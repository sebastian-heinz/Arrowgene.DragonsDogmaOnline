using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataAddStatusData
    {
        public bool IsAddStat1;
        public bool IsAddStat2;
        public ushort AdditionalStatus1;
        public ushort AdditionalStatus2;

        public class Serializer : EntitySerializer<CDataAddStatusData>
        {
            public override void Write(IBuffer buffer, CDataAddStatusData obj)
            {
                WriteBool(buffer, obj.IsAddStat1);
                WriteBool(buffer, obj.IsAddStat2);
                WriteUInt16(buffer, obj.AdditionalStatus1);
                WriteUInt16(buffer, obj.AdditionalStatus2);
            }

            public override CDataAddStatusData Read(IBuffer buffer)
            {
                CDataAddStatusData obj = new CDataAddStatusData();
                obj.IsAddStat1 = ReadBool(buffer);
                obj.IsAddStat2 = ReadBool(buffer);
                obj.AdditionalStatus1 = ReadUInt16(buffer);
                obj.AdditionalStatus2 = ReadUInt16(buffer);
                return obj;
            }
        }
    }

}
