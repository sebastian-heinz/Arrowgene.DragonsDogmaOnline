using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Quests.Extensions
{
    public static class QuestResultCommandExtension
    {
        public static List<CDataQuestCommand> AddResultCmdReleaseAnnounce(this List<CDataQuestCommand> resultCommands, ContentsRelease releaseId)
        {
            resultCommands.Add(QuestManager.ResultCommand.ReleaseAnnounce(releaseId));
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdGeneralAnnounce(this List<CDataQuestCommand> resultCommands, QuestGeneralAnnounceType announceType, int msgNo, bool toChatLog = false)
        {
            resultCommands.Add(QuestManager.ResultCommand.CallGeneralAnnounce((int)announceType, msgNo, toChatLog ? 1 : 0));
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdStageAnnounce(this List<CDataQuestCommand> resultCommands, QuestStageAnnounceType announceType, int waveNumber)
        {
            resultCommands.Add(QuestManager.ResultCommand.StageAnnounce((int)announceType, waveNumber));
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdEndContentsPurpose(this List<CDataQuestCommand> resultCommands, int announceNo, QuestEndContentsAnnounceType announceType)
        {
            resultCommands.Add(QuestManager.ResultCommand.AddEndContentsPurpose(announceNo, (int)announceType));
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdEndContentsPurpose(this List<CDataQuestCommand> resultCommands, int announceNo)
        {
            return resultCommands.AddResultCmdEndContentsPurpose(announceNo, QuestEndContentsAnnounceType.Purpose);
        }

        public static List<CDataQuestCommand> AddResultCmdRemoveEndContentsPurpose(this List<CDataQuestCommand> resultCommands, int announceNo)
        {
            resultCommands.Add(QuestManager.ResultCommand.RemoveEndContentsPurpose(announceNo));
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdCyclePurpose(this List<CDataQuestCommand> resultCommands, int announceNo, QuestEndContentsAnnounceType announceType)
        {
            resultCommands.Add(QuestManager.ResultCommand.AddCyclePurpose(announceNo, (int)announceType));
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdRemoveCyclePurpose(this List<CDataQuestCommand> resultCommands, int announceNo)
        {
            resultCommands.Add(QuestManager.ResultCommand.RemoveCyclePurpose(announceNo));
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdUpdateAnnounceDirect(this List<CDataQuestCommand> resultCommands, int announceNo, QuestAnnounceType announceType)
        {
            resultCommands.Add(QuestManager.ResultCommand.UpdateAnnounceDirect(announceNo, (int)announceType));
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdSetAnnounce(this List<CDataQuestCommand> resultCommands, QuestAnnounceType announceType)
        {
            resultCommands.Add(QuestManager.ResultCommand.SetAnnounce(announceType));
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdSetNpcMsg(this List<CDataQuestCommand> resultCommands, NpcId npcId, int msgNo)
        {
            resultCommands.Add(QuestManager.ResultCommand.QstTalkChg(npcId, msgNo));
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdMyQstFlagOn(this List<CDataQuestCommand> resultCommands, uint flagNo)
        {
            resultCommands.Add(QuestManager.ResultCommand.MyQstFlagOn((int)flagNo));
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdMyQstFlagOff(this List<CDataQuestCommand> resultCommands, uint flagNo)
        {
            resultCommands.Add(QuestManager.ResultCommand.MyQstFlagOff((int)flagNo));
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdQstLayoutFlagOn(this List<CDataQuestCommand> resultCommands, uint flagNo)
        {
            resultCommands.Add(QuestManager.ResultCommand.QstLayoutFlagOn((int)flagNo));
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdQstLayoutFlagOff(this List<CDataQuestCommand> resultCommands, uint flagNo)
        {
            resultCommands.Add(QuestManager.ResultCommand.QstLayoutFlagOff((int)flagNo));
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdWorldManageLayoutFlagOn(this List<CDataQuestCommand> resultCommands, uint flagNo, QuestId questId)
        {
            resultCommands.Add(QuestManager.ResultCommand.WorldManageLayoutFlagOn((int)flagNo, (int)questId));
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdWorldManageLayoutFlagOff(this List<CDataQuestCommand> resultCommands, uint flagNo, QuestId questId)
        {
            resultCommands.Add(QuestManager.ResultCommand.WorldManageLayoutFlagOff((int)flagNo, (int)questId));
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdWorldManageQuestFlagOn(this List<CDataQuestCommand> resultCommands, uint flagNo, QuestId questId)
        {
            resultCommands.Add(QuestManager.ResultCommand.WorldManageQuestFlagOn((int)flagNo, (int)questId));
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdWorldManageQuestFlagOff(this List<CDataQuestCommand> resultCommands, uint flagNo, QuestId questId)
        {
            resultCommands.Add(QuestManager.ResultCommand.WorldManageQuestFlagOff((int)flagNo, (int)questId));
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdQstSceFlagOn(this List<CDataQuestCommand> resultCommands)
        {
            resultCommands.Add(QuestManager.ResultCommand.QstSceFlagOn());
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdLotOn(this List<CDataQuestCommand> resultCommands, StageInfo stageInfo, int lotNo)
        {
            resultCommands.Add(QuestManager.ResultCommand.LotOn(stageInfo.StageNo, lotNo));
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdLotOff(this List<CDataQuestCommand> resultCommands, StageInfo stageInfo, int lotNo)
        {
            resultCommands.Add(QuestManager.ResultCommand.LotOff(stageInfo.StageNo, lotNo));
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdTutorialDialog(this List<CDataQuestCommand> resultCommands, TutorialId guideNo)
        {
            resultCommands.Add(QuestManager.ResultCommand.TutorialDialog(guideNo));
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdTutorialEnemyInvincibility(this List<CDataQuestCommand> resultCommands, bool isInvincible)
        {
            resultCommands.Add(isInvincible ?
                QuestManager.ResultCommand.TutorialEnemyInvincibleOn() :
                QuestManager.ResultCommand.TutorialEnemyInvincibleOff());
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdResetTutorialFlag(this List<CDataQuestCommand> resultCommands)
        {
            resultCommands.Add(QuestManager.ResultCommand.ResetTutorialFlag());
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdButtonGuideFlagOn(this List<CDataQuestCommand> resultCommands, int buttonGuideNo)
        {
            resultCommands.Add(QuestManager.ResultCommand.ButtonGuideFlagOn(buttonGuideNo));
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdButtonGuideFlagOff(this List<CDataQuestCommand> resultCommands, int buttonGuideNo)
        {
            resultCommands.Add(QuestManager.ResultCommand.ButtonGuideFlagOff(buttonGuideNo));
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdBgmStop(this List<CDataQuestCommand> resultCommands)
        {
            resultCommands.Add(QuestManager.ResultCommand.BgmStop());
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdRequestBgm(this List<CDataQuestCommand> resultCommands, BgmType type, int bgmId)
        {
            resultCommands.Add(QuestManager.ResultCommand.BgmRequest((int)type, bgmId));
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdBgmRequestFix(this List<CDataQuestCommand> resultCommands, BgmType type, int bgmId)
        {
            resultCommands.Add(QuestManager.ResultCommand.BgmRequestFix((int)type, bgmId));
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdMarkerAtDest(this List<CDataQuestCommand> resultCommands, StageInfo stageInfo, int x, int y, int z)
        {
            resultCommands.Add(QuestManager.ResultCommand.AddMarkerAtDest(stageInfo.StageNo, x, y, z));
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdMarkerAtItem(this List<CDataQuestCommand> resultCommands, StageInfo stageInfo, int x, int y, int z)
        {
            resultCommands.Add(QuestManager.ResultCommand.AddMarkerAtItem(stageInfo.StageNo, x, y, z));
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdDecideDivideArea(this List<CDataQuestCommand> resultCommands, StageInfo stageInfo, int startPos)
        {
            resultCommands.Add(QuestManager.ResultCommand.DecideDivideArea(stageInfo.StageNo, startPos));
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdDivideSuccess(this List<CDataQuestCommand> resultCommands)
        {
            resultCommands.Add(QuestManager.ResultCommand.DivideSuccess());
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdDivideFailed(this List<CDataQuestCommand> resultCommands)
        {
            resultCommands.Add(QuestManager.ResultCommand.DivideFailed());
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdEnableGetSetQuestList(this List<CDataQuestCommand> resultCommands)
        {
            resultCommands.Add(QuestManager.ResultCommand.EnableGetSetQuestList());
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdPlayCameraEvent(this List<CDataQuestCommand> resultCommands, StageInfo stageInfo, int eventNo)
        {
            resultCommands.Add(QuestManager.ResultCommand.PlayCameraEvent(stageInfo.StageNo, eventNo));
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdPlayMessage(this List<CDataQuestCommand> resultCommands, int groupNo, int waitTime)
        {
            resultCommands.Add(QuestManager.ResultCommand.PlayMessage(groupNo, waitTime));
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdStartTimer(this List<CDataQuestCommand> resultCommands, int timerNo, int waitTimeInSec)
        {
            resultCommands.Add(QuestManager.ResultCommand.StartTimer(timerNo, waitTimeInSec));
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdHandItem(this List<CDataQuestCommand> resultCommands, ItemId itemId, uint amount)
        {
            resultCommands.Add(QuestManager.ResultCommand.HandItem((int)itemId, (int) amount));
            return resultCommands;
        }

        public static List<CDataQuestCommand> PlayCameraEvent(this List<CDataQuestCommand> resultCommands, StageInfo stageInfo, int eventNo)
        {
            resultCommands.Add(QuestManager.ResultCommand.PlayCameraEvent(stageInfo.StageNo, eventNo));
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdQstTalkChg(this List<CDataQuestCommand> resultCommands, NpcId npcId, int msgNo)
        {
            resultCommands.Add(QuestManager.ResultCommand.QstTalkChg(npcId, msgNo));
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdQstTalkDel(this List<CDataQuestCommand> resultCommands, NpcId npcId)
        {
            resultCommands.Add(QuestManager.ResultCommand.QstTalkDel(npcId));
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdEventExec(this List<CDataQuestCommand> resultCommands, StageInfo stageInfo, uint eventNo, StageInfo destStageInfo, uint jumpPos)
        {
            resultCommands.Add(QuestManager.ResultCommand.EventExec(stageInfo.StageNo, (int)eventNo, destStageInfo.StageNo, (int) jumpPos));
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdEventExecCont(this List<CDataQuestCommand> resultCommands, StageInfo stageInfo, uint eventNo, StageInfo destStageInfo, uint jumpPos)
        {
            resultCommands.Add(QuestManager.ResultCommand.EventExecCont(stageInfo.StageNo, (int)eventNo, destStageInfo.StageNo, (int) jumpPos));
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdExeEventAfterStageJump(this List<CDataQuestCommand> resultCommands, StageInfo stageInfo, uint eventNo, uint startPos)
        {
            resultCommands.Add(QuestManager.ResultCommand.ExeEventAfterStageJump(stageInfo.StageNo, (int)eventNo, (int)startPos));
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdExeEventAfterStageJumpContinue(this List<CDataQuestCommand> resultCommands, StageInfo stageInfo, uint eventNo, uint startPos)
        {
            resultCommands.Add(QuestManager.ResultCommand.ExeEventAfterStageJumpContinue(stageInfo.StageNo, (int)eventNo, (int)startPos));
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdStageJump(this List<CDataQuestCommand> resultCommands, StageInfo stageInfo, uint startPos)
        {
            resultCommands.Add(QuestManager.ResultCommand.StageJump(stageInfo.StageNo, (int)startPos));
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdAreaJumpFadeContinue(this List<CDataQuestCommand> resultCommands)
        {
            resultCommands.Add(QuestManager.ResultCommand.AreaJumpFadeContinue());
            return resultCommands;
        }

        // Ghidra-discovered result commands (IDs 99–134)

        public static List<CDataQuestCommand> AddResultCmdSubstoryProgress(this List<CDataQuestCommand> resultCommands, int delta)
        {
            resultCommands.Add(QuestManager.ResultCommand.SubstoryProgress(delta));
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdAddWarProgress(this List<CDataQuestCommand> resultCommands, int gaugeNo, int percentRate)
        {
            resultCommands.Add(QuestManager.ResultCommand.AddWarProgress(gaugeNo, percentRate));
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdUpdateSubstoryProgress(this List<CDataQuestCommand> resultCommands, int progressDelta, int param02 = 0, int param03 = 0, int param04 = 0)
        {
            resultCommands.Add(QuestManager.ResultCommand.UpdateSubstoryProgress(progressDelta, param02, param03, param04));
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdEnableSubstoryGauge(this List<CDataQuestCommand> resultCommands)
        {
            resultCommands.Add(QuestManager.ResultCommand.EnableSubstoryGauge());
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdDisableSubstoryGauge(this List<CDataQuestCommand> resultCommands)
        {
            resultCommands.Add(QuestManager.ResultCommand.DisableSubstoryGauge());
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdQstTalkChgFsm(this List<CDataQuestCommand> resultCommands, NpcId npcId, int msgNo, int param03 = 0, int param04 = 0)
        {
            resultCommands.Add(QuestManager.ResultCommand.QstTalkChgFsm(npcId, msgNo, param03, param04));
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdSetEnemyAggroFlag(this List<CDataQuestCommand> resultCommands, int type, int param02)
        {
            resultCommands.Add(QuestManager.ResultCommand.SetEnemyAggroFlag(type, param02));
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdSetRandom2(this List<CDataQuestCommand> resultCommands, int randomNo, int minValue, int maxValue)
        {
            resultCommands.Add(QuestManager.ResultCommand.SetRandom2(randomNo, minValue, maxValue, resultValue: 0));
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdCallGreatPurpose(this List<CDataQuestCommand> resultCommands, int categoryNo, int purposeNo)
        {
            resultCommands.Add(QuestManager.ResultCommand.CallGreatPurpose(categoryNo, purposeNo));
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdEnableSubstoryTextBox(this List<CDataQuestCommand> resultCommands)
        {
            resultCommands.Add(QuestManager.ResultCommand.EnableSubstoryTextBox());
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdDisableSubstoryTextBox(this List<CDataQuestCommand> resultCommands)
        {
            resultCommands.Add(QuestManager.ResultCommand.DisableSubstoryTextBox());
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdSetCustomWeather(this List<CDataQuestCommand> resultCommands, int param01 = 0, int param02 = 0)
        {
            resultCommands.Add(QuestManager.ResultCommand.SetCustomWeather(param01, param02));
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdResetCustomWeather(this List<CDataQuestCommand> resultCommands)
        {
            resultCommands.Add(QuestManager.ResultCommand.ResetCustomWeather());
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdLayoutFlagRandomOn2(this List<CDataQuestCommand> resultCommands, int flagNo1, int flagNo2, int flagNo3)
        {
            resultCommands.Add(QuestManager.ResultCommand.LayoutFlagRandomOn2(flagNo1, flagNo2, flagNo3, resultNo: 0));
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdSetOmHitCount(this List<CDataQuestCommand> resultCommands, StageInfo stageInfo, int groupNo, int setNo, int count)
        {
            resultCommands.Add(QuestManager.ResultCommand.SetOmHitCount(stageInfo.StageNo, groupNo, setNo, count));
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdSetQuestOmHitCount(this List<CDataQuestCommand> resultCommands, StageInfo stageInfo, int groupNo, int setNo, int count)
        {
            resultCommands.Add(QuestManager.ResultCommand.SetQuestOmHitCount(stageInfo.StageNo, groupNo, setNo, count));
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdSetOmTierUp(this List<CDataQuestCommand> resultCommands, StageInfo stageInfo, int groupNo, int setNo, int tier)
        {
            resultCommands.Add(QuestManager.ResultCommand.SetOmTierUp(stageInfo.StageNo, groupNo, setNo, tier));
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdSetQuestOmTierUp(this List<CDataQuestCommand> resultCommands, StageInfo stageInfo, int groupNo, int setNo, int tier)
        {
            resultCommands.Add(QuestManager.ResultCommand.SetQuestOmTierUp(stageInfo.StageNo, groupNo, setNo, tier));
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdSetOmState(this List<CDataQuestCommand> resultCommands, StageInfo stageInfo, int groupNo, int setNo, int state)
        {
            resultCommands.Add(QuestManager.ResultCommand.SetOmState(stageInfo.StageNo, groupNo, setNo, state));
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdSetQuestOmState(this List<CDataQuestCommand> resultCommands, StageInfo stageInfo, int groupNo, int setNo, int state)
        {
            resultCommands.Add(QuestManager.ResultCommand.SetQuestOmState(stageInfo.StageNo, groupNo, setNo, state));
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdSetNamedEnemyParam(this List<CDataQuestCommand> resultCommands, StageInfo stageInfo, int groupNo, int setNo, int param)
        {
            resultCommands.Add(QuestManager.ResultCommand.SetNamedEnemyParam(stageInfo.StageNo, groupNo, setNo, param));
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdRemoveFsmNpcFromSchedule(this List<CDataQuestCommand> resultCommands, int param01 = 0)
        {
            resultCommands.Add(QuestManager.ResultCommand.RemoveFsmNpcFromSchedule(param01));
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdSetMagmaState(this List<CDataQuestCommand> resultCommands, int phase, int state)
        {
            resultCommands.Add(QuestManager.ResultCommand.SetMagmaState(phase, state));
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdTriggerDarkDungeonEndSequence(this List<CDataQuestCommand> resultCommands)
        {
            resultCommands.Add(QuestManager.ResultCommand.TriggerDarkDungeonEndSequence());
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdEndChain(this List<CDataQuestCommand> resultCommands)
        {
            resultCommands.Add(QuestManager.ResultCommand.EndChain());
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdGetDragonAbility(this List<CDataQuestCommand> resultCommands, int type)
        {
            resultCommands.Add(QuestManager.ResultCommand.GetDragonAbility(type));
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdSetQuestLayoutEnemyBodyPose(this List<CDataQuestCommand> resultCommands, StageInfo stageInfo, int groupNo, int setNo, int poseId)
        {
            resultCommands.Add(QuestManager.ResultCommand.SetQuestLayoutEnemyBodyPose(stageInfo.StageNo, groupNo, setNo, poseId));
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdSetRandom(this List<CDataQuestCommand> resultCommands, int randomNo, int minValue, int maxValue)
        {
            // resultValue=0 here; PatchRandomCommands overwrites Param04 with the server-rolled value at dispatch time.
            resultCommands.Add(QuestManager.ResultCommand.SetRandom(randomNo, minValue, maxValue, resultValue: 0));
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdResetRandom(this List<CDataQuestCommand> resultCommands, int randomNo)
        {
            resultCommands.Add(QuestManager.ResultCommand.ResetRandom(randomNo));
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdStopMessage(this List<CDataQuestCommand> resultCommands)
        {
            resultCommands.Add(QuestManager.ResultCommand.StopMessage());
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdLinkageEnemyFlagOn(this List<CDataQuestCommand> resultCommands, StageInfo stageInfo, int groupNo, int setNo, uint flagNo)
        {
            resultCommands.Add(QuestManager.ResultCommand.LinkageEnemyFlagOn(stageInfo.StageNo, groupNo, setNo, (int)flagNo));
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdLinkageEnemyFlagOff(this List<CDataQuestCommand> resultCommands, StageInfo stageInfo, int groupNo, int setNo, uint flagNo)
        {
            resultCommands.Add(QuestManager.ResultCommand.LinkageEnemyFlagOff(stageInfo.StageNo, groupNo, setNo, (int)flagNo));
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdSetDiePlayerReturnPos(this List<CDataQuestCommand> resultCommands, StageInfo stageInfo, int startPos, int outSceNo)
        {
            resultCommands.Add(QuestManager.ResultCommand.SetDiePlayerReturnPos(stageInfo.StageNo, startPos, outSceNo));
            return resultCommands;
        }

        public static List<CDataQuestCommand> AddResultCmdResetDiePlayerReturnPos(this List<CDataQuestCommand> resultCommands, StageInfo stageInfo, int startPos)
        {
            resultCommands.Add(QuestManager.ResultCommand.ResetDiePlayerReturnPos(stageInfo.StageNo, startPos));
            return resultCommands;
        }
    }
}
