using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SEntryBoardEntryBoardItemLeaveReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_ENTRY_BOARD_ENTRY_BOARD_ITEM_LEAVE_REQ;

        public C2SEntryBoardEntryBoardItemLeaveReq()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SEntryBoardEntryBoardItemLeaveReq>
        {
            public override void Write(IBuffer buffer, C2SEntryBoardEntryBoardItemLeaveReq obj)
            {
            }

            public override C2SEntryBoardEntryBoardItemLeaveReq Read(IBuffer buffer)
            {
                C2SEntryBoardEntryBoardItemLeaveReq obj = new C2SEntryBoardEntryBoardItemLeaveReq();
                return obj;
            }
        }
    }
}
