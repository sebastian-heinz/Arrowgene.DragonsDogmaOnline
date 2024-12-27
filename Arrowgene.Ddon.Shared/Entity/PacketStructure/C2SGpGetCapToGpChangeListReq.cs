using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SGpGetCapToGpChangeListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_GP_GET_CAP_TO_GP_CHANGE_LIST_REQ;

        public C2SGpGetCapToGpChangeListReq()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SGpGetCapToGpChangeListReq>
        {
            public override void Write(IBuffer buffer, C2SGpGetCapToGpChangeListReq obj)
            {
            }

            public override C2SGpGetCapToGpChangeListReq Read(IBuffer buffer)
            {
                C2SGpGetCapToGpChangeListReq obj = new C2SGpGetCapToGpChangeListReq();

                return obj;
            }
        }
    }
}
