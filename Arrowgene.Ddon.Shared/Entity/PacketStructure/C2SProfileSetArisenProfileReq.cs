using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SProfileSetArisenProfileReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PROFILE_SET_ARISEN_PROFILE_REQ;

        public CDataArisenProfile ArisenProfile { get; set; } = new();

        public class Serializer : PacketEntitySerializer<C2SProfileSetArisenProfileReq>
        {
            public override void Write(IBuffer buffer, C2SProfileSetArisenProfileReq obj)
            {
                WriteEntity(buffer, obj.ArisenProfile);
            }

            public override C2SProfileSetArisenProfileReq Read(IBuffer buffer)
            {
                C2SProfileSetArisenProfileReq obj = new C2SProfileSetArisenProfileReq();
                obj.ArisenProfile = ReadEntity<CDataArisenProfile>(buffer);
                return obj;
            }
        }
    }
}
