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
            CustomSkills = new List<CDataSetAcquirementParam>();
        }

        public uint Id
        { 
            get { return CharacterInfo.CharacterId; }
            set { this.CharacterInfo.CharacterId = value; }
        }

        public int AccountId { get; set; }
        public DateTime Created { get; set; }
        public CDataCharacterInfo CharacterInfo { get; set; }
        public List<CDataSetAcquirementParam> CustomSkills { get; set;}
    }
}
