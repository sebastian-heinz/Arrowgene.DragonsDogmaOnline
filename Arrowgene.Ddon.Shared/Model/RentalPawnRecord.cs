using Arrowgene.Ddon.Shared.Entity.Structure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.Shared.Model
{
 
    /// <summary>
    /// IMPORTANT: If this class changes shape, the table `ddon_rental_pawn` needs to be emptied to prevent improperly shared records from trying to be deserialized.
    /// </summary>
    public class RentalPawnRecord
    {
        public uint PawnId { get; set; }
        public uint CharacterId { get; set; }
        public uint CommonId { get; set; }
        public string Name { get; set; }
        public List<CDataPawnReaction> PawnReactionList { get; set; } = [];
        public CDataPawnCraftData CraftData { get; set; } = new();
        public byte[] TrainingStatus { get; set; } = [];
        public List<CDataSpSkill> SpSkills { get; set; } = [];
        public bool IsOfficialPawn { get; set; }
        public CDataEditInfo EditInfo { get; set; } = new();
        public JobId Job { get; set; }
        public bool HideEquipHead { get; set; }
        public bool HideEquipLantern { get; set; }
        public CDataCharacterJobData CharacterJobData { get; set; } = new();
        public Equipment Equipment { get; set; }
        public List<CDataNormalSkillParam> LearnedNormalSkills { get; set; } = [];
        public List<CustomSkill?> EquippedCustomSkills { get; set; } = [];
        public List<Ability?> EquippedAbilities { get; set; } = [];
        public CDataOrbGainExtendParam ExtendedParams { get; set; } = new();
        public DateTime HireDate { get; set; }
        public CharacterProfile PawnProfile { get; set; } = new();

        public RentalPawnRecord()
        {
            // This has to exist for JSON reasons.
        }

        public static RentalPawnRecord FromPawn(Pawn pawn, Character ownerCharacter)
        {
            var record = new RentalPawnRecord()
            {
                PawnId = pawn.PawnId,
                CharacterId = pawn.CharacterId,
                CommonId = pawn.CommonId,
                Name = pawn.Name,
                PawnReactionList = pawn.PawnReactionList,
                CraftData = pawn.CraftData,
                TrainingStatus = pawn.TrainingStatus.GetValueOrDefault(pawn.Job, new byte[64]),
                SpSkills = pawn.SpSkills.GetValueOrDefault(pawn.Job, []),
                IsOfficialPawn = pawn.IsOfficialPawn,
                EditInfo = pawn.EditInfo,
                Job = pawn.Job,
                HideEquipHead = pawn.HideEquipHead,
                HideEquipLantern = pawn.HideEquipLantern,
                CharacterJobData = pawn.CharacterJobDataList.FirstOrDefault(x => x.Job == pawn.Job),
                Equipment = pawn.Equipment,
                LearnedNormalSkills = [.. pawn.LearnedNormalSkills.Where(x => x.Job == pawn.Job)],
                EquippedCustomSkills = pawn.EquippedCustomSkillsDictionary.GetValueOrDefault(pawn.Job),
                EquippedAbilities = pawn.EquippedAbilitiesDictionary.GetValueOrDefault(pawn.Job),
                ExtendedParams = pawn.ExtendedParams 
                    + ownerCharacter.ExtendedJobParams.GetValueOrDefault(JobId.None, new()) 
                    + ownerCharacter.ExtendedJobParams.GetValueOrDefault(pawn.Job, new()),
                HireDate = DateTime.Now,
                PawnProfile = pawn.CharacterProfile
            };

            return record;
        }

        public RentalPawn ToRentalPawn(uint hiringCharacterId, byte adventureCount, byte craftCount, uint killCount = 0)
        {
            RentalPawn pawn = new()
            {
                // RentalPawn Fields
                OwningCharacterId = CharacterId,
                AdventureCount = adventureCount,
                CraftCount = craftCount,
                KillCount = killCount,
                HireDate = HireDate,

                // Pawn Fields
                PawnId = PawnId,
                CharacterId = hiringCharacterId,
                Name = Name,
                HmType = 1,
                PawnType = PawnType.Support,
                PawnReactionList = PawnReactionList,
                CraftData = CraftData,
                TrainingStatus = new() { { Job, TrainingStatus } },
                SpSkills = new() { { Job, SpSkills } },
                IsOfficialPawn = IsOfficialPawn,
                IsRented = true,
                PawnState = PawnState.None,

                // CharacterCommon Fields
                CommonId = CommonId,
                Server = new(), // ???
                EditInfo = EditInfo,
                StatusInfo = new()
                {
                    GainAttack = ExtendedParams.Attack,
                    GainDefense = ExtendedParams.Defence,
                    GainMagicAttack = ExtendedParams.MagicAttack,
                    GainMagicDefense = ExtendedParams.MagicDefence,
                    GainStamina = ExtendedParams.StaminaMax,
                    GainHP = ExtendedParams.HpMax,

                    MaxHP = 760U,
                    MaxStamina = 450U,
                    HP = uint.MaxValue,
                    WhiteHP = uint.MaxValue,
                    Stamina = uint.MaxValue,
                },
                Job = Job,
                HideEquipHead = HideEquipHead,
                HideEquipLantern = HideEquipLantern,
                CharacterJobDataList = [CharacterJobData],
                Equipment = Equipment,
                JewelrySlotNum = (byte)(1 + ExtendedParams.JewelrySlot),
                LearnedNormalSkills = LearnedNormalSkills,
                LearnedCustomSkills = [.. EquippedCustomSkills.Where(x => x is not null)],
                EquippedCustomSkillsDictionary = new() { { Job, EquippedCustomSkills } },
                LearnedAbilities = [.. EquippedAbilities.Where(x => x is not null)],
                EquippedAbilitiesDictionary = new() { { Job, EquippedAbilities } },
                ExtendedParams = ExtendedParams,
                ExtendedJobParams = new() { { Job, new() } },
                OrbRelease = [],
                CharacterProfile = PawnProfile
            };

            
            return pawn;
        }
    }
}
