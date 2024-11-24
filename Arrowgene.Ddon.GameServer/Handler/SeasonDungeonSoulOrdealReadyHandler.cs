using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SeasonDungeonSoulOrdealReadyHandler : GameRequestPacketHandler<C2SSeasonDungeonSoulOrdealReadyReq, S2CSeasonDungeonSoulOrdealReadyRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SeasonDungeonSoulOrdealReadyHandler));

        public SeasonDungeonSoulOrdealReadyHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CSeasonDungeonSoulOrdealReadyRes Handle(GameClient client, C2SSeasonDungeonSoulOrdealReadyReq request)
        {
            Server.DungeonManager.MarkReady(client.Party, client.Character, request.TrialId);
            if (Server.DungeonManager.PartyIsReady(client.Party))
            {
                Server.DungeonManager.StartActivity(client.Party, (server, party, contentId) =>
                {
                    client.Party.SendToAll(new S2CSeasonDungeonGroupReadyNtc()
                    {
                        Ready = true
                    });
                });
            }

            return new S2CSeasonDungeonSoulOrdealReadyRes();
        }
    }
}

