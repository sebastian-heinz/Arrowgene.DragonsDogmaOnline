using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestRaidBossPlayStartNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_QUEST_RAID_BOSS_PLAY_START_NTC;

        public S2CQuestRaidBossPlayStartNtc()
        {
            RaidBossPlayStartData = new CDataRaidBossPlayStartData();
        }

        public CDataRaidBossPlayStartData RaidBossPlayStartData {  get; set; }


        public class Serializer : PacketEntitySerializer<S2CQuestRaidBossPlayStartNtc>
        {
            public override void Write(IBuffer buffer, S2CQuestRaidBossPlayStartNtc obj)
            {
                WriteEntity(buffer, obj.RaidBossPlayStartData);
            }

            public override S2CQuestRaidBossPlayStartNtc Read(IBuffer buffer)
            {
                S2CQuestRaidBossPlayStartNtc obj = new S2CQuestRaidBossPlayStartNtc();
                obj.RaidBossPlayStartData = ReadEntity<CDataRaidBossPlayStartData>(buffer);
                return obj;
            }
        }
    }
}
