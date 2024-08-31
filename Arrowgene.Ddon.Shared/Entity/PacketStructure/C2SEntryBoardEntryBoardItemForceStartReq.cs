using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SEntryBoardEntryBoardItemForceStartReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_ENTRY_BOARD_ENTRY_BOARD_ITEM_FORCE_START_REQ;

        public C2SEntryBoardEntryBoardItemForceStartReq()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SEntryBoardEntryBoardItemForceStartReq>
        {
            public override void Write(IBuffer buffer, C2SEntryBoardEntryBoardItemForceStartReq obj)
            {
            }

            public override C2SEntryBoardEntryBoardItemForceStartReq Read(IBuffer buffer)
            {
                C2SEntryBoardEntryBoardItemForceStartReq obj = new C2SEntryBoardEntryBoardItemForceStartReq();
                return obj;
            }
        }
    }
}
