using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CBinarySaveSetCharacterBinSaveDataRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_BINARY_SAVE_SET_CHARACTER_BIN_SAVEDATA_RES;

        public class Serializer : PacketEntitySerializer<S2CBinarySaveSetCharacterBinSaveDataRes>
        {

            public override void Write(IBuffer buffer, S2CBinarySaveSetCharacterBinSaveDataRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CBinarySaveSetCharacterBinSaveDataRes Read(IBuffer buffer)
            {
                S2CBinarySaveSetCharacterBinSaveDataRes obj = new S2CBinarySaveSetCharacterBinSaveDataRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }

}
