using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CEntryBoardEntryBoardItemInfoChangeRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_ENTRY_BOARD_ENTRY_BOARD_ITEM_INFO_CHANGE_RES;

        public S2CEntryBoardEntryBoardItemInfoChangeRes()
        {
        }

        public CDataEntryItem EntryItemData { get; set; } = new();

        public class Serializer : PacketEntitySerializer<S2CEntryBoardEntryBoardItemInfoChangeRes>
        {
            public override void Write(IBuffer buffer, S2CEntryBoardEntryBoardItemInfoChangeRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntity(buffer, obj.EntryItemData);
            }

            public override S2CEntryBoardEntryBoardItemInfoChangeRes Read(IBuffer buffer)
            {
                S2CEntryBoardEntryBoardItemInfoChangeRes obj = new S2CEntryBoardEntryBoardItemInfoChangeRes();
                ReadServerResponse(buffer, obj);
                obj.EntryItemData = ReadEntity<CDataEntryItem>(buffer);
                return obj;
            }
        }
    }
}
