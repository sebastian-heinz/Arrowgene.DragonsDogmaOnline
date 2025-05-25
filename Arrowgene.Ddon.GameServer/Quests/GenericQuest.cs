using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Quests.Extensions;
using Arrowgene.Ddon.GameServer.Scripting.Interfaces;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Asset;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Quests
{
    public class GenericQuest : Quest
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(GenericQuest));

        public static GenericQuest FromAsset(DdonGameServer server, QuestAssetData questAsset, IQuest backingObject = null)
        {
            var quest = new GenericQuest(server, questAsset.QuestId, questAsset.QuestScheduleId, questAsset.QuestType, questAsset.Discoverable);

            quest.QuestSource = questAsset.QuestSource;
            quest.BackingObject = backingObject;

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
            quest.StageId = questAsset.StageLayoutId;
            quest.MissionParams = questAsset.MissionParams;
            quest.ServerActions = questAsset.ServerActions;
            quest.QuestOrderBackgroundImage = questAsset.QuestOrderBackgroundImage;
            quest.IsImportant = questAsset.IsImportant;
            quest.AdventureGuideCategory = questAsset.AdventureGuideCategory;

            quest.LightQuestDetail = questAsset.LightQuestDetail;
            quest.Enabled = questAsset.Enabled;
            quest.OverrideEnemySpawn = questAsset.OverrideEnemySpawn;
            quest.EnableCancel = questAsset.EnableCancel;
            quest.ContentsRelease = questAsset.ContentsReleased;
            quest.WorldManageUnlocks = questAsset.WorldManageUnlocks;
            quest.QuestProgressWork = questAsset.QuestProgressWork;

            foreach (var pointReward in questAsset.PointRewards)
            {
                quest.ExpRewards.Add(new CDataQuestExp()
                {
                    Type = pointReward.PointType,
                    Reward = pointReward.Amount
                });
            }

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

            foreach (var (_, enemyGroup) in questAsset.EnemyGroups)
            {
                quest.UniqueEnemyGroups.Add(enemyGroup.StageLayoutId);
            }

            foreach (var process in quest.Processes)
            {
                foreach (var block in process.Blocks.Values)
                {
                    foreach (var groupId in block.EnemyGroupIds)
                    {
                        var enemyGroup = quest.EnemyGroups[groupId];
                        quest.Locations.Add(new QuestLocation() { StageId = enemyGroup.StageLayoutId, SubGroupId = (ushort)enemyGroup.SubGroupId });
                    }

                    quest.ContentsRelease.UnionWith(block.ContentsReleased);
                    quest.AddWorldManageUnlock(block.WorldManageUnlocks);
                    quest.AddProgressWorkItems(block.QuestProgressWork);

                    switch (block.BlockType)
                    {
                        case QuestBlockType.KillGroup:
                        case QuestBlockType.SpawnGroup:
                            /* Always populate enemies if there is a list of them */
                            break;
                        case QuestBlockType.DeliverItems:
                        case QuestBlockType.NewDeliverItems:
                        case QuestBlockType.DeliverItemsLight:
                            foreach (var request in block.DeliveryRequests)
                            {
                                quest.DeliveryItems.Add(new QuestDeliveryItem()
                                {
                                    ProcessNo = process.ProcessNo,
                                    BlockNo = block.BlockNo,
                                    ItemId = request.ItemId,
                                    Amount = request.Amount,
                                });
                            }
                            break;
                        case QuestBlockType.KillTargetEnemies:
                            if (block.TargetEnemy.EnemyId > 0)
                            {
                                quest.EnemyHunts.Add(new QuestEnemyHunt()
                                {
                                    ProcessNo = process.ProcessNo,
                                    SequenceNo = 0,
                                    BlockNo = block.BlockNo,
                                    EnemyId = block.TargetEnemy.EnemyId,
                                    MinimumLevel = block.TargetEnemy.Level,
                                    Amount = block.TargetEnemy.Amount,
                                });

                                if (quest.QuestType == QuestType.Light)
                                {
                                    quest.SaveWorkAsStep = true;
                                }
                            }
                            break;
                    }

                    block.QuestProcessState = BlockAsCDataQuestProcessState(quest, block);
                }
            }

            return quest;
        }

        public GenericQuest(DdonGameServer server, QuestId questId, uint questScheduleId, QuestType questType, bool discoverable) : 
            base(server, questId, questScheduleId, questType, discoverable)
        {
            QuestLayoutFlagSetInfo = new List<QuestLayoutFlagSetInfo>();
        }

        public override void SendProgressWorkNotices(GameClient client, StageLayoutId stageId, uint subGroupId)
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
                var questStateManager = QuestManager.GetQuestStateManager(client, this);

                var proccessState = questStateManager.GetProcessState(QuestScheduleId, 0);
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

        public override List<CDataQuestProcessState> StateMachineExecute(DdonGameServer server, GameClient client, QuestProcessState processState, PacketQueue packets, out QuestProgressState questProgressState)
        {
            var questStateManager = QuestManager.GetQuestStateManager(client, this);

            if (processState.ProcessNo >= Processes.Count)
            {
                questProgressState = QuestProgressState.Unknown;
                return new List<CDataQuestProcessState>();
            }

            var process = Processes[processState.ProcessNo];

            ushort nextBlockNo = (ushort)(processState.BlockNo + 1);
            if (!process.Blocks.ContainsKey(nextBlockNo))
            {
                questProgressState = QuestProgressState.Unknown;
                return new List<CDataQuestProcessState>();
            }

            var questBlock = process.Blocks[nextBlockNo];
            if (processState.ProcessNo == 0 && questBlock.SequenceNo == 1)
            {
                questProgressState = QuestProgressState.Complete;
            }
            else if (questBlock.IsCheckpoint)
            {               
                questProgressState = QuestProgressState.Checkpoint;
            }
            else if (questBlock.AnnounceType == QuestAnnounceType.Accept || questBlock.AnnounceType == QuestAnnounceType.Start)
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
                    questStateManager.SetInstanceEnemies(this, enemyGroup.StageLayoutId, (ushort)enemyGroup.SubGroupId, enemyGroup.CreateNewInstance(processState.ProcessNo, processState.BlockNo));
                }
            }
            else
            {
                foreach (var enemyGroupId in questBlock.EnemyGroupIds)
                {
                    var enemies = EnemyGroups[enemyGroupId];
                    questStateManager.SetInstanceEnemies(this, enemies.StageLayoutId, (ushort)enemies.SubGroupId, new List<InstancedEnemy>());
                }
            }

            if (BoardManager.BoardIdIsExm(client.Party.ContentId) && questBlock.TimeAmount > 0)
            {
                var newEndTime = server.PartyQuestContentManager.ExtendTimer(client.Party.Id, questBlock.TimeAmount);
                client.Party.EnqueueToAll(new S2CQuestPlayAddTimerNtc() { PlayEndDateTime = newEndTime }, packets);
            }

            foreach (var item in questBlock.HandPlayerItems)
            {
                var result = server.ItemManager.AddItem(server, client.Character, true, item.ItemId, item.Amount);
                packets.Enqueue(client, new S2CItemUpdateCharacterItemNtc()
                {
                    UpdateType = 0,
                    UpdateItemList = result
                });
            }

            foreach (var item in questBlock.ConsumePlayerItems)
            {
                var result = server.ItemManager.ConsumeItemByIdFromItemBag(server, client.Character, item.ItemId, item.Amount);
                if (result != null)
                {

                    packets.Enqueue(client, new S2CItemUpdateCharacterItemNtc()
                    {
                        UpdateType = 0,
                        UpdateItemList = result
                    });
                }
            }

            // If this quest unlocks something at this step, add it to the unlocked cache
            if (questBlock.ContentsReleased.Count > 0)
            {
                client.Character.ContentsReleased.UnionWith(questBlock.ContentsReleased
                    .Where(x => x.ReleaseId != Shared.Model.ContentsRelease.None)
                    .Select(x => x.ReleaseId)
                    .ToHashSet());
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

            if (questBlock.BlockType == QuestBlockType.ReturnCheckpoint && questBlock.ProcessNo == questBlock.CheckpointDetails.ProcessNo)
            {
                questBlock = process.Blocks[questBlock.CheckpointDetails.BlockNo];
            }

            var questState = questStateManager.GetQuestState(this);

            var cbParam = new QuestCallbackParam(client, questState);
            foreach (var callback in questBlock.Callbacks)
            {
                ((Action<QuestCallbackParam>)callback)(cbParam);
            }

            // Add any newly created process state added by the quest callback
            CDataQuestProcessState questProcessState;
            if (cbParam.ResultCommands.Count > 0 || cbParam.CheckCommands.Count > 0)
            {
                // Make a deep copy and then append dynamically generated commands
                questProcessState = new CDataQuestProcessState(questBlock.QuestProcessState);

                if (cbParam.ResultCommands.Count > 0)
                {
                    questProcessState.ResultCommandList.AddRange(cbParam.ResultCommands);
                }
                
                if (cbParam.CheckCommands.Count > 0)
                {
                    questProcessState.CheckCommandList = QuestManager.CheckCommand.AppendCheckCommands(questProcessState.CheckCommandList, cbParam.CheckCommands);
                }
            }
            else
            {
                questProcessState = questBlock.QuestProcessState;
            }

            return new List<CDataQuestProcessState>()
            {
                questProcessState
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
            List<CDataQuestProgressWork> workCommands = new List<CDataQuestProgressWork>();

            ParseQuestFlags(questBlock.QuestFlags, resultCommands, checkCommands);

            if (questBlock.ShouldStageJump)
            {
                resultCommands.Add(QuestManager.ResultCommand.StageJump(StageManager.ConvertIdToStageNo(questBlock.StageLayoutId), 0));
            }

            if (questBlock.QuestCameraEvent.HasCameraEvent)
            {
                resultCommands.Add(QuestManager.ResultCommand.PlayCameraEvent(StageManager.ConvertIdToStageNo(questBlock.StageLayoutId), questBlock.QuestCameraEvent.EventNo));
            }

            if (questBlock.BgmStop)
            {
                // TODO: This probably needs Start/Stop/Fix behavior
                resultCommands.Add(QuestManager.ResultCommand.BgmStop());
            }

            if (questBlock.Announcements.Caution)
            {
                // This often occurs in conjunction with an Update or Accept announcement.
                // The exact order varies from quest to quest in video evidence, but we always present it first for convenience.
                resultCommands.Add(QuestManager.ResultCommand.SetAnnounce(QuestAnnounceType.Caution));
            }

            if (questBlock.BlockType != QuestBlockType.End)
            {
                switch (questBlock.AnnounceType)
                {
                    case QuestAnnounceType.Accept:
                        resultCommands.Add(QuestManager.ResultCommand.SetAnnounce(QuestAnnounceType.Accept, 1));
                        break;
                    case QuestAnnounceType.Update:
                        resultCommands.Add(QuestManager.ResultCommand.UpdateAnnounce());
                        break;
                    case QuestAnnounceType.Start:
                        // resultCommands.Add(QuestManager.ResultCommand.SetAnnounce(QuestAnnounceType.Start));
                        resultCommands.Add(QuestManager.ResultCommand.StartMissionAnnounce());
                        resultCommands.Add(QuestManager.ResultCommand.Unknown(127));
                        resultCommands.Add(QuestManager.ResultCommand.StartContentsTimer());
                        break;
                    default:
                        resultCommands.Add(QuestManager.ResultCommand.SetAnnounce(questBlock.AnnounceType));
                        break;
                }
            }

            if (questBlock.Announcements.GeneralAnnounceId != 0)
            {
                resultCommands.Add(QuestManager.ResultCommand.CallGeneralAnnounce(0, questBlock.Announcements.GeneralAnnounceId));
            }

            if (questBlock.Announcements.StageStart != 0)
            {
                resultCommands.Add(QuestManager.ResultCommand.StageAnnounce(0, questBlock.Announcements.StageStart));
            }
            else if (questBlock.Announcements.StageClear != 0)
            {
                resultCommands.Add(QuestManager.ResultCommand.StageAnnounce(1, questBlock.Announcements.StageClear));
            }

            if (questBlock.Announcements.EndContentsPurpose != 0)
            {
                resultCommands.Add(QuestManager.ResultCommand.AddEndContentsPurpose(questBlock.Announcements.EndContentsPurpose, 1));
            }

            foreach (var item in questBlock.HandPlayerItems)
            {
                resultCommands.Add(QuestManager.ResultCommand.PushImteToPlBag((int)item.ItemId, (int)item.Amount));
            }

            foreach (var unlock in questBlock.ContentsReleased)
            {
                if (unlock.ReleaseId != Shared.Model.ContentsRelease.None)
                {
                    resultCommands.AddResultCmdReleaseAnnounce(unlock.ReleaseId);
                }
                
                if (unlock.TutorialId != TutorialId.None)
                {
                    resultCommands.AddResultCmdTutorialDialog(unlock.TutorialId);
                }
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
                        var questId = (questBlock.NpcOrderDetails[0].QuestId == QuestId.None) ? quest.QuestId : questBlock.NpcOrderDetails[0].QuestId;
                        checkCommands.Add(QuestManager.CheckCommand.QuestNpcTalkAndOrderUi(
                            StageManager.ConvertIdToStageNo(questBlock.NpcOrderDetails[0].StageId),
                            (int)questBlock.NpcOrderDetails[0].StageId.GroupId,
                            questBlock.NpcOrderDetails[0].StageId.LayerNo,
                            (int) questId));
                        resultCommands.Add(QuestManager.ResultCommand.QstTalkChg(questBlock.NpcOrderDetails[0].NpcId, questBlock.NpcOrderDetails[0].MsgId));
                    }
                    break;
                case QuestBlockType.PartyGather:
                    {
                        checkCommands.Add(QuestManager.CheckCommand.Prt(StageManager.ConvertIdToStageNo(questBlock.StageLayoutId), questBlock.PartyGatherPoint.x, questBlock.PartyGatherPoint.y, questBlock.PartyGatherPoint.z));
                        resultCommands.Add(QuestManager.ResultCommand.Prt(StageManager.ConvertIdToStageNo(questBlock.StageLayoutId), questBlock.PartyGatherPoint.x, questBlock.PartyGatherPoint.y, questBlock.PartyGatherPoint.z));
                    }
                    break;
                case QuestBlockType.IsGatherPartyInStage:
                    {
                        checkCommands.Add(QuestManager.CheckCommand.IsGatherPartyInStage(StageManager.ConvertIdToStageNo(questBlock.StageLayoutId)));
                    }
                    break;
                case QuestBlockType.DiscoverEnemy:
                case QuestBlockType.SeekOutEnemiesAtMarkedLocation:
                    {
                        foreach (var groupId in questBlock.EnemyGroupIds)
                        {
                            var enemyGroup = quest.EnemyGroups[groupId];
                            checkCommands.Add(questBlock.ShowMarker ?
                                QuestManager.CheckCommand.IsEnemyFoundForOrder(StageManager.ConvertIdToStageNo(enemyGroup.StageLayoutId), (int)enemyGroup.StageLayoutId.GroupId, -1) :
                                QuestManager.CheckCommand.IsEnemyFoundWithoutMarker(StageManager.ConvertIdToStageNo(enemyGroup.StageLayoutId), (int)enemyGroup.StageLayoutId.GroupId, -1));
                        }
                    }
                    break;
                case QuestBlockType.SpawnGroup:
                    /* This block is used just to control nodes to spawn/not spawn enemies */
                    break;
                case QuestBlockType.End:
                    {
                        resultCommands.Add(QuestManager.ResultCommand.SetAnnounce(QuestAnnounceType.Clear));
                        resultCommands.Add(QuestManager.ResultCommand.ResetDiePlayerReturnPos(0, 0));
                        resultCommands.Add(QuestManager.ResultCommand.EndEndQuest());
                    }
                    break;
                case QuestBlockType.KillGroup:
                    {
                        foreach (var groupId in questBlock.EnemyGroupIds)
                        {
                            var enemyGroup = quest.EnemyGroups[groupId];
                            checkCommands.Add(QuestManager.CheckCommand.DieEnemy(StageManager.ConvertIdToStageNo(enemyGroup.StageLayoutId), (int)enemyGroup.StageLayoutId.GroupId, -1));
                        }
                    }
                    break;
                case QuestBlockType.WeakenGroup:
                    {
                        foreach (var groupId in questBlock.EnemyGroupIds)
                        {
                            var enemyGroup = quest.EnemyGroups[groupId];
                            foreach (var enemy in enemyGroup.Enemies)
                            {
                                checkCommands.Add(QuestManager.CheckCommand.EmHpLess(StageManager.ConvertIdToStageNo(enemyGroup.StageLayoutId), (int)enemyGroup.StageLayoutId.GroupId, enemy.Index, questBlock.EnemyHpPrecent));
                            }
                        }
                    }
                    break;
                case QuestBlockType.CollectItem:
                    {
                        var questCommand = questBlock.ShowMarker ?
                            QuestManager.CheckCommand.QuestOmSetTouch(StageManager.ConvertIdToStageNo(questBlock.StageLayoutId), (int)questBlock.StageLayoutId.GroupId, questBlock.StageLayoutId.LayerNo) :
                            QuestManager.CheckCommand.QuestOmSetTouchWithoutMarker(StageManager.ConvertIdToStageNo(questBlock.StageLayoutId), (int)questBlock.StageLayoutId.GroupId, questBlock.StageLayoutId.LayerNo);
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
                                            QuestManager.CheckCommand.QuestOmSetTouch(StageManager.ConvertIdToStageNo(questBlock.StageLayoutId), (int)questBlock.StageLayoutId.GroupId, questBlock.StageLayoutId.LayerNo) :
                                            QuestManager.CheckCommand.QuestOmSetTouchWithoutMarker(StageManager.ConvertIdToStageNo(questBlock.StageLayoutId), (int)questBlock.StageLayoutId.GroupId, questBlock.StageLayoutId.LayerNo);
                                        break;
                                    case OmInteractType.Release:
                                        questCommand = questBlock.ShowMarker ?
                                            QuestManager.CheckCommand.QuestOmReleaseTouch(StageManager.ConvertIdToStageNo(questBlock.StageLayoutId), (int)questBlock.StageLayoutId.GroupId, questBlock.StageLayoutId.LayerNo) :
                                            QuestManager.CheckCommand.QuestOmReleaseTouchWithoutMarker(StageManager.ConvertIdToStageNo(questBlock.StageLayoutId), (int)questBlock.StageLayoutId.GroupId, questBlock.StageLayoutId.LayerNo);
                                        break;
                                    case OmInteractType.EndText:
                                        questCommand = QuestManager.CheckCommand.QuestOmEndText(StageManager.ConvertIdToStageNo(questBlock.StageLayoutId), (int)questBlock.StageLayoutId.GroupId, questBlock.StageLayoutId.LayerNo);
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
                                        questCommand = QuestManager.CheckCommand.OmSetTouch(StageManager.ConvertIdToStageNo(questBlock.StageLayoutId), (int)questBlock.StageLayoutId.GroupId, questBlock.StageLayoutId.LayerNo);
                                        break;
                                    case OmInteractType.Release:
                                        questCommand = QuestManager.CheckCommand.OmReleaseTouch(StageManager.ConvertIdToStageNo(questBlock.StageLayoutId), (int)questBlock.StageLayoutId.GroupId, questBlock.StageLayoutId.LayerNo);
                                        break;
                                    case OmInteractType.EndText:
                                        questCommand = QuestManager.CheckCommand.OmEndText(StageManager.ConvertIdToStageNo(questBlock.StageLayoutId), (int)questBlock.StageLayoutId.GroupId, questBlock.StageLayoutId.LayerNo);
                                        break;
                                    case OmInteractType.OpenDoor:
                                        questCommand = questBlock.ShowMarker ?
                                            QuestManager.CheckCommand.IsOpenDoorOmQuestSet(StageManager.ConvertIdToStageNo(questBlock.StageLayoutId), (int)questBlock.StageLayoutId.GroupId, questBlock.StageLayoutId.LayerNo, (int)questBlock.OmInteractEvent.QuestId) :
                                            QuestManager.CheckCommand.IsOpenDoorOmQuestSetNoMarker(StageManager.ConvertIdToStageNo(questBlock.StageLayoutId), (int)questBlock.StageLayoutId.GroupId, questBlock.StageLayoutId.LayerNo, (int)questBlock.OmInteractEvent.QuestId);
                                        break;
                                    case OmInteractType.IsTouchPawnDungon:
                                        questCommand = QuestManager.CheckCommand.IsTouchPawnDungeonOm(StageManager.ConvertIdToStageNo(questBlock.StageLayoutId), (int)questBlock.StageLayoutId.GroupId, questBlock.StageLayoutId.LayerNo);
                                        break;
                                    case OmInteractType.IsBrokenLayout:
                                        questCommand = questBlock.ShowMarker ?
                                            QuestManager.CheckCommand.IsOmBrokenLayout(StageManager.ConvertIdToStageNo(questBlock.StageLayoutId), (int)questBlock.StageLayoutId.GroupId, questBlock.StageLayoutId.LayerNo) :
                                            QuestManager.CheckCommand.IsOmBrokenLayoutNoMarker(StageManager.ConvertIdToStageNo(questBlock.StageLayoutId), (int)questBlock.StageLayoutId.GroupId, questBlock.StageLayoutId.LayerNo);
                                        break;
                                    case OmInteractType.IsBrokenQuest:
                                        questCommand = questBlock.ShowMarker ?
                                            QuestManager.CheckCommand.IsOmBrokenQuest(StageManager.ConvertIdToStageNo(questBlock.StageLayoutId), (int)questBlock.StageLayoutId.GroupId, questBlock.StageLayoutId.LayerNo) :
                                            QuestManager.CheckCommand.IsOmBrokenQuestNoMarker(StageManager.ConvertIdToStageNo(questBlock.StageLayoutId), (int)questBlock.StageLayoutId.GroupId, questBlock.StageLayoutId.LayerNo);
                                        break;
                                    case OmInteractType.TouchNpcUnitMarker:
                                        questCommand = QuestManager.CheckCommand.TouchQuestNpcUnitMarker(StageManager.ConvertIdToStageNo(questBlock.StageLayoutId), (int)questBlock.StageLayoutId.GroupId, questBlock.StageLayoutId.LayerNo, (int)questBlock.OmInteractEvent.QuestId);
                                        break;
                                    case OmInteractType.TouchActNpc:
                                        questCommand = QuestManager.CheckCommand.TouchActQuestNpc(StageManager.ConvertIdToStageNo(questBlock.StageLayoutId), (int)questBlock.StageLayoutId.GroupId, questBlock.StageLayoutId.LayerNo, (int)questBlock.OmInteractEvent.QuestId);
                                        break;
                                    case OmInteractType.UsedKey:
                                        questCommand = QuestManager.CheckCommand.HasUsedKey(StageManager.ConvertIdToStageNo(questBlock.StageLayoutId), (int)questBlock.StageLayoutId.GroupId, questBlock.StageLayoutId.LayerNo, (int)questBlock.OmInteractEvent.QuestId);
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
                            workCommands.Add(new CDataQuestProgressWork()
                            {
                                CommandNo = (uint)QuestNotifyCommand.FulfillDeliverItem,
                                Work01 = 0,
                                Work02 = 1,
                                Work03 = 2,
                                Work04 = 3,
                            });
                        }
                        resultCommands.Add(QuestManager.ResultCommand.SetDeliverInfo(StageManager.ConvertIdToStageNo(questBlock.StageLayoutId), questBlock.NpcOrderDetails[0].NpcId, questBlock.NpcOrderDetails[0].MsgId));
                    }
                    break;
                case QuestBlockType.NewDeliverItems:
                    foreach (var item in questBlock.DeliveryRequests)
                    {
                        checkCommands.Add(QuestManager.CheckCommand.DeliverItem((int)item.ItemId, (int)item.Amount, questBlock.NpcOrderDetails[0].NpcId, questBlock.NpcOrderDetails[0].MsgId));
                    }
                    resultCommands.Add(QuestManager.ResultCommand.SetDeliverInfoQuest(StageManager.ConvertIdToStageNo(questBlock.StageLayoutId), (int)questBlock.StageLayoutId.GroupId, questBlock.StageLayoutId.LayerNo, questBlock.NpcOrderDetails[0].MsgId));
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
                        var questId = (orderDetails.QuestId == QuestId.None) ? quest.QuestId : orderDetails.QuestId;

                        var questCommand = questBlock.ShowMarker ?
                            QuestManager.CheckCommand.NewTalkNpc(StageManager.ConvertIdToStageNo(orderDetails.StageId), (int)orderDetails.StageId.GroupId, orderDetails.StageId.LayerNo, (int)questId) :
                            QuestManager.CheckCommand.NewTalkNpcWithoutMarker(StageManager.ConvertIdToStageNo(orderDetails.StageId), (int)orderDetails.StageId.GroupId, orderDetails.StageId.LayerNo, (int)questId);
                        checkCommands.Add(questCommand);
                        resultCommands.Add(QuestManager.ResultCommand.QstTalkChg(orderDetails.NpcId, orderDetails.MsgId));
                    }
                    break;
                case QuestBlockType.TouchNpc:
                    {
                        checkCommands.Add(QuestManager.CheckCommand.TouchActToNpc(StageManager.ConvertIdToStageNo(questBlock.NpcOrderDetails[0].StageId), questBlock.NpcOrderDetails[0].NpcId));
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
                            case QuestType.WildHunt:
                                // Hack for progressing quest after ordering it
                                checkCommands.Add(QuestManager.CheckCommand.EmDieLight(0, 0, 0));
                                break;
                        }
                    }
                    break;
                case QuestBlockType.IsQuestClear:
                    {
                        switch (questBlock.QuestIsClearDetails.QuestType)
                        {
                            case QuestType.Main:
                                checkCommands.Add(QuestManager.CheckCommand.IsMainQuestClear((int)questBlock.QuestIsClearDetails.QuestId));
                                break;
                            case QuestType.Light:
                                // case QuestType.World:
                                checkCommands.Add(QuestManager.CheckCommand.IsClearLightQuest((int)questBlock.QuestIsClearDetails.QuestId));
                                break;
                            case QuestType.Tutorial:
                                checkCommands.Add(QuestManager.CheckCommand.IsTutorialQuestClear((int)questBlock.QuestIsClearDetails.QuestId));
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
                            QuestManager.CheckCommand.StageNo(StageManager.ConvertIdToStageNo(questBlock.StageLayoutId)) :
                            QuestManager.CheckCommand.StageNoWithoutMarker(StageManager.ConvertIdToStageNo(questBlock.StageLayoutId));
                        checkCommands.Add(questCommand);
                    }
                    break;
                case QuestBlockType.PlayEvent:
                    {
                        checkCommands.Add(QuestManager.CheckCommand.EventEnd(StageManager.ConvertIdToStageNo(questBlock.StageLayoutId), questBlock.QuestEvent.EventId));
                        switch (questBlock.QuestEvent.JumpType)
                        {
                            case QuestJumpType.None:
                            case QuestJumpType.After:
                                resultCommands.Add(QuestManager.ResultCommand.EventExec(
                                                        StageManager.ConvertIdToStageNo(questBlock.StageLayoutId),
                                                        questBlock.QuestEvent.EventId,
                                                        StageManager.ConvertIdToStageNo(questBlock.QuestEvent.JumpStageId),
                                                        questBlock.QuestEvent.StartPosNo));
                                break;
                            case QuestJumpType.Before:
                                resultCommands.Add(QuestManager.ResultCommand.ExeEventAfterStageJump(
                                                        StageManager.ConvertIdToStageNo(questBlock.StageLayoutId),
                                                        questBlock.QuestEvent.EventId,
                                                        questBlock.QuestEvent.StartPosNo));
                                break;
                            case QuestJumpType.Continue:
                                resultCommands.Add(QuestManager.ResultCommand.EventExecCont(
                                                        StageManager.ConvertIdToStageNo(questBlock.StageLayoutId),
                                                        questBlock.QuestEvent.EventId,
                                                        StageManager.ConvertIdToStageNo(questBlock.QuestEvent.JumpStageId),
                                                        questBlock.QuestEvent.StartPosNo));
                                break;
                        }
                    }
                    break;
                case QuestBlockType.StageJump:
                    {
                        checkCommands.Add(QuestManager.CheckCommand.StageNo(StageManager.ConvertIdToStageNo(questBlock.StageLayoutId)));
                        resultCommands.Add(QuestManager.ResultCommand.StageJump(StageManager.ConvertIdToStageNo(questBlock.StageLayoutId), (int) questBlock.JumpPos));
                    }
                    break;
                case QuestBlockType.KillTargetEnemies:
                    {
                        // Handles kill x amount of monster type quests
                        checkCommands.Add(QuestManager.CheckCommand.EmDieLight((int)questBlock.TargetEnemy.EnemyId, (int)questBlock.TargetEnemy.Level, (int)questBlock.TargetEnemy.Amount));
                        workCommands.Add(
                            new CDataQuestProgressWork()
                            {
                                CommandNo = (uint)QuestNotifyCommand.KilledEnemyLight,
                                Work01 = (int)questBlock.TargetEnemy.EnemyId,
                                Work02 = (int)questBlock.TargetEnemy.Level,
                                Work03 = 0,
                                Work04 = 0,
                            });
                    }
                    break;
                case QuestBlockType.DeliverItemsLight:
                    {
                        foreach (var item in questBlock.DeliveryRequests)
                        {
                            checkCommands.Add(QuestManager.CheckCommand.DeliverItem((int)item.ItemId, (int)item.Amount, 0, 0));
                        }
                    }
                    break;
                case QuestBlockType.SceHitIn:
                    {
                        checkCommands.AddCheckCmdSceHitIn(Stage.StageInfoFromStageId(questBlock.StageLayoutId.Id), (int) questBlock.SceNo, questBlock.ShowMarker);
                    }
                    break;
                case QuestBlockType.SceHitOut:
                    {
                        checkCommands.AddCheckCmdSceHitOut(Stage.StageInfoFromStageId(questBlock.StageLayoutId.Id), (int)questBlock.SceNo, questBlock.ShowMarker);
                    }
                    break;
                case QuestBlockType.ReturnCheckpoint:
                    /* This is a pseudo block handeled at the state machine level */
                    checkCommands.Add(QuestManager.CheckCommand.StageNoWithoutMarker(StageManager.ConvertIdToStageNo(questBlock.StageLayoutId)));
                    break;
                case QuestBlockType.Raw:
                    /* handled generically for all blocks */
                    break;
                case QuestBlockType.DestroyGroup:
                    /* This is a pseudo block handeled at the state machine level */
                    break;
                case QuestBlockType.ExtendTime:
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

            /* Add in any additional work commands */
            workCommands.AddRange(questBlock.WorkCommands);

            result.ResultCommandList = resultCommands;
            result.WorkList = workCommands;

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
