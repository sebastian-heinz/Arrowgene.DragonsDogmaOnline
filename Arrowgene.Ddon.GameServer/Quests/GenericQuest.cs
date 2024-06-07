using System.Collections;
using System.Collections.Generic;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Asset;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Quests
{
    public class GenericQuest : Quest
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(GenericQuest));

        private List<QuestProcess> Processes;

        public static GenericQuest FromAsset(QuestAssetData questAsset)
        {
            var quest = new GenericQuest(questAsset.QuestId, questAsset.QuestScheduleId, questAsset.Type, questAsset.Discoverable);

            quest.BaseLevel = questAsset.BaseLevel;
            quest.MinimumItemRank = questAsset.MinimumItemRank;
            quest.NextQuestId = questAsset.NextQuestId;
            quest.QuestLayoutFlagSetInfo = questAsset.QuestLayoutSetInfoFlags;
            quest.QuestLayoutFlags = questAsset.QuestLayoutFlags;
            quest.Processes = questAsset.Processes;
            quest.EnemyGroups = questAsset.EnemyGroups;
            quest.ResetPlayerAfterQuest = questAsset.ResetPlayerAfterQuest;
            quest.OrderConditions = questAsset.OrderConditions;

            quest.ExpRewards.Add(new CDataQuestExp()
            {
                Type = questAsset.ExpType,
                Reward = questAsset.ExpReward
            });

            foreach (var walletReward in questAsset.RewardCurrency)
            {
                quest.WalletRewards.Add(new CDataWalletPoint()
                {
                    Type = walletReward.WalletType,
                    Value = walletReward.Amount
                });
            }

            foreach (var rewardItem in questAsset.RewardItems)
            {
                switch (rewardItem.RewardType)
                {
                    case QuestRewardType.Random:
                    case QuestRewardType.Fixed:
                        quest.ItemRewards.Add(rewardItem);
                        break;
                    case QuestRewardType.Select:
                        quest.SelectedableItemRewards.Add(rewardItem);
                        break;
                }
            }

            foreach (var process in quest.Processes)
            {
                foreach (var block in process.Blocks)
                {
                    switch (block.BlockType)
                    {
                        case QuestBlockType.KillGroup:
                            {
                                foreach (var groupId in block.EnemyGroupIds)
                                {
                                    var enemyGroup = quest.EnemyGroups[groupId];
                                    quest.Locations.Add(new QuestLocation() { StageId = enemyGroup.StageId, SubGroupId = 0 });
                                }
                            }
                            break;
                        case QuestBlockType.DeliverItems:
                            foreach (var request in block.DeliveryRequests)
                            {
                                quest.DeliveryItems.Add(new QuestDeliveryItem()
                                {
                                    ItemId = request.ItemId,
                                    Amount = request.Amount,
                                });
                            }
                            break;
                    }

                    block.QuestProcessState = BlockAsCDataQuestProcessState(quest, block);
                }
            }

            return quest;
        }

        public GenericQuest(QuestId questId, QuestId questScheduleId, QuestType questType, bool discoverable) : base(questId, questScheduleId, questType, discoverable)
        {
            Processes = new List<QuestProcess>();
            QuestLayoutFlagSetInfo = new List<QuestLayoutFlagSetInfo>();
        }

        public override bool HasEnemiesInCurrentStageGroup(QuestState questState, StageId stageId, uint subGroupId)
        {
            // Search to see if a target is required for the current process
            foreach (var process in Processes)
            {
                foreach (var block in process.Blocks)
                {
                    if (!questState.ProcessState.ContainsKey(block.ProcessNo))
                    {
                        continue;
                    }

                    var processState = questState.ProcessState[block.ProcessNo];
                    if (processState.BlockNo != block.BlockNo)
                    {
                        continue;
                    }

                    foreach (var groupId in block.EnemyGroupIds)
                    {
                        var enemyGroup = EnemyGroups[groupId];
                        if ((enemyGroup.StageId.Id == stageId.Id) && (enemyGroup.StageId.GroupId == stageId.GroupId))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public override List<InstancedEnemy> GetEnemiesInStageGroup(StageId stageId, uint subGroupId)
        {
            List<InstancedEnemy> enemies = new List<InstancedEnemy>();

            foreach (var enemyGroup in EnemyGroups.Values)
            {
                if (enemyGroup.StageId.Id != stageId.Id || enemyGroup.StageId.GroupId != stageId.GroupId)
                {
                    continue;
                }

                byte index = 0;
                foreach (var enemy in enemyGroup.Enemies)
                {
                    enemies.Add(new InstancedEnemy(enemy)
                    {
                        Index = (byte)(index + enemyGroup.StartingIndex)
                    });

                    index += 1;
                }
            }

            return enemies;
        }

        public override void SendProgressWorkNotices(GameClient client, StageId stageId, uint subGroupId)
        {
            QuestLocation questLocation = null;
            foreach (var location in Locations)
            {
                if (location.ContainsStageId(stageId, (ushort)subGroupId))
                {
                    questLocation = location;
                    break;
                }
            }

            if (questLocation == null)
            {
                client.Party.SendToAll(new S2CQuestQuestProgressWorkSaveNtc());
            }
            else
            {
                var proccessState = client.Party.QuestState.GetProcessState(QuestId, 0);
                // We need to signal the current block
                client.Party.SendToAll(new S2CQuestQuestProgressWorkSaveNtc()
                {
                    QuestScheduleId = (uint)QuestId,
                    ProcessNo = proccessState.ProcessNo,
                    SequenceNo = 0,
                    BlockNo = proccessState.BlockNo,
                    WorkList = new List<CDataQuestProgressWork>()
                    {
                        QuestManager.NotifyCommand.KilledTargetEnemySetGroup((int) questLocation.QuestLayoutFlag, StageManager.ConvertIdToStageNo(stageId), (int) stageId.GroupId)
                    }
                });
            }
        }

        public override CDataQuestList ToCDataQuestList()
        {
            var quest = base.ToCDataQuestList();

            foreach (var process in Processes)
            {
                if (process.Blocks.Count > 0)
                {
                    quest.QuestProcessStateList.Add(process.Blocks[0].QuestProcessState);
                }
            }

            return quest;
        }

        public override CDataQuestOrderList ToCDataQuestOrderList()
        {
            var quest = base.ToCDataQuestOrderList();

            foreach (var process in Processes)
            {
                if (process.Blocks.Count > 0)
                {
                    quest.QuestProcessStateList.Add(process.Blocks[0].QuestProcessState);
                }
            }

            return quest;
        }

        public override List<CDataQuestProcessState> StateMachineExecute(GameClient client, QuestProcessState processState, out QuestProgressState questProgressState)
        {
            if (processState.ProcessNo >= Processes.Count)
            {
                questProgressState = QuestProgressState.Unknown;
                return new List<CDataQuestProcessState>();
            }

            var process = Processes[processState.ProcessNo];
            if ((processState.BlockNo) >= process.Blocks.Count)
            {
                questProgressState = QuestProgressState.Unknown;
                return new List<CDataQuestProcessState>();
            }

            var questBlock = process.Blocks[processState.BlockNo];
            if (processState.ProcessNo == 0 && questBlock.SequenceNo == 1)
            {
                questProgressState = QuestProgressState.Complete;
            }
            else
            {
                questProgressState = QuestProgressState.InProgress;
            }

            if (questBlock.ResetGroup)
            {
                ResetEnemiesForBlock(client, QuestId, questBlock);
            }

            return new List<CDataQuestProcessState>()
            {
                questBlock.QuestProcessState
            };
        }

        private static CDataQuestProcessState BlockAsCDataQuestProcessState(GenericQuest quest, QuestBlock questBlock)
        {
            CDataQuestProcessState result = new CDataQuestProcessState()
            {
                ProcessNo = questBlock.ProcessNo,
                SequenceNo = questBlock.SequenceNo,
                BlockNo = questBlock.BlockNo,
            };

            List<CDataQuestCommand> resultCommands = new List<CDataQuestCommand>();
            List<CDataQuestCommand> checkCommands = new List<CDataQuestCommand>();

            ParseQuestFlags(questBlock, resultCommands, checkCommands);

            if (questBlock.ShouldStageJump)
            {
                resultCommands.Add(QuestManager.ResultCommand.StageJump(StageManager.ConvertIdToStageNo(questBlock.StageId), 0));
            }

            if (questBlock.SequenceNo != 1)
            {
                switch (questBlock.AnnounceType)
                {
                    case QuestAnnounceType.Accept:
                        resultCommands.Add(QuestManager.ResultCommand.SetAnnounce(QuestAnnounceType.Accept, 1));
                        break;
                    case QuestAnnounceType.Update:
                        resultCommands.Add(QuestManager.ResultCommand.UpdateAnnounce());
                        break;
                }
            }

            switch (questBlock.BlockType)
            {
                case QuestBlockType.NpcTalkAndOrder:
                    checkCommands.Add(QuestManager.CheckCommand.NpcTalkAndOrderUi(StageManager.ConvertIdToStageNo(questBlock.NpcOrderDetails[0].StageId), questBlock.NpcOrderDetails[0].NpcId, 0));
                    resultCommands.Add(QuestManager.ResultCommand.QstTalkChg(questBlock.NpcOrderDetails[0].NpcId, questBlock.NpcOrderDetails[0].MsgId));
                    break;
                case QuestBlockType.PartyGather:
                    checkCommands.Add(QuestManager.CheckCommand.Prt(StageManager.ConvertIdToStageNo(questBlock.StageId), questBlock.PartyGatherPoint.x, questBlock.PartyGatherPoint.y, questBlock.PartyGatherPoint.z));
                    resultCommands.Add(QuestManager.ResultCommand.Prt(StageManager.ConvertIdToStageNo(questBlock.StageId), questBlock.PartyGatherPoint.x, questBlock.PartyGatherPoint.y, questBlock.PartyGatherPoint.z));
                    break;
                case QuestBlockType.DiscoverEnemy:
                case QuestBlockType.SeekOutEnemiesAtMarkedLocation:
                    {
                        foreach (var groupId in questBlock.EnemyGroupIds)
                        {
                            var enemyGroup = quest.EnemyGroups[groupId];
                            checkCommands.Add(QuestManager.CheckCommand.IsEnemyFoundForOrder(StageManager.ConvertIdToStageNo(enemyGroup.StageId), (int)enemyGroup.StageId.GroupId, -1));
                        }
                    }
                    break;
                case QuestBlockType.End:
                    resultCommands.Add(QuestManager.ResultCommand.SetAnnounce(QuestAnnounceType.Clear));
                    resultCommands.Add(QuestManager.ResultCommand.EndEndQuest());
                    break;
                case QuestBlockType.KillGroup:
                    {
                        foreach (var groupId in questBlock.EnemyGroupIds)
                        {
                            var enemyGroup = quest.EnemyGroups[groupId];
                            checkCommands.Add(QuestManager.CheckCommand.DieEnemy(StageManager.ConvertIdToStageNo(enemyGroup.StageId), (int)enemyGroup.StageId.GroupId, -1));
                        }
                    }
                    break;
                case QuestBlockType.CollectItem:
                    {
                        var questCommand = questBlock.ShowMarker ?
                            QuestManager.CheckCommand.QuestOmSetTouch(StageManager.ConvertIdToStageNo(questBlock.StageId), (int)questBlock.StageId.GroupId, 1) :
                            QuestManager.CheckCommand.QuestOmSetTouchWithoutMarker(StageManager.ConvertIdToStageNo(questBlock.StageId), (int)questBlock.StageId.GroupId, 1);
                        checkCommands.Add(questCommand);
                    }
                    break;
                case QuestBlockType.DeliverItems:
                    {
                        foreach (var item in questBlock.DeliveryRequests)
                        {
                            checkCommands.Add(QuestManager.CheckCommand.DeliverItem((int)item.ItemId, (int)item.Amount, questBlock.NpcOrderDetails[0].NpcId, questBlock.NpcOrderDetails[0].MsgId));
                        }
                        resultCommands.Add(QuestManager.ResultCommand.SetDeliverInfo(StageManager.ConvertIdToStageNo(questBlock.StageId), questBlock.NpcOrderDetails[0].NpcId, questBlock.NpcOrderDetails[0].MsgId));
                    }
                    break;
                case QuestBlockType.TalkToNpc:
                    checkCommands.Add(QuestManager.CheckCommand.TalkNpc(StageManager.ConvertIdToStageNo(questBlock.NpcOrderDetails[0].StageId), questBlock.NpcOrderDetails[0].NpcId));
                    resultCommands.Add(QuestManager.ResultCommand.QstTalkChg(questBlock.NpcOrderDetails[0].NpcId, questBlock.NpcOrderDetails[0].MsgId));
                    break;
                case QuestBlockType.IsQuestOrdered:
                    {
                        switch (questBlock.QuestOrderDetails.QuestType)
                        {
                            case QuestType.Main:
                                checkCommands.Add(QuestManager.CheckCommand.IsMainQuestOrder((int)questBlock.QuestOrderDetails.QuestId));
                                break;
                            case QuestType.World:
                                checkCommands.Add(QuestManager.CheckCommand.IsOrderWorldQuest((int)questBlock.QuestOrderDetails.QuestId));
                                break;
                            case QuestType.PersonalQuest:
                                checkCommands.Add(QuestManager.CheckCommand.IsOrderLightQuest((int)questBlock.QuestOrderDetails.QuestId));
                                break;
                            case QuestType.PawnExpedition:
                            case QuestType.PartnerPawnPersonalQuests:
                                // TODO: Needs different arguments
                                // questCommand = QuestManager.CheckCommand.IsOrderPawnQuest((int)questBlock.QuestOrderDetails.QuestId);
                                break;
                            case QuestType.Board:
                            case QuestType.Hidden:
                            case QuestType.ExtremeMissions:
                                break;
                        }
                    }
                    break;
                case QuestBlockType.MyQstFlags:
                    {
                        foreach (var flag in questBlock.MyQstCheckFlags)
                        {
                            checkCommands.Add(QuestManager.CheckCommand.MyQstFlagOn((int)flag));
                        }

                        List<CDataQuestCommand> setFlags = new List<CDataQuestCommand>();
                        foreach (var flag in questBlock.MyQstSetFlags)
                        {
                            resultCommands.Add(QuestManager.ResultCommand.MyQstFlagOn((int)flag));
                        }
                    }
                    break;
                case QuestBlockType.IsStageNo:
                    checkCommands.Add(QuestManager.CheckCommand.StageNo(StageManager.ConvertIdToStageNo(questBlock.StageId)));
                    break;
                case QuestBlockType.PlayEvent:
                    checkCommands.Add(QuestManager.CheckCommand.EventEnd(StageManager.ConvertIdToStageNo(questBlock.StageId), questBlock.QuestEvent.EventId));
                    resultCommands.Add(QuestManager.ResultCommand.EventExec(
                        StageManager.ConvertIdToStageNo(questBlock.StageId),
                        questBlock.QuestEvent.EventId,
                        StageManager.ConvertIdToStageNo(questBlock.QuestEvent.JumpStageId),
                        questBlock.QuestEvent.StartPosNo
                    ));
                    break;
                case QuestBlockType.Raw:
                    checkCommands.AddRange(questBlock.CheckCommands);
                    resultCommands.AddRange(questBlock.ResultCommands);
                    break;
                case QuestBlockType.DummyBlock:
                    /* Filler block which might do some meta things like announce or set flags */
                    break;
                case QuestBlockType.None:
                    /* Place holder for terminal blocks in process other than 0 */
                    break;
                default:
                    Logger.Error($"Unexpected block found '{questBlock.BlockType}'. Unable to translate.");
                    break;
            }

            result.ResultCommandList = resultCommands;
            if (checkCommands.Count > 0)
            {
                result.CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(checkCommands);
            }

            return result;
        }

        private static void ParseQuestFlags(QuestBlock questBlock, List<CDataQuestCommand> resultFlags, List<CDataQuestCommand> checkFlags)
        {
            foreach (var questFlag in questBlock.QuestFlags)
            {
                switch (questFlag.Type)
                {
                    case QuestFlagType.QstLayout:
                        switch (questFlag.Action)
                        {
                            case QuestFlagAction.Set:
                                resultFlags.Add(QuestManager.ResultCommand.QstLayoutFlagOn(questFlag.Value));
                                break;
                            case QuestFlagAction.Clear:
                                resultFlags.Add(QuestManager.ResultCommand.QstLayoutFlagOff(questFlag.Value));
                                break;
                            case QuestFlagAction.CheckOn:
                            case QuestFlagAction.CheckOff:
                                /* Invalid for Layout flags */
                                return;
                        }
                        break;
                    case QuestFlagType.WorldManageLayout:
                        switch (questFlag.Action)
                        {
                            case QuestFlagAction.Set:
                                resultFlags.Add(QuestManager.ResultCommand.WorldManageLayoutFlagOn(questFlag.Value, questFlag.QuestId));
                                break;
                            case QuestFlagAction.Clear:
                                resultFlags.Add(QuestManager.ResultCommand.WorldManageLayoutFlagOff(questFlag.Value, questFlag.QuestId));
                                break;
                            case QuestFlagAction.CheckOn:
                            case QuestFlagAction.CheckOff:
                                /* Invalid for Layout flags */
                                return;
                        }
                        break;
                    case QuestFlagType.MyQst:
                        switch (questFlag.Action)
                        {
                            case QuestFlagAction.Set:
                                resultFlags.Add(QuestManager.ResultCommand.MyQstFlagOn(questFlag.Value));
                                break;
                            case QuestFlagAction.Clear:
                                resultFlags.Add(QuestManager.ResultCommand.MyQstFlagOff(questFlag.Value));
                                break;
                            case QuestFlagAction.CheckOn:
                                checkFlags.Add(QuestManager.CheckCommand.MyQstFlagOn(questFlag.Value));
                                break;
                            case QuestFlagAction.CheckOff:
                                checkFlags.Add(QuestManager.CheckCommand.MyQstFlagOff(questFlag.Value));
                                break;
                        }
                        break;
                    case QuestFlagType.WorldManageQuest:
                        switch (questFlag.Action)
                        {
                            case QuestFlagAction.Set:
                                resultFlags.Add(QuestManager.ResultCommand.WorldManageQuestFlagOn(questFlag.Value, questFlag.QuestId));
                                break;
                            case QuestFlagAction.Clear:
                                resultFlags.Add(QuestManager.ResultCommand.WorldManageQuestFlagOff(questFlag.Value, questFlag.QuestId));
                                break;
                            case QuestFlagAction.CheckOn:
                                checkFlags.Add(QuestManager.CheckCommand.WorldManageQuestFlagOn(questFlag.Value, questFlag.QuestId));
                                break;
                            case QuestFlagAction.CheckOff:
                                checkFlags.Add(QuestManager.CheckCommand.WorldManageQuestFlagOn(questFlag.Value, questFlag.QuestId));
                                break;
                        }
                        break;
                }
            }
        }
    }
}
