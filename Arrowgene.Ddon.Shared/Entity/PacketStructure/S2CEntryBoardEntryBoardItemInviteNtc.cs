using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CEntryBoardEntryBoardItemInviteNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_ENTRY_BOARD_ENTRY_BOARD_ITEM_INVITE_NTC;

        public S2CEntryBoardEntryBoardItemInviteNtc()
        {
            Character = new CDataCharacterListElement();
            Comment = string.Empty;
        }

        public CDataCharacterListElement Character { get; set; }
        public ulong BoardId {  get; set; }
        public uint ItemId { get; set; }
        public string Comment {  get; set; }

        public class Serializer : PacketEntitySerializer<S2CEntryBoardEntryBoardItemInviteNtc>
        {
            public override void Write(IBuffer buffer, S2CEntryBoardEntryBoardItemInviteNtc obj)
            {
                WriteEntity(buffer, obj.Character);
                WriteUInt64(buffer, obj.BoardId);
                WriteUInt32(buffer, obj.ItemId);
                WriteMtString(buffer, obj.Comment);
            }

            public override S2CEntryBoardEntryBoardItemInviteNtc Read(IBuffer buffer)
            {
                S2CEntryBoardEntryBoardItemInviteNtc obj = new S2CEntryBoardEntryBoardItemInviteNtc();
                obj.Character = ReadEntity<CDataCharacterListElement>(buffer);
                obj.BoardId = ReadUInt64(buffer);
                obj.ItemId = ReadUInt32(buffer);
                obj.Comment = ReadMtString(buffer);
                return obj;
            }
        }
    }
}
