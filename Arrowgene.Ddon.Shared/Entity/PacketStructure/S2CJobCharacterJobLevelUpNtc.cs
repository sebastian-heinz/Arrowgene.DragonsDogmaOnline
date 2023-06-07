using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CJobCharacterJobLevelUpNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_JOB_CHARACTER_JOB_LEVEL_UP_NTC;

        public S2CJobCharacterJobLevelUpNtc()
        {
            CharacterLevelParam = new CDataCharacterLevelParam();
        }

        public JobId Job { get; set; }
        public uint Level { get; set; }
        public uint AddJobPoint { get; set; }
        public uint TotalJobPoint { get; set; }
        public CDataCharacterLevelParam CharacterLevelParam { get; set; }

        public class Serializer : PacketEntitySerializer<S2CJobCharacterJobLevelUpNtc>
        {
            public override void Write(IBuffer buffer, S2CJobCharacterJobLevelUpNtc obj)
            {
                WriteByte(buffer, (byte) obj.Job);
                WriteUInt32(buffer, obj.Level);
                WriteUInt32(buffer, obj.AddJobPoint);
                WriteUInt32(buffer, obj.TotalJobPoint);
                WriteEntity<CDataCharacterLevelParam>(buffer, obj.CharacterLevelParam);
            }

            public override S2CJobCharacterJobLevelUpNtc Read(IBuffer buffer)
            {
                S2CJobCharacterJobLevelUpNtc obj = new S2CJobCharacterJobLevelUpNtc();
                obj.Job = (JobId) ReadByte(buffer);
                obj.Level = ReadUInt32(buffer);
                obj.AddJobPoint = ReadUInt32(buffer);
                obj.TotalJobPoint = ReadUInt32(buffer);
                obj.CharacterLevelParam = ReadEntity<CDataCharacterLevelParam>(buffer);
                return obj;
            }
        }

    }
}