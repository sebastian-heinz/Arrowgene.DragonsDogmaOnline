using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SEntryBoardEntryBoardItemEntryReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_ENTRY_BOARD_ENTRY_BOARD_ITEM_ENTRY_REQ;

        public C2SEntryBoardEntryBoardItemEntryReq()
        {
            Password = string.Empty;
        }

        public ulong BoardId {  get; set; }
        public uint EntryId {  get; set; }
        public string Password {  get; set; }

        public class Serializer : PacketEntitySerializer<C2SEntryBoardEntryBoardItemEntryReq>
        {
            public override void Write(IBuffer buffer, C2SEntryBoardEntryBoardItemEntryReq obj)
            {
                WriteUInt64(buffer, obj.BoardId);
                WriteUInt32(buffer, obj.EntryId);
                WriteMtString(buffer, obj.Password);
            }

            public override C2SEntryBoardEntryBoardItemEntryReq Read(IBuffer buffer)
            {
                C2SEntryBoardEntryBoardItemEntryReq obj = new C2SEntryBoardEntryBoardItemEntryReq();
                obj.BoardId = ReadUInt64(buffer);
                obj.EntryId = ReadUInt32(buffer);
                obj.Password = ReadMtString(buffer);
                return obj;
            }
        }
    }
}
