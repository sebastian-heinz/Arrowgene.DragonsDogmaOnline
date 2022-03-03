using Arrowgene.Buffers;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class LobbyLobbyDataMsgHandler : StructurePacketHandler<GameClient, C2SLobbyLobbyDataMsgReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(LobbyLobbyDataMsgHandler));

        public LobbyLobbyDataMsgHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SLobbyLobbyDataMsgReq> packet)
        {
            S2CLobbyLobbyDataMsgNotice res = new S2CLobbyLobbyDataMsgNotice();
            res.Type = packet.Structure.Type;
            res.CharacterID = packet.Structure.CharacterID;
            res.RpcPacket = packet.Structure.RpcPacket;
            res.OnlineStatus = 1; // TODO: Figure out OnlineStatus values

            foreach (GameClient otherClient in Server.Clients)
            {
                if (otherClient != client)
                {
                    client.Send(res);
                }
            }

            IBuffer rpcPacketBuffer = new StreamBuffer(packet.Structure.RpcPacket);
            rpcPacketBuffer.SetPositionStart();
            rpcPacketBuffer.ReadUInt32();
            rpcPacketBuffer.ReadUInt32();
            rpcPacketBuffer.ReadUInt32();
            rpcPacketBuffer.ReadUInt32();
            rpcPacketBuffer.ReadUInt32();
            rpcPacketBuffer.ReadUInt32();
            rpcPacketBuffer.ReadUInt32();
            rpcPacketBuffer.ReadUInt32();
            client.X = rpcPacketBuffer.ReadDouble(Endianness.Big); //m_CliffPosX
            client.Y = rpcPacketBuffer.ReadFloat(Endianness.Big);
            client.Z = rpcPacketBuffer.ReadDouble(Endianness.Big);

            float m_CliffNormalX = rpcPacketBuffer.ReadFloat(Endianness.Big);
            float m_CliffNormalY = rpcPacketBuffer.ReadFloat(Endianness.Big);
            float m_CliffNormalZ = rpcPacketBuffer.ReadFloat(Endianness.Big);

            // double m_CliffStartPosX = rpcPacketBuffer.ReadDouble(Endianness.Big);
            // float m_CliffStartPosY = rpcPacketBuffer.ReadFloat(Endianness.Big);
            // double m_CliffStartPosZ = rpcPacketBuffer.ReadDouble(Endianness.Big);
            // 
            // double m_CliffStartOldPosX = rpcPacketBuffer.ReadDouble(Endianness.Big);
            // float m_CliffStartOldPosY = rpcPacketBuffer.ReadFloat(Endianness.Big);
            // double m_CliffStartOldPosZ = rpcPacketBuffer.ReadDouble(Endianness.Big);
        }
    }
}
