using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataAddStatusParam
    {
        public byte IsAddStat1; // Adds the star, given how the rest of this behaves, this is probably not a bool afterall.
        public bool IsAddStat2; // Changing to True on a 2nd entry of the list gets both 5th and 6th star to show up
        public ushort AdditionalStatus1; // Effect ID.
        public ushort AdditionalStatus2; // setting this above 0 prevents you touching the button. Probably if you do the 6 star you can't change your 5th?

        public class Serializer : EntitySerializer<CDataAddStatusParam>
        {
            public override void Write(IBuffer buffer, CDataAddStatusParam obj)
            {
                WriteByte(buffer, obj.IsAddStat1);
                WriteBool(buffer, obj.IsAddStat2);
                WriteUInt16(buffer, obj.AdditionalStatus1);
                WriteUInt16(buffer, obj.AdditionalStatus2);
            }

            public override CDataAddStatusParam Read(IBuffer buffer)
            {
                CDataAddStatusParam obj = new CDataAddStatusParam();
                obj.IsAddStat1 = ReadByte(buffer);
                obj.IsAddStat2 = ReadBool(buffer);
                obj.AdditionalStatus1 = ReadUInt16(buffer);
                obj.AdditionalStatus2 = ReadUInt16(buffer);
                return obj;
            }
        }
    }

}
