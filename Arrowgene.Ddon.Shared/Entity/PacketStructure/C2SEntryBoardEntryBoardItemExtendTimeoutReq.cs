using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SEntryBoardEntryBoardItemExtendTimeoutReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_ENTRY_BOARD_ENTRY_BOARD_ITEM_EXTEND_TIMEOUT_REQ;

        public C2SEntryBoardEntryBoardItemExtendTimeoutReq()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SEntryBoardEntryBoardItemExtendTimeoutReq>
        {
            public override void Write(IBuffer buffer, C2SEntryBoardEntryBoardItemExtendTimeoutReq obj)
            {
            }

            public override C2SEntryBoardEntryBoardItemExtendTimeoutReq Read(IBuffer buffer)
            {
                C2SEntryBoardEntryBoardItemExtendTimeoutReq obj = new C2SEntryBoardEntryBoardItemExtendTimeoutReq();
                return obj;
            }
        }
    }
}

