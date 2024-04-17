using System.Linq;
using System.Collections.Generic;
using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;

namespace Arrowgene.Ddon.GameServer;

public static class GameStructure
{
    public static void CDataCharacterListElement(
        CDataCharacterListElement cDataCharacterListElement,
        Character character)
    {
        CDataCommunityCharacterBaseInfo(cDataCharacterListElement.CommunityCharacterBaseInfo, character);
        CDataCharacterListElement_common(cDataCharacterListElement, character);
    }

    public static void CDataCharacterListElement(
        CDataCharacterListElement cDataCharacterListElement,
        Pawn pawn)
    {
        CDataCommunityCharacterBaseInfo(cDataCharacterListElement.CommunityCharacterBaseInfo, pawn);
        CDataCharacterListElement_common(cDataCharacterListElement, pawn);
    }

    private static void CDataCharacterListElement_common(
        CDataCharacterListElement cDataCharacterListElement,
        CharacterCommon character)
    {
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
        cDataCommunityCharacterBaseInfo.CharacterId = character.CharacterId;
        CDataCharacterName(cDataCommunityCharacterBaseInfo.CharacterName, character);
        cDataCommunityCharacterBaseInfo.ClanName = ""; // TODO: Clan
    }

    public static void CDataCommunityCharacterBaseInfo(
        CDataCommunityCharacterBaseInfo cDataCommunityCharacterBaseInfo,
        Pawn pawn)
    {
        cDataCommunityCharacterBaseInfo.CharacterId = pawn.CharacterId;
        CDataCharacterName(cDataCommunityCharacterBaseInfo.CharacterName, pawn);
        cDataCommunityCharacterBaseInfo.ClanName = ""; // TODO: Clan
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

    public static void CDataCharacterName(CDataCharacterName cDataCharacterName,
        Pawn character)
    {
        cDataCharacterName.FirstName = character.Name;
        cDataCharacterName.LastName = string.Empty;
    }

    public static void CDataPartyMember(CDataPartyMember cDataPartyMember,
        PlayerPartyMember partyMember)
    {
        CDataCharacterListElement(cDataPartyMember.CharacterListElement, partyMember.Client.Character);
        CDataPartyMember_common(cDataPartyMember, partyMember);
    }

    public static void CDataPartyMember(CDataPartyMember cDataPartyMember,
        PawnPartyMember partyMember)
    {
        CDataCharacterListElement(cDataPartyMember.CharacterListElement, partyMember.Pawn);
        CDataPartyMember_common(cDataPartyMember, partyMember);
    }

    private static void CDataPartyMember_common(CDataPartyMember cDataPartyMember,
        PartyMember partyMember)
    {
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
        cDataPawnInfo.Name = pawn.Name;
        cDataPawnInfo.EditInfo = pawn.EditInfo;
        cDataPawnInfo.State = 0; // TODO: ?
        cDataPawnInfo.MaxHp = pawn.StatusInfo.MaxHP;
        cDataPawnInfo.MaxStamina = pawn.StatusInfo.MaxStamina;
        cDataPawnInfo.JobId = pawn.Job;
        cDataPawnInfo.CharacterJobDataList = pawn.CharacterJobDataList;
        cDataPawnInfo.CharacterEquipDataList = new List<CDataCharacterEquipData>() {
            new CDataCharacterEquipData() {
                Equips = pawn.Equipment.getEquipmentAsCDataEquipItemInfo(pawn.Job, EquipType.Performance)
            }
        };
        cDataPawnInfo.CharacterEquipViewDataList = new List<CDataCharacterEquipData>() {
            new CDataCharacterEquipData() {
                Equips = pawn.Equipment.getEquipmentAsCDataEquipItemInfo(pawn.Job, EquipType.Visual)
            }
        };
        cDataPawnInfo.CharacterEquipJobItemList = pawn.Equipment.getJobItemsAsCDataEquipJobItem(pawn.Job);
        cDataPawnInfo.JewelrySlotNum = pawn.JewelrySlotNum;
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
        cDataPawnInfo.HideEquipHead = pawn.HideEquipHead;
        cDataPawnInfo.HideEquipLantern = pawn.HideEquipLantern;
        // TODO: AdventureCount, CraftCount, MaxAdventureCount, MaxCraftCount
        cDataPawnInfo.AdventureCount = 5;
        cDataPawnInfo.CraftCount = 10;
        cDataPawnInfo.MaxAdventureCount = 5;
        cDataPawnInfo.MaxCraftCount = 10;
        cDataPawnInfo.ContextNormalSkillList = pawn.LearnedNormalSkills.Select(normalSkill => new CDataContextNormalSkillData(normalSkill)).ToList();
        cDataPawnInfo.ContextSkillList = pawn.EquippedCustomSkillsDictionary[pawn.Job]
            .Select((skill, index) => skill?.AsCDataContextAcquirementData((byte)(index+1)))
            .Where(skill => skill != null)
            .ToList();
        cDataPawnInfo.ContextAbilityList = pawn.EquippedAbilitiesDictionary[pawn.Job]
            .Select((ability, index) => ability?.AsCDataContextAcquirementData((byte)(index+1)))
            .Where(ability => ability != null)
            .ToList();
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

    public static void CDataCharacterLevelParam(CDataCharacterLevelParam characterLevelParam, CharacterCommon character)
    {
        CDataCharacterJobData activeCharacterJobData = character.ActiveCharacterJobData;
        characterLevelParam.Attack = activeCharacterJobData.Atk;
        characterLevelParam.MagAttack = activeCharacterJobData.MAtk;
        characterLevelParam.Defence = activeCharacterJobData.Def;
        characterLevelParam.MagDefence = activeCharacterJobData.MDef;
        characterLevelParam.Strength = activeCharacterJobData.Strength;
        characterLevelParam.DownPower = activeCharacterJobData.DownPower;
        characterLevelParam.ShakePower = activeCharacterJobData.ShakePower;
        characterLevelParam.StunPower = activeCharacterJobData.StunPower;
        characterLevelParam.Constitution = activeCharacterJobData.Consitution;
        characterLevelParam.Guts = activeCharacterJobData.Guts;
    }

    public static void CDataContextBase(CDataContextBase contextBase, Character character)
    {
        contextBase.CharacterId = character.CharacterId;
        contextBase.FirstName = character.FirstName;
        contextBase.LastName = character.LastName;
        CDataContextBase_common(contextBase, character);
    }

    public static void CDataContextBase(CDataContextBase contextBase, Pawn pawn)
    {
        contextBase.CharacterId = pawn.CharacterId;
        contextBase.FirstName = pawn.Name;
        contextBase.LastName = string.Empty;
        CDataContextBase_common(contextBase, pawn);
    }

    public static void CDataContextBase_common(CDataContextBase contextBase, CharacterCommon character)
    {
        contextBase.StageNo = 200; // TODO: Replace with the actual stage the player is in. As it is right now it'll probably give issues when new players join outside of WDT
        contextBase.Sex = character.EditInfo.Sex;
        contextBase.HideEquipHead = character.HideEquipHead;
        contextBase.HideEquipLantern = character.HideEquipLantern;
        contextBase.ContextEquipPerformanceList = character.Equipment.getEquipmentAsCDataContextEquipData(character.Job, EquipType.Performance);
        contextBase.ContextEquipVisualList = character.Equipment.getEquipmentAsCDataContextEquipData(character.Job, EquipType.Visual);
        contextBase.ContextEquipJobItemList = character.Equipment.getJobItemsAsCDataEquipJobItem(character.Job)
            .Select(x => new CDataContextEquipJobItemData(x)).ToList();
        contextBase.ContextNormalSkillList = character.LearnedNormalSkills
            .Where(x => x.Job == character.Job)
            .Select(x => new CDataContextNormalSkillData(x)).ToList();
        contextBase.ContextSkillList = character.EquippedCustomSkillsDictionary[character.Job]
            .Select((x, index) => x?.AsCDataContextAcquirementData((byte)(index+1)))
            .Where(x => x != null)
            .ToList();
        contextBase.ContextAbilityList = character.EquippedAbilitiesDictionary[character.Job]
            .Select((x, index) => x?.AsCDataContextAcquirementData((byte)(index+1)))
            .Where(x => x != null)
            .ToList();
        contextBase.Unk0List = new List<CDataContextBaseUnk0>(); // TODO: Figure this one out
    }

    public static void CDataContextPlayerInfo(CDataContextPlayerInfo contextPlayerInfo, CharacterCommon character)
        {
            CDataCharacterJobData characterJobData = character.ActiveCharacterJobData;
            contextPlayerInfo.Job = character.Job;
            contextPlayerInfo.HP = character.StatusInfo.HP;
            contextPlayerInfo.MaxHP = character.StatusInfo.MaxHP;
            contextPlayerInfo.WhiteHP = character.StatusInfo.WhiteHP;
            contextPlayerInfo.Stamina = character.StatusInfo.Stamina;
            contextPlayerInfo.MaxStamina = character.StatusInfo.MaxStamina;
            // Weight?
            contextPlayerInfo.Lv = (ushort) characterJobData.Lv;
            contextPlayerInfo.Exp = characterJobData.Exp;
            contextPlayerInfo.Atk = characterJobData.Atk;
            contextPlayerInfo.Def = characterJobData.Def;
            contextPlayerInfo.MAtk = characterJobData.MAtk;
            contextPlayerInfo.MDef = characterJobData.MDef;
            contextPlayerInfo.Strength = characterJobData.Strength;
            contextPlayerInfo.DownPower = characterJobData.DownPower;
            contextPlayerInfo.ShakePower = characterJobData.ShakePower;
            // StanPower?
            // Constitution?
            contextPlayerInfo.Guts = characterJobData.Guts;
            contextPlayerInfo.JobPoint = characterJobData.JobPoint;
            contextPlayerInfo.GainHp = character.StatusInfo.GainHP;
            contextPlayerInfo.GainStamina = character.StatusInfo.GainStamina;
            contextPlayerInfo.GainAttack = character.StatusInfo.GainAttack;
            contextPlayerInfo.GainDefense = character.StatusInfo.GainDefense;
            contextPlayerInfo.GainMagicAttack = character.StatusInfo.GainMagicAttack;
            contextPlayerInfo.GainMagicDefense = character.StatusInfo.GainMagicDefense;
            // ActNo?
            contextPlayerInfo.RevivePoint = character.StatusInfo.RevivePoint;
            // CustomSkillGroup?
            contextPlayerInfo.JobList = character.CharacterJobDataList
                .Select(x => new CDataContextJobData(x)).ToList();
            contextPlayerInfo.ChargeEffectList = new List<CDataCommonU32>(); // TODO
            contextPlayerInfo.OcdActiveList = new List<CDataOcdActive>(); // TODO
            // CatchType?
            // CatchJointNo?
            // CustomWork?
        }

        public static void CDataLobbyContextPlayer(CDataLobbyContextPlayer lobbyContextPlayer, Character character)
        {
            CDataContextBase(lobbyContextPlayer.Base, character);
            CDataLobbyContextPlayer_common(lobbyContextPlayer, character);
        }

        public static void CDataLobbyContextPlayer(CDataLobbyContextPlayer lobbyContextPlayer, Pawn pawn)
        {
            CDataContextBase(lobbyContextPlayer.Base, pawn);
            CDataLobbyContextPlayer_common(lobbyContextPlayer, pawn);
        }

        public static void CDataLobbyContextPlayer_common(CDataLobbyContextPlayer lobbyContextPlayer, CharacterCommon common)
        {
            CDataContextPlayerInfo(lobbyContextPlayer.PlayerInfo, common);
            lobbyContextPlayer.EditInfo = common.EditInfo;
        }

        public static void CDataContextResist(CDataContextResist contextResist, CharacterCommon character)
        {
            CDataCharacterJobData characterJobData = character.ActiveCharacterJobData;
            contextResist.FireResist = characterJobData.FireResist;
            contextResist.IceResist = characterJobData.IceResist;
            contextResist.ThunderResist = characterJobData.ThunderResist;
            contextResist.HolyResist = characterJobData.HolyResist;
            contextResist.DarkResist = characterJobData.DarkResist;
            contextResist.SpreadResist = characterJobData.SpreadResist;
            contextResist.FreezeResist = characterJobData.FreezeResist;
            contextResist.ShockResist = characterJobData.ShockResist;
            contextResist.AbsorbResist = characterJobData.AbsorbResist;
            contextResist.DarkElmResist = characterJobData.DarkElmResist;
            contextResist.PoisonResist = characterJobData.PoisonResist;
            contextResist.SlowResist = characterJobData.SlowResist;
            contextResist.SleepResist = characterJobData.SleepResist;
            contextResist.StunResist = characterJobData.StunResist;
            contextResist.WetResist = characterJobData.WetResist;
            contextResist.OilResist = characterJobData.OilResist;
            contextResist.SealResist = characterJobData.SealResist;
            contextResist.CurseResist = characterJobData.CurseResist;
            contextResist.SoftResist = characterJobData.SoftResist;
            contextResist.StoneResist = characterJobData.StoneResist;
            contextResist.GoldResist = characterJobData.GoldResist;
            contextResist.FireReduceResist = characterJobData.FireReduceResist;
            contextResist.IceReduceResist = characterJobData.IceReduceResist;
            contextResist.ThunderReduceResist = characterJobData.ThunderReduceResist;
            contextResist.HolyReduceResist = characterJobData.HolyReduceResist;
            contextResist.DarkReduceResist = characterJobData.DarkReduceResist;
            contextResist.AtkDownResist = characterJobData.AtkDownResist;
            contextResist.DefDownResist = characterJobData.DefDownResist;
            contextResist.MAtkDownResist = characterJobData.MAtkDownResist;
            contextResist.MDefDownResist = characterJobData.MDefDownResist;
        }

        public static void S2CContextGetLobbyPlayerContextNtc(S2CContextGetLobbyPlayerContextNtc ntc, Character character)
        {
            ntc.CharacterId = character.CharacterId;
            GameStructure.CDataLobbyContextPlayer(ntc.Context, character);
        }
}
