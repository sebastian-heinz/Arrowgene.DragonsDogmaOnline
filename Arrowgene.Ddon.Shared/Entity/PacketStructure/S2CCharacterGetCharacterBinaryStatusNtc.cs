using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CCharacterGetCharacterBinaryStatusNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_CHARACTER_GET_CHARACTER_BINARY_STATUS_NTC;

        public uint CharacterId { get; set; }
        public uint BinarySize { get; set; }
        public byte[] BinaryData { get; set; } = new byte[512];

        public class Serializer : PacketEntitySerializer<S2CCharacterGetCharacterBinaryStatusNtc>
        {
            public override void Write(IBuffer buffer, S2CCharacterGetCharacterBinaryStatusNtc obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
                WriteUInt32(buffer, obj.BinarySize);
                WriteByteArray(buffer, obj.BinaryData);
            }

            public override S2CCharacterGetCharacterBinaryStatusNtc Read(IBuffer buffer)
            {
                S2CCharacterGetCharacterBinaryStatusNtc obj = new S2CCharacterGetCharacterBinaryStatusNtc();
                obj.CharacterId = ReadUInt32(buffer);
                obj.BinarySize = ReadUInt32(buffer);
                obj.BinaryData = ReadByteArray(buffer, 512);
                return obj;
            }
        }
    }
}
