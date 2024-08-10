using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SBinarySaveSetCharacterBinSaveDataReq : IPacketStructure
    {
        public static int ARRAY_SIZE = 0x400;

        public PacketId Id => PacketId.C2S_BINARY_SAVE_SET_CHARACTER_BIN_SAVEDATA_REQ;

        public C2SBinarySaveSetCharacterBinSaveDataReq()
        {
            BinaryData = new byte[ARRAY_SIZE];
        }

        public byte[] BinaryData { get; set; }

        public class Serializer : PacketEntitySerializer<C2SBinarySaveSetCharacterBinSaveDataReq>
        {
            public override void Write(IBuffer buffer, C2SBinarySaveSetCharacterBinSaveDataReq obj)
            {
                WriteByteArray(buffer, obj.BinaryData);
            }

            public override C2SBinarySaveSetCharacterBinSaveDataReq Read(IBuffer buffer)
            {
                C2SBinarySaveSetCharacterBinSaveDataReq obj = new C2SBinarySaveSetCharacterBinSaveDataReq();
                obj.BinaryData = ReadByteArray(buffer, ARRAY_SIZE);
                return obj;
            }
        }
    }
}
