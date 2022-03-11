using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataWarpPoint
    {
        public CDataWarpPoint(uint id, uint rimPrice)
        {
            Id = id;
            RimPrice = rimPrice;
        }

        public CDataWarpPoint()
        {
            Id = 0;
            RimPrice = 0;
        }

        public uint Id { get; set; }
        public uint RimPrice { get; set; }

        public class Serializer : EntitySerializer<CDataWarpPoint>
        {
            public override void Write(IBuffer buffer, CDataWarpPoint obj)
            {
                WriteUInt32(buffer, obj.Id);
                WriteUInt32(buffer, obj.RimPrice);
            }

            public override CDataWarpPoint Read(IBuffer buffer)
            {
                CDataWarpPoint obj = new CDataWarpPoint();
                obj.Id = ReadUInt32(buffer);
                obj.RimPrice = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
