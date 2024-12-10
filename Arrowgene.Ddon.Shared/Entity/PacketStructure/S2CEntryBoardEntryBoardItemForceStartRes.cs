using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CEntryBoardEntryBoardItemForceStartRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_ENTRY_BOARD_ENTRY_BOARD_ITEM_FORCE_START_RES;

        public S2CEntryBoardEntryBoardItemForceStartRes()
        {
        }

        public class Serializer : PacketEntitySerializer<S2CEntryBoardEntryBoardItemForceStartRes>
        {
            public override void Write(IBuffer buffer, S2CEntryBoardEntryBoardItemForceStartRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CEntryBoardEntryBoardItemForceStartRes Read(IBuffer buffer)
            {
                S2CEntryBoardEntryBoardItemForceStartRes obj = new S2CEntryBoardEntryBoardItemForceStartRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
