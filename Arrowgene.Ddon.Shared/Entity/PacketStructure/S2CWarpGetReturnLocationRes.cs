using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CWarpGetReturnLocationRes
    {
        public S2CWarpGetReturnLocationRes()
        {
            jumpLocation = new CDataJumpLocation();
        }

        public CDataJumpLocation jumpLocation;
    }

    public class S2CWarpGetReturnLocationResSerializer : EntitySerializer<S2CWarpGetReturnLocationRes>
    {
        public override void Write(IBuffer buffer, S2CWarpGetReturnLocationRes obj)
        {
            WriteEntity<CDataJumpLocation>(buffer, obj.jumpLocation);
        }

        public override S2CWarpGetReturnLocationRes Read(IBuffer buffer)
        {
            S2CWarpGetReturnLocationRes obj = new S2CWarpGetReturnLocationRes();
            obj.jumpLocation = ReadEntity<CDataJumpLocation>(buffer);
            return obj;
        }
    }
}