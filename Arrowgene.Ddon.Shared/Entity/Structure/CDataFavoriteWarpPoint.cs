using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure;

public class CDataFavoriteWarpPoint
{
    public CDataFavoriteWarpPoint()
    {
        SlotNo = 0;
        WarpPointId = 0;
        Price = 0;
    }

    public uint SlotNo { get; set; }
    public uint WarpPointId { get; set; }
    public uint Price { get; set; }

    public class Serializer : EntitySerializer<CDataFavoriteWarpPoint>
    {
        public override void Write(IBuffer buffer, CDataFavoriteWarpPoint obj)
        {
            WriteUInt32(buffer, obj.SlotNo);
            WriteUInt32(buffer, obj.WarpPointId);
            WriteUInt32(buffer, obj.Price);
        }

        public override CDataFavoriteWarpPoint Read(IBuffer buffer)
        {
            CDataFavoriteWarpPoint obj = new CDataFavoriteWarpPoint();
            obj.SlotNo = ReadUInt32(buffer);
            obj.WarpPointId = ReadUInt32(buffer);
            obj.Price = ReadUInt32(buffer);
            return obj;
        }
    }
}
