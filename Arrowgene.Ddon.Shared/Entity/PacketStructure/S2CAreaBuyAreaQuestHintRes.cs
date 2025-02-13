using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CAreaBuyAreaQuestHintRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_AREA_BUY_AREA_QUEST_HINT_RES;

        public uint UpdateGold { get; set; }

        public class Serializer : PacketEntitySerializer<S2CAreaBuyAreaQuestHintRes>
        {
            public override void Write(IBuffer buffer, S2CAreaBuyAreaQuestHintRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.UpdateGold);
            }

            public override S2CAreaBuyAreaQuestHintRes Read(IBuffer buffer)
            {
                S2CAreaBuyAreaQuestHintRes obj = new S2CAreaBuyAreaQuestHintRes();
                ReadServerResponse(buffer, obj);
                obj.UpdateGold = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
