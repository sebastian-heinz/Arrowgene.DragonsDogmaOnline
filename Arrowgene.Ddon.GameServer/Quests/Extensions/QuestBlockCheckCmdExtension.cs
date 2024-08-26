using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Quests.Work;
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

        public static QuestBlock AddCheckCmdNpcTouchAndOrderUi(this QuestBlock questBlock, StageInfo stageInfo, NpcId npcId, int noOrderGroupSerial, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdNpcTouchAndOrderUi(stageInfo, npcId, noOrderGroupSerial);
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

            questBlock.QuestProgressWork.Add(new WorldQuestClearedProgressWork(questBlock.QuestScheduleId, questBlock.AsQuestProcessState(), areaId, amount));

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
    }
}
