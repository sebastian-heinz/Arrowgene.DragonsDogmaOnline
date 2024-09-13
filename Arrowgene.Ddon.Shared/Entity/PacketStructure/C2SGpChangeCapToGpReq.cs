using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SGpChangeCapToGpReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_GP_CHANGE_CAP_TO_GP_REQ;

        public uint ChangeListID { get; set; }

        public C2SGpChangeCapToGpReq()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SGpChangeCapToGpReq>
        {
            public override void Write(IBuffer buffer, C2SGpChangeCapToGpReq obj)
            {
                WriteUInt32(buffer, obj.ChangeListID);
            }

            public override C2SGpChangeCapToGpReq Read(IBuffer buffer)
            {
                C2SGpChangeCapToGpReq obj = new C2SGpChangeCapToGpReq();

                obj.ChangeListID = ReadUInt32(buffer);

                return obj;
            }
        }
    }
}
