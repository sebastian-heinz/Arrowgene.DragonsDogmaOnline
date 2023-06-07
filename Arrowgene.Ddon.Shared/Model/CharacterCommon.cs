#nullable enable
using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.Shared.Entity.Structure;

namespace Arrowgene.Ddon.Shared.Model
{
    // TODO: Better name
    // This class contains data common to both players (Character) and pawns (Pawn)
    public class CharacterCommon
    {
        public CharacterCommon()
        {
            Server = new CDataGameServerListInfo();
            EditInfo = new CDataEditInfo();
            StatusInfo = new CDataStatusInfo();
            CharacterJobDataList = new List<CDataCharacterJobData>();
            Equipment = new Equipment();
            CharacterEquipJobItemListDictionary = ((JobId[]) JobId.GetValues(typeof(JobId)))
                .Select(jobId => (jobId, new List<CDataEquipJobItem>()))
                .ToDictionary(pair => pair.jobId, pair => pair.Item2);
            LearnedNormalSkills = new List<CDataNormalSkillParam>();
            LearnedCustomSkills = new List<CustomSkill>();
            EquippedCustomSkillsDictionary = ((JobId[]) JobId.GetValues(typeof(JobId)))
                .Select(jobId => (jobId, Enumerable.Repeat<CustomSkill?>(null, 0x14).ToList())) // Main Palette slots: 0x1, 0x2, 0x3, 0x4 || Sub Palette slots: 0x11, 0x12, 0x13, 0x14
                .ToDictionary(pair => pair.jobId, pair => pair.Item2);
            LearnedAbilities = new List<Ability>();
            EquippedAbilitiesDictionary = ((JobId[]) JobId.GetValues(typeof(JobId)))
                .Select(jobId => (jobId, Enumerable.Repeat<Ability?>(null, 10).ToList()))
                .ToDictionary(pair => pair.jobId, pair => pair.Item2);
        }

        public CDataCharacterJobData? ActiveCharacterJobData
        {
            get { return CharacterJobDataList.Where(x => x.Job == Job).SingleOrDefault(); }
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
        public List<CDataNormalSkillParam> LearnedNormalSkills { get; set; }
        public List<CustomSkill> LearnedCustomSkills { get; set;}
        public Dictionary<JobId, List<CustomSkill?>> EquippedCustomSkillsDictionary { get; set;}
        public List<Ability> LearnedAbilities { get; set; }
        public Dictionary<JobId, List<Ability?>> EquippedAbilitiesDictionary { get; set; }
        public OnlineStatus OnlineStatus;
    }
}