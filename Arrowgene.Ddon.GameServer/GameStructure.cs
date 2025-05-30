using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using System.Collections.Generic;
using System.Linq;

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
        cDataCharacterListElement.OnlineStatus = character.OnlineStatus;
        CDataJobBaseInfo(cDataCharacterListElement.CurrentJobBaseInfo, character.Job,
            (byte)character.ActiveCharacterJobData.Lv);
        CDataJobBaseInfo(cDataCharacterListElement.EntryJobBaseInfo, character.Job,
            (byte)character.ActiveCharacterJobData.Lv);
        cDataCharacterListElement.MatchingProfile = "";
        cDataCharacterListElement.unk2 = 1;
    }

    public static void CDataCommunityCharacterBaseInfo(
        CDataCommunityCharacterBaseInfo cDataCommunityCharacterBaseInfo,
        Character character)
    {
        cDataCommunityCharacterBaseInfo.CharacterId = character.CharacterId;
        CDataCharacterName(cDataCommunityCharacterBaseInfo.CharacterName, character);
        cDataCommunityCharacterBaseInfo.ClanName = character.ClanName.ShortName;
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
        cDataPawnInfo.State = pawn.PawnState;
        cDataPawnInfo.MaxHp = pawn.StatusInfo.MaxHP;
        cDataPawnInfo.MaxStamina = pawn.StatusInfo.MaxStamina;
        cDataPawnInfo.JobId = pawn.Job;
        cDataPawnInfo.CharacterJobDataList = pawn.CharacterJobDataList;
        cDataPawnInfo.CharacterEquipDataList = new List<CDataCharacterEquipData>() {
            new CDataCharacterEquipData() {
                Equips = pawn.Equipment.AsCDataEquipItemInfo(EquipType.Performance)
            }
        };
        cDataPawnInfo.CharacterEquipViewDataList = new List<CDataCharacterEquipData>() {
            new CDataCharacterEquipData() {
                Equips = pawn.Equipment.AsCDataEquipItemInfo(EquipType.Visual)
            }
        };
        cDataPawnInfo.CharacterEquipJobItemList = pawn.EquipmentTemplate.JobItemsAsCDataEquipJobItem(pawn.Job);
        cDataPawnInfo.JewelrySlotNum = pawn.JewelrySlotNum;
        // TODO: Pawn CharacterItemSlotInfoList, CraftData
        cDataPawnInfo.CharacterItemSlotInfoList = new List<CDataCharacterItemSlotInfo>();
        cDataPawnInfo.CraftData = pawn.CraftData;
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
        cDataPawnInfo.ExtendParam = pawn.CalculateFullExtendedParams(); // pawn.ExtendedParams;
        cDataPawnInfo.PawnType = pawn.PawnType;
        // TODO: ShareRange, Likability, Unk1
        cDataPawnInfo.ShareRange = 1;
        cDataPawnInfo.Likability = pawn.PartnerPawnData.NumGifts; // Shows up as Number of Presents
        cDataPawnInfo.TrainingStatus = pawn.TrainingStatus.GetValueOrDefault(pawn.Job, new byte[64]);
        cDataPawnInfo.PawnTrainingProfile = new CDataPawnTrainingProfile() {TrainingExp = 30000, DialogCount = 3, DialogCountMax = 3, AttackFrequencyAndDistance = 1, TrainingLv = 3};
        cDataPawnInfo.SpSkillList = pawn.SpSkills.GetValueOrDefault(pawn.Job, new List<CDataSpSkill>());
    }

    public static void CDataNoraPawnInfo(CDataNoraPawnInfo cDataNoraPawnInfo, Pawn pawn, DdonGameServer server)
    {
        cDataNoraPawnInfo.Name = pawn.Name;
        cDataNoraPawnInfo.EditInfo = pawn.EditInfo;
        cDataNoraPawnInfo.Job = (byte)pawn.Job;
        cDataNoraPawnInfo.CharacterEquipData = new List<CDataCharacterEquipData>() { new CDataCharacterEquipData() {
                    Equips = pawn.Equipment.AsCDataEquipItemInfo(EquipType.Performance)
            }};
        cDataNoraPawnInfo.CharacterEquipViewData = new List<CDataCharacterEquipData>() { new CDataCharacterEquipData() {
                    Equips = pawn.Equipment.AsCDataEquipItemInfo(EquipType.Visual)
            }};
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
        contextBase.ContextEquipPerformanceList = character.Equipment.AsCDataContextEquipData(EquipType.Performance);
        contextBase.ContextEquipVisualList = character.Equipment.AsCDataContextEquipData(EquipType.Visual);
        CDataContextBase_common(contextBase, character);
    }

    public static void CDataContextBase(CDataContextBase contextBase, Pawn pawn)
    {
        contextBase.CharacterId = pawn.CharacterId;
        contextBase.FirstName = pawn.Name;
        contextBase.LastName = string.Empty;
        contextBase.ContextEquipPerformanceList = pawn.Equipment.AsCDataContextEquipData(EquipType.Performance);
        contextBase.ContextEquipVisualList = pawn.Equipment.AsCDataContextEquipData(EquipType.Visual);
        CDataContextBase_common(contextBase, pawn);
    }

    private static void CDataContextBase_common(CDataContextBase contextBase, CharacterCommon character)
    {
        int StageNo = (int)(character.StageNo);
        if (StageNo == 0)
        {
            // WDT
            StageNo = 200;
        }

        contextBase.StageNo = StageNo;
        contextBase.Sex = character.EditInfo.Sex;
        contextBase.HideEquipHead = character.HideEquipHead;
        contextBase.HideEquipLantern = character.HideEquipLantern;
        contextBase.ContextEquipJobItemList = character.EquipmentTemplate.JobItemsAsCDataEquipJobItem(character.Job)
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
        contextBase.EmblemStatList = character.EmblemStatList;
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
        contextPlayerInfo.Atk = (uint) characterJobData.Atk;
        contextPlayerInfo.Def = (uint) characterJobData.Def;
        contextPlayerInfo.MAtk = (uint) characterJobData.MAtk;
        contextPlayerInfo.MDef = (uint) characterJobData.MDef;
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
        contextPlayerInfo.WhiteHP = character.StatusInfo.WhiteHP;
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
        CDataContextPlayerInfo(lobbyContextPlayer.PlayerInfo, character);
        lobbyContextPlayer.EditInfo = character.EditInfo;
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

    public static void S2CContextGetLobbyPlayerContextNtc(DdonGameServer server, S2CContextGetLobbyPlayerContextNtc ntc, Character character)
    {
        ntc.CharacterId = character.CharacterId;
        GameStructure.CDataLobbyContextPlayer(ntc.Context, character);
    }
}
