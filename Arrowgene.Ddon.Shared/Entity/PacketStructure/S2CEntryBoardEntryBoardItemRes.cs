using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CEntryBoardEntryBoardItemRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_ENTRY_BOARD_ENTRY_BOARD_ITEM_INFO_RES;

        public S2CEntryBoardEntryBoardItemRes()
        {
            EntryItemData = new CDataEntryItem();
        }

        public ulong BoardId {  get; set; }
        public CDataEntryItem EntryItemData {  get; set; }

        public class Serializer : PacketEntitySerializer<S2CEntryBoardEntryBoardItemRes>
        {
            public override void Write(IBuffer buffer, S2CEntryBoardEntryBoardItemRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt64(buffer, obj.BoardId);
                WriteEntity(buffer, obj.EntryItemData);
            }

            public override S2CEntryBoardEntryBoardItemRes Read(IBuffer buffer)
            {
                S2CEntryBoardEntryBoardItemRes obj = new S2CEntryBoardEntryBoardItemRes();
                ReadServerResponse(buffer, obj);
                obj.BoardId = ReadUInt64(buffer);
                obj.EntryItemData = ReadEntity<CDataEntryItem>(buffer);
                return obj;
            }
        }
    }
}
