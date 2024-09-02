using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SCharacterEditUpdateCharacterEditParamReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CHARACTER_EDIT_UPDATE_CHARACTER_EDIT_PARAM_REQ;

        public C2SCharacterEditUpdateCharacterEditParamReq()
        {
            EditInfo = new CDataEditInfo();
            EditPrice = new CDataCharacterEditPrice();
        }

        // One of these bytes is UpdateType
        public byte UpdateType { get; set; }
        public CDataCharacterEditPrice EditPrice { get; set; }
        public CDataEditInfo EditInfo { get; set; }

        public class Serializer : PacketEntitySerializer<C2SCharacterEditUpdateCharacterEditParamReq>
        {
            public override void Write(IBuffer buffer, C2SCharacterEditUpdateCharacterEditParamReq obj)
            {
                WriteByte(buffer, obj.UpdateType);
                WriteEntity(buffer, obj.EditPrice);
                WriteEntity(buffer, obj.EditInfo);
            }

            public override C2SCharacterEditUpdateCharacterEditParamReq Read(IBuffer buffer)
            {
                C2SCharacterEditUpdateCharacterEditParamReq obj = new C2SCharacterEditUpdateCharacterEditParamReq();
                obj.UpdateType = ReadByte(buffer);
                obj.EditPrice = ReadEntity<CDataCharacterEditPrice>(buffer);
                obj.EditInfo = ReadEntity<CDataEditInfo>(buffer);
                return obj;
            }
        }

    }
}
