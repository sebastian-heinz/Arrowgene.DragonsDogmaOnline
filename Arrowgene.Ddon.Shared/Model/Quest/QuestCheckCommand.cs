using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Model.Quest
{
    public enum QuestCheckCommand : ushort
    {
        TalkNpc = 1, // (cQuestProcess* this, s32 stageNo, s32 npcId, s32 param03, s32 param04))
        DieEnemy = 2, // (cQuestProcess* this, s32 stageNo, s32 groupNo, s32 setNo, s32 param04))
        SceHitIn = 3, // (cQuestProcess* this, s32 stageNo, s32 sceNo, s32 param03, s32 param04))
        HaveItem = 4, // (cQuestProcess* this, s32 itemId, s32 itemNum, s32 param03, s32 param04))
        DeliverItem = 5, // (cQuestProcess* this, s32 itemId, s32 itemNum, s32 npcId, s32 msgNo))
        EmDieLight = 6, // (cQuestProcess* this, s32 enemyId, s32 enemyLv, s32 enemyNum, s32 param04))
        QstFlagOn = 7, // (cQuestProcess* this, s32 questId, s32 flagNo, s32 param03, s32 param04))
        QstFlagOff = 8, // (cQuestProcess* this, s32 questId, s32 flagNo, s32 param03, s32 param04))
        MyQstFlagOn = 9, // (cQuestProcess* this, s32 flagNo, s32 param02, s32 param03, s32 param04))
        MyQstFlagOff = 10, // (cQuestProcess* this, s32 flagNo, s32 param02, s32 param03, s32 param04))
        Padding00 = 11, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        Padding01 = 12, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        Padding02 = 13, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        StageNo = 14, // (cQuestProcess* this, s32 stageNo, s32 param02, s32 param03, s32 param04))
        EventEnd = 15, // (cQuestProcess* this, s32 stageNo, s32 eventNo, s32 param03, s32 param04))
        Prt = 16, // (cQuestProcess* this, s32 stageNo, s32 x, s32 y, s32 z))
        Clearcount = 17, // (cQuestProcess* this, s32 minCount, s32 maxCount, s32 param03, s32 param04))
        SceFlagOn = 18, // (cQuestProcess* this, s32 flagNo, s32 param02, s32 param03, s32 param04))
        SceFlagOff = 19, // (cQuestProcess* this, s32 flagNo, s32 param02, s32 param03, s32 param04))
        TouchActToNpc = 20, // (cQuestProcess* this, s32 stageNo, s32 npcId, s32 param03, s32 param04))
        OrderDecide = 21, // (cQuestProcess* this, s32 npcId, s32 param02, s32 param03, s32 param04))
        IsEndCycle = 22, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        IsInterruptCycle = 23, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        IsFailedCycle = 24, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        IsEndResult = 25, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        NpcTalkAndOrderUi = 26, // (cQuestProcess* this, s32 stageNo, s32 npcId, s32 noOrderGroupSerial, s32 param04))
        NpcTouchAndOrderUi = 27, // (cQuestProcess* this, s32 stageNo, s32 npcId, s32 noOrderGroupSerial, s32 param04))
        StageNoNotEq = 28, // (cQuestProcess* this, s32 stageNo, s32 param02, s32 param03, s32 param04))
        Warlevel = 29, // (cQuestProcess* this, s32 warLevel, s32 param02, s32 param03, s32 param04))
        TalkNpcWithoutMarker = 30, // (cQuestProcess* this, s32 stageNo, s32 npcId, s32 param03, s32 param04))
        HaveMoney = 31, // (cQuestProcess* this, s32 gold, s32 type, s32 param03, s32 param04))
        SetQuestClearNum = 32, // (cQuestProcess* this, s32 clearNum, s32 areaId, s32 param03, s32 param04))
        MakeCraft = 33, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        PlayEmotion = 34, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        IsEndTimer = 35, // (cQuestProcess* this, s32 timerNo, s32 param02, s32 param03, s32 param04))
        IsEnemyFound = 36, // (cQuestProcess* this, s32 stageNo, s32 groupNo, s32 setNo, s32 param04))
        RandomEq = 37, // 0x00636F60 (cQuestProcess* this, s32 randomNo, s32 value, s32 param03_unused, s32 param04_unused)
        RandomNotEq = 38, // 0x00637030 (cQuestProcess* this, s32 randomNo, s32 value, s32 param03_unused, s32 param04_unused)
        RandomLess = 39, // 0x00637100 (cQuestProcess* this, s32 randomNo, s32 value, s32 param03_unused, s32 param04_unused)
        RandomNotGreater = 40, // 0x006371D0 (cQuestProcess* this, s32 randomNo, s32 value, s32 param03_unused, s32 param04_unused)
        RandomGreater = 41, // 0x006372A0 (cQuestProcess* this, s32 randomNo, s32 value, s32 param03_unused, s32 param04_unused)
        RandomNotLess = 42, // 0x00637370 (cQuestProcess* this, s32 randomNo, s32 value, s32 param03_unused, s32 param04_unused)
        Clearcount02 = 43, // (cQuestProcess* this, s32 div, s32 value, s32 param03, s32 param04))
        IngameTimeRangeEq = 44, // (cQuestProcess* this, s32 minTime, s32 maxTime, s32 param03, s32 param04))
        IngameTimeRangeNotEq = 45, // (cQuestProcess* this, s32 minTime, s32 maxTime, s32 param03, s32 param04))
        PlHp = 46, // (cQuestProcess* this, s32 hpRate, s32 type, s32 param03, s32 param04))
        EmHpNotLess = 47, // (cQuestProcess* this, s32 stageNo, s32 groupNo, s32 setNo, s32 hpRate))
        EmHpLess = 48, // (cQuestProcess* this, s32 stageNo, s32 groupNo, s32 setNo, s32 hpRate))
        WeatherEq = 49, // (cQuestProcess* this, s32 weatherId, s32 param02, s32 param03, s32 param04))
        WeatherNotEq = 50, // (cQuestProcess* this, s32 weatherId, s32 param02, s32 param03, s32 param04))
        PlJobEq = 51, // (cQuestProcess* this, s32 jobId, s32 param02, s32 param03, s32 param04))
        PlJobNotEq = 52, // (cQuestProcess* this, s32 jobId, s32 param02, s32 param03, s32 param04))
        PlSexEq = 53, // (cQuestProcess* this, s32 sex, s32 param02, s32 param03, s32 param04))
        PlSexNotEq = 54, // (cQuestProcess* this, s32 sex, s32 param02, s32 param03, s32 param04))
        SceHitOut = 55, // (cQuestProcess* this, s32 stageNo, s32 sceNo, s32 param03, s32 param04))
        WaitOrder = 56, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        OmSetTouch = 57, // (cQuestProcess* this, s32 stageNo, s32 groupNo, s32 setNo, s32 param04))
        OmReleaseTouch = 58, // (cQuestProcess* this, s32 stageNo, s32 groupNo, s32 setNo, s32 param04))
        JobLevelNotLess = 59, // (cQuestProcess* this, s32 checkType, s32 level, s32 param03, s32 param04))
        JobLevelLess = 60, // (cQuestProcess* this, s32 checkType, s32 level, s32 param03, s32 param04))
        MyQstFlagOnFromFsm = 61, // (cQuestProcess* this, s32 flagNo, s32 param02, s32 param03, s32 param04))
        SceHitInWithoutMarker = 62, // (cQuestProcess* this, s32 stageNo, s32 sceNo, s32 param03, s32 param04))
        SceHitOutWithoutMarker = 63, // (cQuestProcess* this, s32 stageNo, s32 sceNo, s32 param03, s32 param04))
        KeyItemPoint = 64, // (cQuestProcess* this, s32 idx, s32 num, s32 param03, s32 param04))
        IsNotEndTimer = 65, // (cQuestProcess* this, s32 timerNo, s32 param02, s32 param03, s32 param04))
        IsMainQuestClear = 66, // (cQuestProcess* this, s32 questId, s32 param02, s32 param03, s32 param04))
        DogmaOrb = 67, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        IsEnemyFoundForOrder = 68, // (cQuestProcess* this, s32 stageNo, s32 groupNo, s32 setNo, s32 param04))
        IsTutorialFlagOn = 69, // (cQuestProcess* this, s32 flagNo, s32 param02, s32 param03, s32 param04))
        QuestOmSetTouch = 70, // (cQuestProcess* this, s32 stageNo, s32 groupNo, s32 setNo, s32 param04))
        QuestOmReleaseTouch = 71, // (cQuestProcess* this, s32 stageNo, s32 groupNo, s32 setNo, s32 param04))
        NewTalkNpc = 72, // (cQuestProcess* this, s32 stageNo, s32 groupNo, s32 setNo, s32 questId))
        NewTalkNpcWithoutMarker = 73, // (cQuestProcess* this, s32 stageNo, s32 groupNo, s32 setNo, s32 questId))
        IsTutorialQuestClear = 74, // (cQuestProcess* this, s32 questId, s32 param02, s32 param03, s32 param04))
        IsMainQuestOrder = 75, // (cQuestProcess* this, s32 questId, s32 param02, s32 param03, s32 param04))
        IsTutorialQuestOrder = 76, // (cQuestProcess* this, s32 questId, s32 param02, s32 param03, s32 param04))
        IsTouchPawnDungeonOm = 77, // (cQuestProcess* this, s32 stageNo, s32 groupNo, s32 setNo, s32 param04))
        IsOpenDoorOmQuestSet = 78, // (cQuestProcess* this, s32 stageNo, s32 groupNo, s32 setNo, s32 questId))
        EmDieForRandomDungeon = 79, // (cQuestProcess* this, s32 stageNo, s32 enemyId, s32 enemyNum, s32 param04))
        NpcHpNotLess = 80, // (cQuestProcess* this, s32 stageNo, s32 groupNo, s32 setNo, s32 hpRate))
        NpcHpLess = 81, // (cQuestProcess* this, s32 stageNo, s32 groupNo, s32 setNo, s32 hpRate))
        IsEnemyFoundWithoutMarker = 82, // (cQuestProcess* this, s32 stageNo, s32 groupNo, s32 setNo, s32 param04))
        IsEventBoardAccepted = 83, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        WorldManageQuestFlagOn = 84, // (cQuestProcess* this, s32 flagNo, s32 questId, s32 param03, s32 param04))
        WorldManageQuestFlagOff = 85, // (cQuestProcess* this, s32 flagNo, s32 questId, s32 param03, s32 param04))
        TouchEventBoard = 86, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        OpenEntryRaidBoss = 87, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        OepnEntryFortDefense = 88, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        DiePlayer = 89, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        PartyNumNotLessWtihoutPawn = 90, // (cQuestProcess* this, s32 partyMemberNum, s32 param02, s32 param03, s32 param04))
        PartyNumNotLessWithPawn = 91, // (cQuestProcess* this, s32 partyMemberNum, s32 param02, s32 param03, s32 param04))
        LostMainPawn = 92, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        SpTalkNpc = 93, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        OepnJobMaster = 94, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        TouchRimStone = 95, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        GetAchievement = 96, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        DummyNotProgress = 97, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        DieRaidBoss = 98, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        CycleTimerZero = 99, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        CycleTimer = 100, // (cQuestProcess* this, s32 timeSec, s32 param02, s32 param03, s32 param04))
        QuestNpcTalkAndOrderUi = 101, // (cQuestProcess* this, s32 stageNo, s32 groupNo, s32 setNo, s32 questId))
        QuestNpcTouchAndOrderUi = 102, // (cQuestProcess* this, s32 stageNo, s32 groupNo, s32 setNo, s32 questId))
        IsFoundRaidBoss = 103, // (cQuestProcess* this, s32 stageNo, s32 groupNo, s32 setNo, s32 enemyId))
        QuestOmSetTouchWithoutMarker = 104, // (cQuestProcess* this, s32 stageNo, s32 groupNo, s32 setNo, s32 param04))
        QuestOmReleaseTouchWithoutMarker = 105, // (cQuestProcess* this, s32 stageNo, s32 groupNo, s32 setNo, s32 param04))
        TutorialTalkNpc = 106, // (cQuestProcess* this, s32 stageNo, s32 npcId, s32 param03, s32 param04))
        IsLogin = 107, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        IsPlayEndFirstSeasonEndCredit = 108, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        IsKilledTargetEnemySetGroup = 109, // (cQuestProcess* this, s32 flagNo, s32 param02, s32 param03, s32 param04))
        IsKilledTargetEmSetGrpNoMarker = 110, // (cQuestProcess* this, s32 flagNo, s32 param02, s32 param03, s32 param04))
        IsLeftCycleTimer = 111, // (cQuestProcess* this, s32 timeSec, s32 param02, s32 param03, s32 param04))
        OmEndText = 112, // (cQuestProcess* this, s32 stageNo, s32 groupNo, s32 setNo, s32 param04))
        QuestOmEndText = 113, // (cQuestProcess* this, s32 stageNo, s32 groupNo, s32 setNo, s32 param04))
        OpenAreaMaster = 114, // (cQuestProcess* this, s32 areaId, s32 param02, s32 param03, s32 param04))
        HaveItemAllBag = 115, // (cQuestProcess* this, s32 itemId, s32 itemNum, s32 param03, s32 param04))
        OpenNewspaper = 116, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        OpenQuestBoard = 117, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        StageNoWithoutMarker = 118, // (cQuestProcess* this, s32 stageNo, s32 param02, s32 param03, s32 param04))
        TalkQuestNpcUnitMarker = 119, // (cQuestProcess* this, s32 stageNo, s32 groupNo, s32 setNo, s32 questId))
        TouchQuestNpcUnitMarker = 120, // (cQuestProcess* this, s32 stageNo, s32 groupNo, s32 setNo, s32 questId))
        IsExistSecondPawn = 121, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        IsOrderJobTutorialQuest = 122, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        IsOpenWarehouse = 123, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        IsMyquestLayoutFlagOn = 124, // (cQuestProcess* this, s32 FlagNo, s32 param02, s32 param03, s32 param04))
        IsMyquestLayoutFlagOff = 125, // (cQuestProcess* this, s32 FlagNo, s32 param02, s32 param03, s32 param04))
        IsOpenWarehouseReward = 126, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        IsOrderLightQuest = 127, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        IsOrderWorldQuest = 128, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        IsLostMainPawn = 129, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        IsFullOrderQuest = 130, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        IsBadStatus = 131, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        CheckAreaRank = 132, // (cQuestProcess* this, s32 AreaId, s32 AreaRank, s32 param03, s32 param04))
        Padding133 = 133, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        EnablePartyWarp = 134, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        IsHugeble = 135, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        IsDownEnemy = 136, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        OpenAreaMasterSupplies = 137, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        OpenEntryBoard = 138, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        NoticeInterruptContents = 139, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        OpenRetrySelect = 140, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        IsPlWeakening = 141, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        NoticePartyInvite = 142, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        IsKilledAreaBoss = 143, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        IsPartyReward = 144, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        IsFullBag = 145, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        OpenCraftExam = 146, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        LevelUpCraft = 147, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        IsClearLightQuest = 148, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        OpenJobMasterReward = 149, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        TouchActQuestNpc = 150, // (cQuestProcess* this, s32 stageNo, s32 groupNo, s32 setNo, s32 questId))
        IsLeaderAndJoinPawn = 151, // (cQuestProcess* this, s32 pawnNum, s32 param02, s32 param03, s32 param04))
        IsAcceptLightQuest = 152, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        IsReleaseWarpPoint = 153, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        IsSetPlayerSkill = 154, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        IsOrderMyQuest = 155, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        IsNotOrderMyQuest = 156, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        HasMypawn = 157, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        IsFavoriteWarpPoint = 158, // (cQuestProcess* this, s32 warpPointId, s32 param02, s32 param03, s32 param04))
        Craft = 159, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        IsKilledTargetEnemySetGroupGmMain = 160, // (cQuestProcess* this, s32 flagNo, s32 param02, s32 param03, s32 param04))
        IsKilledTargetEnemySetGroupGmSub = 161, // (cQuestProcess* this, s32 flagNo, s32 param02, s32 param03, s32 param04))
        HasUsedKey = 162, // (cQuestProcess* this, s32 stageNo, s32 groupNo, s32 setNo, s32 questId))
        IsCycleFlagOffPeriod = 163, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        IsEnemyFoundGmMain = 164, // (cQuestProcess* this, s32 stageNo, s32 groupNo, s32 setNo, s32 param04))
        IsEnemyFoundGmSub = 165, // (cQuestProcess* this, s32 stageNo, s32 groupNo, s32 setNo, s32 param04))
        IsLoginBugFixedOnly = 166, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        IsSearchClan = 167, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        IsOpenAreaListUi = 168, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        IsReleaseWarpPointAnyone = 169, // (cQuestProcess* this, s32 warpPointId, s32 param02, s32 param03, s32 param04))
        DevidePlayer = 170, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        NowPhase = 171, // (cQuestProcess* this, s32 phaseId, s32 param02, s32 param03, s32 param04))
        IsReleasePortal = 172, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        IsGetAppraiseItem = 173, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        IsSetPartnerPawn = 174, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        IsPresentPartnerPawn = 175, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        IsReleaseMyRoom = 176, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        IsExistDividePlayer = 177, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        NotDividePlayer = 178, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        IsGatherPartyInStage = 179, // (cQuestProcess* this, s32 stageNo, s32 param02, s32 param03, s32 param04))
        IsFinishedEnemyDivideAction = 180, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        IsOpenDoorOmQuestSetNoMarker = 181, // (cQuestProcess* this, s32 stageNo, s32 groupNo, s32 setNo, s32 questId))
        IsFinishedEventOrderNum = 182, // (cQuestProcess* this, s32 stageNo, s32 eventNo, s32 param03, s32 param04))
        IsPresentPartnerPawnNoMarker = 183, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        IsOmBrokenLayout = 184, // (cQuestProcess* this, s32 stageNo, s32 groupNo, s32 setNo, s32 param04))
        IsOmBrokenQuest = 185, // (cQuestProcess* this, s32 stageNo, s32 groupNo, s32 setNo, s32 param04))
        IsHoldingPeriodCycleContents = 186, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        IsNotHoldingPeriodCycleContents = 187, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        IsResetInstanceArea = 188, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        CheckMoonAge = 189, // (cQuestProcess* this, s32 moonAgeStart, s32 moonAgeEnd, s32 param03, s32 param04))
        IsOrderPawnQuest = 190, // (cQuestProcess* this, s32 orderGroupSerial, s32 noOrderGroupSerial, s32 param03, s32 param04))
        IsTakePictures = 191, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        IsStageForMainQuest = 192, // (cQuestProcess* this, s32 stageNo, s32 param02, s32 param03, s32 param04))
        IsReleasePawnExpedition = 193, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        OpenPpMode = 194, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        PpNotLess = 195, // (cQuestProcess* this, s32 point, s32 param02, s32 param03, s32 param04))
        OpenPpShop = 196, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        TouchClanBoard = 197, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        IsOneOffGather = 198, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        IsOmBrokenLayoutNoMarker = 199, // (cQuestProcess* this, s32 stageNo, s32 groupNo, s32 setNo, s32 param04))
        IsOmBrokenQuestNoMarker = 200, // (cQuestProcess* this, s32 stageNo, s32 groupNo, s32 setNo, s32 param04))
        KeyItemPointEq = 201, // (cQuestProcess* this, s32 idx, s32 num, s32 param03, s32 param04))
        IsEmotion = 202, // (cQuestProcess* this, s32 actNo, s32 param02, s32 param03, s32 param04))
        IsEquipColor = 203, // (cQuestProcess* this, s32 color, s32 param02, s32 param03, s32 param04))
        IsEquip = 204, // (cQuestProcess* this, s32 itemId, s32 param02, s32 param03, s32 param04))
        IsTakePicturesNpc = 205, // (cQuestProcess* this, s32 stageNo, s32 npcId01, s32 npcId02, s32 npcId03))
        SayMessage = 206, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        IsTakePicturesWithoutPawn = 207, // (cQuestProcess* this, s32 stageNo, s32 x, s32 y, s32 z))
        IsLinkageEnemyFlag = 208, // (cQuestProcess* this, s32 stageNo, s32 groupNo, s32 setNo, s32 flagNo))
        IsLinkageEnemyFlagOff = 209, // (cQuestProcess* this, s32 stageNo, s32 groupNo, s32 setNo, s32 flagNo))
        IsReleaseSecretRoom = 210, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))

        // The commands below were found through Ghidra analysis of the check function table at .data:02126998
        // Check dispatch function: ~.text:0063E04A  Table bounds check: commandId < 0x101 (257 entries, indices 0-256)
        // @note Parameter names are inferred from decompilation and may not match original source.

        /// <summary>
        /// Returns bit 18 of the substory state word at this→substory+0x20c (field offset 0x5c+0x20c from cQuestProcess base).
        /// </summary>
        IsSubstoryStateBit18 = 211, // 0x00635A00 (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04)

        /// <summary>
        /// Reads bit 17 (0x20000) of *(ctx+0x5c)+0x20c, inverts it, and stores the boolean result into the global
        /// side-effect slot at DAT_021c06b8+0x263. Does NOT return a check result - only writes global state.
        /// Takes no quest parameters; the function signature is effectively void(cQuestProcess* this).
        /// </summary>
        StoreLinkageEnemyFlagGlobal = 212, // 0x00635A30 (cQuestProcess* this, s32 param01_unused, s32 param02_unused, s32 param03_unused, s32 param04_unused)

        /// <summary>
        /// Searches the NPC object list at ctx+0x214/+0x218 for an entry matching param03 (lookup ID) and sets
        /// byte +0x11 on that object to 1. Also stores param04 at ctx+0x5c+0x24c. Returns bit 18 of ctx+0x5c+0x220.
        /// A combined setter+state-bit-check, not a pure conditional.
        /// </summary>
        NpcPreTalkAndOrderUi = 213, // 0x00635A60 (cQuestProcess* this, s32 stageNo, s32 npcId, s32 noOrderGroupSerial, s32 storeVal)

        /// <summary>
        /// Progresses when player chooses a specific dialogue option (int). Used in branching quests (like Extend Garden).
        /// NPC is not marked, so should be used alongside TalkNpc and QstTalkChg. param04 is unused.
        /// Validates stageNo == current stage, npcId == current NPC entity, NPC is in talk-active state,
        /// and NPC choice field +0x90 == choice.
        /// </summary>
        TalkNpcChoice = 214, // 0x00635B10 (cQuestProcess* this, s32 stageNo, s32 npcId, s32 choice, s32 param04_unused)

        /// <summary>
        /// Checks if a specific substory enemy's HP% >= hpRatePercent. Calls FUN_00be10d0(substoryId) to get HP ratio,
        /// multiplies by 100, compares >= param02. (0x00635BF0)
        /// </summary>
        SubstoryEnemyHpNotLess = 215, // 0x00635BF0 (cQuestProcess* this, s32 substoryId, s32 hpRatePercent, s32 param03, s32 param04)

        /// <summary>
        /// Checks if a specific substory enemy's HP% &lt; hpRatePercent. Inverse of SubstoryEnemyHpNotLess.
        /// </summary>
        SubstoryEnemyHpLess = 216, // 0x00635C40 (cQuestProcess* this, s32 substoryId, s32 hpRatePercent, s32 param03, s32 param04)

        /// <summary>
        /// Checks if the average HP% across ALL substory NPCs >= hpRatePercent. Calls FUN_00be1130 for the average.
        /// </summary>
        SubstoryAvgEnemyHpNotLess = 217, // 0x00635C90 (cQuestProcess* this, s32 param01, s32 hpRatePercent, s32 param03, s32 param04)

        /// <summary>
        /// Checks if the average HP% across ALL substory NPCs &lt; hpRatePercent. Inverse of SubstoryAvgEnemyHpNotLess.
        /// </summary>
        SubstoryAvgEnemyHpLess = 218, // 0x00635CD0 (cQuestProcess* this, s32 param01, s32 hpRatePercent, s32 param03, s32 param04)

        /// <summary>
        /// Checks if an OM's behavior state enum matches an expected value. Resolves OM via FUN_0063d480(stageNo, groupNo, setNo),
        /// reads state from +0x90 via FUN_00c0eab0, compares == behaviorState.
        /// </summary>
        IsOmBehaviorState = 219, // 0x00635D70 (cQuestProcess* this, s32 stageNo, s32 groupNo, s32 setNo, s32 behaviorState)

        /// <summary>
        /// Checks if a specific enemy group has spawned in a monster gathering spot.
        /// Reads SpotState from CDataAreaRankMonsterGatheringSpot and checks against param3.
        /// SpotState ranges from 1 to 4 and rotated every 18 hours in the original game, with SpotState = 3 spawning the Spot Boss.
        /// </summary>
        MonsterGatheringSpotState = 220, // 0x00635EF0 (cQuestProcess* this, s32 stageNo, s32 spotId, s32 spotState, s32 param04)

        /// <summary>
        /// Checks if a quest enemy group (type 3) is alive. Looks up via FUN_00a41780, checks status bit 15 via FUN_00bc2da0.
        /// </summary>
        OmEndAnimation = 221, // 0x00636000 (cQuestProcess* this, s32 stageNo, s32 groupNo, s32 setNo, s32 param04)

        /// <summary>
        /// Duplicate/variant of IsQuestEnemyAlive (221). Same decompilation, may differ in calling convention.
        /// </summary>
        OmEndAnimationNoMarker = 222, // 0x00636060 (cQuestProcess* this, s32 stageNo, s32 groupNo, s32 setNo, s32 param04)

        /// <summary>
        /// Checks if a player has interacted with a quest-spawned OM and its animation has played out completely.
        /// </summary>
        QuestOmEndAnimation = 223, // 0x006360B0 (cQuestProcess* this, s32 stageNo, s32 groupNo, s32 setNo, s32 param04)

        /// <summary>
        /// Variant of QuestOmEndAnimation (223) without quest markers.
        /// </summary>
        QuestOmEndAnimationNoMarker = 224, // 0x00636110 (cQuestProcess* this, s32 stageNo, s32 groupNo, s32 setNo, s32 param04)

        /// <summary>
        /// Identical function to DieEnemy in all counts; despite that, it is unable to place markers on enemies at all.
        /// </summary>
        DieEnemyNoMarker = 225, // 0x00636390 (cQuestProcess* this, s32 stageNo, s32 groupNo, s32 setNo, s32 param04)

        /// <summary>
        /// Displays a radius marker around an OM object and progresses when a player enters and interacts with it.
        /// Thunks to FUN_00638940 which calls FUN_006380c0; validates ctx+0x210 (touch event present),
        /// packed (groupNo&lt;&lt;16|stageNo) matches stored touch ID, setNo matches FUN_00be0670(),
        /// and bit 3 of ctx+0x208 is set (touch-enter flag).
        /// If matched, calls FUN_00a336e0(param04) to update the radius marker display.
        /// </summary>
        OmSetTouchRadius = 226, // 0x00636400 (cQuestProcess* this, s32 stageNo, s32 groupNo, s32 setNo, s32 param04)

        /// <summary>
        /// Displays a radius marker around an OM object and progresses when a player exits its radius.
        /// Thunks to FUN_00638ab0 which calls FUN_00638150; same structure as OmSetTouchRadius but checks
        /// bit 4 of ctx+0x208 (touch-exit flag) instead of bit 3. Complementary to OmSetTouchRadius - NOT identical.
        /// </summary>
        OmReleaseTouchRadius = 227, // 0x00636450 (cQuestProcess* this, s32 stageNo, s32 groupNo, s32 setNo, s32 param04)

        /// <summary>
        /// Checks if an NPC interaction with a specific choice has completed. Two code paths:
        /// (1) Cutscene path (FUN_0064c1f0 returns true): checks scene-change counter equality (ctx+0x4654 == ctx+0x45cc).
        /// (2) Normal path: calls FUN_0063d480(stageNo, groupNo, setNo, param04) which validates the local player
        /// entity, NPC talk state, and that an NPC interaction record matches stageNo/npcId/setNo.
        /// @note Use result 104 instead of QstTalkChg so the FSM is allowed to play out.
        /// @note Both work, but 104 works better.
        /// </summary>
        QuestTalkNpcRadius = 228, // 0x006364A0 (cQuestProcess* this, s32 stageNo, s32 groupNo, s32 setNo, s32 param04)

        /// <summary>
        /// Checks if the OM matching (stageNo, groupNo, setNo) is broken in the current phase.
        /// Gets phase context via FUN_009cff50/FUN_00bdf6c0, calls FUN_00be3160(groupNo, setNo),
        /// also checks bit 18 of this+0x5c+0x208.
        /// </summary>
        IsOmBrokenInCurrentPhase = 229, // 0x006364F0 (cQuestProcess* this, s32 stageNo, s32 groupNo, s32 setNo, s32 param04)

        /// <summary>
        /// Places a radius marker on an enemy group and progresses when the player discovers the enemy position.
        /// This works for marking both order and non-order enemies.
        /// Thunks to FUN_00636c20 which iterates area-trigger objects (DAT_021af4f4) matching stageNo+groupNo
        /// (mapped via FUN_00b19320). Delegates to FUN_00b2c2a0/FUN_00b2c910 which scan SceHit packet buffers.
        /// param03 = setNo filter (-1 = any set); param04 = marker display flag.
        /// Must be followed by a kill command to complete the quest block.
        /// @note Internal decompilation matched (stageNo, groupNo, setNo) params against DieEnemy/IsEnemyFound naming.
        /// </summary>
        IsEnemyFoundRadius = 230, // 0x00636560 (cQuestProcess* this, s32 stageNo, s32 groupNo, s32 setNo, s32 markerFlag)

        /// <summary>
        /// Lock-guarded, no-marker variant of IsEnemyFoundRadius. Acquires FUN_00bda180() spin lock;
        /// if already locked (FUN_009cfad0() != 0) returns false immediately, then releases via FUN_00bda1a0().
        /// Calls the same underlying FUN_00636c20 but always passes markerFlag=0.
        /// param03 = setNo filter (-1 = any set); param04 unused (markerFlag forced to 0 internally).
        /// </summary>
        IsEnemyFoundForOrderRadius = 231, // 0x00636610 (cQuestProcess* this, s32 stageNo, s32 groupNo, s32 setNo, s32 param04_unused)

        /// <summary>
        /// Checks if player has an achievement from a given category.
        /// Only seems to work for category 6 (Great Purpose).
        /// </summary>
        HasAchievement = 233, // 0x00636900 (cQuestProcess* this, s32 categoryNo, s32 achievementId, s32 param03_unused, s32 param04_unused)

        /// <summary>
        /// Returns bit 19 of the substory state word at this→substory+0x20c (offset 0x5c+0x20c from base).
        /// </summary>
        IsSubstoryStateBit19 = 234, // 0x00636980 (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04)

        /// <summary>
        /// Iterates 15 player equipment slots and checks against a seasonal item ID list (param01). Returns 1 if any match is found.
        /// 0 = Summer, 1 = Christmas. Lists are not comprehensive.
        /// </summary>
        IsEquipSeasonal = 235, // 0x006369F0 (cQuestProcess* this, s32 listNo, s32 param02, s32 param03, s32 param04)

        /// <summary>
        /// Checks if player has performed a limit break on an equipment (notice bit 20).
        /// </summary>
        DoLimitBreak = 236, // 0x00636B10 (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04)

        /// <summary>
        /// Checks if the player has performed extreme synthesis on an equipment (notice bit 21).
        /// </summary>
        DoExtremeSynthesis = 237, // 0x00636B70 (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04)

        /// <summary>
        /// Checks if player has obtained a High Orb from any source (notice bit 22). Quantity does not matter.
        /// </summary>
        GetHighOrb = 238, // 0x00636BA0 (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04)

        /// <summary>
        /// Checks if player has obtained a Dominion Point from any source (notice bit 23). Quantity does not matter.
        /// </summary>
        GetDominionPoint = 239, // 0x00636BD0 (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04)

        /// <summary>
        /// Directly jumps to IsReleaseWarpPointAnyone, but cannot place quest markers.
        /// </summary>
        IsReleaseWarpPointAnyoneNoMarker = 240, // 0x00636C10 (cQuestProcess* this, s32 waypointId, s32 param02, s32 param03, s32 param04)

        /// <summary>
        /// Checks if the substory quest's mission progress (via FUN_00597d20) is within [minValue, maxValue].
        /// Obtains substory mission ID (seqNo) from quest context, then scans S2C_QUEST_GET_PACKAGE_QUEST_LIST_RES for a match.
        /// Retrieves Unk1 (current progress) and Unk2 (maximum progress), divides them, multiplies the result by 100 and converts the result to int.
        /// Then, checks if progress is within [param01, param02].
        /// NOTE: The result command that activates the progress UI also reads from the same packet.
        /// </summary>
        IsSubstoryProgressInRange = 241, // 0x00636EF0 (cQuestProcess* this, s32 minValue, s32 maxValue, s32 param03, s32 param04)

        /// <summary>
        /// Kill-group completion check gated on content mode.
        /// Checks ctx+0x4654 == ctx+0x45cc (content mode counter), calls isSoloQuest (solo quest or tutorial guide check) and checks for leader only.
        /// If all checks pass, forward all parameters to IsKilledTargetEnemySetGroup.
        /// </summary>
        IsKilledTargetEnemySetGroup2 = 242, // 0x00636FE0 (cQuestProcess* this, s32 flagNo, s32 param02, s32 param03, s32 param04)

        /// <summary>
        /// Identical decompilation to IsKilledTargetEnemySet2 (242). The no-marker distinction is
        /// a runtime property of the this object (this+0x82 == 0 for marker, non-zero for no-marker),
        /// not structural code difference. See ID 242 for full description.
        /// </summary>
        IsKilledTargetEnemySetGroup2NoMarker = 243, // 0x006370B0 (cQuestProcess* this, s32 flagNo, s32 param02, s32 param03, s32 param04)

        /// <summary>
        /// Checks if a timer has elapsed past a certain amount of time.
        /// Requires sending a notify command with work01 = timerNo, work02 = sec and work03 = (int) unix timer start date + param02.
        /// Calls FUN_0064d170(timerNo) which retrieves current time (A) and compares against work03 (B).
        /// Returns 1 if A > B. param02 is unused by the quest command and its sole purpose is date calculation.
        /// </summary>
        IsTimerElapsed = 244, // 0x00637180 (cQuestProcess* this, s32 timerNo, s32 sec, s32 param03, s32 param04_unused)

        /// <summary>
        /// Checks if a timer has not elapsed past a certain amount of time.
        /// Requires sending a notify command with work01 = timerNo, work02 = sec and work03 = (int) unix timer start date + param02.
        /// Calls FUN_0064d170(timerNo) which retrieves current time (A) and compares against work03 (B).
        /// Returns 1 if A ≤ B. param02 is unused by the quest command and its sole purpose is date calculation.
        /// </summary>
        IsTimerNotElapsed = 245, // 0x00637250 (cQuestProcess* this, s32 timerNo, s32 sec, s32 param03, s32 param04_unused)

        /// <summary>
        /// S3-only: checks if the contents mode elapsed timer (FUN_00bc15d0) >= timeSec.
        /// Guards on season-phase pointer at DAT_0220456c+0x9f4.
        /// </summary>
        IsContentsModeTimerNotLess = 246, // 0x00637320 (cQuestProcess* this, s32 timeSec, s32 param02, s32 param03, s32 param04)

        /// <summary>
        /// Fire-once trigger: reads byte at DAT_021af4f4+0xEEA. If == 1, clears it and returns 1; otherwise returns 0.
        /// The flag is set by an external event and this command consumes it atomically.
        /// </summary>
        IsTriggerFlagSetAndClear = 247, // 0x006373F0 (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04)

        /// <summary>
        /// Direct thunk to FUN_0063a5e0 with no content-mode guard (unlike IDs 242/243).
        /// Iterates the kill-group list on this, finds entry matching flagNo by entry+0x14,
        /// checks entry+0x18 (valid) and entry+4 == 0 (kill complete). Only two parameters are used (this, flagNo).
        /// @note Previous description "shape type 4 radius" was incorrect - no shape-type filtering in this function.
        /// The "radius" framing comes from the kill zone's visual representation, not the code.
        /// </summary>
        IsWildHuntEnemyKilled = 248, // 0x00637430 (cQuestProcess* this, s32 flagNo, s32 param02_unused, s32 param03_unused, s32 param04_unused)

        /// <summary>
        /// Checks if a player has reached a chain number from a Chain Dungeon (Extreme Mission).
        /// Reads Unk2 sent by packet S2CSituationDataUpdateObjectivesNtc and checks against param1.
        /// </summary>
        ChainNotLess = 250, // (cQuestProcess* this, s32 chainNo, s32 param02_unused, s32 param03_unused, s32 param04_unused)

        /// <summary>
        /// Checks if a contents timer (Timer List A) has reached state zero. Content-mode gated (same guard as 242/243).
        /// Checks ctx+0x4654 == ctx+0x45cc (content mode counter), calls isSoloQuest (solo quest or tutorial guide check) and checks for leader only.
        /// If all checks pass, the command behaves identically to IsEndTimer.
        /// @note Several S3 commands are turning out to be old commands with these additional checks; what are their use cases?
        /// </summary>
        IsEndTimer2 = 251, // 0x006375E0 (cQuestProcess* this, s32 timerNo, s32 param02_unused, s32 param03_unused, s32 param04_unused)

        /// <summary>
        /// Checks if a player has entered a proximity zone associated with a wild hunt target set.
        /// Iterates the Wild Hunt check-command object's zone-entry list (count at this[2], array at this[5]).
        /// For each entry where entry+0x14 == param01 (zone/linkage ID), walks sub-entries and calls
        /// FUN_00636c20(em_id_field1, em_id_field2, -1, param04) - a generic OM enemy killed-state checker.
        /// Kill state is confirmed via vtable+0x2ec returning 0xc, or via FUN_00c82ac0 (alternate path).
        /// @note "MobHunt" is the internal engine name for the Wild Hunt system (cMobHuntQuestManager).
        /// @note param02 and param03 are unused. param01 = zone/linkage ID filter, param04 = markerFlag passed to FUN_00636c20.
        /// </summary>
        IsWildHuntEnemyFound = 252, // 0x006376E0 (cQuestProcess* this, s32 flagNo, s32 param02_unused, s32 param03_unused, s32 markerFlag)

        /// <summary>
        /// Checks if player has accepted and cleared a wild hunt quest. Calls FUN_00bdee50(0xc) to get area context (mode 0xc),
        /// reads byte at +0x3b; returns true if non-zero.
        /// </summary>
        IsWildHuntClear = 253, // 0x00637820 (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04)

        Padding254 = 254, // 0x00637860 stub/nop - always returns 0

        /// <summary>
        /// Checks if a quest layout's HP-lost percentage &lt;= hpLostPct. Looks up layout (type 3) via FUN_00a41780,
        /// computes HP lost% via FUN_00bc0ab0 (= 100 - current/max*100), returns true if 0 &lt;= lost% &lt;= hpLostPct.
        /// Effectively: layout HP >= threshold.
        /// </summary>
        IsQuestLayoutHpNotGreater = 255, // 0x00637890 (cQuestProcess* this, s32 stageNo, s32 groupNo, s32 setNo, s32 hpLostPct)

        /// <summary>
        /// Checks if a player has cleared a specific Extreme Mission/Grand Mission/Chain Dungeon (category 9 quests).
        /// Guards on content-mode equality: DAT_0220456c+0x9f4+0x4654 == +0x45cc (current-mode match required;
        /// fails during transitions or wrong content phase).
        /// FUN_00a336e0(param01) constructs a typed wrapper object using the Grand Mission vtable
        /// (PTR_FUN_01afa2d0, same vtable as "Grand Mission:Raid Boss" / "Grand Mission:Fort Defense").
        /// FUN_00be1ce0 → FUN_00be1f70(9) walks the type-9 flag list at Grand Mission manager +0x90
        /// and returns 1 if any entry's field (+4) matches the wrapper's key.
        /// @note This is a Grand Mission / Extreme Mission system check, not a general world-quest flag.
        /// </summary>
        IsExtremeMissionClear = 256, // 0x00637950 (cQuestProcess* this, s32 questId, s32 param02_unused, s32 param03_unused, s32 param04_unused)
    }
}
