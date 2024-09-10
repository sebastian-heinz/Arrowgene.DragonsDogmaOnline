using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CEntryBoardEntryBoardItemReadyRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_ENTRY_BOARD_ENTRY_BOARD_ITEM_READY_RES;

        public S2CEntryBoardEntryBoardItemReadyRes()
        {
        }

        public class Serializer : PacketEntitySerializer<S2CEntryBoardEntryBoardItemReadyRes>
        {
            public override void Write(IBuffer buffer, S2CEntryBoardEntryBoardItemReadyRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CEntryBoardEntryBoardItemReadyRes Read(IBuffer buffer)
            {
                S2CEntryBoardEntryBoardItemReadyRes obj = new S2CEntryBoardEntryBoardItemReadyRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
