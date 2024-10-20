using Arrowgene.Ddon.Shared.Entity.Structure;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Model.Quest
{
    public class QuestMissionParams
    {
        public QuestMissionParams()
        {
            QuestPhaseGroupIdList = new List<CDataCommonU32>();
        }
        public byte StartPos { get; set; }
        public uint MinimumMembers { get; set; }
        public uint MaximumMembers { get; set; }
        public uint PlaytimeInSeconds { get; set; }
        public bool IsSolo { get; set; }
        public uint MaxPawns { get; set; }
        // public bool SupportPawnAllowed { get; set; } Is in symbol but doesn't seem to work?
        public bool ArmorAllowed { get; set; }
        public bool JewelryAllowed { get; set; }
        public uint Group {  get; set; }
        public List<CDataCommonU32> QuestPhaseGroupIdList { get; set; }
        public QuestLootDistribution LootDistribution { get; set; }
    }
}
