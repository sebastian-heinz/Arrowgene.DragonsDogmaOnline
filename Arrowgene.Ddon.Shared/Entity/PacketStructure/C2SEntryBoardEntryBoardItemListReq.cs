using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SEntryBoardEntryBoardItemListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_ENTRY_BOARD_ENTRY_BOARD_ITEM_LIST_REQ;

        public C2SEntryBoardEntryBoardItemListReq()
        {
            SearchParameter = new CDataEntryBoardItemSearchParameter();
        }

        public ulong BoardId { get; set; }
        public uint Offset {  get; set; }
        public uint Num {  get; set; }
        public CDataEntryBoardItemSearchParameter SearchParameter { get; set; }

        public class Serializer : PacketEntitySerializer<C2SEntryBoardEntryBoardItemListReq>
        {
            public override void Write(IBuffer buffer, C2SEntryBoardEntryBoardItemListReq obj)
            {
                WriteUInt64(buffer, obj.BoardId);
                WriteUInt32(buffer, obj.Offset);
                WriteUInt32(buffer, obj.Num);
                WriteEntity(buffer, obj.SearchParameter);
            }

            public override C2SEntryBoardEntryBoardItemListReq Read(IBuffer buffer)
            {
                C2SEntryBoardEntryBoardItemListReq obj = new C2SEntryBoardEntryBoardItemListReq();
                obj.BoardId = ReadUInt64(buffer);
                obj.Offset = ReadUInt32(buffer);
                obj.Num = ReadUInt32(buffer);
                obj.SearchParameter = ReadEntity<CDataEntryBoardItemSearchParameter>(buffer);
                return obj;
            }
        }
    }
}
