using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CEntryBoardEntryBoardItemInfoMyselfRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_ENTRY_BOARD_ENTRY_BOARD_ITEM_INFO_MYSELF_RES;

        public S2CEntryBoardEntryBoardItemInfoMyselfRes()
        {
            EntryItem = new CDataEntryItem();
        }

        public ulong BoardId { get; set; }
        public CDataEntryItem EntryItem { get; set; }

        public class Serializer : PacketEntitySerializer<S2CEntryBoardEntryBoardItemInfoMyselfRes>
        {
            public override void Write(IBuffer buffer, S2CEntryBoardEntryBoardItemInfoMyselfRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt64(buffer, obj.BoardId);
                WriteEntity(buffer, obj.EntryItem);
            }

            public override S2CEntryBoardEntryBoardItemInfoMyselfRes Read(IBuffer buffer)
            {
                S2CEntryBoardEntryBoardItemInfoMyselfRes obj = new S2CEntryBoardEntryBoardItemInfoMyselfRes();
                ReadServerResponse(buffer, obj);
                obj.BoardId = ReadUInt64(buffer);
                obj.EntryItem = ReadEntity<CDataEntryItem>(buffer);
                return obj;
            }
        }
    }
}
