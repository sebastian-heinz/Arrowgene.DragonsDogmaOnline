using System;
using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server.Logging;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ServerGetServerListHandler : PacketHandler<GameClient>
    {
        private static readonly DdonLogger Logger = LogProvider.Logger<DdonLogger>(typeof(ServerGetServerListHandler));


        public ServerGetServerListHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_SERVER_GET_SERVER_LIST_REQ;

        public override void Handle(GameClient client, Packet packet)
        {
            IBuffer buffer = new StreamBuffer(GameDump.data_Dump_23);
            buffer.Position = 56;
            string firstIp = "127.0.0.1";
            buffer.WriteUInt16((ushort) firstIp.Length, Endianness.Big);
            buffer.WriteString(firstIp);
            
            buffer.WriteUInt16((ushort) 52200, Endianness.Big);
            
            buffer.WriteBytes(GameDump.data_Dump_23.AsSpan(72 + 2).ToArray());

            client.Send(new Packet(PacketId.S2C_SERVER_GET_SERVER_LIST_RES, buffer.GetAllBytes(), PacketSource.Server));

            // client.Send(GameDump.Dump_23);
        }
    }
}
