using Arrowgene.Ddon.Shared.Entity.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Model.Quest
{
    public class QuestMissionParams
    {
        public QuestMissionParams()
        {
            QuestPhaseGroupIdList = new List<CDataCommonU32>();
        }

        public uint SortieMinimum { get; set; }
        public uint SortieMaximum { get; set; }
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
