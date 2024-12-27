using Arrowgene.Ddon.Shared.Model.Quest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Model
{
    public class AreaRank
    {
        public QuestAreaId AreaId { get; set; }
        public uint Rank { get; set; }
        public uint Point { get; set; }
        public uint WeekPoint { get; set; }
        public uint LastWeekPoint { get; set; }
        public bool CanReceiveSupply { get; set; }
    }
}
