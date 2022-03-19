using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SCharacterCharacterGoldenReviveReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CHARACTER_CHARACTER_GOLDEN_REVIVE_REQ;

        public uint HpMax { get; set; }

        public C2SCharacterCharacterGoldenReviveReq()
        {
            HpMax = 0;
        }

        public class Serializer : PacketEntitySerializer<C2SCharacterCharacterGoldenReviveReq>
        {

            public override void Write(IBuffer buffer, C2SCharacterCharacterGoldenReviveReq obj)
            {
                WriteUInt32(buffer, obj.HpMax);
            }

            public override C2SCharacterCharacterGoldenReviveReq Read(IBuffer buffer)
            {
                C2SCharacterCharacterGoldenReviveReq obj = new C2SCharacterCharacterGoldenReviveReq();
                obj.HpMax = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
