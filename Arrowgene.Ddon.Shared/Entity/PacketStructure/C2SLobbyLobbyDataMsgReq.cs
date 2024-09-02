using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SLobbyLobbyDataMsgReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_LOBBY_LOBBY_DATA_MSG_REQ;

        public C2SLobbyLobbyDataMsgReq()
        {
            Type = 0;
            CharacterId = 0;
            RpcPacket = new byte[0];
        }

        public RpcMsgType Type { get; set; }
        public uint CharacterId { get; set; }
        public byte[] RpcPacket { get; set; }

        public class Serializer : PacketEntitySerializer<C2SLobbyLobbyDataMsgReq>
        {
            public override void Write(IBuffer buffer, C2SLobbyLobbyDataMsgReq obj)
            {
                WriteByte(buffer, (byte) obj.Type);
                WriteUInt32(buffer, obj.CharacterId);
                WriteInt32(buffer, obj.RpcPacket.Length);
                WriteByteArray(buffer, obj.RpcPacket);
            }

            public override C2SLobbyLobbyDataMsgReq Read(IBuffer buffer)
            {
                C2SLobbyLobbyDataMsgReq obj = new C2SLobbyLobbyDataMsgReq();
                obj.Type = (RpcMsgType) ReadByte(buffer);
                obj.CharacterId = ReadUInt32(buffer);
                int rpcPacketLength = ReadInt32(buffer);
                obj.RpcPacket = ReadByteArray(buffer, rpcPacketLength);
                return obj;
            }
        }
    }
}
