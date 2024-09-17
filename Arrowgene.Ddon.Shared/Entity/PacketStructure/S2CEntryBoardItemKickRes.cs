using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CEntryBoardItemKickRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_ENTRY_BOARD_ITEM_KICK_RES;

        public S2CEntryBoardItemKickRes()
        {
        }

        public class Serializer : PacketEntitySerializer<S2CEntryBoardItemKickRes>
        {
            public override void Write(IBuffer buffer, S2CEntryBoardItemKickRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CEntryBoardItemKickRes Read(IBuffer buffer)
            {
                S2CEntryBoardItemKickRes obj = new S2CEntryBoardItemKickRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
