#nullable enable
using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.Shared.Entity.Structure;

namespace Arrowgene.Ddon.Shared.Model
{
    public class Pawn : CharacterCommon
    {
        public Pawn()
        {
            Name = string.Empty;
            OnlineStatus = OnlineStatus.None;
            PawnReactionList = new List<CDataPawnReaction>();
            SpSkills = new Dictionary<JobId, List<CDataSpSkill>>();
            CraftData = new CDataPawnCraftData() {
                CraftExp = 0,
                CraftRank = 1,
                CraftRankLimit = 71,
                CraftPoint = 0,
                PawnCraftSkillList = new List<CDataPawnCraftSkill>() {
                    new() {Type = CraftSkillType.ProductionSpeed, Level = 0},
                    new() {Type = CraftSkillType.EquipmentEnhancement, Level = 0},
                    new() {Type = CraftSkillType.EquipmentQuality, Level = 0},
                    new() {Type = CraftSkillType.ConsumableQuantity, Level = 0},
                    new() {Type = CraftSkillType.CostPerformance, Level = 0},
                    new() {Type = CraftSkillType.ConsumableProductionIsAlwaysGreatSuccess, Level = 0},
                    new() {Type = CraftSkillType.CreatingHighQualityEquipmentIsAlwaysGreatSuccess, Level = 0},
                    new() {Type = CraftSkillType.CostPerformanceEffectUpFactor1, Level = 0},
                    new() {Type = CraftSkillType.CostPerformanceEffectUpFactor2, Level = 0},
                    new() {Type = CraftSkillType.UnknownEffect10, Level = 0}
                }
            };
            TrainingStatus = new Dictionary<JobId, byte[]>();
            IsRented = false;
            PawnState = PawnState.None;
            PartnerPawnData = new PartnerPawnData();
        }
        
        public Pawn(uint ownerCharacterId) : this()
        {
            CharacterId = ownerCharacterId;
        }

        /// <summary>
        /// Id of Pawn
        /// </summary>
        public uint PawnId  { get; set; }
        
        /// <summary>
        /// Id of character who this pawn belongs to
        /// </summary>
        public uint CharacterId { get; set; }

        public string Name { get; set; }
        
        public byte HmType { get; set; }
        public PawnType PawnType { get; set; }

        public List<CDataPawnReaction> PawnReactionList { get; set; }
        public CDataPawnCraftData CraftData { get; set; }

        public Dictionary<JobId, byte[]> TrainingStatus { get; set; }
        public Dictionary<JobId, List<CDataSpSkill>> SpSkills { get; set; }
        public uint TrainingPoints { get; set; } // Training xp?
        public uint AvailableTraining { get; set; } // Training lv?
        public PartnerPawnData PartnerPawnData { get; set; }

        public bool IsOfficialPawn {  get; set; }
        public bool IsRented {  get; set; }
        public PawnState PawnState { get; set; }
    }
}
