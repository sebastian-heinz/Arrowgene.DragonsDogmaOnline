using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SCharacterEditUpdatePawnEditParamReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CHARACTER_EDIT_UPDATE_PAWN_EDIT_PARAM_REQ;

        public C2SCharacterEditUpdatePawnEditParamReq()
        {
            EditPrice = new CDataCharacterEditPrice();
            EditInfo = new CDataEditInfo();
        }

        public byte SlotNo { get; set; }
        public byte UpdateType { get; set; }
        public CDataCharacterEditPrice EditPrice { get; set; }
        public CDataEditInfo EditInfo { get; set; }

        public class Serializer : PacketEntitySerializer<C2SCharacterEditUpdatePawnEditParamReq>
        {
            public override void Write(IBuffer buffer, C2SCharacterEditUpdatePawnEditParamReq obj)
            {
                WriteByte(buffer, obj.SlotNo);
                WriteByte(buffer, obj.UpdateType);
                WriteEntity(buffer, obj.EditPrice);
                WriteEntity(buffer, obj.EditInfo);
            }

            public override C2SCharacterEditUpdatePawnEditParamReq Read(IBuffer buffer)
            {
                C2SCharacterEditUpdatePawnEditParamReq obj = new C2SCharacterEditUpdatePawnEditParamReq();
                obj.SlotNo = ReadByte(buffer);
                obj.UpdateType = ReadByte(buffer);
                obj.EditPrice = ReadEntity<CDataCharacterEditPrice>(buffer);
                obj.EditInfo = ReadEntity<CDataEditInfo>(buffer);
                return obj;
            }
        }

    }
}
