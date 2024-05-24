using Arrowgene.Ddon.Shared.Entity.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Model
{
    public class QuestBlock
    {
        public QuestBlockType BlockType { get; set; }
        public ushort ProcessNo { get; set; }
        public ushort SequenceNo {  get; set; }
        public ushort BlockNo { get; set; }
        public QuestAnnounceType AnnounceType { get; set; }
        // Used for enemy blocks
        public StageId StageId {  get; set; }
        public ushort SubGroupId {  get; set; }
        public uint SetNo { get; set; }
        public uint StartingIndex {  get; set; }
        public uint QuestLayoutFlag { get; set; } // For groups
        public List<uint> QuestLayoutFlagsOn {  get; set; }
        public List<uint> QuestLayoutFlagsOff { get; set; }
        public bool ShowMarker { get; set; }

        public List<Enemy> Enemies { get; set; }
        public List<QuestDeliveryItem> DeliveryRequests {  get; set; }
        public QuestNpcOrder NpcOrderDetails {  get; set; }
        public FetchQuestItemLocation FetchItemLocation { get; set; }

        public CDataQuestProcessState QuestProcessState { get; set; }

        public QuestBlock()
        {
            Enemies = new List<Enemy>();
            NpcOrderDetails = new QuestNpcOrder();
            DeliveryRequests = new List<QuestDeliveryItem>();
            QuestProcessState = new CDataQuestProcessState();
            FetchItemLocation = new FetchQuestItemLocation();
            QuestLayoutFlagsOn = new List<uint>();
            QuestLayoutFlagsOff = new List<uint>();
        }
    }

    public class FetchQuestItemLocation
    {
        public double x {  get; set; }
        public float y {  get; set; }
        public double z {  get; set; }
    }
}
