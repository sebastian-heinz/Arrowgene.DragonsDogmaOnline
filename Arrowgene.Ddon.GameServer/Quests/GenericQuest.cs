using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Asset;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Logging;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Quests
{
    public class GenericQuest : Quest
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(GenericQuest));

        public static GenericQuest FromAsset(QuestAssetData questAsset)
        {
            var quest = new GenericQuest(questAsset.QuestId, questAsset.QuestScheduleId, questAsset.Type, questAsset.Discoverable);

            quest.QuestAreaId = questAsset.QuestAreaId;
            quest.NewsImageId = questAsset.NewsImageId;

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
                        quest.SelectableRewards.Add(rewardItem);
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
                        case QuestBlockType.SpawnGroup:
                            {
                                foreach (var groupId in block.EnemyGroupIds)
                                {
                                    var enemyGroup = quest.EnemyGroups[groupId];
                                    quest.Locations.Add(new QuestLocation() { StageId = enemyGroup.StageId, SubGroupId = (ushort) enemyGroup.SubGroupId });
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
            QuestLayoutFlagSetInfo = new List<QuestLayoutFlagSetInfo>();
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
                    SequenceNo = proccessState.SequenceNo,
                    BlockNo = proccessState.BlockNo,
#if false
                    WorkList = new List<CDataQuestProgressWork>()
                    {
                        QuestManager.NotifyCommand.KilledTargetEnemySetGroup((int) questLocation.QuestLayoutFlag, StageManager.ConvertIdToStageNo(stageId), (int) stageId.GroupId)
                    }
#endif
                });
            }
        }

        public override List<CDataQuestProcessState> StateMachineExecute(DdonGameServer server, GameClient client, QuestProcessState processState, out QuestProgressState questProgressState)
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
            else if (questBlock.IsCheckpoint)
            {
                questProgressState = QuestProgressState.Checkpoint;
            }
            else if (questBlock.AnnounceType == QuestAnnounceType.Accept)
            {
                questProgressState = QuestProgressState.Accepted;
            }
            else
            {
                questProgressState = QuestProgressState.InProgress;
            }

            if (questBlock.BlockType != QuestBlockType.DestroyGroup)
            {
                foreach (var enemyGroupId in questBlock.EnemyGroupIds)
                {
                    var enemyGroup = EnemyGroups[enemyGroupId];
                    client.Party.QuestState.SetInstanceEnemies(this, enemyGroup.StageId, (ushort)enemyGroup.SubGroupId, enemyGroup.CreateNewInstance());
                }
            }
            else
            {
                foreach (var enemyGroupId in questBlock.EnemyGroupIds)
                {
                    var enemies = EnemyGroups[enemyGroupId];
                    client.Party.QuestState.SetInstanceEnemies(this, enemies.StageId, (ushort)enemies.SubGroupId, new List<InstancedEnemy>());
                }
            }

            foreach (var item in questBlock.HandPlayerItems)
            {
                var result = server.ItemManager.AddItem(server, client.Character, true, item.ItemId, item.Amount);
                client.Send(new S2CItemUpdateCharacterItemNtc()
                {
                    UpdateType = 0,
                    UpdateItemList = result
                });
            }

            foreach (var item in questBlock.ConsumePlayerItems)
            {
                var result = server.ItemManager.ConsumeItemByIdFromItemBag(server, client.Character, item.ItemId, item.Amount);
                client.Send(new S2CItemUpdateCharacterItemNtc()
                {
                    UpdateType = 0,
                    UpdateItemList = new List<CDataItemUpdateResult>() { result }
                });
            }

            bool ShouldResetGroup = false;
            if (questBlock.BlockType == QuestBlockType.DestroyGroup)
            {
                ShouldResetGroup = true;
                DestroyEnemiesForBlock(client, questBlock);
            }

            if (questBlock.ResetGroup || ShouldResetGroup)
            {
                ResetEnemiesForBlock(client, questBlock);
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

            ParseQuestFlags(questBlock.QuestFlags, resultCommands, checkCommands);

            if (questBlock.ShouldStageJump)
            {
                resultCommands.Add(QuestManager.ResultCommand.StageJump(StageManager.ConvertIdToStageNo(questBlock.StageId), 0));
            }

            if (questBlock.QuestCameraEvent.HasCameraEvent)
            {
                resultCommands.Add(QuestManager.ResultCommand.PlayCameraEvent(StageManager.ConvertIdToStageNo(questBlock.StageId), questBlock.QuestCameraEvent.EventNo));
            }

            if (questBlock.BgmStop)
            {
                // TODO: This probably needs Start/Stop/Fix behavior
                resultCommands.Add(QuestManager.ResultCommand.BgmStop());
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

            foreach (var item in questBlock.HandPlayerItems)
            {
                resultCommands.Add(QuestManager.ResultCommand.PushImteToPlBag((int)item.ItemId, (int)item.Amount));
            }

            switch (questBlock.BlockType)
            {
                case QuestBlockType.NpcTalkAndOrder:
                    {
                        checkCommands.Add(QuestManager.CheckCommand.NpcTalkAndOrderUi(StageManager.ConvertIdToStageNo(questBlock.NpcOrderDetails[0].StageId), questBlock.NpcOrderDetails[0].NpcId, 0));
                        resultCommands.Add(QuestManager.ResultCommand.QstTalkChg(questBlock.NpcOrderDetails[0].NpcId, questBlock.NpcOrderDetails[0].MsgId));
                    }
                    break;
                case QuestBlockType.NewNpcTalkAndOrder:
                    {
                        checkCommands.Add(QuestManager.CheckCommand.QuestNpcTalkAndOrderUi(
                            StageManager.ConvertIdToStageNo(questBlock.NpcOrderDetails[0].StageId),
                            (int)questBlock.NpcOrderDetails[0].StageId.GroupId,
                            questBlock.NpcOrderDetails[0].StageId.LayerNo,
                            (int)quest.QuestId));
                        resultCommands.Add(QuestManager.ResultCommand.QstTalkChg(questBlock.NpcOrderDetails[0].NpcId, questBlock.NpcOrderDetails[0].MsgId));
                    }
                    break;
                case QuestBlockType.PartyGather:
                    {
                        checkCommands.Add(QuestManager.CheckCommand.Prt(StageManager.ConvertIdToStageNo(questBlock.StageId), questBlock.PartyGatherPoint.x, questBlock.PartyGatherPoint.y, questBlock.PartyGatherPoint.z));
                        resultCommands.Add(QuestManager.ResultCommand.Prt(StageManager.ConvertIdToStageNo(questBlock.StageId), questBlock.PartyGatherPoint.x, questBlock.PartyGatherPoint.y, questBlock.PartyGatherPoint.z));
                    }
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
                case QuestBlockType.SpawnGroup:
                    /* This block is used just to control nodes to spawn/not spawn enemies */
                    break;
                case QuestBlockType.End:
                    {
                        resultCommands.Add(QuestManager.ResultCommand.SetAnnounce(QuestAnnounceType.Clear));
                        resultCommands.Add(QuestManager.ResultCommand.EndEndQuest());
                    }
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
                case QuestBlockType.WeakenGroup:
                    {
                        foreach (var groupId in questBlock.EnemyGroupIds)
                        {
                            var enemyGroup = quest.EnemyGroups[groupId];
                            foreach (var enemy in enemyGroup.CreateNewInstance())
                            {
                                checkCommands.Add(QuestManager.CheckCommand.EmHpLess(StageManager.ConvertIdToStageNo(enemyGroup.StageId), (int)enemyGroup.StageId.GroupId, enemy.Index, questBlock.EnemyHpPrecent));
                            }
                        }
                    }
                    break;
                case QuestBlockType.CollectItem:
                    {
                        var questCommand = questBlock.ShowMarker ?
                            QuestManager.CheckCommand.QuestOmSetTouch(StageManager.ConvertIdToStageNo(questBlock.StageId), (int)questBlock.StageId.GroupId, questBlock.StageId.LayerNo) :
                            QuestManager.CheckCommand.QuestOmSetTouchWithoutMarker(StageManager.ConvertIdToStageNo(questBlock.StageId), (int)questBlock.StageId.GroupId, questBlock.StageId.LayerNo);
                        checkCommands.Add(questCommand);
                    }
                    break;
                case QuestBlockType.OmInteractEvent:
                    {
                        CDataQuestCommand questCommand = new CDataQuestCommand();
                        switch (questBlock.OmInteractEvent.QuestType)
                        {
                            case OmQuestType.MyQuest:
                                switch (questBlock.OmInteractEvent.InteractType)
                                {
                                    case OmInteractType.Touch:
                                        questCommand = questBlock.ShowMarker ?
                                            QuestManager.CheckCommand.QuestOmSetTouch(StageManager.ConvertIdToStageNo(questBlock.StageId), (int)questBlock.StageId.GroupId, questBlock.StageId.LayerNo) :
                                            QuestManager.CheckCommand.QuestOmSetTouchWithoutMarker(StageManager.ConvertIdToStageNo(questBlock.StageId), (int)questBlock.StageId.GroupId, questBlock.StageId.LayerNo);
                                        break;
                                    case OmInteractType.Release:
                                        questCommand = questBlock.ShowMarker ?
                                            QuestManager.CheckCommand.QuestOmReleaseTouch(StageManager.ConvertIdToStageNo(questBlock.StageId), (int)questBlock.StageId.GroupId, questBlock.StageId.LayerNo) :
                                            QuestManager.CheckCommand.QuestOmReleaseTouchWithoutMarker(StageManager.ConvertIdToStageNo(questBlock.StageId), (int)questBlock.StageId.GroupId, questBlock.StageId.LayerNo);
                                        break;
                                    case OmInteractType.EndText:
                                        questCommand = QuestManager.CheckCommand.QuestOmEndText(StageManager.ConvertIdToStageNo(questBlock.StageId), (int)questBlock.StageId.GroupId, questBlock.StageId.LayerNo);
                                        break;
                                    default:
                                        /* TODO: throw exception */
                                        break;
                                }
                                break;
                            case OmQuestType.WorldManageQuest:
                                switch (questBlock.OmInteractEvent.InteractType)
                                {
                                    case OmInteractType.Touch:
                                        questCommand = QuestManager.CheckCommand.OmSetTouch(StageManager.ConvertIdToStageNo(questBlock.StageId), (int)questBlock.StageId.GroupId, questBlock.StageId.LayerNo);
                                        break;
                                    case OmInteractType.Release:
                                        questCommand = QuestManager.CheckCommand.OmReleaseTouch(StageManager.ConvertIdToStageNo(questBlock.StageId), (int)questBlock.StageId.GroupId, questBlock.StageId.LayerNo);
                                        break;
                                    case OmInteractType.EndText:
                                        questCommand = QuestManager.CheckCommand.OmEndText(StageManager.ConvertIdToStageNo(questBlock.StageId), (int)questBlock.StageId.GroupId, questBlock.StageId.LayerNo);
                                        break;
                                    case OmInteractType.OpenDoor:
                                        questCommand = questBlock.ShowMarker ?
                                            QuestManager.CheckCommand.IsOpenDoorOmQuestSet(StageManager.ConvertIdToStageNo(questBlock.StageId), (int)questBlock.StageId.GroupId, questBlock.StageId.LayerNo, (int)questBlock.OmInteractEvent.QuestId) :
                                            QuestManager.CheckCommand.IsOpenDoorOmQuestSetNoMarker(StageManager.ConvertIdToStageNo(questBlock.StageId), (int)questBlock.StageId.GroupId, questBlock.StageId.LayerNo, (int)questBlock.OmInteractEvent.QuestId);
                                        break;
                                    case OmInteractType.IsTouchPawnDungon:
                                        questCommand = QuestManager.CheckCommand.IsTouchPawnDungeonOm(StageManager.ConvertIdToStageNo(questBlock.StageId), (int)questBlock.StageId.GroupId, questBlock.StageId.LayerNo);
                                        break;
                                    case OmInteractType.IsBrokenLayout:
                                        questCommand = questBlock.ShowMarker ?
                                            QuestManager.CheckCommand.IsOmBrokenLayout(StageManager.ConvertIdToStageNo(questBlock.StageId), (int)questBlock.StageId.GroupId, questBlock.StageId.LayerNo) :
                                            QuestManager.CheckCommand.IsOmBrokenLayoutNoMarker(StageManager.ConvertIdToStageNo(questBlock.StageId), (int)questBlock.StageId.GroupId, questBlock.StageId.LayerNo);
                                        break;
                                    case OmInteractType.IsBrokenQuest:
                                        questCommand = questBlock.ShowMarker ?
                                            QuestManager.CheckCommand.IsOmBrokenQuest(StageManager.ConvertIdToStageNo(questBlock.StageId), (int)questBlock.StageId.GroupId, questBlock.StageId.LayerNo) :
                                            QuestManager.CheckCommand.IsOmBrokenQuestNoMarker(StageManager.ConvertIdToStageNo(questBlock.StageId), (int)questBlock.StageId.GroupId, questBlock.StageId.LayerNo);
                                        break;
                                    case OmInteractType.TouchNpcUnitMarker:
                                        questCommand = QuestManager.CheckCommand.TouchQuestNpcUnitMarker(StageManager.ConvertIdToStageNo(questBlock.StageId), (int)questBlock.StageId.GroupId, questBlock.StageId.LayerNo, (int)questBlock.OmInteractEvent.QuestId);
                                        break;
                                    case OmInteractType.TouchActNpc:
                                        questCommand = QuestManager.CheckCommand.TouchActQuestNpc(StageManager.ConvertIdToStageNo(questBlock.StageId), (int)questBlock.StageId.GroupId, questBlock.StageId.LayerNo, (int)questBlock.OmInteractEvent.QuestId);
                                        break;
                                    case OmInteractType.UsedKey:
                                        questCommand = QuestManager.CheckCommand.HasUsedKey(StageManager.ConvertIdToStageNo(questBlock.StageId), (int)questBlock.StageId.GroupId, questBlock.StageId.LayerNo, (int)questBlock.OmInteractEvent.QuestId);
                                        break;
                                    default:
                                        /* TODO: throw exception */
                                        break;
                                }
                                break;
                        }

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
                    {
                        var questCommand = questBlock.ShowMarker ?
                            QuestManager.CheckCommand.TalkNpc(StageManager.ConvertIdToStageNo(questBlock.NpcOrderDetails[0].StageId), questBlock.NpcOrderDetails[0].NpcId) :
                            QuestManager.CheckCommand.TalkNpcWithoutMarker(StageManager.ConvertIdToStageNo(questBlock.NpcOrderDetails[0].StageId), questBlock.NpcOrderDetails[0].NpcId);
                        checkCommands.Add(questCommand);
                        resultCommands.Add(QuestManager.ResultCommand.QstTalkChg(questBlock.NpcOrderDetails[0].NpcId, questBlock.NpcOrderDetails[0].MsgId));
                    }
                    break;
                case QuestBlockType.NewTalkToNpc:
                    {
                        var orderDetails = questBlock.NpcOrderDetails[0];
                        var questCommand = questBlock.ShowMarker ?
                            QuestManager.CheckCommand.NewTalkNpc(StageManager.ConvertIdToStageNo(orderDetails.StageId), (int)orderDetails.StageId.GroupId, orderDetails.StageId.LayerNo, (int)quest.QuestId) :
                            QuestManager.CheckCommand.NewTalkNpcWithoutMarker(StageManager.ConvertIdToStageNo(orderDetails.StageId), (int)orderDetails.StageId.GroupId, orderDetails.StageId.LayerNo, (int)quest.QuestId);
                        checkCommands.Add(questCommand);
                        resultCommands.Add(QuestManager.ResultCommand.QstTalkChg(orderDetails.NpcId, orderDetails.MsgId));
                    }
                    break;
                case QuestBlockType.IsQuestOrdered:
                    {
                        switch (questBlock.QuestOrderDetails.QuestType)
                        {
                            case QuestType.Main:
                                checkCommands.Add(QuestManager.CheckCommand.IsMainQuestOrder((int)questBlock.QuestOrderDetails.QuestId));
                                break;
                            case QuestType.Light:
                                // case QuestType.World:
                                checkCommands.Add(QuestManager.CheckCommand.IsOrderLightQuest((int)questBlock.QuestOrderDetails.QuestId));
                                break;
                            case QuestType.WorldManage:
                                checkCommands.Add(QuestManager.CheckCommand.IsOrderWorldQuest((int)questBlock.QuestOrderDetails.QuestId));
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
                    {
                        var questCommand = questBlock.ShowMarker ?
                            QuestManager.CheckCommand.StageNo(StageManager.ConvertIdToStageNo(questBlock.StageId)) :
                            QuestManager.CheckCommand.StageNoWithoutMarker(StageManager.ConvertIdToStageNo(questBlock.StageId));
                        checkCommands.Add(questCommand);
                    }
                    break;
                case QuestBlockType.PlayEvent:
                    {
                        checkCommands.Add(QuestManager.CheckCommand.EventEnd(StageManager.ConvertIdToStageNo(questBlock.StageId), questBlock.QuestEvent.EventId));
                        switch (questBlock.QuestEvent.JumpType)
                        {
                            case QuestJumpType.None:
                            case QuestJumpType.After:
                                resultCommands.Add(QuestManager.ResultCommand.EventExec(
                                                        StageManager.ConvertIdToStageNo(questBlock.StageId),
                                                        questBlock.QuestEvent.EventId,
                                                        StageManager.ConvertIdToStageNo(questBlock.QuestEvent.JumpStageId),
                                                        questBlock.QuestEvent.StartPosNo));
                                break;
                            case QuestJumpType.Before:
                                resultCommands.Add(QuestManager.ResultCommand.ExeEventAfterStageJump(
                                                        StageManager.ConvertIdToStageNo(questBlock.StageId),
                                                        questBlock.QuestEvent.EventId,
                                                        questBlock.QuestEvent.StartPosNo));
                                break;
                            case QuestJumpType.Continue:
                                resultCommands.Add(QuestManager.ResultCommand.EventExecCont(
                                                        StageManager.ConvertIdToStageNo(questBlock.StageId),
                                                        questBlock.QuestEvent.EventId,
                                                        StageManager.ConvertIdToStageNo(questBlock.QuestEvent.JumpStageId),
                                                        questBlock.QuestEvent.StartPosNo));
                                break;
                        }
                    }
                    break;
                case QuestBlockType.KillTargetEnemies:
                    {
                        // Handles kill x amount of monster type quests
                        checkCommands.Add(QuestManager.CheckCommand.EmDieLight((int)questBlock.TargetEnemy.EnemyId, (int)questBlock.TargetEnemy.Level, (int)questBlock.TargetEnemy.Amount));
                    }
                    break;
                case QuestBlockType.Raw:
                    /* handled generically for all blocks */
                    break;
                case QuestBlockType.DestroyGroup:
                    /* This is a pseudo block handeled at the state machine level */
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

            /* Add in any additional result commands */
            resultCommands.AddRange(questBlock.ResultCommands);

            result.ResultCommandList = resultCommands;

            List<List<CDataQuestCommand>> complexCheckCommands = new List<List<CDataQuestCommand>>();
            if (checkCommands.Count > 0)
            {
                complexCheckCommands.Add(checkCommands);
            }

            if (questBlock.CheckCommands.Count > 0)
            {
                complexCheckCommands.AddRange(questBlock.CheckCommands);
            }

            if (complexCheckCommands.Count > 0)
            {
                result.CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(complexCheckCommands);
            }

            return result;
        }
    }
}
