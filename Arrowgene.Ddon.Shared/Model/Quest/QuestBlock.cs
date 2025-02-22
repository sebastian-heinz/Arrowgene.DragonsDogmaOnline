using Arrowgene.Ddon.Shared.Entity.Structure;
using System;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Model.Quest
{
    public class Announcements
    {
        public int GeneralAnnounceId { get; set; }
        public int StageStart {  get; set; }
        public int StageClear { get; set; }
        public int EndContentsPurpose {  get; set; }
        public bool Caution { get; set; }
    }

    public class QuestReturnCheckpoint
    {
        public ushort ProcessNo { get; set; }
        public ushort BlockNo { get; set; }
    }

    public class QuestIsClear
    {
        public QuestId QuestId { get; set; }
        public QuestType QuestType { get; set; }
    }

    public class QuestBlock
    {
        public QuestBlockType BlockType { get; set; }
        public ushort ProcessNo { get; set; }
        public ushort SequenceNo { get; set; }
        public ushort BlockNo { get; set; }
        public QuestAnnounceType AnnounceType { get; set; }
        public Announcements Announcements { get; set; }
        public StageLayoutId StageId { get; set; }
        public ushort SubGroupId { get; set; }
        public uint SetNo { get; set; }
        public uint QuestLayoutFlag { get; set; } // For groups
        public List<uint> MyQstSetFlags { get; set; }
        public List<uint> MyQstCheckFlags { get; set; }
        public List<QuestFlag> QuestFlags { get; set; }
        public List<QuestFlag> CheckpointQuestFlags { get; set; }
        public List<Action<Object>> Callbacks { get; set; }

        public bool ShouldStageJump { get; set; }
        public uint JumpPos { get; set; }
        public bool IsCheckpoint { get; set; }
        public uint TimeAmount { get; set; }

        public QuestEvent QuestEvent { get; set; }
        public QuestCameraEvent QuestCameraEvent { get; set; }

        public QuestPartyGatherPoint PartyGatherPoint { get; set; }
        public QuestOmInteractEvent OmInteractEvent { get; set; }
        public QuestReturnCheckpoint CheckpointDetails { get; set; }
        public QuestIsClear QuestIsClearDetails { get; set; }

        public bool ShowMarker { get; set; }
        public bool ResetGroup { get; set; }
        public bool BgmStop { get; set; }
        public int EnemyHpPrecent {  get; set; }
        public List<uint> EnemyGroupIds { get; set; }
        public List<QuestItem> DeliveryRequests { get; set; }
        public List<QuestItem> HandPlayerItems {  get; set; }
        public List<QuestItem> ConsumePlayerItems { get; set; }
        public List<QuestNpcOrder> NpcOrderDetails { get; set; }
        public QuestOrder QuestOrderDetails { get; set; }
        
        public QuestTargetEnemy TargetEnemy { get; set; }

        public CDataQuestProcessState QuestProcessState { get; set; }

        public List<string> Annotations { get; set; } // Meta variable used to document quests

        // Used for raw blocks
        public List<List<CDataQuestCommand>> CheckCommands { get; set; }
        public List<CDataQuestCommand> ResultCommands { get; set; }
        public List<CDataQuestProgressWork> WorkCommands { get; set; }

        public QuestBlock(ushort blockNo = 0, ushort seqNo = 0)
        {
            BlockNo = blockNo;
            SequenceNo = seqNo;

            Annotations = new List<string>();

            NpcOrderDetails = new List<QuestNpcOrder>();
            DeliveryRequests = new List<QuestItem>();
            HandPlayerItems = new List<QuestItem>();
            ConsumePlayerItems = new List<QuestItem>();
            QuestProcessState = new CDataQuestProcessState();

            QuestFlags = new List<QuestFlag>();
            CheckpointQuestFlags = new List<QuestFlag>();
            MyQstSetFlags = new List<uint>();
            MyQstCheckFlags = new List<uint>();

            CheckCommands = new List<List<CDataQuestCommand>>();
            ResultCommands = new List<CDataQuestCommand>();
            WorkCommands = new List<CDataQuestProgressWork>();
            QuestOrderDetails = new QuestOrder();
            EnemyGroupIds = new List<uint>();

            PartyGatherPoint = new QuestPartyGatherPoint();
            QuestEvent = new QuestEvent();
            QuestCameraEvent = new QuestCameraEvent();
            OmInteractEvent = new QuestOmInteractEvent();
            CheckpointDetails = new QuestReturnCheckpoint();
            QuestIsClearDetails = new QuestIsClear();

            TargetEnemy = new QuestTargetEnemy();
            Announcements = new Announcements();

            Callbacks = new List<Action<Object>>();
        }

        public QuestBlock AddAnnotation(string msg)
        {
            Annotations.Add(msg);
            return this;
        }

        public QuestBlock SetBlockType(QuestBlockType type)
        {
            BlockType = type;
            return this;
        }

        public QuestBlock SetOrderInfo(ushort processNo, ushort sequenceNo, ushort blockNo)
        {
            ProcessNo = processNo;
            SequenceNo = sequenceNo;
            BlockNo = blockNo;
            return this;
        }

        public QuestBlock SetProcessNo(ushort value)
        {
            ProcessNo = value;
            return this;
        }

        public QuestBlock SetSequenceNo(ushort value)
        {
            SequenceNo = value;
            return this;
        }

        public QuestBlock SetBlockNo(ushort value)
        {
            BlockNo = value;
            return this;
        }

        public static void EvaluateAnnounceType(QuestBlock questBlock, QuestAnnounceType value)
        {
            var isCheckPoint = (value == QuestAnnounceType.Checkpoint) ||
                               (value == QuestAnnounceType.CheckpointAndUpdate);

            switch (value)
            {
                case QuestAnnounceType.Checkpoint:
                    value = QuestAnnounceType.None;
                    break;
                case QuestAnnounceType.CheckpointAndUpdate:
                    value = QuestAnnounceType.Update;
                    break;
            }

            questBlock.IsCheckpoint = isCheckPoint;
            questBlock.AnnounceType = value;
        }

        public QuestBlock SetAnnounceType(QuestAnnounceType value)
        {
            EvaluateAnnounceType(this, value);
            return this;
        }

        public QuestBlock SetAnnouncements(Announcements value)
        {
            Announcements = value;
            return this;
        }

        public QuestBlock SetStageId(StageLayoutId value)
        {
            StageId = value;
            return this;
        }

        public QuestBlock SetSubGroupId(ushort value)
        {
            SubGroupId = value;
            return this;
        }

        public QuestBlock SetSetNo(uint value)
        {
            SetNo = value;
            return this;
        }

        public QuestBlock SetQuestLayoutFlag(uint value)
        {
            QuestLayoutFlag = value;
            return this;
        }

        public QuestBlock AddMyQstSetFlags(List<uint> values)
        {
            MyQstSetFlags.AddRange(values);
            return this;
        }

        public QuestBlock AddMyQstSetFlag(uint value)
        {
            MyQstSetFlags.Add(value);
            return this;
        }

        public QuestBlock AddMyQstCheckFlags(List<uint> values)
        {
            MyQstCheckFlags.AddRange(values);
            return this;
        }

        public QuestBlock AddMyQstCheckFlag(uint value)
        {
            MyQstCheckFlags.Add(value);
            return this;
        }

        public QuestBlock AddQuestFlags(List<QuestFlag> values)
        {
            QuestFlags.AddRange(values);
            return this;
        }

        public QuestBlock AddQuestFlag(QuestFlag value)
        {
            QuestFlags.Add(value);
            return this;
        }

        public QuestBlock AddQuestFlag(QuestFlagType type, QuestFlagAction action, uint value, uint questId = 0)
        {
            QuestFlags.Add(new QuestFlag()
            {
                Type = type,
                Action = action,
                Value = (int) value,
                QuestId = (int) questId
            });
            return this;
        }

        public QuestBlock AddCheckpointQuestFlags(List<QuestFlag> values)
        {
            CheckpointQuestFlags.AddRange(values);
            return this;
        }

        public QuestBlock AddCheckpointQuestFlag(QuestFlag value)
        {
            CheckpointQuestFlags.Add(value);
            return this;
        }

        public QuestBlock AddCheckpointQuestFlag(QuestFlagType type, QuestFlagAction action, uint value, uint questId)
        {
            CheckpointQuestFlags.Add(new QuestFlag()
            {
                Type = type,
                Action = action,
                Value = (int)value,
                QuestId = (int)questId
            });
            return this;
        }

        public QuestBlock AddCheckpointQuestFlag(QuestFlagType type, QuestFlagAction action, uint value, QuestId questId)
        {
            return AddQuestFlag(type, action, value, (uint)questId);
        }

        public QuestBlock SetShouldStageJump(bool value)
        {
            ShouldStageJump = value;
            return this;
        }

        public QuestBlock SetJumpPos(uint value)
        {
            JumpPos = value;
            return this;
        }

        public QuestBlock SetIsCheckpoint(bool value)
        {
            IsCheckpoint = value;
            return this;
        }

        public QuestBlock SetTimeAmount(uint value)
        {
            TimeAmount = value;
            return this;
        }

        public QuestBlock SetQuestEvent(QuestEvent value)
        {
            QuestEvent = value;
            return this;
        }

        public QuestBlock SetQuestEvent(StageLayoutId stageId, uint eventId, uint startPos, QuestJumpType jumpType = QuestJumpType.After)
        {
            QuestEvent = new QuestEvent()
            {
                EventId = (int) eventId,
                JumpStageId = stageId,
                JumpType = jumpType,
                StartPosNo = (int) startPos
            };
            return this;
        }

        public QuestBlock SetQuestCameraEvent(QuestCameraEvent value)
        {
            QuestCameraEvent = value;
            return this;
        }

        public QuestBlock SetQuestCameraEvent(uint eventNo)
        {
            QuestCameraEvent = new QuestCameraEvent()
            {
                EventNo = (int) eventNo,
                HasCameraEvent = true
            };
            return this;
        }

        public QuestBlock SetPartyGatherPoint(QuestPartyGatherPoint value)
        {
            PartyGatherPoint = value;
            return this;
        }

        public QuestBlock SetPartyGatherPoint(int x, int y, int z)
        {
            PartyGatherPoint = new QuestPartyGatherPoint()
            {
                x = x,
                y = y,
                z = z
            };
            return this;
        }

        public QuestBlock SetOmInteractEvent(QuestOmInteractEvent value)
        {
            OmInteractEvent = value;
            return this; 
        }

        public QuestBlock SetOmInteractEvent(OmInteractType interactType, OmQuestType questType, QuestId questId)
        {
            OmInteractEvent = new QuestOmInteractEvent()
            {
                InteractType = interactType,
                QuestType = questType,
                QuestId = questId
            };
            return this;
        }

        public QuestBlock SetOmInteractEvent(OmInteractType interactType, OmQuestType questType, uint questId)
        {
            return SetOmInteractEvent(interactType, questType, (QuestId) questId);
        }

        public QuestBlock SetCheckpointDetails(QuestReturnCheckpoint value)
        {
            CheckpointDetails = value;
            return this;
        }

        public QuestBlock SetCheckpointDetails(ushort processNo, ushort blockNo)
        {
            CheckpointDetails = new QuestReturnCheckpoint()
            {
                ProcessNo = processNo,
                BlockNo = blockNo
            };
            return this;
        }

        public QuestBlock SetQuestIsClearDetails(QuestIsClear value)
        {
            QuestIsClearDetails = value;
            return this;
        }

        public QuestBlock SetQuestIsClearDetails(QuestType questType, QuestId questId)
        {
            QuestIsClearDetails = new QuestIsClear()
            {
                QuestType = questType,
                QuestId = questId,
            };
            return this;
        }

        public QuestBlock SetQuestIsClearDetails(QuestType questType, uint questId)
        {
            return SetQuestIsClearDetails(questType, (QuestId) questId);
        }

        public QuestBlock SetShowMarker(bool value)
        {
            ShowMarker = value;
            return this;
        }

        public QuestBlock SetResetGroup(bool value)
        {
            ResetGroup = value;
            return this;
        }

        public QuestBlock SetBgmStop(bool value)
        {
            BgmStop = value;
            return this;
        }

        public QuestBlock SetEnemyHpPrecent(int value)
        {
            EnemyHpPrecent = value;
            return this;
        }

        public QuestBlock AddEnemyGroupIds(List<uint> values)
        {
            EnemyGroupIds.AddRange(values);
            return this;
        }

        public QuestBlock AddEnemyGroupId(uint value)
        {
            EnemyGroupIds.Add(value);
            return this;
        }

        public QuestBlock AddDeliveryRequests(List<QuestItem> values)
        {
            DeliveryRequests.AddRange(values);
            return this;
        }

        public QuestBlock AddDeliveryRequests(QuestItem value)
        {
            DeliveryRequests.Add(value);
            return this;
        }

        public QuestBlock AddDeliveryRequests(ItemId itemId, uint amount)
        {
            DeliveryRequests.Add(new QuestItem()
            {
                ItemId = (uint) itemId,
                Amount = amount
            });
            return this;
        }

        public QuestBlock AddHandPlayerItems(List<QuestItem> values)
        {
            HandPlayerItems.AddRange(values);
            return this;
        }

        public QuestBlock AddHandPlayerItem(QuestItem value)
        {
            HandPlayerItems.Add(value);
            return this;
        }

        public QuestBlock AddHandPlayerItem(ItemId itemId, uint amount)
        {
            HandPlayerItems.Add(new QuestItem()
            {
                ItemId = (uint) itemId,
                Amount = amount
            });
            return this;
        }

        public QuestBlock AddConsumePlayerItems(List<QuestItem> values)
        {
            ConsumePlayerItems.AddRange(values);
            return this;
        }

        public QuestBlock AddNpcOrderDetails(List<QuestNpcOrder> values)
        {
            NpcOrderDetails.AddRange(values);
            return this;
        }

        public QuestBlock AddNpcOrderDetails(QuestNpcOrder value)
        {
            NpcOrderDetails.Add(value);
            return this;
        }

        public QuestBlock AddNpcOrderDetails(StageLayoutId stageId, NpcId npcId, uint msgId, QuestId questId = QuestId.None)
        {
            NpcOrderDetails.Add(new QuestNpcOrder()
            {
                NpcId = npcId,
                StageId = stageId,
                MsgId = (int) msgId,
                QuestId = questId,
            });
            return this;
        }

        public QuestBlock AddNpcOrderDetails(StageLayoutId stageId, NpcId npcId, uint msgId, uint questId = 0)
        {
            return AddNpcOrderDetails(stageId, npcId, msgId, (QuestId)questId);
        }

        public QuestBlock SetQuestOrderDetails(QuestOrder value)
        {
            QuestOrderDetails = value;
            return this;
        }

        public QuestBlock SetQuestOrderDetails(QuestType type, QuestId questId)
        {
            QuestOrderDetails = new QuestOrder()
            {
                QuestType = type,
                QuestId = questId
            };
            return this;
        }

        public QuestBlock SetQuestOrderDetails(QuestType type, uint questId)
        {
            return SetQuestOrderDetails(type, (QuestId) questId);
        }

        public QuestBlock SetTargetEnemy(QuestTargetEnemy value)
        {
            TargetEnemy = value;
            return this;
        }

        public QuestBlock SetTargetEnemy(EnemyId enemyId, uint amount, uint level)
        {
            return SetTargetEnemy(new QuestTargetEnemy()
            {
                EnemyId = (uint) enemyId,
                Amount = amount,
                Level = level
            });
        }

        public QuestBlock AddCheckCommands(List<List<CDataQuestCommand>> values)
        {
            CheckCommands.AddRange(values);
            return this;
        }

        public QuestBlock AddCheckCommands(List<CDataQuestCommand> value)
        {
            CheckCommands.Add(value);
            return this;
        }

        public QuestBlock AddCheckCommand(CDataQuestCommand value)
        {
            if (CheckCommands.Count == 0)
            {
                CheckCommands.Add(new List<CDataQuestCommand>());
            }

            CheckCommands[0].Add(value);
            return this;
        }

        public QuestBlock AddCheckCommand(ushort checkCommand, int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
        {
            return AddCheckCommand(new CDataQuestCommand() { Command = checkCommand, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 });
        }

        public QuestBlock AddCheckCommand(QuestCheckCommand checkCommand, int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
        {
            return AddCheckCommand((ushort) checkCommand, param01, param02, param03, param04);
        }

        public QuestBlock AddResultCommands (List<CDataQuestCommand> values)
        {
            ResultCommands.AddRange(values);
            return this;
        }

        public QuestBlock AddResultCommand(CDataQuestCommand value)
        {
            ResultCommands.Add(value);
            return this;
        }

        public QuestBlock AddResultCommand(ushort resultCommand, int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
        {
            return AddResultCommand(new CDataQuestCommand()
            {
                Command = resultCommand,
                Param01 = param01,
                Param02 = param02,
                Param03 = param03,
                Param04 = param04,
            });
        }

        public QuestBlock AddResultCommand(QuestResultCommand resultCommand, int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
        {
            return AddResultCommand((ushort) resultCommand, param01, param02, param03, param04);
        }
    }
}
