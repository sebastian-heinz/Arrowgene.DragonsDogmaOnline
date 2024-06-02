#nullable enable
using System.Collections.Generic;
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
            // TODO: Fetch from DB
            CraftData = new CDataPawnCraftData() {
                CraftExp = 0,
                CraftRank = 0,
                CraftRankLimit = 8,
                CraftPoint = 0,
                PawnCraftSkillList = new List<CDataPawnCraftSkill>() {
                    new CDataPawnCraftSkill() {Type = 1, Level = 0},
                    new CDataPawnCraftSkill() {Type = 2, Level = 0},
                    new CDataPawnCraftSkill() {Type = 3, Level = 0},
                    new CDataPawnCraftSkill() {Type = 4, Level = 0},
                    new CDataPawnCraftSkill() {Type = 5, Level = 0},
                    new CDataPawnCraftSkill() {Type = 6, Level = 0},
                    new CDataPawnCraftSkill() {Type = 7, Level = 0},
                    new CDataPawnCraftSkill() {Type = 8, Level = 0},
                    new CDataPawnCraftSkill() {Type = 9, Level = 0},
                    new CDataPawnCraftSkill() {Type = 10, Level = 0}
                }
            };
            TrainingStatus = new Dictionary<JobId, byte[]>();
        }
        
        public Pawn(uint ownerCharacterId):this()
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
        public byte PawnType { get; set; }

        public List<CDataPawnReaction> PawnReactionList { get; set; }
        public CDataPawnCraftData CraftData { get; set; }

        public Dictionary<JobId, byte[]> TrainingStatus { get; set; }
        public Dictionary<JobId, List<CDataSpSkill>> SpSkills { get; set; }
        public uint TrainingPoints { get; set; } // Training xp?
        public uint AvailableTraining { get; set; } // Training lv?
    }
}
