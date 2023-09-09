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
        }

        // One of these bytes is UpdateType
        public byte Unk0 { get; set; }
        public byte Unk1 { get; set; }
        public uint Unk2 { get; set; }
        public CDataEditInfo EditInfo { get; set; }

        public class Serializer : PacketEntitySerializer<C2SCharacterEditUpdateCharacterEditParamReq>
        {
            public override void Write(IBuffer buffer, C2SCharacterEditUpdateCharacterEditParamReq obj)
            {
                WriteByte(buffer, obj.Unk0);
                WriteByte(buffer, obj.Unk1);
                WriteUInt32(buffer, obj.Unk2);
                WriteEntity(buffer, obj.EditInfo);
            }

            public override C2SCharacterEditUpdateCharacterEditParamReq Read(IBuffer buffer)
            {
                C2SCharacterEditUpdateCharacterEditParamReq obj = new C2SCharacterEditUpdateCharacterEditParamReq();
                obj.Unk0 = ReadByte(buffer);
                obj.Unk1 = ReadByte(buffer);
                obj.Unk2 = ReadUInt32(buffer);
                obj.EditInfo = ReadEntity<CDataEditInfo>(buffer);
                return obj;
            }
        }

    }
}