using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataMyMandragoraFertilizerItem
    {
        public uint ItemId { get; set; }
        public uint ItemNum { get; set; }

        public class Serializer : EntitySerializer<CDataMyMandragoraFertilizerItem>
        {
            public override void Write(IBuffer buffer, CDataMyMandragoraFertilizerItem obj)
            {
                WriteUInt32(buffer, obj.ItemId);
                WriteUInt32(buffer, obj.ItemNum);
            }

            public override CDataMyMandragoraFertilizerItem Read(IBuffer buffer)
            {
                CDataMyMandragoraFertilizerItem obj = new CDataMyMandragoraFertilizerItem();
                obj.ItemId = ReadUInt32(buffer);
                obj.ItemNum = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
