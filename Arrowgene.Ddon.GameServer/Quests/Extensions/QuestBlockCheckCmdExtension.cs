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

        public static QuestBlock AddCheckCmdStageNo(this QuestBlock questBlock, StageInfo stageInfo, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdStageNo(stageInfo);
            return questBlock;
        }

        public static QuestBlock AddCheckCmdStageNoNotEq(this QuestBlock questBlock, StageInfo stageInfo, int commandListIndex = 0)
        {
            ValidateIndexAndUpdateCommandList(questBlock.CheckCommands, commandListIndex);
            questBlock.CheckCommands[commandListIndex].AddCheckCmdStageNoNotEq(stageInfo);
            return questBlock;
        }
    }
}
