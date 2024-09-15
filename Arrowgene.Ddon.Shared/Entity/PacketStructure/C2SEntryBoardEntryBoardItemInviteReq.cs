using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SEntryBoardEntryBoardItemInviteReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_ENTRY_BOARD_ENTRY_BOARD_ITEM_INVITE_REQ;

        public C2SEntryBoardEntryBoardItemInviteReq()
        {
            CharacterIds = new List<CDataCommonU32>();
        }

        public List<CDataCommonU32> CharacterIds { get; set; }

        public class Serializer : PacketEntitySerializer<C2SEntryBoardEntryBoardItemInviteReq>
        {
            public override void Write(IBuffer buffer, C2SEntryBoardEntryBoardItemInviteReq obj)
            {
                WriteEntityList(buffer, obj.CharacterIds);
            }

            public override C2SEntryBoardEntryBoardItemInviteReq Read(IBuffer buffer)
            {
                C2SEntryBoardEntryBoardItemInviteReq obj = new C2SEntryBoardEntryBoardItemInviteReq();
                obj.CharacterIds = ReadEntityList<CDataCommonU32>(buffer);
                return obj;
            }
        }
    }
}
