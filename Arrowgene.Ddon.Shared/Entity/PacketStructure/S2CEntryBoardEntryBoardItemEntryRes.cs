using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CEntryBoardEntryBoardItemEntryRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_ENTRY_BOARD_ENTRY_BOARD_ITEM_ENTRY_RES;

        public S2CEntryBoardEntryBoardItemEntryRes()
        {
            EntryItem = new CDataEntryItem();
        }

        public CDataEntryItem EntryItem {  get; set; }

        public class Serializer : PacketEntitySerializer<S2CEntryBoardEntryBoardItemEntryRes>
        {
            public override void Write(IBuffer buffer, S2CEntryBoardEntryBoardItemEntryRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntity(buffer, obj.EntryItem);
            }

            public override S2CEntryBoardEntryBoardItemEntryRes Read(IBuffer buffer)
            {
                S2CEntryBoardEntryBoardItemEntryRes obj = new S2CEntryBoardEntryBoardItemEntryRes();
                ReadServerResponse(buffer, obj);
                obj.EntryItem = ReadEntity<CDataEntryItem>(buffer);
                return obj;
            }
        }
    }
}
