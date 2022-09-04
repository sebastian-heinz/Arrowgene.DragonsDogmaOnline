using System;
using System.Collections.Generic;
using Arrowgene.Ddon.Shared.Entity.Structure;

namespace Arrowgene.Ddon.Shared.Model
{
    public class Character
    {
        public Character()
        {
            CharacterInfo = new CDataCharacterInfo();
            NormalSkills = new List<CDataNormalSkillParam>();
            CustomSkills = new List<CDataSetAcquirementParam>();
            Abilities = new List<CDataSetAcquirementParam>();
        }

        // TODO: Remove this and use CharacterInfo.CharacterId directly in the references
        public uint Id
        { 
            get { return CharacterInfo.CharacterId; }
            set { this.CharacterInfo.CharacterId = value; }
        }

        public int AccountId { get; set; }
        public DateTime Created { get; set; }
        public CDataCharacterInfo CharacterInfo { get; set; }
        public List<CDataNormalSkillParam> NormalSkills { get; set; }
        public List<CDataSetAcquirementParam> CustomSkills { get; set;}
        public List<CDataSetAcquirementParam> Abilities { get; set; }
    }
}
