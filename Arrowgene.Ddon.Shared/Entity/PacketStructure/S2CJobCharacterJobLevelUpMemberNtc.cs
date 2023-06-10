using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CJobCharacterJobLevelUpMemberNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_JOB_CHARACTER_JOB_LEVEL_UP_MEMBER_NTC;

        public S2CJobCharacterJobLevelUpMemberNtc()
        {
            CharacterLevelParam = new CDataCharacterLevelParam();
        }

        public uint CharacterId { get; set; }
        public JobId Job { get; set; }
        public uint Level { get; set; }
        public CDataCharacterLevelParam CharacterLevelParam { get; set; }

        public class Serializer : PacketEntitySerializer<S2CJobCharacterJobLevelUpMemberNtc>
        {
            public override void Write(IBuffer buffer, S2CJobCharacterJobLevelUpMemberNtc obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
                WriteByte(buffer, (byte) obj.Job);
                WriteUInt32(buffer, obj.Level);
                WriteEntity<CDataCharacterLevelParam>(buffer, obj.CharacterLevelParam);
            }

            public override S2CJobCharacterJobLevelUpMemberNtc Read(IBuffer buffer)
            {
                S2CJobCharacterJobLevelUpMemberNtc obj = new S2CJobCharacterJobLevelUpMemberNtc();
                obj.CharacterId = ReadUInt32(buffer);
                obj.Job = (JobId) ReadByte(buffer);
                obj.Level = ReadUInt32(buffer);
                obj.CharacterLevelParam = ReadEntity<CDataCharacterLevelParam>(buffer);
                return obj;
            }
        }

    }
}