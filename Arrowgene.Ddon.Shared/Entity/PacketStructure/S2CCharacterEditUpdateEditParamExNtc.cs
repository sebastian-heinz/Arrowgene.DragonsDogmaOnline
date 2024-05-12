using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CCharacterEditUpdateEditParamExNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_CHARACTER_EDIT_UPDATE_EDIT_PARAM_EX_NTC;

        public S2CCharacterEditUpdateEditParamExNtc()
        {
            EditInfo = new CDataEditInfo();
            Name = string.Empty;
        }

        public uint CharacterId { get; set; }
        public uint PawnId { get; set; }
        public CDataEditInfo EditInfo { get; set; }
        public string Name { get; set; }

        public class Serializer : PacketEntitySerializer<S2CCharacterEditUpdateEditParamExNtc>
        {
            public override void Write(IBuffer buffer, S2CCharacterEditUpdateEditParamExNtc obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
                WriteUInt32(buffer, obj.PawnId);
                WriteEntity(buffer, obj.EditInfo);
                WriteMtString(buffer, obj.Name);
            }

            public override S2CCharacterEditUpdateEditParamExNtc Read(IBuffer buffer)
            {
                S2CCharacterEditUpdateEditParamExNtc obj = new S2CCharacterEditUpdateEditParamExNtc();
                obj.CharacterId = ReadUInt32(buffer);
                obj.PawnId = ReadUInt32(buffer);
                obj.EditInfo = ReadEntity<CDataEditInfo>(buffer);
                obj.Name = ReadMtString(buffer);
                return obj;
            }
        }
    }
}