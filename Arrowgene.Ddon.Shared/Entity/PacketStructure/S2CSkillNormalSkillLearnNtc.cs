using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSkillNormalSkillLearnNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_SKILL_NORMAL_SKILL_LEARN_NTC;

        public uint CharacterId { get; set; }
        public CDataContextNormalSkillData NormalSkillData { get; set; } = new();

        public class Serializer : PacketEntitySerializer<S2CSkillNormalSkillLearnNtc>
        {
            public override void Write(IBuffer buffer, S2CSkillNormalSkillLearnNtc obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
                WriteEntity<CDataContextNormalSkillData>(buffer, obj.NormalSkillData);
            }

            public override S2CSkillNormalSkillLearnNtc Read(IBuffer buffer)
            {
                S2CSkillNormalSkillLearnNtc obj = new S2CSkillNormalSkillLearnNtc();
                obj.CharacterId = ReadUInt32(buffer);
                obj.NormalSkillData = ReadEntity<CDataContextNormalSkillData>(buffer);
                return obj;
            }
        }
    }
}
