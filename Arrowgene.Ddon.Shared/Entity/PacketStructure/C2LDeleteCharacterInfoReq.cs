using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2LDeleteCharacterInfoReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2L_DELETE_CHARACTER_INFO_REQ;

        public uint CharacterId { get; set; }

        public class Serializer : PacketEntitySerializer<C2LDeleteCharacterInfoReq>
        {
            public override void Write(IBuffer buffer, C2LDeleteCharacterInfoReq obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
            }

            public override C2LDeleteCharacterInfoReq Read(IBuffer buffer)
            {
                C2LDeleteCharacterInfoReq obj = new C2LDeleteCharacterInfoReq();
                obj.CharacterId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
