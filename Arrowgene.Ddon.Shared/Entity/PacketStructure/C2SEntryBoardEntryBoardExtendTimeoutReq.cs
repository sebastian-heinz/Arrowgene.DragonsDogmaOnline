using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SEntryBoardEntryBoardExtendTimeoutReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_ENTRY_BOARD_ENTRY_BOARD_ITEM_EXTEND_TIMEOUT_REQ;

        public C2SEntryBoardEntryBoardExtendTimeoutReq()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SEntryBoardEntryBoardExtendTimeoutReq>
        {
            public override void Write(IBuffer buffer, C2SEntryBoardEntryBoardExtendTimeoutReq obj)
            {
            }

            public override C2SEntryBoardEntryBoardExtendTimeoutReq Read(IBuffer buffer)
            {
                C2SEntryBoardEntryBoardExtendTimeoutReq obj = new C2SEntryBoardEntryBoardExtendTimeoutReq();
                return obj;
            }
        }
    }
}

