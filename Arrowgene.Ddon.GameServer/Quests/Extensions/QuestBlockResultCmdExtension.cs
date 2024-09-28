using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;

namespace Arrowgene.Ddon.GameServer.Quests.Extensions
{
    public static class QuestBlockResultCmdExtension
    {
        public static QuestBlock AddResultCmdReleaseAnnounce(this QuestBlock questBlock, int releaseId)
        {
            questBlock.ResultCommands.AddResultCmdReleaseAnnounce(releaseId);
            return questBlock;
        }

        public static QuestBlock AddResultCmdGeneralAnnounce(this QuestBlock questBlock, QuestGeneralAnnounceType announceType, int msgNo)
        {
            questBlock.ResultCommands.AddResultCmdGeneralAnnounce(announceType, msgNo);
            return questBlock;
        }

        public static QuestBlock AddResultCmdStageAnnounce(this QuestBlock questBlock, QuestStageAnnounceType announceType, int waveNumber)
        {
            questBlock.ResultCommands.AddResultCmdStageAnnounce(announceType, waveNumber);
            return questBlock;
        }

        public static QuestBlock AddResultCmdEndContentsPurpose(this QuestBlock questBlock, int announceNo, QuestEndContentsAnnounceType announceType)
        {
            questBlock.ResultCommands.AddResultCmdEndContentsPurpose(announceNo, announceType);
            return questBlock;
        }

        public static QuestBlock AddResultCmdEndContentsPurpose(this QuestBlock questBlock, int announceNo)
        {
            return AddResultCmdEndContentsPurpose(questBlock, announceNo, QuestEndContentsAnnounceType.Purpose);
        }

        public static QuestBlock AddResultCmdUpdateAnnounceDirect(this QuestBlock questBlock, int announceNo, QuestAnnounceType announceType)
        {
            questBlock.ResultCommands.AddResultCmdUpdateAnnounceDirect(announceNo, announceType);
            return questBlock;
        }

        public static QuestBlock AddResultCmdSetAnnounce(this QuestBlock questBlock, QuestAnnounceType announceType)
        {
            questBlock.ResultCommands.AddResultCmdSetAnnounce(announceType);
            return questBlock;
        }

        public static QuestBlock AddResultCmdSetNpcMsg(this QuestBlock questBlock, NpcId npcId, int msgNo)
        {
            questBlock.ResultCommands.AddResultCmdSetNpcMsg(npcId, msgNo);
            return questBlock;
        }

        public static QuestBlock AddResultCmdMyQstFlagOn(this QuestBlock questBlock, uint flagNo)
        {
            questBlock.ResultCommands.AddResultCmdMyQstFlagOn(flagNo);
            return questBlock;
        }

        public static QuestBlock AddResultCmdMyQstFlagOff(this QuestBlock questBlock, uint flagNo)
        {
            questBlock.ResultCommands.AddResultCmdMyQstFlagOff(flagNo);
            return questBlock;
        }

        public static QuestBlock AddResultCmdQstLayoutFlagOn(this QuestBlock questBlock, uint flagNo)
        {
            questBlock.ResultCommands.AddResultCmdQstLayoutFlagOn(flagNo);
            return questBlock;
        }

        public static QuestBlock AddResultCmdQstLayoutFlagOff(this QuestBlock questBlock, uint flagNo)
        {
            questBlock.ResultCommands.AddResultCmdQstLayoutFlagOff(flagNo);
            return questBlock;
        }

        public static QuestBlock AddResultCmdWorldManageLayoutFlagOn(this QuestBlock questBlock, uint flagNo, QuestId questId)
        {
            questBlock.ResultCommands.AddResultCmdWorldManageLayoutFlagOn(flagNo, questId);
            return questBlock;
        }

        public static QuestBlock AddResultCmdWorldManageLayoutFlagOff(this QuestBlock questBlock, uint flagNo, QuestId questId)
        {
            questBlock.ResultCommands.AddResultCmdWorldManageLayoutFlagOff(flagNo, questId);
            return questBlock;
        }

        public static QuestBlock AddResultCmdLotOn(this QuestBlock questBlock, StageInfo stageInfo, int lotNo)
        {
            questBlock.ResultCommands.AddResultCmdLotOn(stageInfo, lotNo);
            return questBlock;
        }

