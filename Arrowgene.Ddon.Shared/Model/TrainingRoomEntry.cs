using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Model
{
    public class TrainingRoomEntry
    {
        public string EntryName { get; set; } = string.Empty;
        public List<CDataLayoutEnemyData> EnemyData { get; set; } = [];
    }
}
