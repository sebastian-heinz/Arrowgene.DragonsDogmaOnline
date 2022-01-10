using Arrowgene.Buffers;
using Arrowgene.Ddon.LoginServer.Dump;
using Arrowgene.Ddon.Server.Logging;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.LoginServer.Handler
{
    public class ClientX6Handler : PacketHandler<LoginClient>
    {
        private static readonly DdonLogger Logger = LogProvider.Logger<DdonLogger>(typeof(ClientX6Handler));


        public ClientX6Handler(DdonLoginServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.X60;

        public override void Handle(LoginClient client, Packet packet)
        {
            client.Send(LoginDump.Dump_28);
            client.Send(LoginDump.Dump_31);

            // Begin writing L2C_NEXT_CONNECT_SERVER_NTC
            IBuffer buffer = new StreamBuffer();
            buffer.WriteUInt32(0, Endianness.Big); // Error

            CDataGameServerListInfoSerializer serializer = new CDataGameServerListInfoSerializer();
            serializer.Write(buffer, new CDataGameServerListInfo
            {
                ID = 17,
                Name = "サーバー017",
                Brief = "",
                TrafficName = "少なめ",
                MaxLoginNum = 1000, // Player cap
                LoginNum = 0x1C, // Current players
                Addr = "127.0.0.1",
                Port = 52000,
                IsHide = false
            });

            buffer.WriteByte(1); // "counter".
            byte[] data = buffer.GetBytes(0, buffer.Position);

            client.Send(new Packet(PacketId.L2C_NEXT_CONNECT_SERVER_NTC, data, PacketSource.Server));
        }
    }
}
