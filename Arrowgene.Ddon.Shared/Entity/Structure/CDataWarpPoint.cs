using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataWarpPoint {
        public CDataWarpPoint() {
            id = 0;
            rimPrice = 0;
        }
        
        public uint id;
        public uint rimPrice;

    }

    public class CDataWarpPointSerializer : EntitySerializer<CDataWarpPoint> {
        public override void Write(IBuffer buffer, CDataWarpPoint obj)
        {
            WriteUInt32(buffer, obj.id);
            WriteUInt32(buffer, obj.rimPrice);
        }

        public override CDataWarpPoint Read(IBuffer buffer)
        {
            CDataWarpPoint obj = new CDataWarpPoint();
            obj.id = ReadUInt32(buffer);
            obj.rimPrice = ReadUInt32(buffer);
            return obj;
        }
    }
}