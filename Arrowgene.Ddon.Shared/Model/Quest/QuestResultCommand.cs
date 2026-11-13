using Arrowgene.Ddon.Shared.Entity.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Model.Quest
{
    public enum QuestResultCommand : ushort
    {
        LotOn = 1, // (cQuestProcess* this, s32 stageNo, s32 lotNo, s32 param03, s32 param04))
        LotOff = 2, // (cQuestProcess* this, s32 stageNo, s32 lotNo, s32 param03, s32 param04))
        HandItem = 3, // (cQuestProcess* this, s32 itemId, s32 itemNum, s32 param03, s32 param04))
        SetAnnounce = 4, // (cQuestProcess* this, s32 announceType, s32 param02, s32 param03, s32 param04))
        UpdateAnnounce = 5, // (cQuestProcess* this, s32 type, s32 param02, s32 param03, s32 param04))
        ChangeMessage = 6, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        QstFlagOn = 7, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        MyQstFlagOn = 8, // (cQuestProcess* this, s32 flagNo, s32 param02, s32 param03, s32 param04))
        GlobalFlagOn = 9, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        QstTalkChg = 10, // (cQuestProcess* this, s32 npcId, s32 msgNo, s32 param03, s32 param04))
        QstTalkDel = 11, // (cQuestProcess* this, s32 npcId, s32 param02, s32 param03, s32 param04))
        StageJump = 12, // (cQuestProcess* this, s32 stageNo, s32 startPos, s32 param03, s32 param04))
        EventExec = 13, // (cQuestProcess* this, s32 stageNo, s32 eventNo, s32 jumpStageNo, s32 jumpStartPosNo))
        CallMessage = 14, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        Prt = 15, // (cQuestProcess* this, s32 stageNo, s32 x, s32 y, s32 z))
        QstLayoutFlagOn = 16, // (cQuestProcess* this, s32 flagNo, s32 param02, s32 param03, s32 param04))
        QstLayoutFlagOff = 17, // (cQuestProcess* this, s32 flagNo, s32 param02, s32 param03, s32 param04))
        QstSceFlagOn = 18, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        QstDogmaOrb = 19, // (cQuestProcess* this, s32 orbNum, s32 param02, s32 param03, s32 param04))
        GotoMainPwanEdit = 20, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        AddFsmNpcList = 21, // (cQuestProcess* this, s32 npcId, s32 param02, s32 param03, s32 param04))
        EndCycle = 22, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        AddCycleTimer = 23, // (cQuestProcess* this, s32 sec, s32 param02, s32 param03, s32 param04))
        AddMarkerAtItem = 24, // (cQuestProcess* this, s32 stageNo, s32 x, s32 y, s32 z))
        AddMarkerAtDest = 25, // (cQuestProcess* this, s32 stageNo, s32 x, s32 y, s32 z))
        AddResultPoint = 26, // (cQuestProcess* this, s32 tableIndex, s32 param02, s32 param03, s32 param04))
        PushImteToPlBag = 27, // (cQuestProcess* this, s32 itemId, s32 itemNum, s32 param03, s32 param04))
        StartTimer = 28, // (cQuestProcess* this, s32 timerNo, s32 sec, s32 param03, s32 param04))
        SetRandom = 29, // 0x00639290 (cQuestProcess* this, s32 randomNo, s32 minValue, s32 maxValue, s32 resultValue)
        ResetRandom = 30, // 0x006393B0 (cQuestProcess* this, s32 randomNo, s32 param02_unused, s32 param03_unused, s32 param04_unused)
        BgmRequest = 31, // (cQuestProcess* this, s32 type, s32 bgmId, s32 param03, s32 param04))
        BgmStop = 32, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        SetWaypoint = 33, // (cQuestProcess* this, s32 npcId, s32 waypointNo0, s32 waypointNo1, s32 waypointNo2))
        ForceTalkQuest = 34, // (cQuestProcess* this, s32 npcId, s32 groupSerial, s32 param03, s32 param04))
        TutorialDialog = 35, // (cQuestProcess* this, s32 guideNo, s32 param02, s32 param03, s32 param04))
        AddKeyItemPoint = 36, // (cQuestProcess* this, s32 keyItemIdx, s32 pointNum, s32 param03, s32 param04))
        DontSaveProcess = 37, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        InterruptCycleContents = 38, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        QuestEvaluationPoint = 39, // (cQuestProcess* this, s32 point, s32 param02, s32 param03, s32 param04))
        CheckOrderCondition = 40, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        WorldManageLayoutFlagOn = 41, // (cQuestProcess* this, s32 flagNo, s32 questId, s32 param03, s32 param04))
        WorldManageLayoutFlagOff = 42, // (cQuestProcess* this, s32 flagNo, s32 questId, s32 param03, s32 param04))
        PlayEndingForFirstSeason = 43, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        AddCyclePurpose = 44, // (cQuestProcess* this, s32 announceNo, s32 type, s32 param03, s32 param04))
        RemoveCyclePurpose = 45, // (cQuestProcess* this, s32 announceNo, s32 param02, s32 param03, s32 param04))
        UpdateAnnounceDirect = 46, // (cQuestProcess* this, s32 announceNo, s32 type, s32 param03, s32 param04))
        SetCheckPoint = 47, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        ReturnCheckPoint = 48, // (cQuestProcess* this, s32 processNo, s32 param02, s32 param03, s32 param04))
        CallGeneralAnnounce = 49, // (cQuestProcess* this, s32 type, s32 msgNo, s32 param03, s32 param04))
        TutorialEnemyInvincibleOff = 50, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        SetDiePlayerReturnPos = 51, // (cQuestProcess* this, s32 stageNo, s32 startPos, s32 outSceNo, s32 param04))
        WorldManageQuestFlagOn = 52, // (cQuestProcess* this, s32 flagNo, s32 questId, s32 param03, s32 param04))
        WorldManageQuestFlagOff = 53, // (cQuestProcess* this, s32 flagNo, s32 questId, s32 param03, s32 param04))
        ReturnCheckPointEx = 54, // (cQuestProcess* this, s32 processNo, s32 param02, s32 param03, s32 param04))
        ResetCheckPoint = 55, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        ResetDiePlayerReturnPos = 56, // (cQuestProcess* this, s32 stageNo, s32 startPos, s32 param03, s32 param04))
        SetBarricade = 57, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        ResetBarricade = 58, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        TutorialEnemyInvincibleOn = 59, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        ResetTutorialFlag = 60, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        StartContentsTimer = 61, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        MyQstFlagOff = 62, // (cQuestProcess* this, s32 flagNo, s32 param02, s32 param03, s32 param04))
        PlayCameraEvent = 63, // (cQuestProcess* this, s32 stageNo, s32 eventNo, s32 param03, s32 param04))
        EndEndQuest = 64, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        ReturnAnnounce = 65, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        AddEndContentsPurpose = 66, // (cQuestProcess* this, s32 announceNo, s32 type, s32 param03, s32 param04))
        RemoveEndContentsPurpose = 67, // (cQuestProcess* this, s32 announceNo, s32 param02, s32 param03, s32 param04))
        StopCycleTimer = 68, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        RestartCycleTimer = 69, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        AddAreaPoint = 70, // (cQuestProcess* this, s32 AreaId, s32 AddPoint, s32 param03, s32 param04))
        LayoutFlagRandomOn = 71, // (cQuestProcess* this, s32 FlanNo1, s32 FlanNo2, s32 FlanNo3, s32 ResultNo))
        SetDeliverInfo = 72, // (cQuestProcess* this, s32 stageNo, s32 npcId, s32 groupSerial, s32 param04))
        SetDeliverInfoQuest = 73, // (cQuestProcess* this, s32 stageNo, s32 groupNo, s32 setNo, s32 groupSerial))
        BgmRequestFix = 74, // (cQuestProcess* this, s32 type, s32 bgmId, s32 param03, s32 param04))
        EventExecCont = 75, // (cQuestProcess* this, s32 stageNo, s32 eventNo, s32 jumpStageNo, s32 jumpStartPosNo))
        PlPadOff = 76, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        PlPadOn = 77, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        EnableGetSetQuestList = 78, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        StartMissionAnnounce = 79, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        StageAnnounce = 80, // (cQuestProcess* this, s32 type, s32 num, s32 param03, s32 param04))
        ReleaseAnnounce = 81, // (cQuestProcess* this, s32 id, s32 param02, s32 param03, s32 param04))
        ButtonGuideFlagOn = 82, // (cQuestProcess* this, s32 buttonGuideNo, s32 param02, s32 param03, s32 param04))
        ButtonGuideFlagOff = 83, // (cQuestProcess* this, s32 buttonGuideNo, s32 param02, s32 param03, s32 param04))
        AreaJumpFadeContinue = 84, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        ExeEventAfterStageJump = 85, // (cQuestProcess* this, s32 stageNo, s32 eventNo, s32 startPos, s32 param04))
        ExeEventAfterStageJumpContinue = 86, // (cQuestProcess* this, s32 stageNo, s32 eventNo, s32 startPos, s32 param04))
        PlayMessage = 87, // (cQuestProcess* this, s32 groupNo, s32 waitTime, s32 param03, s32 param04))
        StopMessage = 88, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        DecideDivideArea = 89, // (cQuestProcess* this, s32 stageNo, s32 startPosNo, s32 param03, s32 param04))
        ShiftPhase = 90, // (cQuestProcess* this, s32 phaseId, s32 param02, s32 param03, s32 param04))
        ReleaseMyRoom = 91, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        DivideSuccess = 92, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        DivideFailed = 93, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        SetProgressBonus = 94, // (cQuestProcess* this, s32 rewardRank, s32 param02, s32 param03, s32 param04))
        RefreshOmKeyDisp = 95, // (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04))
        SwitchPawnQuestTalk = 96, // (cQuestProcess* this, s32 type, s32 param02, s32 param03, s32 param04))
        LinkageEnemyFlagOn = 97, // (cQuestProcess* this, s32 stageNo, s32 groupNo, s32 setNo, s32 flagId))
        LinkageEnemyFlagOff = 98, // (cQuestProcess* this, s32 stageNo, s32 groupNo, s32 setNo, s32 flagId))

        // The commands below were found through Ghidra analysis of the result function table at .data:02126770
        // Result dispatch function: .text:0063E254  Table bounds check: commandId < 0x87 (135 entries, indices 0-134)
        // @note Parameter names are inferred from decompilation and may not match original source.
        /// <summary>
        /// Adds a signed delta to a substory progress value, clamped to [0, max].
        /// The substory category key and objective key are read from stored quest-state fields at
        /// ctx+0x5c+0x244 and ctx+0x5c+0x248 - they are baked into the quest context, not passed as params.
        /// Only param01 (delta) is used: positive advances progress, negative regresses it, zero is a no-op.
        /// Sends packet 0x117 (increase) or 0x118 (decrease) to update the UI, then FUN_00bd3280 to
        /// report old and new progress ratios to the client.
        /// Possibly made obsolete by UpdateSubstoryProgress.
        /// </summary>
        SubstoryProgress = 99, // 0x00633730 (cQuestProcess* this, s32 delta, s32 param02_unused, s32 param03_unused, s32 param04_unused)

        /// <summary>
        /// Finds a substory entry by substoryId and adds progressDelta to its progress value, clamped to [0,100].
        /// Does not seem to work?
        /// </summary>
        AddSubstoryProgress = 100, // 0x006338E0 (cQuestProcess* this, s32 substoryId, s32 progressDelta, s32 param03, s32 param04)

        /// <summary>
        /// Triggers a substory progress update sequence. Checks mode via FUN_009cffc0; if mode==0xb fires FUN_00bdee50 and FUN_00598590.
        /// Sends packet C2S_QUEST_ADD_PACKAGE_QUEST_POINT_REQ with schedule ID and progress value, then plays an UI animation updating mission progress.
        /// </summary>
        UpdateSubstoryProgress = 101, // 0x00633920 (cQuestProcess* this, s32 progressDelta, s32 param02, s32 param03, s32 param04)

        /// <summary>
        /// Triggers display of the substory UI element by writing a value from quest state into the substory
        /// object's display field (+0x44), then signalling a state-machine transition via FUN_00a34da0.
        /// The value written comes from ctx+0x5c+0x228 (a stored quest-state field), not from command params.
        /// The UI element does appear as a downstream effect of the state-machine transition.
        /// No command parameters (param01-param04) are used; the function takes only 'this'.
        /// Chain: FUN_009cff50 (read ctx+0x228) → FUN_00bdee50(0xb) (get substory obj) →
        ///        FUN_005986d0 (write +0x44) → FUN_00a34da0 (reset state-machine dispatch ptr).
        /// </summary>
        EnableSubstoryGauge = 102, // 0x006339B0 (cQuestProcess* this, s32 param01_unused, s32 param02_unused, s32 param03_unused, s32 param04_unused)

        /// <summary>
        /// Disables the substory UI element. Calls FUN_00bdee50(0xb) then FUN_00598670 which clears the +0x44 reference.
        /// </summary>
        DisableSubstoryGauge = 103, // 0x00633A00 (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04)

        /// <summary>
        /// Redirects NPC talk for a substory context. Calls FUN_009cff50 then FUN_009ce930(param01, param02).
        /// </summary>
        QstTalkChgFsm = 104, // 0x00633A30 (cQuestProcess* this, s32 npcId, s32 msgNo, s32 param03, s32 param04)

        /// <summary>
        /// Sets (type = 1) or clears (type = 0) a global enemy aggression flag.
        /// When set, enemies within spawn range endlessly converge on the player's position.
        /// Also calls FUN_00be9b60 with 0 or 1 depending on param02. Unclear what this is supposed to do.
        /// </summary>
        SetEnemyAggroFlag = 105, // 0x00633A80 (cQuestProcess* this, s32 type, s32 param02, s32 param03, s32 param04)

        Padding106 = 106, // 0x00633B00 stub/nop - always returns 0

        /// <summary>
        /// Adds an NPC to the FSM talk NPC list at this+0x94/0xa0. Validates FSM mode via FUN_009d07f0 first.
        /// Uses param01 as npcId/groupId.
        /// </summary>
        SetRandom2 = 107, // 0x00633B30 (cQuestProcess* this, s32 randomNo, s32 minValue, s32 maxValue, s32 resultValue)

        /// <summary>
        /// Looks up a table for Great Purpose (category 6) entries (1-16).
        /// Forwards parameters to an announce display function if it finds a match.
        /// </summary>
        CallGreatPurpose = 108, // 0x00633B80 (cQuestProcess* this, s32 categoryNo, s32 purposeNo, s32 param03, s32 param04)

        /// <summary>
        /// Enables dialogue text boxes for substory cutscenes. Gets area context, calls FUN_00bdee50(0xb) then FUN_00598860 to set +0x4c.
        /// </summary>
        EnableSubstoryTextBox = 109, // 0x00633BB0 (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04)

        /// <summary>
        /// Disables dialogue text boxes for substory cutscenes. Calls FUN_00bdee50(0xb) then FUN_005986A0 which clears +0x4c.
        /// </summary>
        DisableSubstoryTextBox = 110, // 0x00633BF0 (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04)

        /// <summary>
        /// Overrides lighting and clouds on current map. Effects are purely visual and not persistent.
        /// param01 = Time of day (0-23), param02 = clouds (0 = unchanged, 1 = fair clouds, 2 = heavy clouds, there may be more?)
        /// </summary>
        SetCustomWeather = 111, // 0x00633CE0 (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04)

        /// <summary>
        /// Resets weather effects set by SetCustomWeather.
        /// </summary>
        ResetCustomWeather = 112, // 0x00633D20 (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04)

        /// <summary>
        /// Schedules an FSM NPC behavior by calling FUN_009d1a60(param04). param04 is read from stack at +0x10.
        /// </summary>
        LayoutFlagRandomOn2 = 113, // 0x00633D50 (cQuestProcess* this, s32 flagNo1, s32 flagNo2, s32 flagNo3, s32 resultNo)

        /// <summary>
        /// Increases a breakable object's hit counter, lowering its current HP. Breaks instantly if count >= BreakHitNum (each num = 10 player hits).
        /// Looks up by (stageNo, groupNo, setNo) via FUN_00a41780, then calls FUN_00bc0670 (layout unique ID, count).
        /// Only works for OMs 503136 and 503143 (War Mission flag objects), does nothing otherwise.
        /// </summary>
        SetOmHitCount = 114, // 0x00633D80 (cQuestProcess* this, s32 stageNo, s32 groupNo, s32 setNo, s32 count)

        /// <summary>
        /// Quest object variant of SetOmHitCount.
        /// Only works for OMs 503136 and 503143 (War Mission flag objects), does nothing otherwise.
        /// </summary>
        SetQuestOmHitCount = 115, // 0x00633E30 (cQuestProcess* this, s32 stageNo, s32 groupNo, s32 setNo, s32 count)

        /// <summary>
        /// Sets the danger tier of a breakable object (bits 23-21) via FUN_00bc0720 and restores its health to full.
        /// What "tier" does is still up in the air. May need WM quest context for full functionality.
        /// Only works for OMs 503136 and 503143 (War Mission flag objects), does nothing otherwise.
        /// </summary>
        SetOmTierUp = 116, // 0x00633F30 (cQuestProcess* this, s32 stageNo, s32 groupNo, s32 setNo, s32 tier)

        /// <summary>
        /// Variant of SetOmTierUp for quest-spawned objects.
        /// Only works for OMs 503136 and 503143 (War Mission flag objects), does nothing otherwise.
        /// </summary>
        SetQuestOmTierUp = 117, // 0x00633FC0 (cQuestProcess* this, s32 stageNo, s32 groupNo, s32 setNo, s32 tier)

        /// <summary>
        /// Sets a body/stance pose (1-6) on an NPC/enemy via FUN_00bbf670. Looks up by (stageNo, groupNo, setNo).
        /// </summary>
        SetOmState = 118, // 0x006341A0 (cQuestProcess* this, s32 stageNo, s32 groupNo, s32 setNo, s32 state)

        /// <summary>
        /// Quest object variant of SetOmState.
        /// </summary>
        SetQuestOmState = 119, // 0x006341F0 (cQuestProcess* this, s32 stageNo, s32 groupNo, s32 setNo, s32 state)

        Padding120 = 120, // 0x006342D0 stub/nop - always returns 0

        /// <summary>
        /// Sets the named param of an enemy by queuing it into a critical-section-guarded buffer via FUN_00b55e70.
        /// GM-mode guarded. Buffer holds up to 10 entries at this+0xe48. One enemy per command (setNo > -1).
        /// </summary>
        SetNamedEnemyParam = 121, // 0x00634300 (cQuestProcess* this, s32 stageNo, s32 groupNo, s32 setNo, s32 param)

        Padding122 = 122, // 0x00634390 stub/nop - always returns 0
        Padding123 = 123, // 0x006343C0 stub/nop - always returns 0

        /// <summary>
        /// Removes an FSM NPC entry from the process list at this+0x28/0x1c, freeing its children. Calls FUN_0063dda0(param01).
        /// </summary>
        RemoveFsmNpcFromSchedule = 124, // 0x006343F0 (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04)

        Padding125 = 125, // 0x00634410 stub/nop - always returns 0

        /// <summary>
        /// Changes magma OM behaviour on Evil Dragon's Roost maps to match boss phase mechanics.
        /// Phase 2: Sets a global counter to 10, which immediately flips over to 1 (reset). Floor magma expands or contracts based on counter value.
        /// Evil Dragon and their crystals directly increment or decrement this counter as part of their moveset.
        /// Phase 3: Iterates a list of objects and sets state = param02 on them. Makes magma rise on the map based on state.
        /// Requires the list (DAT) to be populated by a function beforehand (Evil Dragon populates this DAT with SetInfoOmRisingMagma).
        /// </summary>
        SetMagmaState = 126, // 0x00634450 (cQuestProcess* this, s32 phase, s32 state, s32 param03, s32 param04)

        Padding127 = 127, // 0x006344B0 stub/nop - always returns 0

        /// <summary>
        /// Fires a chain dungeon ending sequence. Calls FUN_00be9960, FUN_00b85670, then sends messages 0x25f and 0x260
        /// to the world manager NPC via FUN_00b82b00. Also changes objectives on the chain dungeon UI.
        /// </summary>
        TriggerDarkDungeonEndSequence = 128, // 0x006345D0 (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04)

        Padding129 = 129, // 0x00634690 stub/nop - always returns 0

        /// <summary>
        /// Sends packet 63.5.16 (C2S_CHAIN_DUNGEON_END_CHAIN_NTC) kickstarting the rewards phase of chain dungeons.
        /// In the dumps, packet was responded by 63.2.16 (situation data update) and 63.6.16 (spawns chests based on progress tracked by 63.2.16).
        /// Should have conditions: OM state == 4 and a specific animation condition via FUN_0087dc50, but always sends the packet regardless?
        /// </summary>
        EndChain = 130, // 0x006346B0 (cQuestProcess* this, s32 param01, s32 param02, s32 param03, s32 param04)

        Padding131 = 131, // 0x00634720 stub/nop - always returns 0
        Padding132 = 132, // 0x00634750 stub/nop - always returns 0

        /// <summary>
        /// Activates a player's bonus dragon abilities if they meet the threshold. Gated by type.
        /// Type 1 = Dragon Protection, Type 2 = Dragon Blessing. Plays GUI message.
        /// Dragon protection seems functional and actually raises stats.
        /// </summary>
        GetDragonAbility = 133, // 0x006347A0 (cQuestProcess* this, s32 type, s32 param02, s32 param03, s32 param04)

        /// <summary>
        /// Sets a body/pose mode on a layout enemy (type 2). Looks up entity via FUN_00a41780, checks OM alive,
        /// then calls FUN_005be380(poseId) to set the stance mode.
        /// </summary>
        SetQuestLayoutEnemyBodyPose = 134, // 0x00634820 (cQuestProcess* this, s32 stageNo, s32 groupNo, s32 setNo, s32 poseId)
    }

    public static class QuestResultCommandExtension
    {
        public static readonly Dictionary<QuestResultCommand, (QuestFlagType Type, QuestFlagAction Action)> gFlagStateChangeCommands = new()
        {
            [QuestResultCommand.QstLayoutFlagOn] = (QuestFlagType.QstLayout, QuestFlagAction.Set),
            [QuestResultCommand.QstLayoutFlagOff] = (QuestFlagType.QstLayout, QuestFlagAction.Clear),
            [QuestResultCommand.WorldManageLayoutFlagOn] = (QuestFlagType.WorldManageLayout, QuestFlagAction.Set),
            [QuestResultCommand.WorldManageLayoutFlagOff] = (QuestFlagType.WorldManageLayout, QuestFlagAction.Clear),
            [QuestResultCommand.WorldManageQuestFlagOn] = (QuestFlagType.WorldManageQuest, QuestFlagAction.Set),
            [QuestResultCommand.WorldManageQuestFlagOff] = (QuestFlagType.WorldManageQuest, QuestFlagAction.Clear),
            [QuestResultCommand.MyQstFlagOn] = (QuestFlagType.MyQst, QuestFlagAction.Set),
            [QuestResultCommand.MyQstFlagOff] = (QuestFlagType.MyQst, QuestFlagAction.Clear),
            [QuestResultCommand.LotOn] = (QuestFlagType.Lot, QuestFlagAction.Set),
            [QuestResultCommand.LotOff] = (QuestFlagType.Lot, QuestFlagAction.Clear),
        };

        public static bool IsFlagStateChangeCommand(this QuestResultCommand commandId)
        {
            return gFlagStateChangeCommands.ContainsKey(commandId);
        }

        public static (QuestFlagType Type, QuestFlagAction Action) GetFlagStateChangeProperties(this QuestResultCommand commandId)
        {
            if (!gFlagStateChangeCommands.ContainsKey(commandId))
            {
                return (QuestFlagType.None, QuestFlagAction.None);
            }
            return gFlagStateChangeCommands[commandId];
        }

        public static QuestFlag ToQuestFlag(CDataQuestCommand questCommand)
        {
            QuestResultCommand cmd = (QuestResultCommand) questCommand.Command;
            if (!cmd.IsFlagStateChangeCommand())
            {
                return null;
            }

            var props = cmd.GetFlagStateChangeProperties();
            var questFlag = new QuestFlag()
            {
                Type = props.Type,
                Action = props.Action,
                Value = questCommand.Param01,
                PreventReplay = false,
            };

            switch (props.Type)
            {
                case QuestFlagType.WorldManageLayout:
                case QuestFlagType.WorldManageQuest:
                    questFlag.QuestId = questCommand.Param02;
                    break;
                case QuestFlagType.Lot:
                    questFlag.stageInfo = Stage.StageInfoFromStageNo((uint)questCommand.Param01);
                    questFlag.Value = questCommand.Param02;
                    break;
            }

            return questFlag;
        }
    }
}
