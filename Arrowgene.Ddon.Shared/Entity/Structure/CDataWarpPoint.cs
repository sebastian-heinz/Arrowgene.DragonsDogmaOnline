using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataWarpPoint
    {
        public CDataWarpPoint(uint id, uint rimPrice)
        {
            ID=id;
            RimPrice=rimPrice;
        }

        public CDataWarpPoint()
        {
            ID=0;
            RimPrice=0;
        }

        public uint ID { get; set; }
        public uint RimPrice { get; set; }

        public class Serializer : EntitySerializer<CDataWarpPoint>
        {
            public override void Write(IBuffer buffer, CDataWarpPoint obj)
            {
                WriteUInt32(buffer, obj.ID);
                WriteUInt32(buffer, obj.RimPrice);
            }

            public override CDataWarpPoint Read(IBuffer buffer)
            {
                CDataWarpPoint obj = new CDataWarpPoint();
                obj.ID = ReadUInt32(buffer);
                obj.RimPrice = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}