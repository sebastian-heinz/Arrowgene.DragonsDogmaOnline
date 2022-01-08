using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.GameServer.Logging;
using Arrowgene.Ddon.GameServer.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class GetErrorMessageListHandler : PacketHandler
    {
        private static readonly DdonLogger Logger = LogProvider.Logger<DdonLogger>(typeof(GetErrorMessageListHandler));


        public GetErrorMessageListHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2L_GET_ERROR_MESSAGE_LIST_REQ;

        public override void Handle(Client client, Packet packet)
        {
            //L2C_GET_ERROR_MESSAGE_LIST_NTC
            client.Send(LoginDump.Dump_8);
            client.Send(LoginDump.Dump_9);
            client.Send(LoginDump.Dump_10);
            client.Send(LoginDump.Dump_11);
            client.Send(LoginDump.Dump_12);
            client.Send(LoginDump.Dump_13);
            client.Send(LoginDump.Dump_14);
            client.Send(LoginDump.Dump_15);
            client.Send(LoginDump.Dump_16);
            client.Send(LoginDump.Dump_17);

            //L2C_GET_ERROR_MESSAGE_LIST_RES
            client.Send(LoginDump.Dump_18);
        }
    }
}
