using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model.EpitaphRoad;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SeasonDungeonSoulOrdealCancelReadyHandler : GameRequestPacketHandler<C2SSeasonDungeonSoulOrdealCancelReadyReq, S2CSeasonDungeonSoulOrdealCancelReadyRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SeasonDungeonSoulOrdealCancelReadyHandler));

        public SeasonDungeonSoulOrdealCancelReadyHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CSeasonDungeonSoulOrdealCancelReadyRes Handle(GameClient client, C2SSeasonDungeonSoulOrdealCancelReadyReq request)
        {
            if (client.IsPartyLeader())
            {
                Server.DungeonManager.EndPartyReadyCheck(client.Party);

                client.Party.SendToAll(new S2CSeasonDungeonEndSoulOrdealNtc()
                {
                    EndState = SoulOrdealEndState.Cancel
                });
            }
            else
            {
                Server.DungeonManager.MarkNotReady(client.Party, client.Character);
            }

            return new S2CSeasonDungeonSoulOrdealCancelReadyRes()
            {
                Unk0 = true
            };
        }
    }
}

