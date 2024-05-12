using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SProfileGetMyCharacterProfileReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PROFILE_GET_MY_CHARACTER_PROFILE_REQ;

        public class Serializer : PacketEntitySerializer<C2SProfileGetMyCharacterProfileReq>
        {
            public override void Write(IBuffer buffer, C2SProfileGetMyCharacterProfileReq obj)
            {
            }

            public override C2SProfileGetMyCharacterProfileReq Read(IBuffer buffer)
            {
                return new C2SProfileGetMyCharacterProfileReq();
            }
        }

    }
}