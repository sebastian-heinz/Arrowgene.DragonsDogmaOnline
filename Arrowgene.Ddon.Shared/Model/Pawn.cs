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
            SpSkillList = new List<CDataSpSkill>();
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
        public List<CDataSpSkill> SpSkillList { get; set; }
    }
}
