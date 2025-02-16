using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Shared.Entity.Structure;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Model.Quest
{
    public static class QuestBlockUtil
    {
        private static QuestBlock CreateGenericBlock(ushort blockNo, ushort seqNo, QuestBlockType blockType, QuestAnnounceType announceType)
        {
            return new QuestBlock(blockNo, seqNo)
                .SetBlockType(blockType)
                .SetAnnounceType(announceType);
        }

        public static QuestBlock AddNpcTalkAndOrderBlock(this QuestProcess process, StageLayoutId stageId, NpcId npcId, uint msgId)
        {
            var block = CreateGenericBlock(0, 0, QuestBlockType.NpcTalkAndOrder, QuestAnnounceType.None)
                .AddNpcOrderDetails(stageId, npcId, msgId, QuestId.None);
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddNpcTalkAndOrderBlock(this QuestProcess process, StageInfo stageInfo, NpcId npcId, uint msgId)
        {
            return AddNpcTalkAndOrderBlock(process, stageInfo.AsStageLayoutId(0, 0), npcId, msgId);
        }

        public static QuestBlock AddNewNpcTalkAndOrderBlock(this QuestProcess process, StageLayoutId stageId, NpcId npcId, uint msgId)
        {
            var block = CreateGenericBlock(0, 0, QuestBlockType.NewNpcTalkAndOrder, QuestAnnounceType.None)
                .AddNpcOrderDetails(stageId, npcId, msgId, QuestId.None);
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddNewNpcTalkAndOrderBlock(this QuestProcess process, StageInfo stageInfo, uint groupId, byte setId, NpcId npcId, uint msgId)
        {
            return AddNewNpcTalkAndOrderBlock(process, stageInfo.AsStageLayoutId(setId, groupId), npcId, msgId);
        }

        public static QuestBlock AddQuestNpcTalkAndOrderBlock(this QuestProcess process, QuestId questId, StageLayoutId stageId, NpcId npcId, uint msgId)
        {
            var block = CreateGenericBlock(0, 0, QuestBlockType.NewNpcTalkAndOrder, QuestAnnounceType.None)
                .AddNpcOrderDetails(stageId, npcId, msgId, questId);
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddQuestNpcTalkAndOrderBlock(this QuestProcess process, QuestId questId, StageInfo stageInfo, uint groupId, byte setId, NpcId npcId, uint msgId)
        {
            return AddQuestNpcTalkAndOrderBlock(process, questId, stageInfo.AsStageLayoutId(setId, groupId), npcId, msgId);
        }

        public static QuestBlock AddIsStageNoBlock(this QuestProcess process, QuestAnnounceType announceType, StageLayoutId stageId)
        {
            var block = CreateGenericBlock(0, 0, QuestBlockType.IsStageNo, announceType)
                .SetStageId(stageId);
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddIsStageNoBlock(this QuestProcess process, QuestAnnounceType announceType, StageInfo stageInfo)
        {
            return AddIsStageNoBlock(process, announceType, stageInfo.AsStageLayoutId(0, 0));
        }

        public static QuestBlock AddTalkToNpcBlock(this QuestProcess process, QuestAnnounceType announceType, StageLayoutId stageId, NpcId npcId, uint msgId, bool showMarker = true)
        {
            var block = CreateGenericBlock(0, 0, QuestBlockType.TalkToNpc, announceType)
                .SetShowMarker(showMarker)
                .AddNpcOrderDetails(stageId, npcId, msgId, QuestId.None);
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddTalkToNpcBlock(this QuestProcess process, QuestAnnounceType announceType, StageInfo stageInfo, NpcId npcId, uint msgId, bool showMarker = true)
        {
            return AddTalkToNpcBlock(process, announceType, stageInfo.AsStageLayoutId(0, 0), npcId, msgId, showMarker);
        }

        public static QuestBlock AddNewTalkToNpcBlock(this QuestProcess process, QuestAnnounceType announceType, StageLayoutId stageId, NpcId npcId, uint msgId, bool showMarker = true)
        {
            var block = CreateGenericBlock(0, 0, QuestBlockType.NewTalkToNpc, announceType)
                .SetShowMarker(showMarker)
                .AddNpcOrderDetails(stageId, npcId, msgId, QuestId.None);
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddNewTalkToNpcBlock(this QuestProcess process, QuestAnnounceType announceType, StageInfo stageInfo, uint groupId, byte setId, NpcId npcId, uint msgId, bool showMarker = true)
        {
            return AddNewTalkToNpcBlock(process, announceType, stageInfo.AsStageLayoutId(setId, groupId), npcId, msgId, showMarker);
        }

        public static QuestBlock AddNewTalkToNpcBlock(this QuestProcess process, QuestAnnounceType announceType, QuestId questId, StageLayoutId stageId, NpcId npcId, uint msgId, bool showMarker = true)
        {
            var block = CreateGenericBlock(0, 0, QuestBlockType.NewTalkToNpc, announceType)
                .SetShowMarker(showMarker)
                .AddNpcOrderDetails(stageId, npcId, msgId, questId);
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddNewTalkToNpcBlock(this QuestProcess process, QuestAnnounceType announceType, QuestId questId, StageInfo stageInfo, uint groupId, byte setId, NpcId npcId, uint msgId, bool showMarker = true)
        {
            return AddNewTalkToNpcBlock(process, announceType, questId, stageInfo.AsStageLayoutId(setId, groupId), npcId, msgId, showMarker);
        }

        public static QuestBlock AddDiscoverGroupBlock(this QuestProcess process, QuestAnnounceType announceType, List<uint> groupIds, bool resetGroup = true, bool showMarker = true)
        {
            var block = CreateGenericBlock(0, 0, QuestBlockType.DiscoverEnemy, announceType)
                .SetShowMarker(showMarker)
                .SetResetGroup(resetGroup)
                .AddEnemyGroupIds(groupIds);
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddDiscoverGroupBlock(this QuestProcess process, QuestAnnounceType announceType, uint groupId, bool resetGroup = true, bool showMarker = true)
        {
            return AddDiscoverGroupBlock(process, announceType, new List<uint>() { groupId }, resetGroup, showMarker);
        }

        public static QuestBlock AddDestroyGroupBlock(this QuestProcess process, QuestAnnounceType announceType, List<uint> groupIds, bool resetGroup = true, bool showMarker = true)
        {
            var block = CreateGenericBlock(0, 0, QuestBlockType.KillGroup, announceType)
                .SetShowMarker(showMarker)
                .SetResetGroup(resetGroup)
                .AddEnemyGroupIds(groupIds);
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddDestroyGroupBlock(this QuestProcess process, QuestAnnounceType announceType, uint groupId, bool resetGroup = true, bool showMarker = true)
        {
            return AddDestroyGroupBlock(process, announceType, new List<uint>() { groupId }, resetGroup, showMarker);
        }

        public static QuestBlock AddSpawnGroupsBlock(this QuestProcess process, QuestAnnounceType announceType, List<uint> groupIds, bool resetGroup = true)
        {
            var block = CreateGenericBlock(0, 0, QuestBlockType.SpawnGroup, announceType)
                .SetResetGroup(resetGroup)
                .AddEnemyGroupIds(groupIds);
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddSpawnGroupBlock(this QuestProcess process, QuestAnnounceType announceType, uint groupId, bool resetGroup = true)
        {
            return AddSpawnGroupsBlock(process, announceType, new List<uint>() { groupId }, resetGroup);
        }

        public static QuestBlock AddPartyGatherBlock(this QuestProcess process, QuestAnnounceType announceType, StageLayoutId stageId, int x, int y, int z, bool showMarker = true)
        {
            var block = CreateGenericBlock(0, 0, QuestBlockType.PartyGather, announceType)
                .SetStageId(stageId)
                .SetShowMarker(showMarker)
                .SetPartyGatherPoint(x, y, z);
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddPartyGatherBlock(this QuestProcess process, QuestAnnounceType announceType, StageInfo stageInfo, int x, int y, int z, bool showMarker = true)
        {
            return AddPartyGatherBlock(process, announceType, stageInfo.AsStageLayoutId(0, 0), x, y, z, showMarker);
        }

        public static QuestBlock AddIsGatherPartyInStageBlock(this QuestProcess process, QuestAnnounceType announceType, StageLayoutId stageId)
        {
            var block = CreateGenericBlock(0, 0, QuestBlockType.IsGatherPartyInStage, announceType)
                .SetStageId(stageId);
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddIsGatherPartyInStageBlock(this QuestProcess process, QuestAnnounceType announceType, StageInfo stageInfo)
        {
            return AddIsGatherPartyInStageBlock(process, announceType, stageInfo.AsStageLayoutId(0, 0));
        }

        public static QuestBlock AddOmInteractEventBlock(this QuestProcess process, QuestAnnounceType announceType, StageLayoutId stageId, OmQuestType questType, OmInteractType interactType, QuestId questId = QuestId.None, bool showMarker = true)
        {
            var block = CreateGenericBlock(0, 0, QuestBlockType.OmInteractEvent, announceType)
                .SetShowMarker(showMarker)
                .SetStageId(stageId)
                .SetOmInteractEvent(interactType, questType, questId);
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddOmInteractEventBlock(this QuestProcess process, QuestAnnounceType announceType, StageInfo stageInfo, uint groupId, byte setId, OmQuestType questType, OmInteractType interactType, QuestId questId = QuestId.None, bool showMarker = true)
        {
            return AddOmInteractEventBlock(process, announceType, stageInfo.AsStageLayoutId(setId, groupId), questType, interactType, questId, showMarker);
        }

        public static QuestBlock AddCheckBagEventBlock(this QuestProcess process, QuestAnnounceType announceType, ItemId itemId, int amount)
        {
            var block = CreateGenericBlock(0, 0, QuestBlockType.Raw, announceType)
                .AddCheckCommand(QuestManager.CheckCommand.HaveItemAllBag((int) itemId, amount));
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddDeliverItemsBlock(this QuestProcess process, QuestAnnounceType announceType, StageLayoutId stageId, NpcId npcId, ItemId itemId, uint amount, uint msgId)
        {
            var block = CreateGenericBlock(0, 0, QuestBlockType.DeliverItems, announceType)
                .AddNpcOrderDetails(stageId, npcId, msgId, QuestId.None)
                .AddDeliveryRequests(itemId, amount);
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddDeliverItemsBlock(this QuestProcess process, QuestAnnounceType announceType, StageInfo stageInfo, NpcId npcId, ItemId itemId, uint amount, uint msgId)
        {
            return AddDeliverItemsBlock(process, announceType, stageInfo.AsStageLayoutId(0, 0), npcId, itemId, amount, msgId);
        }

        public static QuestBlock AddNewDeliverItemsBlock(this QuestProcess process, QuestAnnounceType announceType, StageLayoutId stageId, NpcId npcId, ItemId itemId, uint amount, uint msgId)
        {
            var block = CreateGenericBlock(0, 0, QuestBlockType.NewDeliverItems, announceType)
                .SetStageId(stageId)
                .AddNpcOrderDetails(stageId, npcId, msgId, QuestId.None)
                .AddDeliveryRequests(itemId, amount);
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddNewDeliverItemsBlock(this QuestProcess process, QuestAnnounceType announceType, StageInfo stageInfo, uint groupId, byte setId, NpcId npcId, ItemId itemId, uint amount, uint msgId)
        {
            return AddNewDeliverItemsBlock(process, announceType, stageInfo.AsStageLayoutId(setId, groupId), npcId, itemId, amount, msgId);
        }

        public static QuestBlock AddCheckSayBlock(this QuestProcess process, QuestAnnounceType announceType)
        {
            var block = CreateGenericBlock(0, 0, QuestBlockType.Raw, announceType)
                .AddCheckCommand(QuestManager.CheckCommand.SayMessage());
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddIsQuestClearBlock(this QuestProcess process, QuestAnnounceType announceType, QuestType questType, QuestId questId)
        {
            var block = CreateGenericBlock(0, 0, QuestBlockType.IsQuestClear, announceType)
                .SetQuestIsClearDetails(questType, questId);
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddIsQuestClearBlock(this QuestProcess process, QuestAnnounceType announceType, QuestType questType, uint questId)
        {
            return AddIsQuestClearBlock(process, announceType, questType, (QuestId) questId);
        }

        public static QuestBlock AddPlayEventBlock(this QuestProcess process, QuestAnnounceType announceType, StageLayoutId stageId, uint eventId, uint startPos)
        {
            var block = CreateGenericBlock(0, 0, QuestBlockType.PlayEvent, announceType)
                .SetStageId(stageId)
                .SetQuestEvent(stageId, eventId, startPos, QuestJumpType.None);
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddPlayEventBlock(this QuestProcess process, QuestAnnounceType announceType, StageInfo stageInfo, uint eventId, uint startPos)
        {
            return AddPlayEventBlock(process, announceType, stageInfo.AsStageLayoutId(0, 0), eventId, startPos);
        }

        public static QuestBlock AddPlayEventBlock(this QuestProcess process, QuestAnnounceType announceType, StageLayoutId stageId, uint eventId, uint startPos, QuestJumpType jumpType, StageLayoutId eventStageId)
        {
            var block = CreateGenericBlock(0, 0, QuestBlockType.PlayEvent, announceType)
                .SetStageId(stageId)
                .SetQuestEvent(eventStageId, eventId, startPos, jumpType);
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddPlayEventBlock(this QuestProcess process, QuestAnnounceType announceType, StageInfo stageInfo, uint eventId, uint startPos, QuestJumpType jumpType, StageInfo eventStageInfo)
        {
            return AddPlayEventBlock(process, announceType, stageInfo.AsStageLayoutId(0, 0), eventId, startPos, jumpType, eventStageInfo.AsStageLayoutId(0, 0));
        }

        public static QuestBlock AddStageJumpBlock(this QuestProcess process, QuestAnnounceType announceType, StageLayoutId stageId, uint jumpPos)
        {
            var block = CreateGenericBlock(0, 0, QuestBlockType.StageJump, announceType)
                .SetStageId(stageId)
                .SetJumpPos(jumpPos);
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddStageJumpBlock(this QuestProcess process, QuestAnnounceType announceType, StageInfo stageInfo, uint jumpPos)
        {
            return AddStageJumpBlock(process, announceType, stageInfo.AsStageLayoutId(0, 0), jumpPos);
        }

        public static QuestBlock AddMyQstFlagsBlock(this QuestProcess process, QuestAnnounceType announceType, List<uint> checkFlags, List<uint> setFlags)
        {
            var block = CreateGenericBlock(0, 0, QuestBlockType.MyQstFlags, announceType)
                .AddMyQstSetFlags(setFlags)
                .AddMyQstCheckFlags(checkFlags);
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddMyQstFlagsBlock(this QuestProcess process, QuestAnnounceType announceType)
        {
            var block = CreateGenericBlock(0, 0, QuestBlockType.MyQstFlags, announceType);
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddExtendTimeBlock(this QuestProcess process, QuestAnnounceType announceType, uint amount)
        {
            var block = CreateGenericBlock(0, 0, QuestBlockType.ExtendTime, announceType)
                .SetTimeAmount(amount);
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddProcessEndBlock(this QuestProcess process, bool isTerminal)
        {
            var block = CreateGenericBlock(0, 1, isTerminal ? QuestBlockType.End : QuestBlockType.None, QuestAnnounceType.None);
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddReturnCheckPointBlock(this QuestProcess process, ushort processNo, ushort blockNo)
        {
            var block = CreateGenericBlock(0, 0, QuestBlockType.ReturnCheckpoint, QuestAnnounceType.None)
                .SetCheckpointDetails(processNo, blockNo)
                .AddCheckCommand(QuestManager.CheckCommand.DummyNotProgress());
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddNoProgressBlock(this QuestProcess process)
        {
            var block = CreateGenericBlock(0, 0, QuestBlockType.Raw, QuestAnnounceType.None)
                .AddCheckCommand(QuestManager.CheckCommand.DummyNotProgress());
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddRawBlock(this QuestProcess process, QuestAnnounceType announceType)
        {
            var block = CreateGenericBlock(0, 0, QuestBlockType.Raw, announceType);
            process.AddBlock(block);
            return block;
        }

        #region "Quest Block Functions"
        public static QuestBlock AddGeneralAnnounce(this QuestBlock questBlock, QuestGeneralAnnounceType announceType, int msgNo)
        {
            questBlock.ResultCommands.Add(QuestManager.ResultCommand.CallGeneralAnnounce((int)announceType, msgNo));
            return questBlock;
        }

        public static QuestBlock AddStageAnnounce(this QuestBlock questBlock, QuestStageAnnounceType announceType, int waveNumber)
        {
            questBlock.ResultCommands.Add(QuestManager.ResultCommand.StageAnnounce((int)announceType, waveNumber));
            return questBlock;
        }

        public static QuestBlock AddEndContentsPurpose(this QuestBlock questBlock, int announceNo, QuestEndContentsAnnounceType announceType)
        {
            questBlock.ResultCommands.Add(QuestManager.ResultCommand.AddEndContentsPurpose(announceNo, (int) announceType));
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

        public static QuestBlock SetNpcMsg(this QuestBlock questBlock, NpcId npcId, int msgNo)
        {
            questBlock.ResultCommands.Add(QuestManager.ResultCommand.QstTalkChg(npcId, msgNo));
            return questBlock;
        }
        #endregion
    }
}
