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
            res.CharacterId = client.Character.Id;
            res.RpcPacket = packet.Structure.RpcPacket;
            res.OnlineStatus = 0x08; // TODO: Figure out OnlineStatus values

            foreach (GameClient otherClient in Server.Clients)
            {
                if (otherClient != client)
                {
                    client.Send(res);
                }
            }

            IBuffer buffer = packet.AsBuffer();
            byte unk0 = buffer.ReadByte();
            uint unk1 = buffer.ReadUInt32(Endianness.Big);
            int length = buffer.ReadInt32(Endianness.Big);
            if (length > 52)
            {
                buffer.ReadUInt32();
                buffer.ReadUInt32();
                buffer.ReadUInt32();
                buffer.ReadUInt32();
                buffer.ReadUInt32(); // 20
                buffer.ReadUInt32();
                buffer.ReadUInt32();
                buffer.ReadUInt32(); // 32
                client.X = buffer.ReadDouble(Endianness.Big); //m_CliffPosX
                client.Y = buffer.ReadFloat(Endianness.Big);
                client.Z = buffer.ReadDouble(Endianness.Big); //52
            }

            // float m_CliffNormalX = buffer.ReadFloat(Endianness.Big);
            // float m_CliffNormalY = buffer.ReadFloat(Endianness.Big);
            // float m_CliffNormalZ = buffer.ReadFloat(Endianness.Big);

            // double m_CliffStartPosX = buffer.ReadDouble(Endianness.Big);
            // float m_CliffStartPosY = buffer.ReadFloat(Endianness.Big);
            // double m_CliffStartPosZ = buffer.ReadDouble(Endianness.Big);
            // 
            // double m_CliffStartOldPosX = buffer.ReadDouble(Endianness.Big);
            // float m_CliffStartOldPosY = buffer.ReadFloat(Endianness.Big);
            // double m_CliffStartOldPosZ = buffer.ReadDouble(Endianness.Big);
        }
    }
}
