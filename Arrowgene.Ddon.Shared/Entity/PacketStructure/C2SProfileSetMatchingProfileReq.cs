using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SProfileSetMatchingProfileReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PROFILE_SET_MATCHING_PROFILE_REQ;

        public CDataMatchingProfile MatchingProfile { get; set; } = new();

        public class Serializer : PacketEntitySerializer<C2SProfileSetMatchingProfileReq>
        {
            public override void Write(IBuffer buffer, C2SProfileSetMatchingProfileReq obj)
            {
                WriteEntity(buffer, obj.MatchingProfile);
            }

            public override C2SProfileSetMatchingProfileReq Read(IBuffer buffer)
            {
                C2SProfileSetMatchingProfileReq obj = new C2SProfileSetMatchingProfileReq();
                obj.MatchingProfile = ReadEntity<CDataMatchingProfile>(buffer);
                return obj;
            }
        }
    }
}
