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

        public static List<CDataQuestCommand> AddCheckCmdTouchActToNpc(this List<CDataQuestCommand> checkCommands, StageInfo stageInfo, NpcId npcId)
        {
            checkCommands.Add(QuestManager.CheckCommand.TouchActToNpc(stageInfo.StageNo, npcId));
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdNpcTouchAndOrderUi(this List<CDataQuestCommand> checkCommands, StageInfo stageInfo, NpcId npcId, int noOrderGroupSerial)
        {
            checkCommands.Add(QuestManager.CheckCommand.NpcTouchAndOrderUi(stageInfo.StageNo, npcId, noOrderGroupSerial));
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

        public static List<CDataQuestCommand> AddCheckCmdCheckAreaRank(this List<CDataQuestCommand> checkCommands, QuestAreaId areaId, uint areaRank)
        {
            checkCommands.Add(QuestManager.CheckCommand.CheckAreaRank((int)areaId, (int)areaRank));
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdWorldQuestClearNum(this List<CDataQuestCommand> checkCommands, QuestAreaId areaId, uint amount)
        {
            checkCommands.Add(QuestManager.CheckCommand.SetQuestClearNum((int)amount, (int)areaId));
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdIsResetInstanceArea(this List<CDataQuestCommand> checkCommands)
        {
            checkCommands.Add(QuestManager.CheckCommand.IsResetInstanceArea());
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdIsMainQuestClear(this List<CDataQuestCommand> checkCommands, QuestId questId)
        {
            checkCommands.Add(QuestManager.CheckCommand.IsMainQuestClear((int)questId));
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdIsTutorialQuestClear(this List<CDataQuestCommand> checkCommands, QuestId questId)
        {
            checkCommands.Add(QuestManager.CheckCommand.IsTutorialQuestClear((int) questId));
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdIsClearLightQuest(this List<CDataQuestCommand> checkCommands)
        {
            checkCommands.Add(QuestManager.CheckCommand.IsClearLightQuest());
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdIsOrderLightQuest(this List<CDataQuestCommand> checkCommands)
        {
            checkCommands.Add(QuestManager.CheckCommand.IsOrderLightQuest());
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdIsAcceptLightQuest(this List<CDataQuestCommand> checkCommands)
        {
            checkCommands.Add(QuestManager.CheckCommand.IsAcceptLightQuest());
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdIsSetPlayerSkill(this List<CDataQuestCommand> checkCommands)
        {
            checkCommands.Add(QuestManager.CheckCommand.IsSetPlayerSkill());
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdOpenQuestBoard(this List<CDataQuestCommand> checkCommands)
        {
            checkCommands.Add(QuestManager.CheckCommand.OpenQuestBoard());
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdIsOpenWarehouseReward(this List<CDataQuestCommand> checkCommands)
        {
            checkCommands.Add(QuestManager.CheckCommand.IsOpenWarehouseReward());
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdTouchRimStone(this List<CDataQuestCommand> checkCommands)
        {
            checkCommands.Add(QuestManager.CheckCommand.TouchRimStone());
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdOpenNewspaper(this List<CDataQuestCommand> checkCommands)
        {
            checkCommands.Add(QuestManager.CheckCommand.OpenNewspaper());
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdIsOpenAreaListUi(this List<CDataQuestCommand> checkCommands)
        {
            checkCommands.Add(QuestManager.CheckCommand.IsOpenAreaListUi());
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdOpenAreaMaster(this List<CDataQuestCommand> checkCommands, QuestAreaId areaId)
        {
            checkCommands.Add(QuestManager.CheckCommand.OpenAreaMaster((int) areaId));
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdOpenAreaMasterSupplies(this List<CDataQuestCommand> checkCommands)
        {
            checkCommands.Add(QuestManager.CheckCommand.OpenAreaMasterSupplies());
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdIsStageNo(this List<CDataQuestCommand> checkCommands, StageInfo stageInfo, bool showMarker = true)
        {
            checkCommands.Add(showMarker ? 
                QuestManager.CheckCommand.StageNo(stageInfo.StageNo) :
                QuestManager.CheckCommand.StageNoWithoutMarker(stageInfo.StageNo));
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdStageNoNotEq(this List<CDataQuestCommand> checkCommands, StageInfo stageInfo)
        {
            checkCommands.Add(QuestManager.CheckCommand.StageNoNotEq(stageInfo.StageNo));
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdHaveItemAllBag(this List<CDataQuestCommand> checkCommands, ItemId itemId, uint amount)
        {
            checkCommands.Add(QuestManager.CheckCommand.HaveItemAllBag((int) itemId, (int) amount));
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdIsFullBag(this List<CDataQuestCommand> checkCommands)
        {
            checkCommands.Add(QuestManager.CheckCommand.IsFullBag());
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdCraft(this List<CDataQuestCommand> checkCommands)
        {
            checkCommands.Add(QuestManager.CheckCommand.Craft());
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdMakeCraft(this List<CDataQuestCommand> checkCommands)
        {
            checkCommands.Add(QuestManager.CheckCommand.MakeCraft());
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdOpenCraftExam(this List<CDataQuestCommand> checkCommands)
        {
            checkCommands.Add(QuestManager.CheckCommand.OpenCraftExam());
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdLevelUpCraft(this List<CDataQuestCommand> checkCommands)
        {
            checkCommands.Add(QuestManager.CheckCommand.LevelUpCraft());
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdDogmaOrb(this List<CDataQuestCommand> checkCommands)
        {
            checkCommands.Add(QuestManager.CheckCommand.DogmaOrb());
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdIsMyquestLayoutFlagOn(this List<CDataQuestCommand> checkCommands, uint flagNo)
        {
            checkCommands.Add(QuestManager.CheckCommand.IsMyquestLayoutFlagOn((int)flagNo));
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdIsMyquestLayoutFlagOff(this List<CDataQuestCommand> checkCommands, uint flagNo)
        {
            checkCommands.Add(QuestManager.CheckCommand.IsMyquestLayoutFlagOff((int)flagNo));
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdMyQstFlagOn(this List<CDataQuestCommand> checkCommands, uint flagNo)
        {
            checkCommands.Add(QuestManager.CheckCommand.MyQstFlagOn((int)flagNo));
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdMyQstFlagOff(this List<CDataQuestCommand> checkCommands, uint flagNo)
        {
            checkCommands.Add(QuestManager.CheckCommand.MyQstFlagOff((int)flagNo));
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdIsTutorialFlagOn(this List<CDataQuestCommand> checkCommands, uint flagNo)
        {
            checkCommands.Add(QuestManager.CheckCommand.IsTutorialFlagOn((int)flagNo));
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdMyQstFlagOnFromFsm(this List<CDataQuestCommand> checkCommands, uint flagNo)
        {
            checkCommands.Add(QuestManager.CheckCommand.MyQstFlagOnFromFsm((int)flagNo));
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdQstFlagOn(this List<CDataQuestCommand> checkCommands, QuestId questId, uint flagNo)
        {
            checkCommands.Add(QuestManager.CheckCommand.QstFlagOn((int)questId, (int)flagNo));
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdQstFlagOff(this List<CDataQuestCommand> checkCommands, QuestId questId, uint flagNo)
        {
            checkCommands.Add(QuestManager.CheckCommand.QstFlagOff((int)questId, (int)flagNo));
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdWorldManageQuestFlagOn(this List<CDataQuestCommand> checkCommands, QuestId questId, uint flagNo)
        {
            checkCommands.Add(QuestManager.CheckCommand.WorldManageQuestFlagOn((int)questId, (int)flagNo));
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdWorldManageQuestFlagOff(this List<CDataQuestCommand> checkCommands, QuestId questId, uint flagNo)
        {
            checkCommands.Add(QuestManager.CheckCommand.WorldManageQuestFlagOff((int)questId, (int)flagNo));
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdJobLevelNotLess(this List<CDataQuestCommand> checkCommands, QuestLevelCheckType checkType, uint level)
        {
            checkCommands.Add(QuestManager.CheckCommand.JobLevelNotLess((int)checkType, (int) level));
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdJobLevelLess(this List<CDataQuestCommand> checkCommands, QuestLevelCheckType checkType, uint level)
        {
            checkCommands.Add(QuestManager.CheckCommand.JobLevelLess((int)checkType, (int)level));
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdOpenPpMode(this List<CDataQuestCommand> checkCommands)
        {
            checkCommands.Add(QuestManager.CheckCommand.OpenPpMode());
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdOpenPpShop(this List<CDataQuestCommand> checkCommands)
        {
            checkCommands.Add(QuestManager.CheckCommand.OpenPpShop());
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdPpNotLess(this List<CDataQuestCommand> checkCommands, uint points)
        {
            checkCommands.Add(QuestManager.CheckCommand.PpNotLess((int)points));
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdIsOrderMyQuest(this List<CDataQuestCommand> checkCommands)
        {
            checkCommands.Add(QuestManager.CheckCommand.IsOrderMyQuest());
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdIsNotOrderMyQuest(this List<CDataQuestCommand> checkCommands)
        {
            checkCommands.Add(QuestManager.CheckCommand.IsNotOrderMyQuest());
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdIsPresentPartnerPawn(this List<CDataQuestCommand> checkCommands)
        {
            checkCommands.Add(QuestManager.CheckCommand.IsPresentPartnerPawn());
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdIsPresentPartnerPawnNoMarker(this List<CDataQuestCommand> checkCommands)
        {
            checkCommands.Add(QuestManager.CheckCommand.IsPresentPartnerPawnNoMarker());
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdIsSetPartnerPawn(this List<CDataQuestCommand> checkCommands)
        {
            checkCommands.Add(QuestManager.CheckCommand.IsSetPartnerPawn());
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdIsFavoriteWarpPoint(this List<CDataQuestCommand> checkCommands, int warpPointId)
        {
            checkCommands.Add(QuestManager.CheckCommand.IsFavoriteWarpPoint(warpPointId));
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdOrderDecide(this List<CDataQuestCommand> checkCommands, NpcId npcId)
        {
            checkCommands.Add(QuestManager.CheckCommand.OrderDecide(npcId));
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdSpTalkNpc(this List<CDataQuestCommand> checkCommands)
        {
            checkCommands.Add(QuestManager.CheckCommand.SpTalkNpc());
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdTutorialTalkNpc(this List<CDataQuestCommand> checkCommands, StageInfo stageInfo, NpcId npcId)
        {
            checkCommands.Add(QuestManager.CheckCommand.TutorialTalkNpc(stageInfo.StageNo, npcId));
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdEventEnd(this List<CDataQuestCommand> checkCommands, StageInfo stageInfo, uint eventNo)
        {
            checkCommands.Add(QuestManager.CheckCommand.EventEnd(stageInfo.StageNo, (int) eventNo));
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdIsLogin(this List<CDataQuestCommand> checkCommands)
        {
            checkCommands.Add(QuestManager.CheckCommand.IsLogin());
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdIsLoginBugFixedOnly(this List<CDataQuestCommand> checkCommands)
        {
            checkCommands.Add(QuestManager.CheckCommand.IsLoginBugFixedOnly());
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdPlJobEq(this List<CDataQuestCommand> checkCommands, JobId jobId)
        {
            checkCommands.Add(QuestManager.CheckCommand.PlJobEq((int)jobId));
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdPlJobNotEq(this List<CDataQuestCommand> checkCommands, JobId jobId)
        {
            checkCommands.Add(QuestManager.CheckCommand.PlJobNotEq((int)jobId));
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCmdIsOneOffGather(this List<CDataQuestCommand> checkCommands)
        {
            checkCommands.Add(QuestManager.CheckCommand.IsOneOffGather());
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCommandIsTutorialQuestOrder(this List<CDataQuestCommand> checkCommands, QuestId questId)
        {
            checkCommands.Add(QuestManager.CheckCommand.IsTutorialQuestOrder((int) questId));
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCommandQstFlagOn(this List<CDataQuestCommand> checkCommands, QuestId questId, int flagNo)
        {
            checkCommands.Add(QuestManager.CheckCommand.QstFlagOn((int) questId, flagNo));
            return checkCommands;
        }

        public static List<CDataQuestCommand> AddCheckCommandQstFlagOff(this List<CDataQuestCommand> checkCommands, QuestId questId, int flagNo)
        {
            checkCommands.Add(QuestManager.CheckCommand.QstFlagOff((int)questId, flagNo));
            return checkCommands;
        }
    }
}
