using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CEntryBoardEntryBoardItemChangeMemberNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_ENTRY_BOARD_ENTRY_BOARD_ITEM_CHANGE_MEMBER_NTC;

        public S2CEntryBoardEntryBoardItemChangeMemberNtc()
        {
            MemberData = new CDataEntryMemberData();
        }

        public bool EntryFlag {  get; set; }
        public CDataEntryMemberData MemberData { get; set; }

        public class Serializer : PacketEntitySerializer<S2CEntryBoardEntryBoardItemChangeMemberNtc>
        {
            public override void Write(IBuffer buffer, S2CEntryBoardEntryBoardItemChangeMemberNtc obj)
            {
                WriteBool(buffer, obj.EntryFlag);
                WriteEntity(buffer, obj.MemberData);
            }

            public override S2CEntryBoardEntryBoardItemChangeMemberNtc Read(IBuffer buffer)
            {
                S2CEntryBoardEntryBoardItemChangeMemberNtc obj = new S2CEntryBoardEntryBoardItemChangeMemberNtc();
                obj.EntryFlag = ReadBool(buffer);
                obj.MemberData = ReadEntity<CDataEntryMemberData>(buffer);
                return obj;
            }
        }
    }
}
