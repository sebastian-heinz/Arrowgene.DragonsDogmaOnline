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
            Unk0 = new CDataCharacterEditUpdatePawnEditParamReqUnk0();
            EditInfo = new CDataEditInfo();
        }

        public byte SlotNo { get; set; }
        public byte UpdateType { get; set; }
        public CDataCharacterEditUpdatePawnEditParamReqUnk0 Unk0 { get; set; }
        public CDataEditInfo EditInfo { get; set; }

        public class Serializer : PacketEntitySerializer<C2SCharacterEditUpdatePawnEditParamReq>
        {
            public override void Write(IBuffer buffer, C2SCharacterEditUpdatePawnEditParamReq obj)
            {
                WriteByte(buffer, obj.SlotNo);
                WriteByte(buffer, obj.UpdateType);
                WriteEntity(buffer, obj.Unk0);
                WriteEntity(buffer, obj.EditInfo);
            }

            public override C2SCharacterEditUpdatePawnEditParamReq Read(IBuffer buffer)
            {
                C2SCharacterEditUpdatePawnEditParamReq obj = new C2SCharacterEditUpdatePawnEditParamReq();
                obj.SlotNo = ReadByte(buffer);
                obj.UpdateType = ReadByte(buffer);
                obj.Unk0 = ReadEntity<CDataCharacterEditUpdatePawnEditParamReqUnk0>(buffer);
                obj.EditInfo = ReadEntity<CDataEditInfo>(buffer);
                return obj;
            }
        }

    }
}