using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SEntryBoardEntryBoardItemReadyReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_ENTRY_BOARD_ENTRY_BOARD_ITEM_READY_REQ;

        public C2SEntryBoardEntryBoardItemReadyReq()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SEntryBoardEntryBoardItemReadyReq>
        {
            public override void Write(IBuffer buffer, C2SEntryBoardEntryBoardItemReadyReq obj)
            {
            }

            public override C2SEntryBoardEntryBoardItemReadyReq Read(IBuffer buffer)
            {
                C2SEntryBoardEntryBoardItemReadyReq obj = new C2SEntryBoardEntryBoardItemReadyReq();
                return obj;
            }
        }
    }
}
