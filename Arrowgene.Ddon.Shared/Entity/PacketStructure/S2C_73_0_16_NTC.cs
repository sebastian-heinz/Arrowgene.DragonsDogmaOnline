using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2C_73_0_16_NTC : IPacketStructure
    {
        public S2C_73_0_16_NTC()
        {
        }

        public List<CDataDragonSkill> DragonSkillList { get; set; }

        public PacketId Id => PacketId.S2C_73_0_16_NTC;

        public class Serializer : PacketEntitySerializer<S2C_73_0_16_NTC>
        {
            public override void Write(IBuffer buffer, S2C_73_0_16_NTC obj)
            {
                WriteEntityList(buffer, obj.DragonSkillList);
            }

            public override S2C_73_0_16_NTC Read(IBuffer buffer)
            {
                var obj = new S2C_73_0_16_NTC();
                obj.DragonSkillList = ReadEntityList<CDataDragonSkill>(buffer);
                return obj;
            }
        }
    }
}
