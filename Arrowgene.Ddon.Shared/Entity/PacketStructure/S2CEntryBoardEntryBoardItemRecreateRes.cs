using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CEntryBoardEntryBoardItemRecreateRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_ENTRY_BOARD_ENTRY_BOARD_ITEM_RECREATE_RES;

        public S2CEntryBoardEntryBoardItemRecreateRes()
        {
            EntryItem = new CDataEntryItem();
        }

        public ulong BoardId {  get; set; }
        public CDataEntryItem EntryItem {  get; set; }

        public class Serializer : PacketEntitySerializer<S2CEntryBoardEntryBoardItemRecreateRes>
        {
            public override void Write(IBuffer buffer, S2CEntryBoardEntryBoardItemRecreateRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt64(buffer, obj.BoardId);
                WriteEntity(buffer, obj.EntryItem);
            }

            public override S2CEntryBoardEntryBoardItemRecreateRes Read(IBuffer buffer)
            {
                S2CEntryBoardEntryBoardItemRecreateRes obj = new S2CEntryBoardEntryBoardItemRecreateRes();
                ReadServerResponse(buffer, obj);
                obj.BoardId = ReadUInt64(buffer);
                obj.EntryItem = ReadEntity<CDataEntryItem>(buffer);
                return obj;
            }
        }
    }
}
