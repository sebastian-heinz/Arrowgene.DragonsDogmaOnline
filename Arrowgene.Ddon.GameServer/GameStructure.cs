using System.Linq;
using System.Collections.Generic;
using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.GameServer;

public static class GameStructure
{
    public static void CDataCharacterListElement(
        CDataCharacterListElement cDataCharacterListElement,
        Character character)
    {
        CDataCommunityCharacterBaseInfo(cDataCharacterListElement.CommunityCharacterBaseInfo, character);
        cDataCharacterListElement.ServerId = character.Server.Id;
        cDataCharacterListElement.OnlineStatus = 0;
        CDataJobBaseInfo(cDataCharacterListElement.CurrentJobBaseInfo, character.Job,
            (byte)character.ActiveCharacterJobData.Lv);
        CDataJobBaseInfo(cDataCharacterListElement.EntryJobBaseInfo, character.Job,
            (byte)character.ActiveCharacterJobData.Lv);
        cDataCharacterListElement.MatchingProfile = "";
        cDataCharacterListElement.unk2 = 0;
    }

    public static void CDataCommunityCharacterBaseInfo(
        CDataCommunityCharacterBaseInfo cDataCommunityCharacterBaseInfo,
        Character character)
    {
        cDataCommunityCharacterBaseInfo.CharacterId = character.Id;
        CDataCharacterName(cDataCommunityCharacterBaseInfo.CharacterName, character);
        cDataCommunityCharacterBaseInfo.ClanName = "";
    }

    public static void CDataJobBaseInfo(CDataJobBaseInfo cDataJobBaseInfo,
        JobId jobId,
        byte jobLevel)
    {
        cDataJobBaseInfo.Job = jobId;
        cDataJobBaseInfo.Level = jobLevel;
    }

    public static void CDataCharacterName(CDataCharacterName cDataCharacterName,
        Character character)
    {
        cDataCharacterName.FirstName = character.FirstName;
        cDataCharacterName.LastName = character.LastName;
    }

    public static void CDataPartyMember(CDataPartyMember cDataPartyMember,
        PartyMember partyMember)
    {
        CDataCharacterListElement(cDataPartyMember.CharacterListElement, partyMember.Character);
        cDataPartyMember.MemberType = partyMember.MemberType;
        cDataPartyMember.MemberIndex = partyMember.MemberIndex;
        cDataPartyMember.PawnId = partyMember.PawnId;
        cDataPartyMember.IsLeader = partyMember.IsLeader;
        cDataPartyMember.IsPawn = partyMember.IsPawn;
        cDataPartyMember.IsPlayEntry = partyMember.IsPlayEntry;
        cDataPartyMember.JoinState = partyMember.JoinState;
        cDataPartyMember.AnyValueList = partyMember.AnyValueList;
        cDataPartyMember.SessionStatus = partyMember.SessionStatus;
    }

    public static void CDataPawnInfo(CDataPawnInfo cDataPawnInfo, Pawn pawn)
    {
        
        cDataPawnInfo.Version = 0;
        cDataPawnInfo.Name = pawn.Character.FirstName;
        cDataPawnInfo.EditInfo = pawn.Character.EditInfo;
        cDataPawnInfo.State = 0; // TODO: ?
        cDataPawnInfo.MaxHp = pawn.Character.StatusInfo.MaxHP;
        cDataPawnInfo.MaxStamina = pawn.Character.StatusInfo.MaxStamina;
        cDataPawnInfo.JobId = pawn.Character.Job;
        cDataPawnInfo.CharacterJobDataList = pawn.Character.CharacterJobDataList;
        cDataPawnInfo.CharacterEquipDataList = new List<CDataCharacterEquipData>() {
            new CDataCharacterEquipData() {
                Equips = pawn.Character.Equipment.getEquipmentAsCDataEquipItemInfo(pawn.Character.Job, EquipType.Performance)
            }
        };
        cDataPawnInfo.CharacterEquipViewDataList = new List<CDataCharacterEquipData>() {
            new CDataCharacterEquipData() {
                Equips = pawn.Character.Equipment.getEquipmentAsCDataEquipItemInfo(pawn.Character.Job, EquipType.Visual)
            }
        };
        cDataPawnInfo.CharacterEquipJobItemList = pawn.Character.CharacterEquipJobItemListDictionary[pawn.Character.Job];
        cDataPawnInfo.JewelrySlotNum = pawn.Character.JewelrySlotNum;
        // TODO: Pawn CharacterItemSlotInfoList, CraftData
        cDataPawnInfo.CharacterItemSlotInfoList = new List<CDataCharacterItemSlotInfo>();
        cDataPawnInfo.CraftData = new CDataPawnCraftData() {
            CraftExp = 391,
            CraftRank = 4,
            CraftRankLimit = 8,
            CraftPoint = 0,
            PawnCraftSkillList = new List<CDataPawnCraftSkill>() {
                new CDataPawnCraftSkill() {Type = 1, Level = 0},
                new CDataPawnCraftSkill() {Type = 2, Level = 3},
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
        cDataPawnInfo.PawnReactionList = pawn.PawnReactionList;
        cDataPawnInfo.HideEquipHead = pawn.Character.HideEquipHead;
        cDataPawnInfo.HideEquipLantern = pawn.Character.HideEquipLantern;
        // TODO: AdventureCount, CraftCount, MaxAdventureCount, MaxCraftCount
        cDataPawnInfo.AdventureCount = 5;
        cDataPawnInfo.CraftCount = 10;
        cDataPawnInfo.MaxAdventureCount = 5;
        cDataPawnInfo.MaxCraftCount = 10;
        cDataPawnInfo.ContextNormalSkillList = pawn.Character.NormalSkills.Select(normalSkill => new CDataContextNormalSkillData(normalSkill)).ToList();
        cDataPawnInfo.ContextSkillList = pawn.Character.CustomSkills.Select(skill => skill.AsCDataContextAcquirementData()).ToList();
        cDataPawnInfo.ContextAbilityList = pawn.Character.Abilities.Select(ability => ability.AsCDataContextAcquirementData()).ToList();
        // TODO: AbilityCostMax, ExtendParam
        cDataPawnInfo.AbilityCostMax = 15;
        cDataPawnInfo.ExtendParam = new CDataOrbGainExtendParam() {
            HpMax = 0x29E,
            StaminaMax = 0x0,
            Attack = 0x10,
            Defence = 0x10,
            MagicAttack = 0xF,
            MagicDefence = 0x10,
            AbilityCost = 0x0,
            JewelrySlot = 0x0,
            UseItemSlot = 0x0,
            MaterialItemSlot = 0x0,
            EquipItemSlot = 0x0,
            MainPawnSlot = 0x0,
            SupportPawnSlot = 0x0
        };
        cDataPawnInfo.PawnType = pawn.PawnType;
        // TODO: ShareRange, Likability, Unk0, Unk1
        cDataPawnInfo.ShareRange = 1;
        cDataPawnInfo.Likability = 2;
        cDataPawnInfo.Unk0 = new byte[64];
        cDataPawnInfo.Unk1 = new CData_772E80() {Unk0 = 0x7530, Unk1 = 0x3, Unk2 = 0x3, Unk3 = 0x1, Unk4 = 0x3};
        cDataPawnInfo.SpSkillList = pawn.SpSkillList;
    }
}
