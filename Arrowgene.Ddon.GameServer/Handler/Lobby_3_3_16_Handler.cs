using Arrowgene.Buffers;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class Lobby_3_3_16_Handler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(Lobby_3_3_16_Handler));

        public Lobby_3_3_16_Handler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_LOBBY_3_3_16_NTC;

        public override void Handle(GameClient client, IPacket packet)
        {
            IBuffer buffer = packet.AsBuffer();
            byte unk0 = buffer.ReadByte();
            uint unk1 = buffer.ReadUInt32(Endianness.Big);
            int unk2length = buffer.ReadInt32(Endianness.Big);
            buffer.ReadUInt32();
            buffer.ReadUInt32();
            buffer.ReadUInt32();
            buffer.ReadUInt32();
            buffer.ReadUInt32();
            buffer.ReadUInt32();
            buffer.ReadUInt32();
            buffer.ReadUInt32();
            client.X = buffer.ReadDouble(Endianness.Big); //m_CliffPosX
            client.Y = buffer.ReadFloat(Endianness.Big);
            client.Z = buffer.ReadDouble(Endianness.Big);

            float m_CliffNormalX = buffer.ReadFloat(Endianness.Big);
            float m_CliffNormalY = buffer.ReadFloat(Endianness.Big);
            float m_CliffNormalZ = buffer.ReadFloat(Endianness.Big);

            // double m_CliffStartPosX = buffer.ReadDouble(Endianness.Big);
            // float m_CliffStartPosY = buffer.ReadFloat(Endianness.Big);
            // double m_CliffStartPosZ = buffer.ReadDouble(Endianness.Big);
            // 
            // double m_CliffStartOldPosX = buffer.ReadDouble(Endianness.Big);
            // float m_CliffStartOldPosY = buffer.ReadFloat(Endianness.Big);
            // double m_CliffStartOldPosZ = buffer.ReadDouble(Endianness.Big);


            foreach (GameClient otherClient in Server.Clients)
            {
                if (otherClient != client)
                {
                    Packet response = new Packet(PacketId.S2C_LOBBY_3_4_16_NTC, buffer.GetAllBytes());
                    client.Send(response);
                }
            }
        }
    }
}
