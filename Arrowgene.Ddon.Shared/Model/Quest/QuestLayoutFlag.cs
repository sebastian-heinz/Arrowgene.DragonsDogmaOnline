using Arrowgene.Ddon.Shared.Entity.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Model.Quest
{
    public class QuestLayoutFlag
    {
        public uint FlagId { get; set; }

        public CDataQuestLayoutFlag AsCDataQuestLayoutFlag()
        {
            return new CDataQuestLayoutFlag()
            {
                FlagId = FlagId
            };
        }
    }

    public enum QuestLayoutFlagId : uint
    {
        // q70000001
        st0201_Mysial = 1215,
        st0201_Klaus = 1216,
        st0201_Joseph = 1217,
        st0201_Leo = 1218,
        st0201_Iris = 1220,
        st0201_TheWhiteDragon0 = 1293,
        st0201_TheWhiteDragon1 = 1294,
        st0201_TheWhiteDragon2 = 1295,
        st0201_TheWhiteDragon3 = 1296,
    }
}
