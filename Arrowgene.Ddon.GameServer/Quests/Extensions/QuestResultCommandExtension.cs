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
    }
}
