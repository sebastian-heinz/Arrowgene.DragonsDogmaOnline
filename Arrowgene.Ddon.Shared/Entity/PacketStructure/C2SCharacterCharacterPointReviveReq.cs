using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SCharacterCharacterPointReviveReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CHARACTER_CHARACTER_POINT_REVIVE_REQ;

        public uint HpMax { get; set; }

        public C2SCharacterCharacterPointReviveReq()
        {
            HpMax = 0;
        }

        public class Serializer : PacketEntitySerializer<C2SCharacterCharacterPointReviveReq>
        {

            public override void Write(IBuffer buffer, C2SCharacterCharacterPointReviveReq obj)
            {
                WriteUInt32(buffer, obj.HpMax);
            }

            public override C2SCharacterCharacterPointReviveReq Read(IBuffer buffer)
            {
                C2SCharacterCharacterPointReviveReq obj = new C2SCharacterCharacterPointReviveReq();
                obj.HpMax = ReadUInt32(buffer);
                return obj;
            }
        }
    }

}
