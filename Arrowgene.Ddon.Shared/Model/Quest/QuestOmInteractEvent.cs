using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Model.Quest
{
    public enum OmQuestType
    {
        MyQuest,
        WorldManageQuest
    }

    public enum OmInteractType
    {
        Touch,
        Release,
        EndText,
        OpenDoor,
        IsTouchPawnDungon,
        IsBrokenLayout,
        IsBrokenQuest,
        TouchNpcUnitMarker,
        TouchActNpc,
        UsedKey
    }

    public class QuestOmInteractEvent
    {
        public QuestId QuestId { get; set; } // Some OM interactions require a quest ID to source marker information from.
        public OmQuestType QuestType { get; set; }
        public OmInteractType InteractType { get; set; }
    }
}
