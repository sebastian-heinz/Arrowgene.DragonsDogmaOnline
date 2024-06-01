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
        public List<uint> QuestLayoutFlagsOn { get; set; }
        public List<uint> QuestLayoutFlagsOff { get; set; }
        public List<uint> MyQstSetFlags { get; set; }
        public List<uint> MyQstCheckFlags { get; set; }

        public bool ShowMarker { get; set; }
        public bool ResetGroup { get; set; }
        public List<uint> EnemyGroupIds { get; set; }
        public List<QuestDeliveryItem> DeliveryRequests { get; set; }
        public List<QuestNpcOrder> NpcOrderDetails { get; set; }
        public QuestOrder QuestOrderDetails { get; set; }

        public CDataQuestProcessState QuestProcessState { get; set; }

        // Used for raw blocks
        public List<CDataQuestCommand> CheckCommands { get; set; }
        public List<CDataQuestCommand> ResultCommands { get; set; }

        public QuestBlock()
        {
            NpcOrderDetails = new List<QuestNpcOrder>();
            DeliveryRequests = new List<QuestDeliveryItem>();
            QuestProcessState = new CDataQuestProcessState();
            QuestLayoutFlagsOn = new List<uint>();
            QuestLayoutFlagsOff = new List<uint>();

            MyQstSetFlags = new List<uint>();
            MyQstCheckFlags = new List<uint>();

            CheckCommands = new List<CDataQuestCommand>();
            ResultCommands = new List<CDataQuestCommand>();
            QuestOrderDetails = new QuestOrder();
            EnemyGroupIds = new List<uint>();
        }
    }
}
