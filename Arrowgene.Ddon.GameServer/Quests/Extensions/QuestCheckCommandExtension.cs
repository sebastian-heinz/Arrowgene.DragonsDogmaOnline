using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Quests.Extensions
{
    public static class QuestCheckCommandExtension
    {
        public static List<CDataQuestCommand> AddCheckCmdSceFlagOff(this List<CDataQuestCommand> checkCommands, int flagNo)
        {
            checkCommands.Add(QuestManager.CheckCommand.SceFlagOff(flagNo));
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdSceFlagOn(this List<CDataQuestCommand> checkCommands, int flagNo)
        {
            checkCommands.Add(QuestManager.CheckCommand.SceFlagOn(flagNo));
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdSceHitIn(this List<CDataQuestCommand> checkCommands, StageInfo stageInfo, int sceNo, bool showMarker = true)
        {
            checkCommands.Add(showMarker ?
                QuestManager.CheckCommand.SceHitIn(stageInfo.StageNo, sceNo) :
                QuestManager.CheckCommand.SceHitInWithoutMarker(stageInfo.StageNo, sceNo));
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdSceHitOut(this List<CDataQuestCommand> checkCommands, StageInfo stageInfo, int sceNo, bool showMarker = true)
        {
            checkCommands.Add(showMarker ?
                QuestManager.CheckCommand.SceHitOut(stageInfo.StageNo, sceNo) :
                QuestManager.CheckCommand.SceHitOutWithoutMarker(stageInfo.StageNo, sceNo));
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdDieEnemy(this List<CDataQuestCommand> checkCommands, StageInfo stageInfo, uint groupId, int setNo)
        {
            checkCommands.Add(QuestManager.CheckCommand.DieEnemy(stageInfo.StageNo, (int)groupId, setNo));
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdDiePlayer(this List<CDataQuestCommand> checkCommands)
        {
            checkCommands.Add(QuestManager.CheckCommand.DiePlayer());
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdDieRaidBoss(this List<CDataQuestCommand> checkCommands)
        {
            checkCommands.Add(QuestManager.CheckCommand.DieRaidBoss());
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdIsDownEnemy(this List<CDataQuestCommand> checkCommands)
        {
            checkCommands.Add(QuestManager.CheckCommand.IsDownEnemy());
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdEmHpLess(this List<CDataQuestCommand> checkCommands, StageInfo stageInfo, uint groupId, uint setNo, uint hpRate)
        {
            checkCommands.Add(QuestManager.CheckCommand.EmHpLess(stageInfo.StageNo, (int)groupId, (int)setNo, (int)hpRate));
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdEmHpNotLess(this List<CDataQuestCommand> checkCommands, StageInfo stageInfo, uint groupId, uint setNo, uint hpRate)
        {
            checkCommands.Add(QuestManager.CheckCommand.EmHpNotLess(stageInfo.StageNo, (int)groupId, (int)setNo, (int)hpRate));
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdNpcHpLess(this List<CDataQuestCommand> checkCommands, StageInfo stageInfo, uint groupId, uint setNo, uint hpRate)
        {
            checkCommands.Add(QuestManager.CheckCommand.NpcHpLess(stageInfo.StageNo, (int)groupId, (int)setNo, (int)hpRate));
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdNpcHpNotLess(this List<CDataQuestCommand> checkCommands, StageInfo stageInfo, uint groupId, uint setNo, uint hpRate)
        {
            checkCommands.Add(QuestManager.CheckCommand.NpcHpNotLess(stageInfo.StageNo, (int)groupId, (int)setNo, (int)hpRate));
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdPlHp(this List<CDataQuestCommand> checkCommands, uint hpRate, PlayerHpTypeCheck checkType)
        {
            checkCommands.Add(QuestManager.CheckCommand.PlHp((int)hpRate, (int)checkType));
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdIsEnemyFound(this List<CDataQuestCommand> checkCommands, StageInfo stageInfo, uint groupId, int setNo, bool showMarker = true)
        {
            checkCommands.Add(showMarker ?
                QuestManager.CheckCommand.IsEnemyFound(stageInfo.StageNo, (int)groupId, setNo) :
                QuestManager.CheckCommand.IsEnemyFoundWithoutMarker(stageInfo.StageNo, (int)groupId, setNo));
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdIsEnemyFoundForOrder(this List<CDataQuestCommand> checkCommands, StageInfo stageInfo, uint groupId, int setNo)
        {
            checkCommands.Add(QuestManager.CheckCommand.IsEnemyFoundForOrder(stageInfo.StageNo, (int)groupId, setNo));
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdIsEnemyFoundGmMain(this List<CDataQuestCommand> checkCommands, StageInfo stageInfo, uint groupId, int setNo)
        {
            checkCommands.Add(QuestManager.CheckCommand.IsEnemyFoundGmMain(stageInfo.StageNo, (int)groupId, setNo));
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdIsEnemyFoundGmSub(this List<CDataQuestCommand> checkCommands, StageInfo stageInfo, uint groupId, int setNo, bool showMarker = true)
        {
            checkCommands.Add(QuestManager.CheckCommand.IsEnemyFoundGmSub(stageInfo.StageNo, (int)groupId, setNo));
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdIsKilledTargetEnemySetGroup(this List<CDataQuestCommand> checkCommands, uint flagNo, bool showMarker = true)
        {
            checkCommands.Add(showMarker ?
                QuestManager.CheckCommand.IsKilledTargetEnemySetGroup((int)flagNo) :
                QuestManager.CheckCommand.IsKilledTargetEmSetGrpNoMarker((int)flagNo));
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdIsKilledTargetEnemySetGroupGmMain(this List<CDataQuestCommand> checkCommands, uint flagNo)
        {
            checkCommands.Add(QuestManager.CheckCommand.IsKilledTargetEnemySetGroupGmMain((int)flagNo));
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdIsKilledTargetEnemySetGroupGmSub(this List<CDataQuestCommand> checkCommands, uint flagNo)
        {
            checkCommands.Add(QuestManager.CheckCommand.IsKilledTargetEnemySetGroupGmSub((int)flagNo));
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdEmDieForRandomDungeon(this List<CDataQuestCommand> checkCommands, StageInfo stageInfo, EnemyId enemyId, uint amount)
        {
            checkCommands.Add(QuestManager.CheckCommand.EmDieForRandomDungeon(stageInfo.StageNo, (int)enemyId, (int)amount));
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdOmSetTouch(this List<CDataQuestCommand> checkCommands, StageInfo stageInfo, uint groupId, uint setNo)
        {
            checkCommands.Add(QuestManager.CheckCommand.OmSetTouch(stageInfo.StageNo, (int)groupId, (int)setNo));
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdOmReleaseTouch(this List<CDataQuestCommand> checkCommands, StageInfo stageInfo, uint groupId, uint setNo)
        {
            checkCommands.Add(QuestManager.CheckCommand.OmReleaseTouch(stageInfo.StageNo, (int)groupId, (int)setNo));
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdQuestOmSetTouch(this List<CDataQuestCommand> checkCommands, StageInfo stageInfo, uint groupId, uint setNo, bool showMarker = true)
        {
            checkCommands.Add(showMarker ?
                QuestManager.CheckCommand.QuestOmSetTouch(stageInfo.StageNo, (int)groupId, (int)setNo) :
                QuestManager.CheckCommand.QuestOmSetTouchWithoutMarker(stageInfo.StageNo, (int)groupId, (int)setNo));
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdQuestOmReleaseTouch(this List<CDataQuestCommand> checkCommands, StageInfo stageInfo, uint groupId, uint setNo, bool showMarker = true)
        {
            checkCommands.Add(showMarker ?
                QuestManager.CheckCommand.QuestOmReleaseTouch(stageInfo.StageNo, (int)groupId, (int)setNo) :
                QuestManager.CheckCommand.QuestOmReleaseTouchWithoutMarker(stageInfo.StageNo, (int)groupId, (int)setNo));
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdOmEndText(this List<CDataQuestCommand> checkCommands, StageInfo stageInfo, uint groupId, uint setNo)
        {
            checkCommands.Add(QuestManager.CheckCommand.OmEndText(stageInfo.StageNo, (int)groupId, (int)setNo));
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdQuestOmEndText(this List<CDataQuestCommand> checkCommands, StageInfo stageInfo, uint groupId, uint setNo)
        {
            checkCommands.Add(QuestManager.CheckCommand.QuestOmEndText(stageInfo.StageNo, (int)groupId, (int)setNo));
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdIsOpenDoorOmQuestSet(this List<CDataQuestCommand> checkCommands, StageInfo stageInfo, uint groupId, uint setNo, QuestId questId, bool showMarker = true)
        {
            checkCommands.Add(showMarker ?
                QuestManager.CheckCommand.IsOpenDoorOmQuestSet(stageInfo.StageNo, (int)groupId, (int)setNo, (int)questId) :
                QuestManager.CheckCommand.IsOpenDoorOmQuestSetNoMarker(stageInfo.StageNo, (int)groupId, (int)setNo, (int)questId));
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdIsTouchPawnDungeonOm(this List<CDataQuestCommand> checkCommands, StageInfo stageInfo, uint groupId, uint setNo)
        {
            checkCommands.Add(QuestManager.CheckCommand.IsTouchPawnDungeonOm(stageInfo.StageNo, (int)groupId, (int)setNo));
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdIsOmBrokenLayout(this List<CDataQuestCommand> checkCommands, StageInfo stageInfo, uint groupId, uint setNo, bool showMarker = true)
        {
            checkCommands.Add(showMarker ?
                QuestManager.CheckCommand.IsOmBrokenLayout(stageInfo.StageNo, (int)groupId, (int)setNo) :
                QuestManager.CheckCommand.IsOmBrokenLayoutNoMarker(stageInfo.StageNo, (int)groupId, (int)setNo));
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdIsOmBrokenQuest(this List<CDataQuestCommand> checkCommands, StageInfo stageInfo, uint groupId, uint setNo, bool showMarker = true)
        {
            checkCommands.Add(showMarker ?
                QuestManager.CheckCommand.IsOmBrokenQuest(stageInfo.StageNo, (int)groupId, (int)setNo) :
                QuestManager.CheckCommand.IsOmBrokenQuestNoMarker(stageInfo.StageNo, (int)groupId, (int)setNo));
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdTouchQuestNpcUnitMarker(this List<CDataQuestCommand> checkCommands, StageInfo stageInfo, uint groupId, uint setNo, QuestId questId)
        {
            checkCommands.Add(QuestManager.CheckCommand.TouchQuestNpcUnitMarker(stageInfo.StageNo, (int)groupId, (int)setNo, (int)questId));
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdTouchActQuestNpc(this List<CDataQuestCommand> checkCommands, StageInfo stageInfo, uint groupId, uint setNo, QuestId questId)
        {
            checkCommands.Add(QuestManager.CheckCommand.TouchActQuestNpc(stageInfo.StageNo, (int)groupId, (int)setNo, (int)questId));
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdHasUsedKey(this List<CDataQuestCommand> checkCommands, StageInfo stageInfo, uint groupId, uint setNo, QuestId questId)
        {
            checkCommands.Add(QuestManager.CheckCommand.HasUsedKey(stageInfo.StageNo, (int)groupId, (int)setNo, (int)questId));
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdIsEndTimer(this List<CDataQuestCommand> checkCommands, int timerNo)
        {
            checkCommands.Add(QuestManager.CheckCommand.IsEndTimer(timerNo));
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdIsNotEndTimer(this List<CDataQuestCommand> checkCommands, int timerNo)
        {
            checkCommands.Add(QuestManager.CheckCommand.IsNotEndTimer(timerNo));
            return checkCommands;
        }
    }
}
