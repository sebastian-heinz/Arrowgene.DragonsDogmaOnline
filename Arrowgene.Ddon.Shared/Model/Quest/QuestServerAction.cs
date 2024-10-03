using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Model.Quest
{
    public enum QuestSeverActionType
    {
        None,
        OmSetInstantValue
    }

    public class QuestServerAction
    {
        public QuestSeverActionType ActionType { get; set; }
        public StageId StageId { get; set; }
        public ulong Key { get; set; }
        public uint Value { get; set; }
        public OmInstantValueAction OmInstantValueAction { get; set; }
    }
}
