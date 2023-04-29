using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arrowgene.Ddon.Shared.Entity.Structure;

namespace Arrowgene.Ddon.Shared.Model
{
    // TODO: Better name
    // This class contains data common to both players (Character) and pawns (Pawn)
    public class CharacterCommon
    {
        public CharacterCommon()
        {
            EditInfo = new CDataEditInfo();
            StatusInfo = new CDataStatusInfo();
            CharacterJobDataList = new List<CDataCharacterJobData>();
            Equipment = new Equipment();
            CharacterEquipJobItemListDictionary = new Dictionary<JobId, List<CDataEquipJobItem>>();
            NormalSkills = new List<CDataNormalSkillParam>();
            CustomSkills = new List<CustomSkill>();
            Abilities = new List<Ability>();
        }

        public CDataCharacterJobData ActiveCharacterJobData
        {
            get { return CharacterJobDataList.Where(x => x.Job == Job).Single(); }
        }

        public uint CommonId { get; set; }
        public CDataGameServerListInfo Server { get; set; }
        public CDataEditInfo EditInfo { get; set; }
        public CDataStatusInfo StatusInfo { get; set; }
        public JobId Job { get; set; }
        public bool HideEquipHead { get; set; }
        public bool HideEquipLantern { get; set; }
        public List<CDataCharacterJobData> CharacterJobDataList { get; set; }
        public Equipment Equipment { get; set; }
        public Dictionary<JobId, List<CDataEquipJobItem>> CharacterEquipJobItemListDictionary { get; set; }
        public byte JewelrySlotNum { get; set; }
        public List<CDataNormalSkillParam> NormalSkills { get; set; }
        public List<CustomSkill> CustomSkills { get; set;}
        public List<Ability> Abilities { get; set; }
        public OnlineStatus OnlineStatus;
    }
}