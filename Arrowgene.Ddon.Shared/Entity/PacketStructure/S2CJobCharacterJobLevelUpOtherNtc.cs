using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CJobCharacterJobLevelUpOtherNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_JOB_CHARACTER_JOB_LEVEL_UP_OTHER_NTC;

        public uint CharacterId { get; set; }
        public JobId Job { get; set; }
        public uint Level { get; set; }

        public class Serializer : PacketEntitySerializer<S2CJobCharacterJobLevelUpOtherNtc>
        {
            public override void Write(IBuffer buffer, S2CJobCharacterJobLevelUpOtherNtc obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
                WriteByte(buffer, (byte) obj.Job);
                WriteUInt32(buffer, obj.Level);
            }

            public override S2CJobCharacterJobLevelUpOtherNtc Read(IBuffer buffer)
            {
                S2CJobCharacterJobLevelUpOtherNtc obj = new S2CJobCharacterJobLevelUpOtherNtc();
                obj.CharacterId = ReadUInt32(buffer);
                obj.Job = (JobId) ReadByte(buffer);
                obj.Level = ReadUInt32(buffer);
                return obj;
            }
        }

    }
}