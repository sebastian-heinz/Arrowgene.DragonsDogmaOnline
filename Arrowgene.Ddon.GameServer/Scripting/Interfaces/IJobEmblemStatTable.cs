using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Scripting.Interfaces
{
    public abstract class IJobEmblemStatTable
    {
        public abstract EquipStatId StatId { get; }
        public List<CDataJobEmblemStatUpgradeData> StatUpgradeData { get; private set; }

        public IJobEmblemStatTable()
        {
            StatUpgradeData = new List<CDataJobEmblemStatUpgradeData>();
        }

        public IJobEmblemStatTable Add(byte level, ushort amount)
        {
            CDataJobEmblemStatUpgradeData upgrade = new CDataJobEmblemStatUpgradeData()
            {
                StatId = StatId,
                Level = level,
                Amount = amount,
            };
            StatUpgradeData.Add(upgrade);
            return this;
        }
    }
}
