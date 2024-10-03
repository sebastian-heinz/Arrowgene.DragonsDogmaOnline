using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CEntryBoardEntryBoardItemLeaveRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_ENTRY_BOARD_ENTRY_BOARD_ITEM_LEAVE_RES;

        public S2CEntryBoardEntryBoardItemLeaveRes()
        {
        }

        public class Serializer : PacketEntitySerializer<S2CEntryBoardEntryBoardItemLeaveRes>
        {
            public override void Write(IBuffer buffer, S2CEntryBoardEntryBoardItemLeaveRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CEntryBoardEntryBoardItemLeaveRes Read(IBuffer buffer)
            {
                S2CEntryBoardEntryBoardItemLeaveRes obj = new S2CEntryBoardEntryBoardItemLeaveRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
