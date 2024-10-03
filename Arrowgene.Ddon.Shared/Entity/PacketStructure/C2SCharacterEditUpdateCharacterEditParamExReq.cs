using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SCharacterEditUpdateCharacterEditParamExReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CHARACTER_EDIT_UPDATE_CHARACTER_EDIT_PARAM_EX_REQ;

        public C2SCharacterEditUpdateCharacterEditParamExReq()
        {
            EditInfo = new CDataEditInfo();
            EditPrice = new CDataCharacterEditPrice();
            FirstName = string.Empty;
        }

        // One of these bytes is UpdateType
        public byte UpdateType { get; set; }
        public CDataCharacterEditPrice EditPrice { get; set; }
        public CDataEditInfo EditInfo { get; set; }
        public string FirstName { get; set; }

        public class Serializer : PacketEntitySerializer<C2SCharacterEditUpdateCharacterEditParamExReq>
        {
            public override void Write(IBuffer buffer, C2SCharacterEditUpdateCharacterEditParamExReq obj)
            {
                WriteByte(buffer, obj.UpdateType);
                WriteEntity(buffer, obj.EditPrice);
                WriteEntity(buffer, obj.EditInfo);
                WriteMtString(buffer, obj.FirstName);
            }

            public override C2SCharacterEditUpdateCharacterEditParamExReq Read(IBuffer buffer)
            {
                C2SCharacterEditUpdateCharacterEditParamExReq obj = new C2SCharacterEditUpdateCharacterEditParamExReq();
                obj.UpdateType = ReadByte(buffer);
                obj.EditPrice = ReadEntity<CDataCharacterEditPrice>(buffer);
                obj.EditInfo = ReadEntity<CDataEditInfo>(buffer);
                obj.FirstName = ReadMtString(buffer);
                return obj;
            }
        }

    }
}
