using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SCharacterEditUpdatePawnEditParamExReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CHARACTER_EDIT_UPDATE_PAWN_EDIT_PARAM_EX_REQ;

        public C2SCharacterEditUpdatePawnEditParamExReq()
        {
            Unk0 = new CDataCharacterEditUpdatePawnEditParamReqUnk0();
            EditInfo = new CDataEditInfo();
            Name = string.Empty;
        }

        public byte SlotNo { get; set; }
        public byte UpdateType { get; set; }
        public CDataCharacterEditUpdatePawnEditParamReqUnk0 Unk0 { get; set; }
        public CDataEditInfo EditInfo { get; set; }
        public string Name { get; set; }

        public class Serializer : PacketEntitySerializer<C2SCharacterEditUpdatePawnEditParamExReq>
        {
            public override void Write(IBuffer buffer, C2SCharacterEditUpdatePawnEditParamExReq obj)
            {
                WriteByte(buffer, obj.SlotNo);
                WriteByte(buffer, obj.UpdateType);
                WriteEntity(buffer, obj.Unk0);
                WriteEntity(buffer, obj.EditInfo);
                WriteMtString(buffer, obj.Name);
            }

            public override C2SCharacterEditUpdatePawnEditParamExReq Read(IBuffer buffer)
            {
                C2SCharacterEditUpdatePawnEditParamExReq obj = new C2SCharacterEditUpdatePawnEditParamExReq();
                obj.SlotNo = ReadByte(buffer);
                obj.UpdateType = ReadByte(buffer);
                obj.Unk0 = ReadEntity<CDataCharacterEditUpdatePawnEditParamReqUnk0>(buffer);
                obj.EditInfo = ReadEntity<CDataEditInfo>(buffer);
                obj.Name = ReadMtString(buffer);
                return obj;
            }
        }

    }
}