        public static QuestBlock AddResultCmdLotOff(this QuestBlock questBlock, StageInfo stageInfo, int lotNo)
        {
            questBlock.ResultCommands.AddResultCmdLotOff(stageInfo, lotNo);
            return questBlock;
        }

        public static QuestBlock AddResultCmdTutorialDialog(this QuestBlock questBlock, int guideNo)
        {
            questBlock.ResultCommands.AddResultCmdTutorialDialog(guideNo);
            return questBlock;
        }

        public static QuestBlock AddResultCmdTutorialEnemyInvincibility(this QuestBlock questBlock, bool isInvincible)
        {
            questBlock.ResultCommands.AddResultCmdTutorialEnemyInvincibility(isInvincible);
            return questBlock;
        }

        public static QuestBlock AddResultCmdResetTutorialFlag(this QuestBlock questBlock)
        {
            questBlock.ResultCommands.AddResultCmdResetTutorialFlag();
            return questBlock;
        }

        public static QuestBlock AddResultCmdButtonGuideFlagOn(this QuestBlock questBlock, int buttonGuideNo)
        {
            questBlock.ResultCommands.AddResultCmdButtonGuideFlagOn(buttonGuideNo);
            return questBlock;
        }

        public static QuestBlock AddResultCmdButtonGuideFlagOff(this QuestBlock questBlock, int buttonGuideNo)
        {
            questBlock.ResultCommands.AddResultCmdButtonGuideFlagOff(buttonGuideNo);
            return questBlock;
        }

        public static QuestBlock AddResultCmdBgmStop(this QuestBlock questBlock)
        {
            questBlock.ResultCommands.AddResultCmdBgmStop();
            return questBlock;
        }

        public static QuestBlock AddResultCmdRequestBgm(this QuestBlock questBlock, BgmType type, int bgmId)
        {
            questBlock.ResultCommands.AddResultCmdRequestBgm(type, bgmId);
            return questBlock;
        }

        public static QuestBlock AddResultCmdBgmRequestFix(this QuestBlock questBlock, BgmType type, int bgmId)
        {
            questBlock.ResultCommands.AddResultCmdBgmRequestFix(type, bgmId);
            return questBlock;
        }

        public static QuestBlock AddResultCmdMarkerAtDest(this QuestBlock questBlock, StageInfo stageInfo, int x, int y, int z)
        {
            questBlock.ResultCommands.AddResultCmdMarkerAtDest(stageInfo, x, y, z);
            return questBlock;
        }

        public static QuestBlock AddResultCmdMarkerAtItem(this QuestBlock questBlock, StageInfo stageInfo, int x, int y, int z)
        {
            questBlock.ResultCommands.AddResultCmdMarkerAtItem(stageInfo, x, y, z);
            return questBlock;
        }

        public static QuestBlock AddResultCmdDecideDivideArea(this QuestBlock questBlock, StageInfo stageInfo, int startPos)
        {
            questBlock.ResultCommands.AddResultCmdDecideDivideArea(stageInfo, startPos);
            return questBlock;
        }

        public static QuestBlock AddResultCmdDivideSuccess(this QuestBlock questBlock)
        {
            questBlock.ResultCommands.AddResultCmdDivideSuccess();
            return questBlock;
        }

        public static QuestBlock AddResultCmdDivideFailed(this QuestBlock questBlock)
        {
            questBlock.ResultCommands.AddResultCmdDivideFailed();
            return questBlock;
        }

        public static QuestBlock AddResultCmdEnableGetSetQuestList(this QuestBlock questBlock)
        {
            questBlock.ResultCommands.AddResultCmdEnableGetSetQuestList();
            return questBlock;
        }

        public static QuestBlock AddResultCmdPlayCameraEvent(this QuestBlock questBlock, StageInfo stageInfo, int eventNo)
        {
            questBlock.ResultCommands.AddResultCmdPlayCameraEvent(stageInfo, eventNo);
            return questBlock;
        }

        public static QuestBlock AddResultCmdPlayMessage(this QuestBlock questBlock, int groupNo, int waitTime)
        {
            questBlock.ResultCommands.AddResultCmdPlayMessage(groupNo, waitTime);
            return questBlock;
        }

        public static QuestBlock AddResultCmdStartTimer(this QuestBlock questBlock, int timerNo, int waitTimeInSec)
        {
            questBlock.ResultCommands.AddResultCmdStartTimer(timerNo, waitTimeInSec);
            return questBlock;
        }
    }
}
