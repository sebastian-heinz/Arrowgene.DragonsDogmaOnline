using Arrowgene.Buffers;
using Arrowgene.Ddon.Server.Logging;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared;
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
            IBuffer res = new StreamBuffer();
            res.WriteUInt32(0, Endianness.Big);
            res.WriteUInt32(0, Endianness.Big);
            ushort len = 10;
            res.WriteUInt32(len, Endianness.Big);
            for (ushort i = 0; i < len; i++)
            {
                res.WriteUInt16(i, Endianness.Big);
                res.WriteMtString("TestA" + i);
                res.WriteUInt16(0, Endianness.Big);
                res.WriteMtString("TestB" + i);
                res.WriteUInt32(0, Endianness.Big);
                res.WriteUInt32(0, Endianness.Big);
                res.WriteUInt32(0, Endianness.Big);
                res.WriteMtString("127.0.0.1" + i);
                res.WriteUInt16(52000, Endianness.Big);
                res.WriteByte(0);
            }
            client.Send(new Packet(PacketId.S2C_SERVER_GET_SERVER_LIST_RES, res.GetAllBytes(), PacketSource.Server));
        }
    }
}
