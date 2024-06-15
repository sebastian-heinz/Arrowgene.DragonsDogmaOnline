using Arrowgene.Ddon.Shared.Entity.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Model.Quest
{
    public class QuestBlock
    {
        public QuestBlockType BlockType { get; set; }
        public ushort ProcessNo { get; set; }
        public ushort SequenceNo { get; set; }
        public ushort BlockNo { get; set; }
        public QuestAnnounceType AnnounceType { get; set; }
        public StageId StageId { get; set; }
        public ushort SubGroupId { get; set; }
        public uint SetNo { get; set; }
        public uint QuestLayoutFlag { get; set; } // For groups
        public List<uint> MyQstSetFlags { get; set; }
        public List<uint> MyQstCheckFlags { get; set; }
        public List<QuestFlag> QuestFlags { get; set; }

        public bool ShouldStageJump {  get; set; }
        public bool IsCheckpoint { get; set; }

        public QuestEvent QuestEvent {  get; set; }
        public QuestCameraEvent QuestCameraEvent { get; set; }

        public QuestPartyGatherPoint PartyGatherPoint {  get; set; }
        public QuestOmInteractEvent OmInteractEvent { get; set; }

        public bool ShowMarker { get; set; }
        public bool ResetGroup { get; set; }
        public bool BgmStop { get; set; }
        public List<uint> EnemyGroupIds { get; set; }
        public List<QuestItem> DeliveryRequests { get; set; }
        public List<QuestItem> HandPlayerItems {  get; set; }
        public List<QuestNpcOrder> NpcOrderDetails { get; set; }
        public QuestOrder QuestOrderDetails { get; set; }

        public CDataQuestProcessState QuestProcessState { get; set; }

        // Used for raw blocks
        public List<List<CDataQuestCommand>> CheckCommands { get; set; }
        public List<CDataQuestCommand> ResultCommands { get; set; }

        public QuestBlock()
        {
            NpcOrderDetails = new List<QuestNpcOrder>();
            DeliveryRequests = new List<QuestItem>();
            HandPlayerItems = new List<QuestItem>();
            QuestProcessState = new CDataQuestProcessState();

            QuestFlags = new List<QuestFlag>();
            MyQstSetFlags = new List<uint>();
            MyQstCheckFlags = new List<uint>();

            CheckCommands = new List<List<CDataQuestCommand>>();
            ResultCommands = new List<CDataQuestCommand>();
            QuestOrderDetails = new QuestOrder();
            EnemyGroupIds = new List<uint>();

            PartyGatherPoint = new QuestPartyGatherPoint();
            QuestEvent = new QuestEvent();
            QuestCameraEvent = new QuestCameraEvent();
            OmInteractEvent = new QuestOmInteractEvent();
        }
    }
}
