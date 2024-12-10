using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SEntryBoardEntryBoardItemReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_ENTRY_BOARD_ENTRY_BOARD_ITEM_INFO_REQ;

        public C2SEntryBoardEntryBoardItemReq()
        {
        }

        public ulong BoardId {  get; set; }
        public uint EntryId {  get; set; }

        public class Serializer : PacketEntitySerializer<C2SEntryBoardEntryBoardItemReq>
        {
            public override void Write(IBuffer buffer, C2SEntryBoardEntryBoardItemReq obj)
            {
                WriteUInt64(buffer, obj.BoardId);
                WriteUInt32(buffer, obj.EntryId);
            }

            public override C2SEntryBoardEntryBoardItemReq Read(IBuffer buffer)
            {
                C2SEntryBoardEntryBoardItemReq obj = new C2SEntryBoardEntryBoardItemReq();
                obj.BoardId = ReadUInt64(buffer);
                obj.EntryId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
