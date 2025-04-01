using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SProfileGetCharacterProfileReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PROFILE_GET_CHARACTER_PROFILE_REQ;

        public uint CharacterId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SProfileGetCharacterProfileReq>
        {
            public override void Write(IBuffer buffer, C2SProfileGetCharacterProfileReq obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
            }

            public override C2SProfileGetCharacterProfileReq Read(IBuffer buffer)
            {
                C2SProfileGetCharacterProfileReq obj = new C2SProfileGetCharacterProfileReq();
                obj.CharacterId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
