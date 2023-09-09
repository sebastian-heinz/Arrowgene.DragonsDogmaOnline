using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    // Used in both C2S_CHARACTER_EDIT_UPDATE_PAWN_EDIT_PARAM_REQ and C2S_CHARACTER_EDIT_UPDATE_PAWN_EDIT_PARAM_EX_REQ
    public class CDataCharacterEditUpdatePawnEditParamReqUnk0
    {    
        public byte Unk0 { get; set; }
        public uint Unk1 { get; set; }
    
        public class Serializer : EntitySerializer<CDataCharacterEditUpdatePawnEditParamReqUnk0>
        {
            public override void Write(IBuffer buffer, CDataCharacterEditUpdatePawnEditParamReqUnk0 obj)
            {
                WriteByte(buffer, obj.Unk0);
                WriteUInt32(buffer, obj.Unk1);
            }
        
            public override CDataCharacterEditUpdatePawnEditParamReqUnk0 Read(IBuffer buffer)
            {
                CDataCharacterEditUpdatePawnEditParamReqUnk0 obj = new CDataCharacterEditUpdatePawnEditParamReqUnk0();
                obj.Unk0 = ReadByte(buffer);
                obj.Unk1 = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}