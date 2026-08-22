using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Quests.Extensions
{
    public static class QuestBlockCheckCmdExtension
    {
        private static void ValidateIndexAndUpdateCommandList(List<List<CDataQuestCommand>> checkCommands, int index)
        {
            if (index >= checkCommands.Count)
            {
                for (int i = checkCommands.Count; i < index + 1; i++)
                {
                    checkCommands.Add(new List<CDataQuestCommand>());
                }
            }
        }

        public static QuestBlock AddCheckCmdDieEnemy(this QuestBlock questBlock, StageInfo stageInfo, uint groupId, int setNo, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdDieEnemy(stageInfo, groupId, setNo);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdDiePlayer(this QuestBlock questBlock, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdDiePlayer();
            return questBlock;
        }

        public static QuestBlock AddCheckCmdIsDownEnemy(this QuestBlock questBlock, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdIsDownEnemy();
            return questBlock;
        }

        public static QuestBlock AddCheckCmdEmHpLess(this QuestBlock questBlock, StageInfo stageInfo, uint groupId, uint setNo, uint hpRate, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdEmHpLess(stageInfo, groupId, setNo, hpRate);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdEmHpNotLess(this QuestBlock questBlock, StageInfo stageInfo, uint groupId, uint setNo, uint hpRate, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdEmHpNotLess(stageInfo, groupId, setNo, hpRate);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdNpcHpLess(this QuestBlock questBlock, StageInfo stageInfo, uint groupId, uint setNo, uint hpRate, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdNpcHpLess(stageInfo, groupId, setNo, hpRate);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdNpcHpNotLess(this QuestBlock questBlock, StageInfo stageInfo, uint groupId, uint setNo, uint hpRate, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdNpcHpNotLess(stageInfo, groupId, setNo, hpRate);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdPlHp(this QuestBlock questBlock, uint hpRate, PlayerHpTypeCheck checkType, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdPlHp(hpRate, checkType);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdIsEnemyFound(this QuestBlock questBlock, StageInfo stageInfo, uint groupId, int setNo, bool showMarker = true, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdIsEnemyFound(stageInfo, groupId, setNo, showMarker);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdIsEnemyFoundForOrder(this QuestBlock questBlock, StageInfo stageInfo, uint groupId, int setNo, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdIsEnemyFoundForOrder(stageInfo, groupId, setNo);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdIsEnemyFoundGmMain(this QuestBlock questBlock, StageInfo stageInfo, uint groupId, int setNo, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdIsEnemyFoundGmMain(stageInfo, groupId, setNo);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdIsEnemyFoundGmSub(this QuestBlock questBlock, StageInfo stageInfo, uint groupId, int setNo, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdIsEnemyFoundGmMain(stageInfo, groupId, setNo);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdIsKilledTargetEnemySetGroup(this QuestBlock questBlock, uint flagNo, bool showMarker = true, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdIsKilledTargetEnemySetGroup(flagNo, showMarker);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdIsKilledTargetEnemySetGroupGmMain(this QuestBlock questBlock, uint flagNo, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdIsKilledTargetEnemySetGroupGmMain(flagNo);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdIsKilledTargetEnemySetGroupGmSub(this QuestBlock questBlock, uint flagNo, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdIsKilledTargetEnemySetGroupGmSub(flagNo);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdEmDieForRandomDungeon(this QuestBlock questBlock, StageInfo stageInfo, EnemyId enemyId, uint amount, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdEmDieForRandomDungeon(stageInfo, enemyId, amount);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdOmSetTouch(this QuestBlock questBlock, StageInfo stageInfo, uint groupId, uint setNo, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdOmSetTouch(stageInfo, groupId, setNo);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdOmReleaseTouch(this QuestBlock questBlock, StageInfo stageInfo, uint groupId, uint setNo, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdOmReleaseTouch(stageInfo, groupId, setNo);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdOmEndText(this QuestBlock questBlock, StageInfo stageInfo, uint groupId, uint setNo, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdOmEndText(stageInfo, groupId, setNo);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdQuestOmEndText(this QuestBlock questBlock, StageInfo stageInfo, uint groupId, uint setNo, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdQuestOmEndText(stageInfo, groupId, setNo);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdIsTouchPawnDungeonOm(this QuestBlock questBlock, StageInfo stageInfo, uint groupId, uint setNo, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdIsTouchPawnDungeonOm(stageInfo, groupId, setNo);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdQuestOmSetTouch(this QuestBlock questBlock, StageInfo stageInfo, uint groupId, uint setNo, bool showMarker = true, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdQuestOmSetTouch(stageInfo, groupId, setNo, showMarker);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdQuestOmReleaseTouch(this QuestBlock questBlock, StageInfo stageInfo, uint groupId, uint setNo, bool showMarker = true, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdQuestOmReleaseTouch(stageInfo, groupId, setNo, showMarker);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdIsOmBrokenLayout(this QuestBlock questBlock, StageInfo stageInfo, uint groupId, uint setNo, bool showMarker = true, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdIsOmBrokenLayout(stageInfo, groupId, setNo, showMarker);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdIsOmBrokenQuest(this QuestBlock questBlock, StageInfo stageInfo, uint groupId, uint setNo, bool showMarker = true, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdIsOmBrokenQuest(stageInfo, groupId, setNo, showMarker);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdTouchQuestNpcUnitMarker(this QuestBlock questBlock, StageInfo stageInfo, uint groupId, uint setNo, QuestId questId, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdTouchQuestNpcUnitMarker(stageInfo, groupId, setNo, questId);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdTouchActQuestNpc(this QuestBlock questBlock, StageInfo stageInfo, uint groupId, uint setNo, QuestId questId, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdTouchActQuestNpc(stageInfo, groupId, setNo, questId);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdTouchActToNpc(this QuestBlock questBlock, StageInfo stageInfo, NpcId npcId, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdTouchActToNpc(stageInfo, npcId);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdNpcTouchAndOrderUi(this QuestBlock questBlock, StageInfo stageInfo, NpcId npcId, uint noOrderGroupSerial, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdNpcTouchAndOrderUi(stageInfo, npcId, noOrderGroupSerial);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdQuestNpcTouchAndOrderUi(this QuestBlock questBlock, StageInfo stageInfo, uint groupNo, uint setNo, QuestId questId, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdQuestNpcTouchAndOrderUi(stageInfo, groupNo, setNo, questId);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdHasUsedKey(this QuestBlock questBlock, StageInfo stageInfo, uint groupId, uint setNo, QuestId questId, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdHasUsedKey(stageInfo, groupId, setNo, questId);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdIsOpenDoorOmQuestSet(this QuestBlock questBlock, StageInfo stageInfo, uint groupId, uint setNo, QuestId questId, bool showMarker = true, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdIsOpenDoorOmQuestSet(stageInfo, groupId, setNo, questId, showMarker);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdSceHitIn(this QuestBlock questBlock, StageInfo stageInfo, int sceNo, bool showMarker = true, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdSceHitIn(stageInfo, sceNo, showMarker);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdSceHitOut(this QuestBlock questBlock, StageInfo stageInfo, int sceNo, bool showMarker = true, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdSceHitOut(stageInfo, sceNo, showMarker);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdIsEndTimer(this QuestBlock questBlock, int timerNo, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdIsEndTimer(timerNo);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdIsNotEndTimer(this QuestBlock questBlock, int timerNo, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdIsNotEndTimer(timerNo);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdCheckAreaRank(this QuestBlock questBlock, QuestAreaId areaId, uint areaRank, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdCheckAreaRank(areaId, areaRank);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdWorldQuestClearNum(this QuestBlock questBlock, QuestAreaId areaId, uint amount, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdWorldQuestClearNum(areaId, amount);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdIsResetInstanceArea(this QuestBlock questBlock, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdIsResetInstanceArea();
            return questBlock;
        }

        public static QuestBlock AddCheckCmdIsMainQuestClear(this QuestBlock questBlock, QuestId questId, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdIsMainQuestClear(questId);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdIsTutorialQuestClear(this QuestBlock questBlock, QuestId questId, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdIsTutorialQuestClear(questId);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdIsClearLightQuest(this QuestBlock questBlock, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdIsClearLightQuest();
            return questBlock;
        }

        public static QuestBlock AddCheckCmdIsOrderLightQuest(this QuestBlock questBlock, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdIsOrderLightQuest();
            return questBlock;
        }

        public static QuestBlock AddCheckCmdIsAcceptLightQuest(this QuestBlock questBlock, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdIsAcceptLightQuest();
            return questBlock;
        }

        public static QuestBlock AddCheckCmdIsSetPlayerSkill(this QuestBlock questBlock, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdIsSetPlayerSkill();
            return questBlock;
        }

        public static QuestBlock AddCheckCmdOpenQuestBoard(this QuestBlock questBlock, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdOpenQuestBoard();
            return questBlock;
        }

        public static QuestBlock AddCheckCmdIsOpenWarehouseReward(this QuestBlock questBlock, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdIsOpenWarehouseReward();
            return questBlock;
        }

        public static QuestBlock AddCheckCmdOpenNewspaper(this QuestBlock questBlock, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdOpenNewspaper();
            return questBlock;
        }

        public static QuestBlock AddCheckCmdIsOpenAreaListUi(this QuestBlock questBlock, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdIsOpenAreaListUi();
            return questBlock;
        }

        public static QuestBlock AddCheckCmdOpenAreaMaster(this QuestBlock questBlock, QuestAreaId areaId, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdOpenAreaMaster(areaId);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdOpenAreaMasterSupplies(this QuestBlock questBlock, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdOpenAreaMasterSupplies();
            return questBlock;
        }

        public static QuestBlock AddCheckCmdIsStageNo(this QuestBlock questBlock, StageInfo stageInfo, bool showMarker = true, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdIsStageNo(stageInfo, showMarker);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdStageNoNotEq(this QuestBlock questBlock, StageInfo stageInfo, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdStageNoNotEq(stageInfo);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdHaveItemAllBag(this QuestBlock questBlock, ItemId itemId, uint amount, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdHaveItemAllBag(itemId, amount);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdIsFullBag(this QuestBlock questBlock, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdIsFullBag();
            return questBlock;
        }

        public static QuestBlock AddCheckCmdIsMyquestLayoutFlagOn(this QuestBlock questBlock, uint flagNo, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdIsMyquestLayoutFlagOn(flagNo);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdIsMyquestLayoutFlagOff(this QuestBlock questBlock, uint flagNo, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdIsMyquestLayoutFlagOff(flagNo);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdMyQstFlagOn(this QuestBlock questBlock, uint flagNo, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdMyQstFlagOn(flagNo);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdMyQstFlagOff(this QuestBlock questBlock, uint flagNo, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdMyQstFlagOff(flagNo);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdIsTutorialFlagOn(this QuestBlock questBlock, uint flagNo, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdIsTutorialFlagOn(flagNo);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdMyQstFlagOnFromFsm(this QuestBlock questBlock, uint flagNo, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdMyQstFlagOnFromFsm(flagNo);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdQstFlagOn(this QuestBlock questBlock, QuestId questId, uint flagNo, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdQstFlagOn(questId, flagNo);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdQstFlagOff(this QuestBlock questBlock, QuestId questId, uint flagNo, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdQstFlagOff(questId, flagNo);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdWorldManageQuestFlagOn(this QuestBlock questBlock, QuestId questId, uint flagNo, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdWorldManageQuestFlagOn(questId, flagNo);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdWorldManageQuestFlagOff(this QuestBlock questBlock, QuestId questId, uint flagNo, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdWorldManageQuestFlagOff(questId, flagNo);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdCraft(this QuestBlock questBlock, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdCraft();
            return questBlock;
        }

        public static QuestBlock AddCheckCmdMakeCraft(this QuestBlock questBlock, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdMakeCraft();
            return questBlock;
        }

        public static QuestBlock AddCheckCmdOpenCraftExam(this QuestBlock questBlock, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdOpenCraftExam();
            return questBlock;
        }

        public static QuestBlock AddCheckCmdLevelUpCraft(this QuestBlock questBlock, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdLevelUpCraft();
            return questBlock;
        }

        public static QuestBlock AddCheckCmdDogmaOrb(this QuestBlock questBlock, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdDogmaOrb();
            return questBlock;
        }

        public static QuestBlock AddCheckCmdJobLevelNotLess(this QuestBlock questBlock, QuestLevelCheckType checkType, uint level, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdJobLevelNotLess(checkType, level);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdJobLevelLess(this QuestBlock questBlock, QuestLevelCheckType checkType, uint level, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdJobLevelLess(checkType, level);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdOpenPpMode(this QuestBlock questBlock, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdOpenPpMode();
            return questBlock;
        }

        public static QuestBlock AddCheckCmdOpenPpShop(this QuestBlock questBlock, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdOpenPpShop();
            return questBlock;
        }

        public static QuestBlock AddCheckCmdPpNotLess(this QuestBlock questBlock, uint points, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdPpNotLess(points);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdIsOrderMyQuest(this QuestBlock questBlock, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdIsOrderMyQuest();
            return questBlock;
        }

        public static QuestBlock AddCheckCmdIsNotOrderMyQuest(this QuestBlock questBlock, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdIsNotOrderMyQuest();
            return questBlock;
        }

        public static QuestBlock AddCheckCmdTouchRimStone(this QuestBlock questBlock, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdTouchRimStone();
            return questBlock;
        }

        public static QuestBlock AddCheckCmdIsPresentPartnerPawn(this QuestBlock questBlock, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdIsPresentPartnerPawn();
            return questBlock;
        }

        public static QuestBlock AddCheckCmdIsPresentPartnerPawnNoMarker(this QuestBlock questBlock, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdIsPresentPartnerPawnNoMarker();
            return questBlock;
        }

        public static QuestBlock AddCheckCmdIsSetPartnerPawn(this QuestBlock questBlock, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdIsSetPartnerPawn();
            return questBlock;
        }

        public static QuestBlock AddCheckCmdIsFavoriteWarpPoint(this QuestBlock questBlock, int warpPointId, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdIsFavoriteWarpPoint(warpPointId);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdOrderDecide(this QuestBlock questBlock, NpcId npcId, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdOrderDecide(npcId);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdSpTalkNpc(this QuestBlock questBlock, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdSpTalkNpc();
            return questBlock;
        }

        public static QuestBlock AddCheckCmdTutorialTalkNpc(this QuestBlock questBlock, StageInfo stageInfo, NpcId npcId, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdTutorialTalkNpc(stageInfo, npcId);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdEventEnd(this QuestBlock questBlock, StageInfo stageInfo, uint eventNo, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdEventEnd(stageInfo, eventNo);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdIsLogin(this QuestBlock questBlock, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdIsLogin();
            return questBlock;
        }

        public static QuestBlock AddCheckCmdIsLoginBugFixedOnly(this QuestBlock questBlock, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdIsLoginBugFixedOnly();
            return questBlock;
        }

        public static QuestBlock AddCheckCmdPlJobEq(this QuestBlock questBlock, JobId jobId, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdPlJobEq(jobId);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdPlJobNotEq(this QuestBlock questBlock, JobId jobId, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdPlJobNotEq(jobId);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdIsOneOffGather(this QuestBlock questBlock, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdIsOneOffGather();
            return questBlock;
        }

        public static QuestBlock AddCheckCommandIsTutorialQuestOrder(this QuestBlock questBlock, QuestId questId, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCommandIsTutorialQuestOrder(questId);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdIsEquip(this QuestBlock questBlock, ItemId itemId, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCommandIsEquip(itemId);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdIsTakePicturesNpc(this QuestBlock questBlock, StageInfo stageInfo, NpcId npcId0, NpcId npcId1 = NpcId.None, NpcId npcId2 = NpcId.None, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCommandIsTakePicturesNpc(stageInfo, npcId0, npcId1, npcId2);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdIsReleaseWarpPointAnyone(this QuestBlock questBlock, int warpPointId, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdIsReleaseWarpPointAnyone(warpPointId);
            return questBlock;
        }

        public static QuestBlock AddCheckCommandNewTalkNpc(this QuestBlock questBlock, StageInfo stageInfo, int groupNo, int setNo, QuestId questId = QuestId.None, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdNewTalkNpc(stageInfo, groupNo, setNo, questId);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdNewTalkNpcWithoutMarker(this QuestBlock questBlock, StageInfo stageInfo, int groupNo, int setNo, QuestId questId = QuestId.None, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdNewTalkNpcWithoutMarker(stageInfo, groupNo, setNo, questId);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdTalkNpcChoice(this QuestBlock questBlock, StageInfo stageInfo, NpcId npcId, int choice, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdTalkNpcChoice(stageInfo, npcId, choice);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdOmSetTouchRadius(this QuestBlock questBlock, StageInfo stageInfo, int groupNo, int setNo, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdOmSetTouchRadius(stageInfo, groupNo, setNo);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdOmReleaseTouchRadius(this QuestBlock questBlock, StageInfo stageInfo, int groupNo, int setNo, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdOmReleaseTouchRadius(stageInfo, groupNo, setNo);
            return questBlock;
        }

        // Ghidra-discovered check commands (IDs 211–256)

        public static QuestBlock AddCheckCmdIsSubstoryStateBit18(this QuestBlock questBlock, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdIsSubstoryStateBit18();
            return questBlock;
        }

        public static QuestBlock AddCheckCmdStoreLinkageEnemyFlagGlobal(this QuestBlock questBlock, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdStoreLinkageEnemyFlagGlobal();
            return questBlock;
        }

        public static QuestBlock AddCheckCmdNpcPreTalkAndOrderUi(this QuestBlock questBlock, StageInfo stageInfo, NpcId npcId, int noOrderGroupSerial, int storeVal, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdNpcPreTalkAndOrderUi(stageInfo, npcId, noOrderGroupSerial, storeVal);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdSubstoryEnemyHpNotLess(this QuestBlock questBlock, int substoryId, int hpRatePercent, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdSubstoryEnemyHpNotLess(substoryId, hpRatePercent);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdSubstoryEnemyHpLess(this QuestBlock questBlock, int substoryId, int hpRatePercent, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdSubstoryEnemyHpLess(substoryId, hpRatePercent);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdSubstoryAvgEnemyHpNotLess(this QuestBlock questBlock, int param01, int hpRatePercent, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdSubstoryAvgEnemyHpNotLess(param01, hpRatePercent);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdSubstoryAvgEnemyHpLess(this QuestBlock questBlock, int param01, int hpRatePercent, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdSubstoryAvgEnemyHpLess(param01, hpRatePercent);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdIsOmBehaviorState(this QuestBlock questBlock, StageInfo stageInfo, int groupNo, int setNo, int behaviorState, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdIsOmBehaviorState(stageInfo, groupNo, setNo, behaviorState);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdMonsterGatheringSpotState(this QuestBlock questBlock, StageInfo stageInfo, int spotId, int spotState, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdMonsterGatheringSpotState(stageInfo, spotId, spotState);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdOmEndAnimation(this QuestBlock questBlock, StageInfo stageInfo, int groupNo, int setNo, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdOmEndAnimation(stageInfo, groupNo, setNo);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdOmEndAnimationNoMarker(this QuestBlock questBlock, StageInfo stageInfo, int groupNo, int setNo, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdOmEndAnimationNoMarker(stageInfo, groupNo, setNo);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdQuestOmEndAnimation(this QuestBlock questBlock, StageInfo stageInfo, int groupNo, int setNo, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdQuestOmEndAnimation(stageInfo, groupNo, setNo);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdQuestOmEndAnimationNoMarker(this QuestBlock questBlock, StageInfo stageInfo, int groupNo, int setNo, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdQuestOmEndAnimationNoMarker(stageInfo, groupNo, setNo);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdDieEnemyNoMarker(this QuestBlock questBlock, int stageNo, int groupNo, int setNo, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdDieEnemyNoMarker(stageNo, setNo, setNo);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdQuestTalkNpcRadius(this QuestBlock questBlock, StageInfo stageInfo, uint groupNo, int setNo, int param04 = 0, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdQuestTalkNpcRadius(stageInfo, groupNo, setNo, param04);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdIsOmBrokenInCurrentPhase(this QuestBlock questBlock, StageInfo stageInfo, int groupNo, int setNo, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdIsOmBrokenInCurrentPhase(stageInfo, groupNo, setNo);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdIsEnemyFoundRadius(this QuestBlock questBlock, StageInfo stageInfo, int groupNo, int setNo = -1, int markerFlag = 0, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdIsEnemyFoundRadius(stageInfo, groupNo, setNo, markerFlag);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdIsEnemyFoundForOrderRadius(this QuestBlock questBlock, StageInfo stageInfo, int groupNo, int setNo = -1, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdIsEnemyFoundForOrderRadius(stageInfo, groupNo, setNo);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdHasAchievement(this QuestBlock questBlock, int categoryNo, int achievementId, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdHasAchievement(categoryNo, achievementId);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdIsSubstoryStateBit19(this QuestBlock questBlock, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdIsSubstoryStateBit19();
            return questBlock;
        }

        public static QuestBlock AddCheckCmdIsEquipSeasonal(this QuestBlock questBlock, int listNo, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdIsEquipSeasonal(listNo);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdDoLimitBreak(this QuestBlock questBlock, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdDoLimitBreak();
            return questBlock;
        }

        public static QuestBlock AddCheckCmdDoExtremeSynthesis(this QuestBlock questBlock, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdDoExtremeSynthesis();
            return questBlock;
        }

        public static QuestBlock AddCheckCmdGetHighOrb(this QuestBlock questBlock, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdGetHighOrb();
            return questBlock;
        }

        public static QuestBlock AddCheckCmdGetDominionPoint(this QuestBlock questBlock, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdGetDominionPoint();
            return questBlock;
        }

        public static QuestBlock AddCheckCmdIsReleaseWarpPointAnyoneNoMarker(this QuestBlock questBlock, int waypointId, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdIsReleaseWarpPointAnyoneNoMarker(waypointId);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdIsSubstoryProgressInRange(this QuestBlock questBlock, int minValue, int maxValue, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdIsSubstoryProgressInRange(minValue, maxValue);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdIsKilledTargetEnemySetGroup2(this QuestBlock questBlock, int flagNo, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdIsKilledTargetEnemySetGroup2(flagNo);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdIsKilledTargetEnemySetGroup2NoMarker(this QuestBlock questBlock, int flagNo, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdIsKilledTargetEnemySetGroup2NoMarker(flagNo);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdIsTimerElapsed(this QuestBlock questBlock, int timerNo, int sec, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdIsTimerElapsed(timerNo, sec);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdIsTimerNotElapsed(this QuestBlock questBlock, int timerNo, int sec, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdIsTimerNotElapsed(timerNo, sec);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdIsContentsModeTimerNotLess(this QuestBlock questBlock, int timeSec, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdIsContentsModeTimerNotLess(timeSec);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdIsTriggerFlagSetAndClear(this QuestBlock questBlock, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdIsTriggerFlagSetAndClear();
            return questBlock;
        }

        public static QuestBlock AddCheckCmdIsWildHuntEnemyKilled(this QuestBlock questBlock, int flagNo, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdIsWildHuntEnemyKilled(flagNo);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdIsEndTimer2(this QuestBlock questBlock, int timerNo, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdIsEndTimer2(timerNo);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdIsWildHuntEnemyFound(this QuestBlock questBlock, int flagNo, int markerFlag = 0, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdIsWildHuntEnemyFound(flagNo, markerFlag);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdIsWildHuntClear(this QuestBlock questBlock, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdIsWildHuntClear();
            return questBlock;
        }

        public static QuestBlock AddCheckCmdIsQuestLayoutHpNotGreater(this QuestBlock questBlock, StageInfo stageInfo, int groupNo, int setNo, int hpLostPct, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdIsQuestLayoutHpNotGreater(stageInfo, groupNo, setNo, hpLostPct);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdIsExtremeMissionClear(this QuestBlock questBlock, QuestId questId, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdIsExtremeMissionClear(questId);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdRandomEq(this QuestBlock questBlock, int randomNo, int value, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdRandomEq(randomNo, value);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdRandomNotEq(this QuestBlock questBlock, int randomNo, int value, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdRandomNotEq(randomNo, value);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdRandomLess(this QuestBlock questBlock, int randomNo, int value, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdRandomLess(randomNo, value);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdRandomNotGreater(this QuestBlock questBlock, int randomNo, int value, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdRandomNotGreater(randomNo, value);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdRandomGreater(this QuestBlock questBlock, int randomNo, int value, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdRandomGreater(randomNo, value);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdRandomNotLess(this QuestBlock questBlock, int randomNo, int value, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdRandomNotLess(randomNo, value);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdIsLinkageEnemyFlag(this QuestBlock questBlock, StageInfo stageInfo, int groupNo, int setNo, uint flagNo, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdIsLinkageEnemyFlag(stageInfo, groupNo, setNo, flagNo);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdIsLinkageEnemyFlagOff(this QuestBlock questBlock, StageInfo stageInfo, int groupNo, int setNo, uint flagNo, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdIsLinkageEnemyFlagOff(stageInfo, groupNo, setNo, flagNo);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdChainNotLess(this QuestBlock questBlock, int chainNo, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdChainNotLess(chainNo);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdIsSearchClan(this QuestBlock questBlock, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdIsSearchClan();
            return questBlock;
        }

        public static QuestBlock AddCheckCmdTouchClanBoard(this QuestBlock questBlock, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdTouchClanBoard();
            return questBlock;
        }

        public static QuestBlock AddCheckCmdIsOrderJobTutorialQuest(this QuestBlock questBlock, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdIsOrderJobTutorialQuest();
            return questBlock;
        }
    }
}
