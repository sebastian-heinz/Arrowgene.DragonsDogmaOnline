using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CLobbyLobbyDataMsgNotice : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_LOBBY_LOBBY_DATA_MSG_NTC;

        public S2CLobbyLobbyDataMsgNotice()
        {
            Type = 0;
            CharacterId = 0;
            RpcPacket = new byte[0];
            OnlineStatus = 0;
        }

        public RpcMsgType Type { get; set; }
        public uint CharacterId { get; set; }
        public byte[] RpcPacket { get; set; }
        public OnlineStatus OnlineStatus { get; set; }

        public class Serializer : PacketEntitySerializer<S2CLobbyLobbyDataMsgNotice>
        {
            public override void Write(IBuffer buffer, S2CLobbyLobbyDataMsgNotice obj)
            {
                WriteByte(buffer, (byte) obj.Type);
                WriteUInt32(buffer, obj.CharacterId);
                WriteInt32(buffer, obj.RpcPacket.Length);
                WriteByteArray(buffer, obj.RpcPacket);
                WriteByte(buffer, (byte) obj.OnlineStatus);
            }

            public override S2CLobbyLobbyDataMsgNotice Read(IBuffer buffer)
            {
                S2CLobbyLobbyDataMsgNotice obj = new S2CLobbyLobbyDataMsgNotice();
                obj.Type = (RpcMsgType) ReadByte(buffer);
                obj.CharacterId = ReadUInt32(buffer);
                int rpcPacketLength = ReadInt32(buffer);
                obj.RpcPacket = ReadByteArray(buffer, rpcPacketLength);
                obj.OnlineStatus = (OnlineStatus) ReadByte(buffer);
                return obj;
            }
        }
    }
}
