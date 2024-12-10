using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ClanClanInviteHandler : GameRequestPacketHandler<C2SClanClanInviteReq, S2CClanClanInviteRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClanClanInviteHandler));


        public ClanClanInviteHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CClanClanInviteRes Handle(GameClient client, C2SClanClanInviteReq request)
        {
            var targetClient = Server.ClientLookup.GetClientByCharacterId(request.CharacterId);

            if (targetClient != null)
            {
                var ntc = new S2CClanClanInviteNtc()
                {
                    ClanId = client.Character.ClanId,
                    ClanName = client.Character.ClanName.Name,
                };
                GameStructure.CDataCharacterListElement(ntc.CharacterListElement, client.Character);
                targetClient.Send(ntc);
            }


            return new S2CClanClanInviteRes();
        }
    }
}
