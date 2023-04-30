using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CJobPawnJobLevelUpMemberNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_JOB_PAWN_JOB_LEVEL_UP_MEMBER_NTC;

        public S2CJobPawnJobLevelUpMemberNtc()
        {
            CharacterLevelParam = new CDataCharacterLevelParam();
        }

        public uint CharacterId { get; set; }
        public uint PawnId { get; set; }
        public JobId Job { get; set; }
        public uint Level { get; set; }
        public CDataCharacterLevelParam CharacterLevelParam { get; set; }
        

        public class Serializer : PacketEntitySerializer<S2CJobPawnJobLevelUpMemberNtc>
        {
            public override void Write(IBuffer buffer, S2CJobPawnJobLevelUpMemberNtc obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
                WriteUInt32(buffer, obj.PawnId);
                WriteByte(buffer, (byte) obj.Job);
                WriteUInt32(buffer, obj.Level);
                WriteEntity<CDataCharacterLevelParam>(buffer, obj.CharacterLevelParam);
            }

            public override S2CJobPawnJobLevelUpMemberNtc Read(IBuffer buffer)
            {
                S2CJobPawnJobLevelUpMemberNtc obj = new S2CJobPawnJobLevelUpMemberNtc();
                obj.CharacterId = ReadUInt32(buffer);
                obj.PawnId = ReadUInt32(buffer);
                obj.Job = (JobId) ReadByte(buffer);
                obj.Level = ReadUInt32(buffer);
                obj.CharacterLevelParam = ReadEntity<CDataCharacterLevelParam>(buffer);
                return obj;
            }
        }

    }
}