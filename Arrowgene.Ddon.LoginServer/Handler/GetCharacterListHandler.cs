using Arrowgene.Ddon.LoginServer.Dump;
using Arrowgene.Ddon.Server.Logging;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.LoginServer.Handler
{
    public class GetCharacterListHandler : PacketHandler<LoginClient>
    {
        private static readonly DdonLogger Logger = LogProvider.Logger<DdonLogger>(typeof(GetCharacterListHandler));


        public GetCharacterListHandler(DdonLoginServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2L_GET_CHARACTER_LIST_REQ;

        public override void Handle(LoginClient client, Packet packet)
        {
            // IBuffer buffer = new StreamBuffer();
            // CDataCharacterListInfo character = new CDataCharacterListInfo();
            // EntitySerializer.Get<CDataCharacterListInfo>().Write(buffer, character);
            // Packet response = new Packet(PacketId.L2C_CLIENT_CHALLENGE_RES, buffer.GetAllBytes(), PacketSource.Server);
            //  client.Send(response);

            client.Send(LoginDump.Dump_24);
        }
    }
}
