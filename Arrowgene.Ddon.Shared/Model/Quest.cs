using Arrowgene.Ddon.Shared.Asset;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.GameServer.Quests
{
    public abstract class Quest
    {
        public bool IsDiscoverable { get; set; }
        public bool HasEnemy { get; set; }
        public uint CurrentProcessNo { get; set; }
        public QuestType QuestType { get; set; }

        public Quest()
        {
        }

        public abstract CDataQuestList ToCDataQuestList();
        public abstract List<CDataQuestProcessState> StateMachineExecute(uint keyId, uint questScheduleId, uint processNo, out QuestState questState);
        public abstract S2CItemUpdateCharacterItemNtc CreateRewardsPacket();
        public abstract bool HasEnemiesInCurrentStageGroup(uint stageNo, uint groupId, uint subGroupId);
    }
}
