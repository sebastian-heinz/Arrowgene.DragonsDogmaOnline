using Arrowgene.Ddon.GameServer.Characters;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Model.Quest
{
    public static class QuestBlockUtil
    {
        public static QuestBlock CreateGenericBlock(ushort blockNo, ushort seqNo, QuestBlockType blockType, QuestAnnounceType announceType)
        {
            return new QuestBlock(blockNo, seqNo)
                .SetBlockType(blockType)
                .SetAnnounceType(announceType);
        }

        public static QuestBlock AddNpcTalkAndOrderBlock(this QuestProcess process, StageId stageId, NpcId npcId, uint msgId)
        {
            var block = CreateGenericBlock(0, 0, QuestBlockType.NpcTalkAndOrder, QuestAnnounceType.None)
                .AddNpcOrderDetails(stageId, npcId, msgId, QuestId.None);
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddNewNpcTalkAndOrderBlock(this QuestProcess process, StageId stageId, NpcId npcId, uint msgId)
        {
            var block = CreateGenericBlock(0, 0, QuestBlockType.NewNpcTalkAndOrder, QuestAnnounceType.None)
                .AddNpcOrderDetails(stageId, npcId, msgId, QuestId.None);
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddQuestNpcTalkAndOrderBlock(this QuestProcess process, QuestId questId, StageId stageId, NpcId npcId, uint msgId)
        {
            var block = CreateGenericBlock(0, 0, QuestBlockType.NewNpcTalkAndOrder, QuestAnnounceType.None)
                .AddNpcOrderDetails(stageId, npcId, msgId, questId);
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddIsStageNoBlock(this QuestProcess process, QuestAnnounceType announceType, StageId stageId)
        {
            var block = CreateGenericBlock(0, 0, QuestBlockType.IsStageNo, announceType)
                .SetStageId(stageId);
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddTalkToNpcBlock(this QuestProcess process, QuestAnnounceType announceType, StageId stageId, NpcId npcId, uint msgId, bool showMarker = true)
        {
            var block = CreateGenericBlock(0, 0, QuestBlockType.TalkToNpc, announceType)
                .SetShowMarker(showMarker)
                .AddNpcOrderDetails(stageId, npcId, msgId, QuestId.None);
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddNewTalkToNpcBlock(this QuestProcess process, QuestAnnounceType announceType, StageId stageId, NpcId npcId, uint msgId, bool showMarker = true)
        {
            var block = CreateGenericBlock(0, 0, QuestBlockType.NewTalkToNpc, announceType)
                .SetShowMarker(showMarker)
                .AddNpcOrderDetails(stageId, npcId, msgId, QuestId.None);
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddNewTalkToNpcBlock(this QuestProcess process, QuestAnnounceType announceType, QuestId questId, StageId stageId, NpcId npcId, uint msgId, bool showMarker = true)
        {
            var block = CreateGenericBlock(0, 0, QuestBlockType.NewTalkToNpc, announceType)
                .SetShowMarker(showMarker)
                .AddNpcOrderDetails(stageId, npcId, msgId, questId);
            process.AddBlock(block);
            return block;
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

        public static QuestBlock AddPartyGatherBlock(this QuestProcess process, QuestAnnounceType announceType, StageId stageId, int x, int y, int z, bool showMarker = true)
        {
            var block = CreateGenericBlock(0, 0, QuestBlockType.PartyGather, announceType)
                .SetStageId(stageId)
                .SetShowMarker(showMarker)
                .SetPartyGatherPoint(x, y, z);
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddPartyGatherInStageBlock(this QuestProcess process, QuestAnnounceType announceType, StageId stageId, bool showMarker = true)
        {
            var block = CreateGenericBlock(0, 0, QuestBlockType.PartyGather, announceType)
                .SetShowMarker(showMarker)
                .SetStageId(stageId);
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddOmInteractEventBlock(this QuestProcess process, QuestAnnounceType announceType, StageId stageId, OmQuestType questType, OmInteractType interactType, QuestId questId = QuestId.None, bool showMarker = true)
        {
            var block = CreateGenericBlock(0, 0, QuestBlockType.OmInteractEvent, announceType)
                .SetShowMarker(showMarker)
                .SetStageId(stageId)
                .SetOmInteractEvent(interactType, questType, questId);
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddCheckBagEventBlock(this QuestProcess process, QuestAnnounceType announceType, ItemId itemId, int amount)
        {
            var block = CreateGenericBlock(0, 0, QuestBlockType.Raw, announceType)
                .AddCheckCommand(QuestManager.CheckCommand.HaveItemAllBag((int) itemId, amount));
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddDeliverItemsBlock(this QuestProcess process, QuestAnnounceType announceType, StageId stageId, NpcId npcId, ItemId itemId, uint amount, uint msgId)
        {
            var block = CreateGenericBlock(0, 0, QuestBlockType.DeliverItems, announceType)
                .AddNpcOrderDetails(stageId, npcId, msgId, QuestId.None)
                .AddDeliveryRequests(itemId, amount);
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddNewDeliverItemsBlock(this QuestProcess process, QuestAnnounceType announceType, StageId stageId, NpcId npcId, ItemId itemId, uint amount, uint msgId)
        {
            var block = CreateGenericBlock(0, 0, QuestBlockType.NewDeliverItems, announceType)
                .SetStageId(stageId)
                .AddNpcOrderDetails(stageId, npcId, msgId, QuestId.None)
                .AddDeliveryRequests(itemId, amount);
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

        public static QuestBlock AddPlayEventBlock(this QuestProcess process, QuestAnnounceType announceType, StageId stageId, uint eventId, uint startPos)
        {
            var block = CreateGenericBlock(0, 0, QuestBlockType.PlayEvent, announceType)
                .SetStageId(stageId)
                .SetQuestEvent(stageId, eventId, startPos, QuestJumpType.None);
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddPlayEventBlock(this QuestProcess process, QuestAnnounceType announceType, StageId stageId, uint eventId, uint startPos, QuestJumpType jumpType, StageId eventStageId)
        {
            var block = CreateGenericBlock(0, 0, QuestBlockType.PlayEvent, announceType)
                .SetStageId(stageId)
                .SetQuestEvent(eventStageId, eventId, startPos, jumpType);
            process.AddBlock(block);
            return block;
        }

        public static QuestBlock AddStageJumpBlock(this QuestProcess process, QuestAnnounceType announceType, StageId stageId, uint jumpPos)
        {
            var block = CreateGenericBlock(0, 0, QuestBlockType.StageJump, announceType)
                .SetStageId(stageId)
                .SetJumpPos(jumpPos);
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
    }
}
