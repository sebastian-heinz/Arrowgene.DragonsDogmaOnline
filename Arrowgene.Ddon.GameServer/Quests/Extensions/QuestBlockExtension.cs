using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Quests.Extensions
{
    public static class QuestBlockExtension
    {
        private static QuestBlock CreateGenericBlock(uint questScheduleId, ushort blockNo, ushort seqNo, QuestBlockType blockType, QuestAnnounceType announceType)
        {
            return new QuestBlock(blockNo, seqNo)
                .SetQuestScheduleId(questScheduleId)
                .SetBlockType(blockType)
                .SetAnnounceType(announceType);
        }

        public static QuestBlock AddRawBlock(this QuestProcess process, QuestAnnounceType announceType)
        {
            var block = CreateGenericBlock(process.QuestScheduleId, 0, 0, QuestBlockType.Raw, announceType);
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddNpcTalkAndOrderBlock(this QuestProcess process, StageLayoutId stageId, NpcId npcId, uint msgId)
        {
            var block = CreateGenericBlock(process.QuestScheduleId, 0, 0, QuestBlockType.NpcTalkAndOrder, QuestAnnounceType.None)
                .AddNpcOrderDetails(stageId, npcId, msgId, QuestId.None);
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddNpcTalkAndOrderBlock(this QuestProcess process, StageInfo stageInfo, NpcId npcId, uint msgId)
        {
            return process.AddNpcTalkAndOrderBlock(stageInfo.AsStageLayoutId(0, 0), npcId, msgId);
        }

        public static QuestBlock AddNewNpcTalkAndOrderBlock(this QuestProcess process, StageLayoutId stageId, NpcId npcId, uint msgId, QuestId questId = QuestId.None)
        {
            var block = CreateGenericBlock(process.QuestScheduleId, 0, 0, QuestBlockType.NewNpcTalkAndOrder, QuestAnnounceType.None)
                .AddNpcOrderDetails(stageId, npcId, msgId, questId);
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddNewNpcTalkAndOrderBlock(this QuestProcess process, StageInfo stageInfo, uint groupId, byte setId, NpcId npcId, uint msgId, QuestId questId = QuestId.None)
        {
            return process.AddNewNpcTalkAndOrderBlock(stageInfo.AsStageLayoutId(setId, groupId), npcId, msgId, questId);
        }

        public static QuestBlock AddQuestNpcTalkAndOrderBlock(this QuestProcess process, QuestId questId, StageLayoutId stageId, NpcId npcId, uint msgId)
        {
            var block = CreateGenericBlock(process.QuestScheduleId, 0, 0, QuestBlockType.NewNpcTalkAndOrder, QuestAnnounceType.None)
                .AddNpcOrderDetails(stageId, npcId, msgId, questId);
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddQuestNpcTalkAndOrderBlock(this QuestProcess process, QuestId questId, StageInfo stageInfo, uint groupId, byte setId, NpcId npcId, uint msgId)
        {
            return process.AddQuestNpcTalkAndOrderBlock(questId, stageInfo.AsStageLayoutId(setId, groupId), npcId, msgId);
        }

        public static QuestBlock AddIsStageNoBlock(this QuestProcess process, QuestAnnounceType announceType, StageInfo stageInfo, bool showMarker = true)
        {
            var block = CreateGenericBlock(process.QuestScheduleId, 0, 0, QuestBlockType.Raw, announceType)
                .AddCheckCmdIsStageNo(stageInfo, showMarker);
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddTalkToNpcBlock(this QuestProcess process, QuestAnnounceType announceType, StageLayoutId stageId, NpcId npcId, uint msgId, bool showMarker = true)
        {
            var block = CreateGenericBlock(process.QuestScheduleId, 0, 0, QuestBlockType.TalkToNpc, announceType)
                .SetShowMarker(showMarker)
                .AddNpcOrderDetails(stageId, npcId, msgId, QuestId.None);
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddTalkToNpcBlock(this QuestProcess process, QuestAnnounceType announceType, StageInfo stageInfo, NpcId npcId, uint msgId, bool showMarker = true)
        {
            return process.AddTalkToNpcBlock(announceType, stageInfo.AsStageLayoutId(0, 0), npcId, msgId, showMarker);
        }

        public static QuestBlock AddNewTalkToNpcBlock(this QuestProcess process, QuestAnnounceType announceType, StageLayoutId stageId, NpcId npcId, uint msgId, bool showMarker = true)
        {
            var block = CreateGenericBlock(process.QuestScheduleId, 0, 0, QuestBlockType.NewTalkToNpc, announceType)
                .SetShowMarker(showMarker)
                .AddNpcOrderDetails(stageId, npcId, msgId, QuestId.None);
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddNewTalkToNpcBlock(this QuestProcess process, QuestAnnounceType announceType, StageInfo stageInfo, uint groupId, byte setId, NpcId npcId, uint msgId, bool showMarker = true)
        {
            return process.AddNewTalkToNpcBlock(announceType, stageInfo.AsStageLayoutId(setId, groupId), npcId, msgId, showMarker);
        }

        public static QuestBlock AddNewTalkToNpcBlock(this QuestProcess process, QuestAnnounceType announceType, QuestId questId, StageLayoutId stageId, NpcId npcId, uint msgId, bool showMarker = true)
        {
            var block = CreateGenericBlock(process.QuestScheduleId, 0, 0, QuestBlockType.NewTalkToNpc, announceType)
                .SetShowMarker(showMarker)
                .AddNpcOrderDetails(stageId, npcId, msgId, questId);
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddNewTalkToNpcBlock(this QuestProcess process, QuestAnnounceType announceType, QuestId questId, StageInfo stageInfo, uint groupId, byte setId, NpcId npcId, uint msgId, bool showMarker = true)
        {
            return process.AddNewTalkToNpcBlock(announceType, questId, stageInfo.AsStageLayoutId(setId, groupId), npcId, msgId, showMarker);
        }

        public static QuestBlock AddDiscoverGroupBlock(this QuestProcess process, QuestAnnounceType announceType, List<uint> groupIds, bool resetGroup = true, bool showMarker = true)
        {
            var block = CreateGenericBlock(process.QuestScheduleId, 0, 0, QuestBlockType.Raw, announceType)
                .SetResetGroup(resetGroup)
                .AddEnemyGroupIds(groupIds);

            foreach (var enemyGroupId in groupIds)
            {
                if (!process.EnemyGroups.ContainsKey(enemyGroupId))
                {
                    throw new Exception($"Unable to locate '{process.QuestScheduleId}::{enemyGroupId}' for quest instance");
                }

                var enemyGroup = process.EnemyGroups[enemyGroupId];
                var stageInfo = Stage.StageInfoFromStageLayoutId(enemyGroup.StageLayoutId);
                switch (enemyGroup.TargetType)
                {
                    case QuestTargetType.Enemy:
                        block.AddCheckCmdIsEnemyFound(stageInfo, enemyGroup.StageLayoutId.GroupId, -1, showMarker);
                        break;
                    case QuestTargetType.EnemyForOrder:
                        block.AddCheckCmdIsEnemyFoundForOrder(stageInfo, enemyGroup.StageLayoutId.GroupId, -1);
                        break;
                    case QuestTargetType.ExmMain:
                        block.AddCheckCmdIsEnemyFoundGmMain(stageInfo, enemyGroup.StageLayoutId.GroupId, -1);
                        break;
                    case QuestTargetType.ExmSub:
                        block.AddCheckCmdIsEnemyFoundGmSub(stageInfo, enemyGroup.StageLayoutId.GroupId, -1);
                        break;
                }

                block.AddResultCmdQstLayoutFlagOn(enemyGroupId);
            }

            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddDiscoverGroupBlock(this QuestProcess process, QuestAnnounceType announceType, uint groupId, bool resetGroup = true, bool showMarker = true)
        {
            return process.AddDiscoverGroupBlock(announceType, new List<uint>() { groupId }, resetGroup, showMarker);
        }

        public static QuestBlock AddDestroyGroupBlock(this QuestProcess process, QuestAnnounceType announceType, List<uint> groupIds, bool resetGroup = true)
        {
            var block = CreateGenericBlock(process.QuestScheduleId, 0, 0, QuestBlockType.Raw, announceType)
                .SetResetGroup(resetGroup)
                .AddEnemyGroupIds(groupIds);

            foreach (var enemyGroupId in groupIds)
            {
                if (!process.EnemyGroups.ContainsKey(enemyGroupId))
                {
                    throw new Exception($"Unable to locate '{process.QuestScheduleId}::{enemyGroupId}' for quest instance");
                }

                var enemyGroup = process.EnemyGroups[enemyGroupId];
                var stageInfo = Stage.StageInfoFromStageLayoutId(enemyGroup.StageLayoutId);
                switch (enemyGroup.TargetType)
                {
                    case QuestTargetType.Enemy:
                        block.AddCheckCmdDieEnemy(stageInfo, enemyGroup.StageLayoutId.GroupId, -1);
                        break;
                    case QuestTargetType.EnemyForOrder:
                        block.AddCheckCmdIsKilledTargetEnemySetGroup(enemyGroupId);
                        break;
                    case QuestTargetType.ExmMain:
                        block.AddCheckCmdIsKilledTargetEnemySetGroupGmMain(enemyGroupId);
                        break;
                    case QuestTargetType.ExmSub:
                        block.AddCheckCmdIsKilledTargetEnemySetGroupGmSub(enemyGroupId);
                        break;
                }
            }
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddDestroyGroupBlock(this QuestProcess process, QuestAnnounceType announceType, uint groupId, bool resetGroup = true)
        {
            return process.AddDestroyGroupBlock(announceType, new List<uint>() { groupId }, resetGroup);
        }

        public static QuestBlock AddSpawnGroupsBlock(this QuestProcess process, QuestAnnounceType announceType, List<uint> groupIds, bool resetGroup = true)
        {
            var block = CreateGenericBlock(process.QuestScheduleId, 0, 0, QuestBlockType.SpawnGroup, announceType)
                .SetResetGroup(resetGroup)
                .AddEnemyGroupIds(groupIds);
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddSpawnGroupBlock(this QuestProcess process, QuestAnnounceType announceType, uint groupId, bool resetGroup = true)
        {
            return process.AddSpawnGroupsBlock(announceType, new List<uint>() { groupId }, resetGroup);
        }

        public static QuestBlock AddIsBrokenLayoutBlock(this QuestProcess process, QuestAnnounceType announceType, StageInfo stageInfo, int groupId, int setNo, bool showMarker = true)
        {
            var block = CreateGenericBlock(process.QuestScheduleId, 0, 0, QuestBlockType.Raw, announceType)
                .AddCheckCommand(showMarker ?
                    QuestManager.CheckCommand.IsOmBrokenLayout(stageInfo.StageNo, groupId, setNo) :
                    QuestManager.CheckCommand.IsOmBrokenLayoutNoMarker(stageInfo.StageNo, groupId, setNo));
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddPartyGatherBlock(this QuestProcess process, QuestAnnounceType announceType, StageLayoutId stageId, int x, int y, int z, bool showMarker = true)
        {
            var block = CreateGenericBlock(process.QuestScheduleId, 0, 0, QuestBlockType.PartyGather, announceType)
                .SetStageId(stageId)
                .SetShowMarker(showMarker)
                .SetPartyGatherPoint(x, y, z);
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddPartyGatherBlock(this QuestProcess process, QuestAnnounceType announceType, StageInfo stageInfo, int x, int y, int z, bool showMarker = true)
        {
            return process.AddPartyGatherBlock(announceType, stageInfo.AsStageLayoutId(0, 0), x, y, z, showMarker);
        }

        public static QuestBlock AddIsGatherPartyInStageBlock(this QuestProcess process, QuestAnnounceType announceType, StageLayoutId stageId)
        {
            var block = CreateGenericBlock(process.QuestScheduleId, 0, 0, QuestBlockType.IsGatherPartyInStage, announceType)
                .SetStageId(stageId);
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddIsGatherPartyInStageBlock(this QuestProcess process, QuestAnnounceType announceType, StageInfo stageInfo)
        {
            return process.AddIsGatherPartyInStageBlock(announceType, stageInfo.AsStageLayoutId(0, 0));
        }

        public static QuestBlock AddSceHitInBlock(this QuestProcess process, QuestAnnounceType announceType, StageInfo stageInfo, int sceNo, bool showMarker = true)
        {
            var block = CreateGenericBlock(process.QuestScheduleId, 0, 0, QuestBlockType.None, announceType)
                .AddCheckCmdSceHitIn(stageInfo, sceNo, showMarker);
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddSceHitOutBlock(this QuestProcess process, QuestAnnounceType announceType, StageInfo stageInfo, int sceNo, bool showMarker = true)
        {
            var block = CreateGenericBlock(process.QuestScheduleId, 0, 0, QuestBlockType.None, announceType)
                .AddCheckCmdSceHitOut(stageInfo, sceNo, showMarker);
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddOmInteractEventBlock(this QuestProcess process, QuestAnnounceType announceType, StageLayoutId stageId, OmQuestType questType, OmInteractType interactType, QuestId questId = QuestId.None, bool showMarker = true)
        {
            var block = CreateGenericBlock(process.QuestScheduleId, 0, 0, QuestBlockType.OmInteractEvent, announceType)
                .SetShowMarker(showMarker)
                .SetStageId(stageId)
                .SetOmInteractEvent(interactType, questType, questId);
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddOmInteractEventBlock(this QuestProcess process, QuestAnnounceType announceType, StageInfo stageInfo, uint groupId, byte setId, OmQuestType questType, OmInteractType interactType, QuestId questId = QuestId.None, bool showMarker = true)
        {
            return process.AddOmInteractEventBlock(announceType, stageInfo.AsStageLayoutId(setId, groupId), questType, interactType, questId, showMarker);
        }

        public static QuestBlock AddCheckBagEventBlock(this QuestProcess process, QuestAnnounceType announceType, ItemId itemId, int amount)
        {
            var block = CreateGenericBlock(process.QuestScheduleId, 0, 0, QuestBlockType.Raw, announceType)
                .AddCheckCommand(QuestManager.CheckCommand.HaveItemAllBag((int)itemId, amount));
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddDeliverItemsBlock(this QuestProcess process, QuestAnnounceType announceType, StageLayoutId stageId, NpcId npcId, ItemId itemId, uint amount, uint msgId)
        {
            var block = CreateGenericBlock(process.QuestScheduleId, 0, 0, QuestBlockType.DeliverItems, announceType)
                .AddNpcOrderDetails(stageId, npcId, msgId, QuestId.None)
                .AddDeliveryRequests(itemId, amount);
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddDeliverItemsBlock(this QuestProcess process, QuestAnnounceType announceType, StageInfo stageInfo, NpcId npcId, ItemId itemId, uint amount, uint msgId)
        {
            return process.AddDeliverItemsBlock(announceType, stageInfo.AsStageLayoutId(0, 0), npcId, itemId, amount, msgId);
        }

        public static QuestBlock AddNewDeliverItemsBlock(this QuestProcess process, QuestAnnounceType announceType, StageLayoutId stageId, NpcId npcId, ItemId itemId, uint amount, uint msgId)
        {
            var block = CreateGenericBlock(process.QuestScheduleId, 0, 0, QuestBlockType.NewDeliverItems, announceType)
                .SetStageId(stageId)
                .AddNpcOrderDetails(stageId, npcId, msgId, QuestId.None)
                .AddDeliveryRequests(itemId, amount);
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddNewDeliverItemsBlock(this QuestProcess process, QuestAnnounceType announceType, StageInfo stageInfo, uint groupId, byte setId, NpcId npcId, ItemId itemId, uint amount, uint msgId)
        {
            return process.AddNewDeliverItemsBlock(announceType, stageInfo.AsStageLayoutId(setId, groupId), npcId, itemId, amount, msgId);
        }

        public static QuestBlock AddCheckSayBlock(this QuestProcess process, QuestAnnounceType announceType)
        {
            var block = CreateGenericBlock(process.QuestScheduleId, 0, 0, QuestBlockType.Raw, announceType)
                .AddCheckCommand(QuestManager.CheckCommand.SayMessage());
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddIsQuestClearBlock(this QuestProcess process, QuestAnnounceType announceType, QuestType questType, QuestId questId)
        {
            var block = CreateGenericBlock(process.QuestScheduleId, 0, 0, QuestBlockType.IsQuestClear, announceType)
                .SetQuestIsClearDetails(questType, questId);
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddIsQuestClearBlock(this QuestProcess process, QuestAnnounceType announceType, QuestType questType, uint questId)
        {
            return process.AddIsQuestClearBlock(announceType, questType, (QuestId)questId);
        }

        public static QuestBlock AddPlayEventBlock(this QuestProcess process, QuestAnnounceType announceType, StageLayoutId stageId, uint eventId, uint startPos)
        {
            var block = CreateGenericBlock(process.QuestScheduleId, 0, 0, QuestBlockType.PlayEvent, announceType)
                .SetStageId(stageId)
                .SetQuestEvent(stageId, eventId, startPos, QuestJumpType.None);
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddPlayEventBlock(this QuestProcess process, QuestAnnounceType announceType, StageInfo stageInfo, uint eventId, uint startPos)
        {
            return process.AddPlayEventBlock(announceType, stageInfo.AsStageLayoutId(0, 0), eventId, startPos);
        }

        public static QuestBlock AddPlayEventBlock(this QuestProcess process, QuestAnnounceType announceType, StageLayoutId stageId, uint eventId, uint startPos, QuestJumpType jumpType, StageLayoutId eventStageId)
        {
            var block = CreateGenericBlock(process.QuestScheduleId, 0, 0, QuestBlockType.PlayEvent, announceType)
                .SetStageId(stageId)
                .SetQuestEvent(eventStageId, eventId, startPos, jumpType);
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddPlayEventBlock(this QuestProcess process, QuestAnnounceType announceType, StageInfo stageInfo, uint eventId, uint startPos, QuestJumpType jumpType, StageInfo eventStageInfo)
        {
            return process.AddPlayEventBlock(announceType, stageInfo.AsStageLayoutId(0, 0), eventId, startPos, jumpType, eventStageInfo.AsStageLayoutId(0, 0));
        }

        public static QuestBlock AddEventExecBlock(this QuestProcess process, QuestAnnounceType announceType, StageInfo stageInfo, uint eventNo, StageInfo destStage, uint jumpPos)
        {
            var block = CreateGenericBlock(process.QuestScheduleId, 0, 0, QuestBlockType.PlayEvent, announceType)
                .AddResultCmdEventExec(stageInfo, eventNo, destStage, jumpPos)
                .AddCheckCmdEventEnd(stageInfo, eventNo);
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddEventExecBlock(this QuestProcess process, QuestAnnounceType announceType, StageInfo stageInfo, uint eventNo)
        {
            return AddEventExecBlock(process, announceType, stageInfo, eventNo, stageInfo, 0);
        }

        public static QuestBlock AddStageJumpBlock(this QuestProcess process, QuestAnnounceType announceType, StageLayoutId stageId, uint jumpPos)
        {
            var block = CreateGenericBlock(process.QuestScheduleId, 0, 0, QuestBlockType.StageJump, announceType)
                .SetStageId(stageId)
                .SetJumpPos(jumpPos);
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddStageJumpBlock(this QuestProcess process, QuestAnnounceType announceType, StageInfo stageInfo, uint jumpPos)
        {
            return process.AddStageJumpBlock(announceType, stageInfo.AsStageLayoutId(0, 0), jumpPos);
        }

        public static QuestBlock AddEventAfterJumpBlock(this QuestProcess process, QuestAnnounceType announceType, StageInfo stageInfo, uint eventNo, uint jumpPos)
        {
            var block = CreateGenericBlock(process.QuestScheduleId, 0, 0, QuestBlockType.Raw, announceType)
                            .AddCheckCmdEventEnd(stageInfo, eventNo)
                            .AddResultCmdExeEventAfterStageJump(stageInfo, eventNo, jumpPos);
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddWaitForEventEndBlock(this QuestProcess process, QuestAnnounceType announceType, StageInfo stageInfo, int eventNo)
        {
            var block = CreateGenericBlock(process.QuestScheduleId, 0, 0, QuestBlockType.Raw, announceType)
                .AddCheckCommand(QuestManager.CheckCommand.EventEnd(stageInfo.StageNo, eventNo));
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddMyQstFlagsBlock(this QuestProcess process, QuestAnnounceType announceType, List<uint> checkFlags, List<uint> setFlags)
        {
            var block = CreateGenericBlock(process.QuestScheduleId, 0, 0, QuestBlockType.MyQstFlags, announceType)
                .AddMyQstSetFlags(setFlags)
                .AddMyQstCheckFlags(checkFlags);
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddMyQstFlagsBlock(this QuestProcess process, QuestAnnounceType announceType)
        {
            return process.AddMyQstFlagsBlock(announceType, new List<uint>() { }, new List<uint>() { });
        }

        public static QuestBlock AddExtendTimeBlock(this QuestProcess process, QuestAnnounceType announceType, uint amount)
        {
            var block = CreateGenericBlock(process.QuestScheduleId, 0, 0, QuestBlockType.ExtendTime, announceType)
                .SetTimeAmount(amount);
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddProcessEndBlock(this QuestProcess process, QuestAnnounceType announceType, bool isTerminal)
        {
            var block = CreateGenericBlock(process.QuestScheduleId, 0, 1, isTerminal ? QuestBlockType.End : QuestBlockType.None, announceType);
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddProcessEndBlock(this QuestProcess process, bool isTerminal)
        {
            return process.AddProcessEndBlock(QuestAnnounceType.None, isTerminal);
        }

        public static QuestBlock AddReturnCheckPointBlock(this QuestProcess process, ushort processNo, ushort blockNo)
        {
            var block = CreateGenericBlock(process.QuestScheduleId, 0, 0, QuestBlockType.ReturnCheckpoint, QuestAnnounceType.None)
                .SetCheckpointDetails(processNo, blockNo)
                .AddCheckCommand(QuestManager.CheckCommand.DummyNotProgress());
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddNoProgressBlock(this QuestProcess process)
        {
            var block = CreateGenericBlock(process.QuestScheduleId, 0, 0, QuestBlockType.Raw, QuestAnnounceType.None)
                .AddCheckCommand(QuestManager.CheckCommand.DummyNotProgress());
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddDelayBlock(this QuestProcess process, QuestAnnounceType announceType, int timerNo, int waitTimeInSeconds)
        {
            var block = CreateGenericBlock(process.QuestScheduleId, 0, 0, QuestBlockType.Raw, announceType)
                .AddResultCmdStartTimer(timerNo, waitTimeInSeconds)
                .AddCheckCmdIsEndTimer(timerNo);
            process.AddBlock(block);
            return block;
        }

        // QuestBlock PseudoCommand Functions
        public static QuestBlock AddCallback(this QuestBlock questBlock, Action<QuestCallbackParam> callback)
        {
            questBlock.Callbacks.Add(callback);
            return questBlock;
        }

        public static QuestBlock AddWorkCommand(this QuestBlock questBlock, QuestNotifyCommand notifyCommand, int work01 = 0, int work02 = 0, int work03 = 0, int work04 = 0)
        {
            questBlock.WorkCommands.Add(new CDataQuestProgressWork()
            {
                CommandNo = (uint)notifyCommand,
                Work01 = work01,
                Work02 = work02,
                Work03 = work03,
                Work04 = work04,
            });
            return questBlock;
        }

        public static QuestBlock AddContentsReleased(this QuestBlock questBlock, HashSet<ContentsRelease> releaseIds)
        {
            questBlock.ContentsReleased.UnionWith(releaseIds.Select(x => new QuestUnlock() { ReleaseId = x }).ToHashSet());
            return questBlock;
        }

        public static QuestBlock AddContentsReleased(this QuestBlock questBlock, ContentsRelease releaseId)
        {
            return AddContentsReleased(questBlock, new HashSet<ContentsRelease> { releaseId });
        }

        public static QuestBlock AddWorldManageUnlock(this QuestBlock questBlock, QuestFlagInfo questFlagInfo)
        {
            questBlock.WorldManageUnlocks.Add(questFlagInfo);
            questBlock.AddQuestFlag(QuestFlagAction.Set, questFlagInfo);
            return questBlock;
        }
    }
}
