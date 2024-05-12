using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataCraftSupportPawnID
    {
        public CDataCraftSupportPawnID(uint value)
        {
            PawnId = value;
        }

        public CDataCraftSupportPawnID()
        {
            PawnId=0;
        }

        public uint PawnId { get; set; }

        public class Serializer : EntitySerializer<CDataCraftSupportPawnID>
        {
            public override void Write(IBuffer buffer, CDataCraftSupportPawnID obj)
            {
                WriteUInt32(buffer, obj.PawnId);
            }

            public override CDataCraftSupportPawnID Read(IBuffer buffer)
            {
                CDataCraftSupportPawnID obj = new CDataCraftSupportPawnID();
                obj.PawnId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
