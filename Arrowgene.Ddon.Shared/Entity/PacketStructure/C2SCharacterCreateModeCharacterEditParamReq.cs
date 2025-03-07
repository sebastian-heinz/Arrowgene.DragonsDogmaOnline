using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SCharacterCreateModeCharacterEditParamReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CHARACTER_CREATE_MODE_CHARACTER_EDIT_PARAM_REQ;

        public C2SCharacterCreateModeCharacterEditParamReq()
        {
        }

        public byte UpdateType { get; set; }
        public CDataEditInfo EditInfo { get; set; } = new();

        public class Serializer : PacketEntitySerializer<C2SCharacterCreateModeCharacterEditParamReq>
        {
            public override void Write(IBuffer buffer, C2SCharacterCreateModeCharacterEditParamReq obj)
            {
                WriteByte(buffer, obj.UpdateType);
                WriteEntity(buffer, obj.EditInfo);
            }

            public override C2SCharacterCreateModeCharacterEditParamReq Read(IBuffer buffer)
            {
                C2SCharacterCreateModeCharacterEditParamReq obj = new C2SCharacterCreateModeCharacterEditParamReq();
                obj.UpdateType = ReadByte(buffer);
                obj.EditInfo = ReadEntity<CDataEditInfo>(buffer);
                return obj;
            }
        }
    }
}
