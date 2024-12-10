using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SEntryBoardEntryBoardItemInfoMyselfReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_ENTRY_BOARD_ENTRY_BOARD_ITEM_INFO_MYSELF_REQ;

        public C2SEntryBoardEntryBoardItemInfoMyselfReq()
        {
        }

        public bool Unk0 {  get; set; }

        public class Serializer : PacketEntitySerializer<C2SEntryBoardEntryBoardItemInfoMyselfReq>
        {
            public override void Write(IBuffer buffer, C2SEntryBoardEntryBoardItemInfoMyselfReq obj)
            {
                WriteBool(buffer, obj.Unk0);
            }

            public override C2SEntryBoardEntryBoardItemInfoMyselfReq Read(IBuffer buffer)
            {
                C2SEntryBoardEntryBoardItemInfoMyselfReq obj = new C2SEntryBoardEntryBoardItemInfoMyselfReq();
                obj.Unk0 = ReadBool(buffer);
                return obj;
            }
        }
    }
}
