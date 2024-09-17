using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CEntryBoardEntryBoardItemExtendTimeoutRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_ENTRY_BOARD_ENTRY_BOARD_ITEM_EXTEND_TIMEOUT_RES;

        public S2CEntryBoardEntryBoardItemExtendTimeoutRes()
        {
        }

        public ushort TimeOut {  get; set; }

        public class Serializer : PacketEntitySerializer<S2CEntryBoardEntryBoardItemExtendTimeoutRes>
        {
            public override void Write(IBuffer buffer, S2CEntryBoardEntryBoardItemExtendTimeoutRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt16(buffer, obj.TimeOut);
            }

            public override S2CEntryBoardEntryBoardItemExtendTimeoutRes Read(IBuffer buffer)
            {
                S2CEntryBoardEntryBoardItemExtendTimeoutRes obj = new S2CEntryBoardEntryBoardItemExtendTimeoutRes();
                ReadServerResponse(buffer, obj);
                obj.TimeOut = ReadUInt16(buffer);
                return obj;
            }
        }
    }
}

