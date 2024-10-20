using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CEntryBoardEntryBoardItemRecreateNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_ENTRY_BOARD_ENTRY_BOARD_ITEM_RECREATE_NTC;

        public S2CEntryBoardEntryBoardItemRecreateNtc()
        {
            EntryItem = new CDataEntryItem();
        }

        public ulong BoardId {  get; set; }
        public CDataEntryItem EntryItem {  get; set; }

        public class Serializer : PacketEntitySerializer<S2CEntryBoardEntryBoardItemRecreateNtc>
        {
            public override void Write(IBuffer buffer, S2CEntryBoardEntryBoardItemRecreateNtc obj)
            {
                WriteUInt64(buffer, obj.BoardId);
                WriteEntity(buffer, obj.EntryItem);
            }

            public override S2CEntryBoardEntryBoardItemRecreateNtc Read(IBuffer buffer)
            {
                S2CEntryBoardEntryBoardItemRecreateNtc obj = new S2CEntryBoardEntryBoardItemRecreateNtc();
                obj.BoardId = ReadUInt64(buffer);
                obj.EntryItem = ReadEntity<CDataEntryItem>(buffer);
                return obj;
            }
        }
    }
}
