using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataAreaWarpPoint
    {
        public uint AreaId { get; set; }
        public uint WarpPointId { get; set; }
        public uint Price { get; set; }

        public class Serializer : EntitySerializer<CDataAreaWarpPoint>
        {
            public override void Write(IBuffer buffer, CDataAreaWarpPoint obj)
            {
                WriteUInt32(buffer, obj.AreaId);
                WriteUInt32(buffer, obj.WarpPointId);
                WriteUInt32(buffer, obj.Price);
            }

            public override CDataAreaWarpPoint Read(IBuffer buffer)
            {
                CDataAreaWarpPoint obj = new CDataAreaWarpPoint();
                obj.AreaId = ReadUInt32(buffer);
                obj.WarpPointId = ReadUInt32(buffer);
                obj.Price = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
