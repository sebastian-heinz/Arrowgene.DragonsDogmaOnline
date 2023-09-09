using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CCharacterEditUpdateEditParamNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_CHARACTER_EDIT_UPDATE_EDIT_PARAM_NTC;

        public S2CCharacterEditUpdateEditParamNtc()
        {
            EditInfo = new CDataEditInfo();
        }

        public uint CharacterId { get; set; }
        public uint PawnId { get; set; }
        public CDataEditInfo EditInfo { get; set; }

        public class Serializer : PacketEntitySerializer<S2CCharacterEditUpdateEditParamNtc>
        {
            public override void Write(IBuffer buffer, S2CCharacterEditUpdateEditParamNtc obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
                WriteUInt32(buffer, obj.PawnId);
                WriteEntity(buffer, obj.EditInfo);
            }

            public override S2CCharacterEditUpdateEditParamNtc Read(IBuffer buffer)
            {
                S2CCharacterEditUpdateEditParamNtc obj = new S2CCharacterEditUpdateEditParamNtc();
                obj.CharacterId = ReadUInt32(buffer);
                obj.PawnId = ReadUInt32(buffer);
                obj.EditInfo = ReadEntity<CDataEditInfo>(buffer);
                return obj;
            }
        }
    }
}