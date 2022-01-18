using System;
using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ConnectionReserveServerHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ConnectionReserveServerHandler));


        public ConnectionReserveServerHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_CONNECTION_RESERVE_SERVER_REQ;

        public override void Handle(GameClient client, Packet packet)
        {
            
            IBuffer buffer = new StreamBuffer();
            buffer.WriteUInt32(0, Endianness.Big);
            buffer.WriteUInt32(0, Endianness.Big);
            buffer.WriteBytes(new byte[]
            {
                0x00, 0x05, 0x00, 0x00, 0x00, 0x01, 0x00, 0x20, 0x4F, 0xD8, 0x1B, 0x02, 0x84, 0x14, 0xB0
            });
            client.Send(new Packet(PacketId.S2C_CONNECTION_RESERVE_SERVER_RES, buffer.GetAllBytes()));
            
            //client.Send(GameDump.Dump_27);
        }
    }
}
