using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SLobbyLobbyDataMsgReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_LOBBY_LOBBY_DATA_MSG_REQ;

        public C2SLobbyLobbyDataMsgReq()
        {
            Type=0;
            CharacterID=0;
            RpcPacket=new byte[0];
        }

        public byte Type { get; set; }
        public uint CharacterID { get; set; }
        public byte[] RpcPacket { get; set; }

        public class Serializer : PacketEntitySerializer<C2SLobbyLobbyDataMsgReq>
        {
            public override void Write(IBuffer buffer, C2SLobbyLobbyDataMsgReq obj)
            {
                WriteByte(buffer, obj.Type);
                WriteUInt32(buffer, obj.CharacterID);
                WriteInt32(buffer, obj.RpcPacket.Length);
                WriteByteArray(buffer, obj.RpcPacket);
            }

            public override C2SLobbyLobbyDataMsgReq Read(IBuffer buffer)
            {
                C2SLobbyLobbyDataMsgReq obj = new C2SLobbyLobbyDataMsgReq();
                obj.Type = ReadByte(buffer);
                obj.CharacterID = ReadUInt32(buffer);
                int rpcPacketLength = ReadInt32(buffer);
                obj.RpcPacket = ReadByteArray(buffer, rpcPacketLength);
                return obj;
            }
        }
    }
}