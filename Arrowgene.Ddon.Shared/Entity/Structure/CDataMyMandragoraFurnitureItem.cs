using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataMyMandragoraFurnitureItem
    {
        public uint MandragoraId { get; set; }
        public uint FurnitureItemId { get; set; }

        public class Serializer : EntitySerializer<CDataMyMandragoraFurnitureItem>
        {
            public override void Write(IBuffer buffer, CDataMyMandragoraFurnitureItem obj)
            {
                WriteUInt32(buffer, obj.MandragoraId);
                WriteUInt32(buffer, obj.FurnitureItemId);
            }

            public override CDataMyMandragoraFurnitureItem Read(IBuffer buffer)
            {
                CDataMyMandragoraFurnitureItem obj = new CDataMyMandragoraFurnitureItem();
                obj.MandragoraId = ReadUInt32(buffer);
                obj.FurnitureItemId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
