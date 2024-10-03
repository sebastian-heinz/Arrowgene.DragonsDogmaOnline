using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SEntryBoardEntryBoardItemCreateReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_ENTRY_BOARD_ENTRY_BOARD_ITEM_CREATE_REQ;

        public C2SEntryBoardEntryBoardItemCreateReq()
        {
            Password = string.Empty;
            CreateParam = new CDataEntryItemParam();
        }

        public ulong BoardId {  get; set; }
        public string Password {  get; set; }
        public CDataEntryItemParam CreateParam { get; set; }

        public class Serializer : PacketEntitySerializer<C2SEntryBoardEntryBoardItemCreateReq>
        {
            public override void Write(IBuffer buffer, C2SEntryBoardEntryBoardItemCreateReq obj)
            {
                WriteUInt64(buffer, obj.BoardId);
                WriteMtString(buffer, obj.Password);
                WriteEntity(buffer, obj.CreateParam);
            }

            public override C2SEntryBoardEntryBoardItemCreateReq Read(IBuffer buffer)
            {
                C2SEntryBoardEntryBoardItemCreateReq obj = new C2SEntryBoardEntryBoardItemCreateReq();
                obj.BoardId = ReadUInt64(buffer);
                obj.Password = ReadMtString(buffer);
                obj.CreateParam = ReadEntity<CDataEntryItemParam>(buffer);
                return obj;
            }
        }
    }
}

