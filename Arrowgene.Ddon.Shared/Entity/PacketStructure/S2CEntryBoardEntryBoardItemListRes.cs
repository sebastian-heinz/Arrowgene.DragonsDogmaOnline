using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CEntryBoardEntryBoardItemListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_ENTRY_BOARD_ENTRY_BOARD_ITEM_LIST_RES;

        public S2CEntryBoardEntryBoardItemListRes()
        {
            EntryItemList = new List<CDataEntryItem>();
        }

        public ulong BoardId {  get; set; }
        public List<CDataEntryItem> EntryItemList {  get; set; }

        public class Serializer : PacketEntitySerializer<S2CEntryBoardEntryBoardItemListRes>
        {
            public override void Write(IBuffer buffer, S2CEntryBoardEntryBoardItemListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt64(buffer, obj.BoardId);
                WriteEntityList(buffer, obj.EntryItemList);
            }

            public override S2CEntryBoardEntryBoardItemListRes Read(IBuffer buffer)
            {
                S2CEntryBoardEntryBoardItemListRes obj = new S2CEntryBoardEntryBoardItemListRes();
                ReadServerResponse(buffer, obj);
                obj.BoardId = ReadUInt64(buffer);
                obj.EntryItemList = ReadEntityList<CDataEntryItem>(buffer);
                return obj;
            }
        }
    }
}
