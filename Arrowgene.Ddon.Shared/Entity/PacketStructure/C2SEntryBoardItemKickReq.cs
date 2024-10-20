using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SEntryBoardItemKickReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_ENTRY_BOARD_ITEM_KICK_REQ;

        public C2SEntryBoardItemKickReq()
        {
        }

        public uint CharacterId {  get; set; }

        public class Serializer : PacketEntitySerializer<C2SEntryBoardItemKickReq>
        {
            public override void Write(IBuffer buffer, C2SEntryBoardItemKickReq obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
            }

            public override C2SEntryBoardItemKickReq Read(IBuffer buffer)
            {
                C2SEntryBoardItemKickReq obj = new C2SEntryBoardItemKickReq();
                obj.CharacterId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
