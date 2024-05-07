using System.Collections.Generic;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Asset
{
    public class QuestAsset
    {
        public QuestAsset()
        {
            MainQuests = new Dictionary<uint, CDataQuestList>();
        }

        public Dictionary<uint, CDataQuestList> MainQuests;
    }
}
