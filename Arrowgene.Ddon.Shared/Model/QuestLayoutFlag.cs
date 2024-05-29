using Arrowgene.Ddon.Shared.Entity.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Model
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
}
