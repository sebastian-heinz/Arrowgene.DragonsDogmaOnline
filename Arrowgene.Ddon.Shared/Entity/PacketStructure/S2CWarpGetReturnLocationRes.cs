using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CWarpGetReturnLocationRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_WARP_GET_RETURN_LOCATION_RES;

        public CDataJumpLocation JumpLocation { get; set; }

        public S2CWarpGetReturnLocationRes()
        {
            JumpLocation = new CDataJumpLocation();
        }

        public class Serializer : PacketEntitySerializer<S2CWarpGetReturnLocationRes>
        {            

            public override void Write(IBuffer buffer, S2CWarpGetReturnLocationRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntity<CDataJumpLocation>(buffer, obj.JumpLocation);
            }

            public override S2CWarpGetReturnLocationRes Read(IBuffer buffer)
            {
                S2CWarpGetReturnLocationRes obj = new S2CWarpGetReturnLocationRes();
                ReadServerResponse(buffer, obj);
                obj.JumpLocation = ReadEntity<CDataJumpLocation>(buffer);
                return obj;
            }
        }
    }

}
