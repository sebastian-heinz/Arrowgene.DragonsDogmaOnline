using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.GameServer.Logging;
using Arrowgene.Ddon.GameServer.Network;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class GetCharacterListHandler : PacketHandler
    {
        private static readonly DdonLogger Logger = LogProvider.Logger<DdonLogger>(typeof(GetCharacterListHandler));


        public GetCharacterListHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2L_GET_CHARACTER_LIST_REQ;

        public override void Handle(Client client, Packet packet)
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
