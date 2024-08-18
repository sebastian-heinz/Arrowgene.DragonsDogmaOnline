using System.Collections.Generic;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Appraisal;
using Arrowgene.Ddon.Shared.Model.BattleContent;

namespace Arrowgene.Ddon.Shared.Asset
{
    public class BitterblackMazeAsset
    {
        public BitterblackMazeAsset()
        {
            Stages = new Dictionary<StageId, BitterblackMazeConfig>();
            StarterEquipment = new Dictionary<JobId, Dictionary<EquipType, List<Item>>>();
            JobEquipment = new Dictionary<JobId, List<Item>>();
            RareItemAppraisalList = new List<CDataCommonU32>();
            ItemTakeawayList = new List<CDataCommonU32>();
            StageProgressionList = new List<CDataBattleContentStageProgression>();
        }

        public Dictionary<StageId, BitterblackMazeConfig> Stages { get; set; }
        public Dictionary<JobId, Dictionary<EquipType, List<Item>>> StarterEquipment {  get; set; }
        public Dictionary<JobId, List<Item>> JobEquipment { get; set; }
        public List<CDataCommonU32> RareItemAppraisalList { get; set; }
        public List<CDataCommonU32> ItemTakeawayList {  get; set; }
        public List<CDataBattleContentStageProgression> StageProgressionList {  get; set; }
    }
}
