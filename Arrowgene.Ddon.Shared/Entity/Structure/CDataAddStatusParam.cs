using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataAddStatusParam
    {
        public bool IsAddStat1;
        public bool IsAddStat2;
        public ushort AdditionalStatus1;
        public ushort AdditionalStatus2;

        public class Serializer : EntitySerializer<CDataAddStatusParam>
        {
            public override void Write(IBuffer buffer, CDataAddStatusParam obj)
            {
                WriteBool(buffer, obj.IsAddStat1);
                WriteBool(buffer, obj.IsAddStat2);
                WriteUInt16(buffer, obj.AdditionalStatus1);
                WriteUInt16(buffer, obj.AdditionalStatus2);
            }

            public override CDataAddStatusParam Read(IBuffer buffer)
            {
                CDataAddStatusParam obj = new CDataAddStatusParam();
                obj.IsAddStat1 = ReadBool(buffer);
                obj.IsAddStat2 = ReadBool(buffer);
                obj.AdditionalStatus1 = ReadUInt16(buffer);
                obj.AdditionalStatus2 = ReadUInt16(buffer);
                return obj;
            }
        }
    }
}
