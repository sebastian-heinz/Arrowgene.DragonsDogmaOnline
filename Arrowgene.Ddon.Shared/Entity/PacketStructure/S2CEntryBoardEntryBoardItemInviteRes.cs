using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CEntryBoardEntryBoardItemInviteRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_ENTRY_BOARD_ENTRY_BOARD_ITEM_INVITE_RES;

        public S2CEntryBoardEntryBoardItemInviteRes()
        {
        }

        public class Serializer : PacketEntitySerializer<S2CEntryBoardEntryBoardItemInviteRes>
        {
            public override void Write(IBuffer buffer, S2CEntryBoardEntryBoardItemInviteRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CEntryBoardEntryBoardItemInviteRes Read(IBuffer buffer)
            {
                S2CEntryBoardEntryBoardItemInviteRes obj = new S2CEntryBoardEntryBoardItemInviteRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
